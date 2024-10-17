using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Helper;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.User;
using System.Numerics;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlVendorRepository : IVendorRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BusinessUnitHelper _businessUnitHelper;

        public SqlVendorRepository(IUnitOfWork unitOfWork, BusinessUnitHelper businessUnitHelper)
        {
            _unitOfWork = unitOfWork;
            _businessUnitHelper = businessUnitHelper;
        }


        private async Task<int> ModifyVendorAsync(int userId, Vendor vendor)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF 
                                        DECLARE @NewVendorId int 
                                        EXEC SP_Vendors_IUD @VendorId,
                                                            @BusinessUnitId,
                                                            @VendorCode,
                                                            @VendorName,
                                                            @TaxId,
                                                            @TaxOffice,
                                                            @Location,
                                                            @Website,
                                                            @PaymentTerms,
                                                            @CreditDays,
                                                            @_0DaysPayment,
                                                            @Country,
                                                            @UserId,
                                                            @OtherProducts,
                                                            @Id,
                                                            @CompanyAddress,
                                                            @CompanyRegistrationDate,
                                                            @Rating,
                                                            @BlackList,
                                                            @BlackListDescription,
                                                            @ReviseNo,
                                                            @ReviseDate,
                                                            @Description,
                                                            @Address2,
                                                            @DefaultCurrency,
                                                            @Postal,
                                                            @PhoneNo,
                                                            @Email,
                                                            @ContactPerson,
                                                            @DeliveryTermId,
                                                            @WithHoldingTaxId,
                                                            @TaxesId,
                                                            @ShipmentId,
                                                            @CompanyLogoFile,
                                                            @NewVendorId = @NewVendorId OUTPUT
                                                            SELECT	@NewVendorId as [@NewVendorId]";


                command.Parameters.AddWithValue(command, "@VendorId", vendor.VendorId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", null);
                command.Parameters.AddWithValue(command, "@VendorCode", vendor.VendorCode);
                command.Parameters.AddWithValue(command, "@VendorName", vendor.CompanyName);
                command.Parameters.AddWithValue(command, "@TaxId", vendor.TaxId);
                command.Parameters.AddWithValue(command, "@TaxOffice", vendor.TaxOffice);
                command.Parameters.AddWithValue(command, "@Location", vendor.City);
                command.Parameters.AddWithValue(command, "@Website", vendor.WebSite);
                command.Parameters.AddWithValue(command, "@PaymentTerms", null);
                command.Parameters.AddWithValue(command, "@CreditDays", vendor.CreditDays);
                command.Parameters.AddWithValue(command, "@_0DaysPayment", vendor.AgreeWithDefaultDays);
                command.Parameters.AddWithValue(command, "@Country", vendor.Country);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@OtherProducts", vendor.OtherProducts);
                command.Parameters.AddWithValue(command, "@Id", null);
                command.Parameters.AddWithValue(command, "@CompanyAddress", vendor.CompanyAdress);
                command.Parameters.AddWithValue(command, "@CompanyRegistrationDate", vendor.RegistrationDate);
                command.Parameters.AddWithValue(command, "@Rating", vendor.Rating);
                command.Parameters.AddWithValue(command, "@BlackList", vendor.BlackList);
                command.Parameters.AddWithValue(command, "@BlackListDescription", vendor.BlackListDescription);
                command.Parameters.AddWithValue(command, "@ReviseNo", vendor.ReviseNo);
                command.Parameters.AddWithValue(command, "@ReviseDate", vendor.ReviseDate);
                command.Parameters.AddWithValue(command, "@Description", vendor.Description);
                command.Parameters.AddWithValue(command, "@Address2", vendor.Address2);
                command.Parameters.AddWithValue(command, "@DefaultCurrency", vendor.DefaultCurrency);
                command.Parameters.AddWithValue(command, "@Postal", vendor.Postal);
                command.Parameters.AddWithValue(command, "@PhoneNo", vendor.PhoneNo);
                command.Parameters.AddWithValue(command, "@Email", vendor.Email);
                command.Parameters.AddWithValue(command, "@ContactPerson", vendor.ContactPerson);
                command.Parameters.AddWithValue(command, "@WithHoldingTaxId", vendor.WithHoldingTaxId);
                command.Parameters.AddWithValue(command, "@ShipmentId", vendor.ShipmentId);
                command.Parameters.AddWithValue(command, "@TaxesId", vendor.TaxesId);
                command.Parameters.AddWithValue(command, "@DeliveryTermId", vendor.DeliveryTermId);
                command.Parameters.AddWithValue(command, "@CompanyLogoFile", vendor.CompanyLogoFile);

                int newVendorId = 0;

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    newVendorId = reader.Get<int>("@NewVendorId");

                return newVendorId;
            }
        }

        private async Task<int> ModifyBankDetailsAsync(int userId, VendorBankDetail bankDetail)
        {
            try
            {

                using (var command = _unitOfWork.CreateCommand() as DbCommand)
                {
                    command.CommandText = @"DECLARE	 @NewVendorBankId int
                                        SET NOCOUNT OFF EXEC SP_VendorBankDetails_IUD @VendorBankDetailId,
                                                                      @VendorId,
                                                                      @Beneficiary,
                                                                      @BeneficiaruTaxId,
                                                                      @Address,
                                                                      @AccountNumber,
                                                                      @Bank,
                                                                      @SWIFT,
                                                                      @BankCode,
                                                                      @Currency,
                                                                      @BankTaxId,
                                                                      @CoresspondentAccount,
                                                                      @UserId,
                                                                      @NewVendorBankId = @NewVendorBankId OUTPUT
                                                                      SELECT @NewVendorBankId as N'@NewVendorBankId'";


                    command.Parameters.AddWithValue(command, "@VendorBankDetailId", bankDetail.Id);
                    command.Parameters.AddWithValue(command, "@VendorId", bankDetail.VendorId);
                    command.Parameters.AddWithValue(command, "@Beneficiary", bankDetail.Beneficiary);
                    command.Parameters.AddWithValue(command, "@BeneficiaruTaxId", bankDetail.BeneficiaruTaxId);
                    command.Parameters.AddWithValue(command, "@Address", bankDetail.Address);
                    command.Parameters.AddWithValue(command, "@AccountNumber", bankDetail.AccountNumber);
                    command.Parameters.AddWithValue(command, "@Bank", bankDetail.Bank);
                    command.Parameters.AddWithValue(command, "@SWIFT", bankDetail.SWIFT);
                    command.Parameters.AddWithValue(command, "@BankCode", bankDetail.BankCode);
                    command.Parameters.AddWithValue(command, "@Currency", bankDetail.Currency);
                    command.Parameters.AddWithValue(command, "@BankTaxId", bankDetail.BankTaxId);
                    command.Parameters.AddWithValue(command, "@CoresspondentAccount", bankDetail.CorrespondentAccount);
                    command.Parameters.AddWithValue(command, "@UserId", userId);



                    using var reader = await command.ExecuteReaderAsync();

                    int id = 0;
                    if (reader.Read())
                        id = reader.Get<int>("@NewVendorBankId");


                    return id;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> AddBankDetailsAsync(int userId, VendorBankDetail bankDetail)
        {
            return await ModifyBankDetailsAsync(userId, new()
            {
                Id = 0,
                VendorId = bankDetail.VendorId,
                BeneficiaruTaxId = bankDetail.BeneficiaruTaxId,
                Bank = bankDetail.Bank,
                BankCode = bankDetail.BankCode,
                BankTaxId = bankDetail.BankTaxId,
                Address = bankDetail.Address,
                AccountNumber = bankDetail.AccountNumber,
                SWIFT = bankDetail.SWIFT,
                CorrespondentAccount = bankDetail.CorrespondentAccount,
                Beneficiary = bankDetail.Beneficiary,
                Currency = bankDetail.Currency,
            });
        }

        public async Task<int> DeleteBankDetailsAsync(int userId, int id)
            => await ModifyBankDetailsAsync(userId, new() { Id = id });

        public async Task<int> UpdateBankDetailsAsync(int userId, VendorBankDetail bankDetail)
            => await ModifyBankDetailsAsync(userId, bankDetail);


        public async Task<int> AddAsync(int userId, Vendor vendor)
        {
            vendor.VendorId = 0;
            return await ModifyVendorAsync(userId, vendor);
        }

        public async Task<int> UpdateAsync(int userId, Vendor vendor)
            => await ModifyVendorAsync(userId, vendor);

        public async Task<int> DeleteAsync(int userId, int id)
            => await ModifyVendorAsync(userId, new() { VendorId = id });

        public async Task<bool> ChangeStatusAsync(int vendorId, int status, int sequence, string comment, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorApprove @VendorId,
                                                                              @UserId,
                                                                              @ApproveStatusId,
                                                                              @Comment,
                                                                              @Sequence";

                command.Parameters.AddWithValue(command, "@VendorId", vendorId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@ApproveStatusId", status);
                command.Parameters.AddWithValue(command, "@Sequence", sequence);
                command.Parameters.AddWithValue(command, "@Comment", comment);

                await _unitOfWork.SaveChangesAsync();

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<VendorInfo> GetByTaxAsync(string taxId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_VendorListByTaxId @TaxId";
                command.Parameters.AddWithValue(command, "@TaxId", taxId);

                using var reader = await command.ExecuteReaderAsync();
                VendorInfo vendorInfo = new VendorInfo();

                while (reader.Read()) vendorInfo = reader.GetByEntityStructure<VendorInfo>();
                return vendorInfo;
            }
        }

        public async Task<List<VendorWFA>> GetWFAAsync(int userId, VendorFilter filter)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"exec SP_VendorWFA @userId,
                                      @BusinessCategoryId,@ProductServiceId,@VendorTypeId";


                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@VendorTypeId", string.Join(",", filter.VendorTypeId));
                command.Parameters.AddWithValue(command, "@ProductServiceId",
                    string.Join(",", filter.ProductServiceId));
                command.Parameters.AddWithValue(command, "@BusinessCategoryId",
                    string.Join(",", filter.BusinessCategoryId));

                using var reader = await command.ExecuteReaderAsync();
                List<VendorWFA> data = new List<VendorWFA>();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorWFA>());

                return data;
            }
        }

        public async Task<List<VendorAll>> GetAll(int userId, VendorAllCommandRequest request)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"exec SP_VendorAll @userId,
                                      @BusinessCategoryId,@ProductServiceId,@VendorTypeId,
                                      @status,@ApproveStatus";


                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@VendorTypeId", string.Join(",", request.VendorTypeId));
                command.Parameters.AddWithValue(command, "@ProductServiceId",
                    string.Join(",", request.ProductServiceId));
                command.Parameters.AddWithValue(command, "@BusinessCategoryId",
                    string.Join(",", request.BusinessCategoryId));
                command.Parameters.AddWithValue(command, "@status", string.Join(",", request.Status));
                command.Parameters.AddWithValue(command, "@ApproveStatus", string.Join(",", request.Approval));

                using var reader = await command.ExecuteReaderAsync();
                List<VendorAll> data = new List<VendorAll>();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorAll>());

                return data;
            }
        }

        public async Task<List<VendorInfo>> Vendors(int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_VendorList @userId";
                command.Parameters.AddWithValue(command, "@userId", userId);

                using var reader = await command.ExecuteReaderAsync();
                List<VendorInfo> data = new List<VendorInfo>();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorInfo>());

                return data;
            }
        }

        public async Task<List<VendorWFA>> GetHeldAsync(int userId, VendorFilter filter)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorHeld @UserId,
                                      @BusinessCategoryId,
                                      @ProductServiceId,
                                      @VendorTypeId";


                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@VendorTypeId", string.Join(",", filter.VendorTypeId));
                command.Parameters.AddWithValue(command, "@ProductServiceId",
                    string.Join(",", filter.ProductServiceId));
                command.Parameters.AddWithValue(command, "@BusinessCategoryId",
                    string.Join(",", filter.BusinessCategoryId));

                List<VendorWFA> data = new List<VendorWFA>();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorWFA>());

                return data;
            }
        }

        public async Task<List<VendorDraft>> GetDraftAsync(int userId, VendorFilter filter)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorDraft @UserId,
                                         @BusinessCategoryId,
                                         @ProductServiceId,
                                         @VendorTypeId";


                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@VendorTypeId", string.Join(",", filter.VendorTypeId));
                command.Parameters.AddWithValue(command, "@ProductServiceId",
                    string.Join(",", filter.ProductServiceId));
                command.Parameters.AddWithValue(command, "@BusinessCategoryId",
                    string.Join(",", filter.BusinessCategoryId));

                using var reader = await command.ExecuteReaderAsync();
                List<VendorDraft> data = new List<VendorDraft>();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorDraft>());

                return data;
            }
        }

        public async Task<VendorLoad> GetHeader(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"Exec SP_Vendor_load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                using var reader = await command.ExecuteReaderAsync();
                VendorLoad vendorCard = new VendorLoad();

                if (reader.Read())
                    vendorCard = reader.GetByEntityStructure<VendorLoad>("Logo");

                return vendorCard;
            }
        }

        public async Task<bool> ApproveAsync(VendorApproveModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText =
                    @"SET NOCOUNT OFF EXEC SP_VendorApprove @VendorId,@UserId,@ApproveStatusId,@Comment,@Sequence";


                command.Parameters.AddWithValue(command, "@VendorId", model.VendorId);
                command.Parameters.AddWithValue(command, "@UserId", model.UserId);
                command.Parameters.AddWithValue(command, "@ApproveStatusId", model.ApproveStatusId);
                command.Parameters.AddWithValue(command, "@Comment", model.Comment);
                command.Parameters.AddWithValue(command, "@Sequence", model.Sequence);
                var data = await command.ExecuteNonQueryAsync() > 0;
                return data;
            }
        }

        public async Task<bool> SendToApprove(VendorSendToApproveRequest request)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                var result = string.Join(",", request.VendorIds);
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorSendToApprove @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", result);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<VendorWFA>> GetRejectedAsync(int userId, VendorFilter filter)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorRejected @UserId,
                                            @BusinessCategoryId,
                                            @ProductServiceId,
                                            @VendorTypeId";

                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@VendorTypeId", string.Join(",", filter.VendorTypeId));
                command.Parameters.AddWithValue(command, "@ProductServiceId",
                    string.Join(",", filter.ProductServiceId));
                command.Parameters.AddWithValue(command, "@BusinessCategoryId",
                    string.Join(",", filter.BusinessCategoryId));

                using var reader = await command.ExecuteReaderAsync();
                List<VendorWFA> data = new List<VendorWFA>();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorWFA>());

                return data;
            }
        }

        public async Task<List<VendorApproved>> GetApprovedAsync(int userId)
        {
            List<VendorApproved> data = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorApproved @UserId";
                command.Parameters.AddWithValue(command, "@UserId", userId);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorApproved>());

                return data;
            }
        }

        public async Task<string> GetVendorLogo(int vendorId)
        {
            string result = string.Empty;
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select * from [dbo].[FN_GetVendorImageData](@VendorId)";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    result = reader.Get<string>("FileData");

                return result;
            }
        }

        public async Task<bool> HasVendorName(string vendorName, int userId)
        {
            var currentUserVendorName = await GetVendorNameByUserId(userId);
            if (currentUserVendorName == vendorName)
            {
                return false;
            }

            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "SELECT * FROM Procurement.Vendors WHERE VendorName = @VendorName";
            command.Parameters.AddWithValue(command, "@VendorName", vendorName);

            return (await command.ExecuteReaderAsync()).HasRows;
        }

        public async Task<string> GetVendorNameByUserId(int userId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "select VendorName from Procurement.Vendors inner join Config.AppUser " +
                                  "on AppUser.VendorId = Vendors.VendorId " +
                                  "where AppUser.Id = @UserId";

            command.Parameters.AddWithValue(command, "@UserId", userId);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                return reader.Get<string>("VendorName");

            return null;
        }

        public async Task<List<VendorRFQListDto>> GetVendorRFQList(string vendorCode, int userId)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = "EXEC dbo.SP_VendorRFQList @VendorCode, @UserId";
            command.Parameters.AddWithValue(command, "@vendorCode", vendorCode);
            command.Parameters.AddWithValue(command, "@UserId", userId);

            await using var reader = await command.ExecuteReaderAsync();
            var list = new List<VendorRFQListDto>();
            while (await reader.ReadAsync())
            {
                list.Add(new VendorRFQListDto
                {
                    BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                    BidMainId = reader.Get<int>("BidMainId"),
                    RFQMainId = reader.Get<int>("RFQMainId"),
                    LineNo = reader.Get<long>("LineNo"),
                    ParticipationStatus = reader.Get<string>("ParticipationStatus"),
                    Emergency = reader.Get<int>("Emergency"),
                    RFQStatus = reader.Get<int>("RFQStatus"),
                    RFQNo = reader.Get<string>("RFQNo"),
                    BusinessCategoryId = reader.Get<int>("BusinessCategoryId"),
                    BusinessCategoryName = reader.Get<string>("BusinessCategoryName"),
                    RFQType = reader.Get<int>("RFQType"),
                    DesiredDeliveryDate = reader.Get<DateTime>("DesiredDeliveryDate"),
                    RFQDate = reader.Get<DateTime>("RFQDate"),
                    RFQDeadline = reader.Get<DateTime>("RFQDeadline"),
                    RespondedDate = reader.Get<DateTime>("RespondedDate"),
                    EnteredBy = reader.Get<string>("EnteredBy"),
                    SentDate = reader.Get<DateTime>("SentDate"),
                    CreatedDate = reader.Get<DateTime>("CreatedDate"),
                    BiddingType = reader.Get<string>("BiddingType")
                });
            }

            return list;
        }

        public async Task<bool> RFQVendorResponseChangeStatus(int rfqMainId, int status, string vendorCode)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText =
                @"SET NOCOUNT OFF EXEC SP_RFQVendorResponseChangeStatus @RFQMainid, @Status, @VendorCode";
            command.Parameters.AddWithValue(command, "@RFQMainid", rfqMainId);
            command.Parameters.AddWithValue(command, "@Status", status);
            command.Parameters.AddWithValue(command, "@VendorCode", vendorCode);

            await _unitOfWork.SaveChangesAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<int> GetRevisionVendorIdByVendorCode(string vendorCode)
        {
            int result = 0;
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"SELECT TOP 1 VendorId
                                    FROM Procurement.Vendors
                                    WHERE VendorCode = @VendorCode ORDER BY ReviseNo DESC ";
            command.Parameters.AddWithValue(command, "@VendorCode", vendorCode);

            await using DbDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                result = reader.Get<int>("VendorId");

            return result;
        }

        public async Task<int> GetRevisionNumberByVendorCode(string vendorCode)
        {
            int result = 0;
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = @"SELECT TOP 1 ReviseNo
                                    FROM Procurement.Vendors
                                    WHERE VendorCode = @VendorCode ORDER BY ReviseNo DESC ";
            command.Parameters.AddWithValue(command, "@VendorCode", vendorCode);

            await using DbDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                result = reader.Get<int>("ReviseNo");

            return result;
        }

        public async Task<bool> TransferToIntegration(CreateVendorRequest request)
        {
            await using var command = _unitOfWork.CreateCommand() as DbCommand;
            command.CommandText = _businessUnitHelper.BuildQueryForIntegration(request.BusinessUnitId,
                "SP_Vendor_IUD @BusinessUnitId, @VendorCode, @UserId");
            command.Parameters.AddWithValue(command, "@BusinessUnitId", request.BusinessUnitId);
            command.Parameters.AddWithValue(command, "@VendorCode", request.VendorCode);
            command.Parameters.AddWithValue(command, "@UserId", request.UserId);
            await _unitOfWork.SaveChangesAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<string> GetCompanyLogoFileAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $"select CompanyLogoFile from Procurement.Vendors where VendorId = @vendorId";
                command.Parameters.AddWithValue(command, "@vendorId", vendorId);
                using var reader = await command.ExecuteReaderAsync();

                string image = string.Empty;
                if (reader.Read())
                    image = reader.Get<string>("CompanyLogoFile");

                return image;
            }
        }

        public async Task VendorSubmit(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF 
                                        EXEC SP_VendorSubmit @VendorId"
                ;

                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                using var reader = await command.ExecuteReaderAsync();
            }
        }

        public async Task<VendorLoad> GetVendorPreviousHeader(int vendorId)
        {
            VendorLoad data = new();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC GetVendorPreviousVersion @vendorId";
                command.Parameters.AddWithValue(command, "@vendorId", vendorId);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    data = reader.GetByEntityStructure<VendorLoad>("VendorType", "Logo");

                return data;
            }
        }

        public async Task<int> GetVendorPreviousVendorId(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $"select [dbo].[SF_GetPreviousVendorId](@vendorId) as VendorId";
                command.Parameters.AddWithValue(command, "@vendorId", vendorId);
                using var reader = await command.ExecuteReaderAsync();


                if (reader.Read())
                    vendorId = reader.Get<int>("VendorId");

                if (vendorId > 0) return vendorId;
                return -1;
            }
        }
    }
}