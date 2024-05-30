using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Entities.ApproveStage;
using System.Data;
using SolaERP.Job.Enums;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.Entities.Procedure;
using Procedure = SolaERP.Job.Enums.Procedure;
namespace SolaERP.Job
{
    public class Helper
    {
        private readonly IUnitOfWork _unitOfWork;
        public Helper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public enum IsSentValue
        {
            IsSent1 = 0,
            IsSent2 = 1,
            IsSent3 = 2,
        }
        public string GetCommandString(IsSentValue isSentValue)
        {
            switch (isSentValue)
            {
                case IsSentValue.IsSent1:
                    return " AND IsSent = 0 ";
                case IsSentValue.IsSent2:
                    return " AND IsSent = 1 and IsSent2 = 0 ";
                case IsSentValue.IsSent3:
                    return " AND IsSent = 1 and IsSent2 = 1 and IsSent3 = 0 ";
                default:
                    return "";
            }
        }

        public async Task<HashSet<RowInfoDraft>> GetOperationCommand(Procedure procedure, int userId, IsSentValue isSentValue)
        {
            HashSet<RowInfoDraft> rows = new HashSet<RowInfoDraft>();

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select NotificationSenderId," +
                                      "ISNULL(ST.ApprovalStatusName,'In Approve Stage') ApproveStatus," +
                                      "IsSent,IsSent2,IsSent3,NS.SourceId,RM.RequestNo,au.FullName Requester,ISNULL(RM.RequestComment,'') Comment,P.ProcedureName,L.ActionId,L.Date LocalDateTime" +
                                      " from Config.NotificationSender NS INNER JOIN Config.Procedures P ON NS.ProcedureId = P.ProcedureId INNER JOIN Register.Logs L ON " +
                                      "L.SourceId = NS.SourceId INNER JOIN Register.LogTypes LT ON L.LogTypeId = LT.LogTypeId INNER JOIN Procurement.RequestMain RM ON" +
                                      " NS.SourceId = RM.RequestMainId INNER JOIN Config.AppUser au on RM.Requester = au.Id " +
                                      "LEFT JOIN Register.ApprovalStatus ST ON\r\nNS.ApproveStatusId = ST.ApprovalStatusId" +
                                      $" where L.ActionId = 1  and NS.ProcedureId = @procedureId and NS.UserId = @userId {GetCommandString(isSentValue)}";

                command.Parameters.AddWithValue(command, "@procedureId", procedure);
                command.Parameters.AddWithValue(command, "@userId", userId);
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    rows.Add(GetFromReaderForRowInfo(reader));
                }
                return rows;
            }
        }

        public async Task<HashSet<RowInfoDraft>> GetRowInfosForIsSent(Procedure procedure, int userId, IsSentValue isSentValue)
        {
            return await GetOperationCommand(procedure, userId, isSentValue);
        }


        public async Task<List<PersonDraft>> GetUsers(Procedure procedure)
        {
            List<PersonDraft> users = new List<PersonDraft>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select distinct UserId,AU.Email,AU.FullName,AU.Language from " +
                                      "Config.NotificationSender NS INNER JOIN Config.AppUser AU ON NS.UserId = AU.Id " +
                                      "where ProcedureId = @procedureId and IsSent = 0";

                command.Parameters.AddWithValue(command, "@procedureId", procedure);
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    users.Add(GetFromReaderForPerson(reader));
                }
                return users;
            }
        }

        private RowInfoDraft GetFromReaderForRowInfo(IDataReader reader)
        {
            return new RowInfoDraft
            {
                notificationSenderId = reader.Get<int>("NotificationSenderId"),
                approveStatus = reader.Get<string>("ApproveStatus"),
                comment = reader.Get<string>("Comment"),
                localDateTime = reader.Get<DateTime>("LocalDateTime").ToString(),
                name = reader.Get<string>("Requester"),
                number = reader.Get<string>("RequestNo"),
                procedure = reader.Get<string>("ProcedureName"),
                reasonDescription = "",
                sequence = 1,

            };
        }

        private PersonDraft GetFromReaderForPerson(IDataReader reader)
        {
            return new PersonDraft
            {
                Email = reader.Get<string>("Email"),
                Language = reader.Get<string>("Language"),
                UserName = reader.Get<string>("FullName"),
                UserId = reader.Get<int>("UserId")
            };
        }

        public async Task<bool> UpdateIsSent(int[] ids)
        {
            var idsRes = string.Join(",", ids);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @$"set nocount off update Config.NotificationSender set IsSent = 1 where NotificationSenderId IN({idsRes})";
                command.Parameters.AddWithValue(command, "@ids", idsRes);
                try
                {
                    var res = await command.ExecuteNonQueryAsync() > 0;
                    return res;
                }
                catch (Exception ex)
                {
                    return false;
                }


            }
        }
    }
}
