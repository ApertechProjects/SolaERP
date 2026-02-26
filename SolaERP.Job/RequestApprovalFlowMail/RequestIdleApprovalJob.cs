using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Enums;
using SolaERP.Application.ViewModels;

namespace SolaERP.Job.RequestApprovalFlowMail
{
    public sealed class RequestIdleApprovalJob : IJob
    {
        private const int RejectReasonId = 1;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RequestIdleApprovalJob> _logger;

        public RequestIdleApprovalJob(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<RequestIdleApprovalJob> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            using var scope = _scopeFactory.CreateScope();

            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
            var emailNotificationService = scope.ServiceProvider.GetRequiredService<IEmailNotificationService>();
            var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
            

            var candidates = await LoadCandidates(context.CancellationToken);

            _logger.LogInformation("Found {Count} idle approval candidates", candidates.Count);
            
            var uiBase = (_configuration["Mail:ServerUrlUI"] ?? "").TrimEnd('/');
            
            foreach (var c in candidates)
            {
                try
                {
                    var (templateKey, notificationType) = c.IdleDays switch
                    {
                        7 => (EmailTemplateKey.R_R7, "REMINDER7"),
                        14 => (EmailTemplateKey.R_W14, "WARNING14"),
                        >= 15 => (EmailTemplateKey.R_F15, "FINAL15"),
                        _ => (default(EmailTemplateKey), null)
                    };

                    if (notificationType is null)
                        continue;
                    

                    var approverInserted = await TryInsertNotificationLog(
                        c.RequestApprovalId,
                        c.ApproverUserId,
                        c.ApproverEmail,
                        notificationType,
                        context.CancellationToken);
                    
                    var templates = await emailNotificationService.GetEmailTemplateData(templateKey);

                    var requestUrl =
                        !string.IsNullOrWhiteSpace(uiBase)
                            ? $"{uiBase}/requests/{c.RequestMainId}"
                            : null;
                    
                    var vm = new VM_RequestApprovalFlow()
                    {
                        Language = Language.en,
                        TemplateKey = templateKey,
                        RequestNo = c.RequestNo,
                        Sequence = c.Sequence,
                        FullName = c.ApproverFullName,
                        AwaitingReviewSince = c.EnteredAt,
                        RequestUrl = requestUrl,
                        RequesterFullName = c.RequesterFullName,
                        
                    };
                    
                    var template = templates.FirstOrDefault();
                    if (template != null)
                    {
                        vm.Header = template.Header;
                        vm.Subject = template.Subject;
                        vm.Body = new HtmlString(template.Body ?? string.Empty);
                    }

                    if (approverInserted)
                    {
                        await mailService.SendQueueUsingTemplate(
                            subject: vm.GenerateSubject()?.ToString() ?? "Request notification",
                            viewModel: vm,
                            templateName: vm.TemplateName(),
                            imageName: null,
                            tos: new List<string> { c.ApproverEmail });

                        _logger.LogInformation(
                            "Approver notification sent. RequestNo={RequestNo}, Email={Email}",
                            c.RequestNo,
                            c.ApproverEmail);
                    }

                    _logger.LogInformation(
                        "Notification sent. RequestNo={RequestNo}, IdleDays={IdleDays}",
                        c.RequestNo,
                        c.IdleDays);
                    
                    if (c.IdleDays >= 15)
                    {
                        var requesterInserted = await TryInsertNotificationLog(
                            c.RequestApprovalId,
                            0,
                            c.RequesterEmail,
                            "RequesterRejectMail",
                            context.CancellationToken);

                        if (requesterInserted)
                        {
                            var requesterVm = new VM_RequestApprovalFlow()
                            {
                                Language = Language.en,
                                TemplateKey = EmailTemplateKey.R_F15, // ayrıca key olmalıdır
                                RequestNo = c.RequestNo,
                                Sequence = c.Sequence,
                                FullName = c.RequesterFullName,
                                AwaitingReviewSince = c.EnteredAt,
                                RequestUrl = requestUrl,
                                RequesterFullName = c.RequesterFullName,
                            };

                            var requesterTemplates =
                                await emailNotificationService.GetEmailTemplateData(EmailTemplateKey.R_F15);

                            var requesterTemplate = requesterTemplates.FirstOrDefault();
                            if (requesterTemplate != null)
                            {
                                requesterVm.Header = requesterTemplate.Header;
                                requesterVm.Subject = requesterTemplate.Subject;
                                requesterVm.Body = new HtmlString(requesterTemplate.Body ?? string.Empty);
                            }

                            await mailService.SendQueueUsingTemplate(
                                subject: requesterVm.GenerateSubject()?.ToString() ?? "Request auto rejected",
                                viewModel: requesterVm,
                                templateName: requesterVm.TemplateName(),
                                imageName: null,
                                tos: new List<string> { c.RequesterEmail });

                            _logger.LogInformation(
                                "Requester reject mail sent. RequestNo={RequestNo}, Email={Email}",
                                c.RequestNo,
                                c.RequesterEmail);
                        }

                        await requestService.ChangeDetailStatusAsync(
                            "619",
                            c.RequestDetailId,
                            approveStatusId: 2,
                            comment: "Auto rejected due to inactivity (15 days without approval).",
                            sequence: c.Sequence,
                            rejectReasonId: RejectReasonId);

                        _logger.LogWarning(
                            "Request auto rejected. RequestNo={RequestNo}",
                            c.RequestNo);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error processing RequestNo={RequestNo}",
                        c.RequestNo);
                }
            }

            _logger.LogInformation("RequestIdleApprovalJob finished at {Time}", DateTime.UtcNow);
        }
        
        
        
        
        

