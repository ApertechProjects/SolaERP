using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlSupplierEvaluationRepository : ISupplierEvaluationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlSupplierEvaluationRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> AddDueDesign(VendorDueDiligenceModel vendorDueDiligence)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorDueDiligence_IUD @DueDiligenceDesignId,@VendorId
                                                                      ,@TextboxValue,@TextareaValue
                                                                      ,@CheckboxValue,@RadioboxValue
`                                                                     ,@IntValue,@DecimalValue
                                                                      ,@DateTimeValue,@AgreementValue";

                command.Parameters.AddWithValue(command, "@DueDiligenceDesignId", vendorDueDiligence.DueDiligenceDesignId);
                command.Parameters.AddWithValue(command, "@VendorId", vendorDueDiligence.VendorId);
                command.Parameters.AddWithValue(command, "@TextboxValue", vendorDueDiligence.TextboxValue);
                command.Parameters.AddWithValue(command, "@TextareaValue", vendorDueDiligence.TextareaValue);
                command.Parameters.AddWithValue(command, "@CheckboxValue", vendorDueDiligence.CheckboxValue);
                command.Parameters.AddWithValue(command, "@RadioboxValue", vendorDueDiligence.RadioboxValue);
                command.Parameters.AddWithValue(command, "@IntValue", vendorDueDiligence.IntValue);
                command.Parameters.AddWithValue(command, "@DecimalValue", vendorDueDiligence.DecimalValue);
                command.Parameters.AddWithValue(command, "@DateTimeValue", vendorDueDiligence.DateTimeValue);
                command.Parameters.AddWithValue(command, "@AgreementValue", vendorDueDiligence.AgreementValue);


                return await command.ExecuteNonQueryAsync() > 0;
            }

        }

        public async Task<bool> AddDueDesignGrid(DueDiligenceGridModel gridModel)
        {
            return await SaveDueDesignGridAsync(new()
            {
                DueDesignId = gridModel.DueDesignId,
                Column1 = gridModel.Column1,
                Column2 = gridModel.Column2,
                Column3 = gridModel.Column3,
                Column4 = gridModel.Column4,
                Column5 = gridModel.Column5,
            });
        }


        private async Task<bool> SaveDueDesignGridAsync(DueDiligenceGridUpdateModel gridModel)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_DueDiligenceGridData_IUD @DueDiligenceGridDataId,@DueDiligenceDesignId,
                                                                                         @Column1,@Column2,@Column3,@Column4,@Column5";

                command.Parameters.AddWithValue(command, "@DueDiligenceGridDataId", gridModel.Id);
                command.Parameters.AddWithValue(command, "@DueDiligenceDesignId", gridModel.DueDesignId);
                command.Parameters.AddWithValue(command, "@Column1", gridModel.Column1);
                command.Parameters.AddWithValue(command, "@Column2", gridModel.Column2);
                command.Parameters.AddWithValue(command, "@Column3", gridModel.Column3);
                command.Parameters.AddWithValue(command, "@Column4", gridModel.Column4);
                command.Parameters.AddWithValue(command, "@Column5", gridModel.Column5);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }



        public async Task<List<BusinessCategory>> GetBusinessCategoriesAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_BusinessCategory";

                List<BusinessCategory> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<BusinessCategory>());

                return resultList;
            }
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_Country_List";

                List<Country> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<Country>());

                return resultList;
            }
        }

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_UNI_Currency_List";

                List<Currency> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<Currency>());

                return resultList;
            }
        }

        public async Task<List<DueDiligenceGrid>> GetDueDiligenceGridsAsync(int dueDesignId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_DueDiligenceGridData_Load @dueDesignId";
                command.Parameters.AddWithValue(command, "@dueDesignId", dueDesignId);

                List<DueDiligenceGrid> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<DueDiligenceGrid>());

                return resultList;
            }
        }

        public async Task<List<DueDiligenceDesign>> GetDueDiligencesDesignAsync(Language language)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_DueDiligenceDesign_Load @langCode";
                command.Parameters.AddWithValue(command, "@langCode", language.ToString());

                List<DueDiligenceDesign> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(GetDueDesignFromReader(reader));

                return resultList;
            }
        }

        public async Task<List<PaymentTerms>> GetPaymentTermsAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_PaymentTerms_List";

                List<PaymentTerms> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<PaymentTerms>());

                return resultList;
            }
        }

        public async Task<List<PrequalificationCategory>> GetPrequalificationCategoriesAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_PrequalificationCategoryList";

                List<PrequalificationCategory> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<PrequalificationCategory>());

                return resultList;
            }
        }

        public async Task<List<ProductService>> GetProductServicesAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_ProductService_List";

                List<ProductService> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<ProductService>());

                return resultList;
            }
        }

        public async Task<List<VendorBankDetails>> GetVondorBankDetailsAsync(int vendorid)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorBank_Load @vendorId";
                command.Parameters.AddWithValue(command, "@vendorId", vendorid);

                List<VendorBankDetails> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<VendorBankDetails>());

                return resultList;
            }
        }


        private DueDiligenceDesign GetDueDesignFromReader(IDataReader reader)
        {
            return new()
            {
                Title = reader.Get<string>("Title"),
                Question = reader.Get<string>("Questions"),
                DesignId = reader.Get<int>("DueDiligenceDesignId"),
                LineNo = reader.Get<int>("LineNo"),
                HasTextBox = reader.Get<decimal>("HasTextBox"),
                HasCheckBox = reader.Get<decimal>("HasCheckBox"),
                HasRadioBox = reader.Get<decimal>("HasRadioBox"),
                HasInt = reader.Get<decimal>("HasInt"),
                HasDecimal = reader.Get<decimal>("HasDecimal"),
                HasDateTime = reader.Get<decimal>("HasDateTime"),
                HasAttachment = reader.Get<decimal>("HasAttachment"),
                HasBankList = reader.Get<decimal>("HasBankList"),
                HasTexArea = reader.Get<decimal>("HasTextarea"),
                ParentCompanies = reader.Get<bool>("ParentCompanies"),
                HasGrid = reader.Get<decimal>("HasGrid"),
                GridRowLimit = reader.Get<int>("GridRowLimit"),
                GridColumnCount = reader.Get<int>("GridColumnCount"),
                HasAgreement = reader.Get<bool>("HasAgreement"),
                AgreementText = reader.Get<string>("AgreementText"),
                Column1Alias = reader.Get<string>("Column1Alias"),
                Column2Alias = reader.Get<string>("Column2Alias"),
                Column3Alias = reader.Get<string>("Column3Alias"),
                Column4Alias = reader.Get<string>("Column4Alias"),
                Column5Alias = reader.Get<string>("Column5Alias"),
            };
        }

        public Task<bool> AddPrequalificationAsync(VendorPreInputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrequalificationAsync(int id, VendorPreInputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePrequalificationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddDueDiligenceAsync(VendorDueDiligenceModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDueDiligenceAsync(int id, VendorDueDiligenceModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDueDiligenceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddVendorAsync(VendorModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateVendorAsync(int id, VendorModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteVendorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDueDesignGrid(DueDiligenceGridUpdateModel gridModel)
        {
            return await SaveDueDesignGridAsync(gridModel);
        }

        public async Task<bool> DeleteDueDesignGrid(int id)
        {
            return await SaveDueDesignGridAsync(new() { Id = id });
        }



        private async Task<bool> SaveVendorPreAsync(int id, VendorPreInputModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorPrequalification_IUD @VendorPrequalificationId,@PrequalificationDesignId
                                                                           @VendorId,@TextboxValue,@TextareaValue,@CheckboxValue,
                                                                           @RadioboxValue,@IntValue,@DecimalValue,@DateTimeValue,@Scoring";

                command.Parameters.AddWithValue(command, "@VendorPrequalificationId", id);
                command.Parameters.AddWithValue(command, "@PrequalificationDesignId", model?.DesignId);
                command.Parameters.AddWithValue(command, "@TextboxValue", model?.TextBoxValue);
                command.Parameters.AddWithValue(command, "@TextareaValue", model?.TextareaValue);
                command.Parameters.AddWithValue(command, "@CheckboxValue", model?.CheckboxValue);
                command.Parameters.AddWithValue(command, "@RadioboxValue", model?.RadioboxValue);
                command.Parameters.AddWithValue(command, "@IntValue", model?.IntValue);
                command.Parameters.AddWithValue(command, "@DecimalValue", model?.DecimalValue);
                command.Parameters.AddWithValue(command, "@DateTimeValue", model?.DateTimeValue);
                command.Parameters.AddWithValue(command, "@Scoring", model?.Scoring);
                command.Parameters.AddWithValue(command, "@VendorId", model?.VendorId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }


        private async Task<bool> SaveVendorAsync(int id, VendorModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_Vendors_IUD @VendorId,@BusinessUnitId,@VendorName,
                                                            @TaxId,@TaxOffice,@Location,
                                                            @Website,@PaymentTerms,@CreditDays,
                                                            @_0DaysPayment,@Country,@UserId,
                                                            @OtherProducts,@ApproveStageMainId,@CompanyAddress,
                                                            @CompanyRegistrationDate";

                command.Parameters.AddWithValue(command, "@VendorId", id);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", model?.BusinessUnitId);
                command.Parameters.AddWithValue(command, "@VendorName", model?.VendorName);
                command.Parameters.AddWithValue(command, "@TaxId", model?.TaxId);
                command.Parameters.AddWithValue(command, "@TaxOffice", model?.TaxOffice);
                command.Parameters.AddWithValue(command, "@Location", model?.Location);
                command.Parameters.AddWithValue(command, "@Website", model?.Website);
                command.Parameters.AddWithValue(command, "@PaymentTerms", model?.PaymentTerms);
                command.Parameters.AddWithValue(command, "@CreditDays", model?.CreditDays);
                command.Parameters.AddWithValue(command, "@_0DaysPayment", model?._0DaysPayment);
                command.Parameters.AddWithValue(command, "@Country", model?.Country);
                command.Parameters.AddWithValue(command, "@UserId", model?.UserId);
                command.Parameters.AddWithValue(command, "@OtherProducts", model?.OtherProducts);
                command.Parameters.AddWithValue(command, "@ApproveStageMainId", model?.ApproveStageMainId);
                command.Parameters.AddWithValue(command, "@CompanyAddress", model?.CompanyAddress);
                command.Parameters.AddWithValue(command, "@CompanyRegistrationDate", model?.CompanyRegistrationDate);


                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public Task<bool> UpdateCOBCAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCOBCAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddCOBCAsync()
        {
            throw new NotImplementedException();
        }


        //private Task<bool> SaveDueDiligenceAsync(int id, VendorDueDiligenceModel model)
        //{
        //    using(var command = _unitOfWork.CreateCommand() as DbCommand) 
        //    {
        //        command.CommandText = "EXEC "
        //    }
        //}




    }
}
