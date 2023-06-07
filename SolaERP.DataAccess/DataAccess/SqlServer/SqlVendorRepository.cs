using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

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
                command.CommandText = @"DECLARE @NewVendorId int 
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
                                                            @NewVendorId = @NewVendorId OUTPUT
                                                            SELECT	@NewVendorId as [@NewVendorId]";


                command.Parameters.AddWithValue(command, "@VendorId", vendor.VendorId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", null);
                command.Parameters.AddWithValue(command, "@VendorName", vendor.CompanyName);
                command.Parameters.AddWithValue(command, "@TaxId", vendor.TaxOffice);
                command.Parameters.AddWithValue(command, "@TaxOffice", vendor.TaxOffice);
                command.Parameters.AddWithValue(command, "@Location", vendor.WebSite);
                command.Parameters.AddWithValue(command, "@Website", vendor.WebSite);
                command.Parameters.AddWithValue(command, "@PaymentTerms", vendor.PaymentTerms);
                command.Parameters.AddWithValue(command, "@CreditDays", vendor.CreditDays);
                command.Parameters.AddWithValue(command, "@_0DaysPayment", vendor.AgreeWithDefaultDays);
                command.Parameters.AddWithValue(command, "@Country", vendor.City);
                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@OtherProducts", null);
                command.Parameters.AddWithValue(command, "@ApproveStageMainId", null);
                command.Parameters.AddWithValue(command, "@CompanyAddress", vendor.CompanyAdress);
                command.Parameters.AddWithValue(command, "@CompanyRegistrationDate", vendor.RegistrationDate);

                int newVendorId = 0;

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    newVendorId = reader.Get<int>("@NewVendorId");

                return newVendorId;
            }
        }
        private async Task<bool> ModifyBankDetailsAsync(int userId, VendorBankDetail bankDetail)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorBankDetails_IUD @VendorBankDetailId,
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
                                                                      @UserId";


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

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<VendorInfo> GetVendorByTaxIdAsync(string taxId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec SP_VendorListByTaxId @TaxId";
                command.Parameters.AddWithValue(command, "@TaxId", taxId);
                using var reader = await command.ExecuteReaderAsync();
                VendorInfo result = new();

                while (reader.Read()) result = reader.GetByEntityStructure<VendorInfo>();

                return result;
            }
        }

        public async Task<bool> AddBankDetailsAsync(int userId, VendorBankDetail bankDetail)
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

        public async Task<bool> DeleteBankDetailsAsync(int userId, int id)
             => await ModifyBankDetailsAsync(userId, new() { VendorBankDetailId = id });
        public async Task<bool> UpdateBankDetailsAsync(int userId, VendorBankDetail bankDetail)
             => await ModifyBankDetailsAsync(userId, bankDetail);


        public async Task<int> AddVendorAsync(int userId, Vendor vendor)
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

        public async Task<int> UpdateVendorAsync(int userId, Vendor vendor)
            => await ModifyVendorAsync(userId, vendor);

        public async Task<int> DeleteVendorAsync(int userId, int id)
            => await DeleteVendorAsync(userId, id);
    }
}