        private async Task<List<CandidateRow>> LoadCandidates(CancellationToken ct)
        {
            var connStr = _configuration.GetConnectionString("DevelopmentConnectionString");
            var list = new List<CandidateRow>();

            await using var conn = new SqlConnection(connStr);
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Procurement.SP_RequestApprovalIdleCandidates";

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new CandidateRow
                {
                    RequestDetailId = reader.GetInt32(reader.GetOrdinal("RequestDetailId")),
                    RequestApprovalId = reader.GetInt32(reader.GetOrdinal("RequestApprovalId")),
                    Sequence = reader.GetInt32(reader.GetOrdinal("Sequence")),
                    IdleDays = reader.GetInt32(reader.GetOrdinal("IdleDays")),
                    RequestNo = reader.GetString(reader.GetOrdinal("RequestNo")),
                    ApproverFullName = reader.GetString(reader.GetOrdinal("ApproverFullName")),
                    ApproverEmail = reader.GetString(reader.GetOrdinal("ApproverEmail")),
                    ApproverUserId = reader.GetInt32(reader.GetOrdinal("ApproverUserId")),
                    RequesterEmail = reader.GetString(reader.GetOrdinal("RequesterEmail")),
                    RequesterFullName = reader.GetString(reader.GetOrdinal("RequesterFullName")),
                    EnteredAt = reader.GetDateTime(reader.GetOrdinal("EnteredAt")),
                    RequestMainId = reader.GetInt32(reader.GetOrdinal("RequestMainId")),
                });
            }

            return list;
        }

        private async Task<bool> TryInsertNotificationLog(
            int requestApprovalId,
            int? recipientUserId,
            string recipientEmail,
            string type,
            CancellationToken ct)
        {
            var connStr = _configuration.GetConnectionString("DevelopmentConnectionString");

            await using var conn = new SqlConnection(connStr);
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"
BEGIN TRY
    INSERT INTO Procurement.RequestApprovalNotificationLog
        (RequestApprovalId, RecipientUserId, RecipientEmail, NotificationType)
    VALUES (@RequestApprovalId, @RecipientUserId, @RecipientEmail, @NotificationType);

    SELECT 1;
END TRY
BEGIN CATCH
    SELECT 0;
END CATCH";

            cmd.Parameters.Add(new SqlParameter("@RequestApprovalId", SqlDbType.Int) { Value = requestApprovalId });
            cmd.Parameters.Add(new SqlParameter("@RecipientUserId", SqlDbType.Int) { Value = (object?)recipientUserId ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@RecipientEmail", SqlDbType.NVarChar, 256) { Value = recipientEmail });
            cmd.Parameters.Add(new SqlParameter("@NotificationType", SqlDbType.NVarChar, 30) { Value = type });

            var result = (int)await cmd.ExecuteScalarAsync(ct);
            return result == 1;
        }

        private sealed class CandidateRow
        {
            public int RequestDetailId { get; set; }
            public int RequestApprovalId { get; set; }
            public int Sequence { get; set; }
            public int IdleDays { get; set; }
            public string RequestNo { get; set; } = "";
            public string ApproverFullName { get; set; } = "";
            public string ApproverEmail { get; set; } = "";
            public int ApproverUserId { get; set; }
            public string RequesterEmail { get; set; } = "";
            public string RequesterFullName { get; set; } = "";
            public DateTime EnteredAt { get; set; }
            public int RequestMainId { get; set; }
        }
    }
}