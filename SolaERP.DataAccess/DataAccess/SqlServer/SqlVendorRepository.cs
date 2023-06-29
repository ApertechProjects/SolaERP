using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.ApproveStages;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Entities.PrequalificationCategory;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlVendorRepository : IVendorRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlVendorRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        private async Task<int> ModifyVendorAsync(int userId, Vendor vendor)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF 
                                        DECLARE @NewVendorId int 
                                        EXEC SP_Vendors_IUD @VendorId,
                                                            @BusinessUnitId,
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
                                                            @ApproveStageMainId,
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
                                                            @NewVendorId = @NewVendorId OUTPUT
                                                            SELECT	@NewVendorId as [@NewVendorId]";


                command.Parameters.AddWithValue(command, "@VendorId", vendor.VendorId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", null);
                command.Parameters.AddWithValue(command, "@VendorName", vendor.CompanyName);
                command.Parameters.AddWithValue(command, "@TaxId", vendor.TaxId);
                command.Parameters.AddWithValue(command, "@TaxOffice", vendor.TaxOffice);
                command.Parameters.AddWithValue(command, "@Location", vendor.City);
                command.Parameters.AddWithValue(command, "@Website", vendor.WebSite);
                command.Parameters.AddWithValue(command, "@PaymentTerms", vendor.PaymentTerms);
                command.Parameters.AddWithValue(command, "@CreditDays", vendor.CreditDays);
                command.Parameters.AddWithValue(command, "@_0DaysPayment", vendor.AgreeWithDefaultDays);
                command.Parameters.AddWithValue(command, "@Country", vendor.Country);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@OtherProducts", null);
                command.Parameters.AddWithValue(command, "@ApproveStageMainId", null);
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

                int newVendorId = 0;

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    newVendorId = reader.Get<int>("@NewVendorId");

                return newVendorId;
            }
        }
        private async Task<int> ModifyBankDetailsAsync(int userId, VendorBankDetail bankDetail)
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
                                                                      SELECT @NewVendorBankId as N'NewVendorBankId'";


                command.Parameters.AddWithValue(command, "@VendorBankDetailId", bankDetail.VendorBankDetailId);
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
                command.Parameters.AddWithValue(command, "@CoresspondentAccount", bankDetail.CoresspondentAccount);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                using var reader = await command.ExecuteReaderAsync();

                int id = 0;
                if (reader.Read())
                    id = reader.Get<int>("NewVendorBankId");


                return id;
            }
        }

        public async Task<int> AddBankDetailsAsync(int userId, VendorBankDetail bankDetail)
        {
            return await ModifyBankDetailsAsync(userId, new()
            {
                VendorBankDetailId = 0,
                VendorId = bankDetail.VendorId,
                BeneficiaruTaxId = bankDetail.BeneficiaruTaxId,
                Bank = bankDetail.Bank,
                BankCode = bankDetail.BankCode,
                BankTaxId = bankDetail.BankTaxId,
                Address = bankDetail.Address,
                AccountNumber = bankDetail.AccountNumber,
                SWIFT = bankDetail.SWIFT,
                CoresspondentAccount = bankDetail.CoresspondentAccount,
                Beneficiary = bankDetail.Beneficiary,
                Currency = bankDetail.Currency,
            });
        }

        public async Task<int> DeleteBankDetailsAsync(int userId, int id)
             => await ModifyBankDetailsAsync(userId, new() { VendorBankDetailId = id });
        public async Task<int> UpdateBankDetailsAsync(int userId, VendorBankDetail bankDetail)
             => await ModifyBankDetailsAsync(userId, bankDetail);


        public async Task<int> AddAsync(int userId, Vendor vendor)
        {
            return await ModifyVendorAsync(userId, new()
            {
                VendorId = 0,
                BusinessCategoryId = vendor.BusinessCategoryId,
                Buid = vendor.Buid,
                City = vendor.City,
                CompanyAdress = vendor.CompanyAdress,
                CompanyName = vendor.CompanyName,
                CreditDays = vendor.CreditDays,
                PrequalificationCategoryId = vendor.PrequalificationCategoryId,
                RegistrationDate = vendor.RegistrationDate,
                AgreeWithDefaultDays = vendor.AgreeWithDefaultDays,
                TaxId = vendor.TaxId,
                TaxOffice = vendor.TaxOffice,
                RepresentedCompanies = vendor.RepresentedCompanies,
                RepresentedProducts = vendor.RepresentedProducts,
                WebSite = vendor.WebSite,

            });
        }

        public async Task<int> UpdateAsync(int userId, Vendor vendor)
            => await ModifyVendorAsync(userId, vendor);

        public async Task<int> DeleteAsync(int userId, int id)
            => await DeleteAsync(userId, id);

        public async Task<bool> ChangeStatusAsync(int vendorId, int status, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorsChangeStatus @VendorId,
                                                                                    @UserId,
                                                                                    @Status";

                command.Parameters.AddWithValue(command, "@VendorId", vendorId);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@Status", status);

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
                                      @PrequalificationCategoryId,
                                      @BusinessCategoryId,@ProductServiceId,@VendorTypeId";



                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@VendorTypeId", string.Join(",", filter.VendorTypeId));
                command.Parameters.AddWithValue(command, "@ProductServiceId", string.Join(",", filter.ProductServiceId));
                command.Parameters.AddWithValue(command, "@BusinessCategoryId", string.Join(",", filter.BusinessCategoryId));
                command.Parameters.AddWithValue(command, "@PrequalificationCategoryId", string.Join(",", filter.BusinessCategoryId));

                using var reader = await command.ExecuteReaderAsync();
                List<VendorWFA> data = new List<VendorWFA>();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorWFA>());

                return data;
            }
        }

        public async Task<List<VendorAll>> GetAll(int userId, VendorFilter filter, int status, int approval)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"exec SP_VendorAll @userId, @PrequalificationCategoryId,
                                      @BusinessCategoryId,@ProductServiceId,@VendorTypeId,
                                      @status,@ApproveStatus";


                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@VendorTypeId", string.Join(",", filter.VendorTypeId));
                command.Parameters.AddWithValue(command, "@ProductServiceId", string.Join(",", filter.ProductServiceId));
                command.Parameters.AddWithValue(command, "@BusinessCategoryId", string.Join(",", filter.BusinessCategoryId));
                command.Parameters.AddWithValue(command, "@PrequalificationCategoryId", string.Join(",", filter.BusinessCategoryId));
                command.Parameters.AddWithValue(command, "@status", status);
                command.Parameters.AddWithValue(command, "@ApproveStatus", approval);

                using var reader = await command.ExecuteReaderAsync();
                List<VendorAll> data = new List<VendorAll>();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorAll>());

                return data;
            }
        }

        public async Task<List<VendorInfo>> Get(int businessUnitId, int userId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_VendorList @userId, @businessUnitId";
                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@businessUnitId", businessUnitId);

                using var reader = await command.ExecuteReaderAsync();
                List<VendorInfo> data = new List<VendorInfo>();

                while (reader.Read())
                    data.Add(reader.GetByEntityStructure<VendorInfo>());

                return data;
            }
        }
    }
}
