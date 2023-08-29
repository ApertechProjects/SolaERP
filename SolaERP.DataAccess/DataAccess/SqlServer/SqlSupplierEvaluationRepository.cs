using Newtonsoft.Json.Linq;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlSupplierEvaluationRepository : ISupplierEvaluationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlSupplierEvaluationRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        private async Task<bool> ModifyDueDiligence(VendorDueDiligenceModel vendorDueDiligence)
        {
            try
            {
                using (var command = _unitOfWork.CreateCommand() as DbCommand)
                {
                    command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorDueDiligence_IUD @VendorDueDiligenceId,
                                                                       @DueDiligenceDesignId,@VendorId
                                                                      ,@TextboxValue,@TextareaValue
                                                                      ,@CheckboxValue,@RadioboxValue
                                                                      ,@IntValue,@DecimalValue
                                                                      ,@DateTimeValue,@AgreementValue,@Scoring,@BankListValue";

                    command.Parameters.AddWithValue(command, "@VendorDueDiligenceId", vendorDueDiligence.VendorDueDiligenceId);
                    command.Parameters.AddWithValue(command, "@DueDiligenceDesignId", vendorDueDiligence.DesignId);
                    command.Parameters.AddWithValue(command, "@VendorId", vendorDueDiligence.VendorId);
                    command.Parameters.AddWithValue(command, "@TextboxValue", vendorDueDiligence.TextboxValue);
                    command.Parameters.AddWithValue(command, "@TextareaValue", vendorDueDiligence.TextareaValue);
                    command.Parameters.AddWithValue(command, "@CheckboxValue", vendorDueDiligence.CheckboxValue);
                    command.Parameters.AddWithValue(command, "@RadioboxValue", vendorDueDiligence.RadioboxValue);
                    command.Parameters.AddWithValue(command, "@IntValue", vendorDueDiligence.IntValue);
                    command.Parameters.AddWithValue(command, "@DecimalValue", vendorDueDiligence.DecimalValue);
                    command.Parameters.AddWithValue(command, "@DateTimeValue", vendorDueDiligence.DateTimeValue);
                    command.Parameters.AddWithValue(command, "@AgreementValue", vendorDueDiligence.AgreementValue);
                    command.Parameters.AddWithValue(command, "@Scoring", vendorDueDiligence.Scoring);
                    command.Parameters.AddWithValue(command, "@BankListValue", vendorDueDiligence.BankListValue);


                    return await command.ExecuteNonQueryAsync() > 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }


        public async Task<bool> AddDueAsync(VendorDueDiligenceModel model)
        {
            return await ModifyDueDiligence(new()
            {
                VendorDueDiligenceId = 0,
                DesignId = model.DesignId,
                DateTimeValue = model.DateTimeValue,
                DecimalValue = model.DecimalValue,
                AgreementValue = model.AgreementValue,
                CheckboxValue = model.CheckboxValue,
                IntValue = model.IntValue,
                RadioboxValue = model.RadioboxValue,
                Scoring = model.Scoring,
                TextareaValue = model.TextareaValue,
                TextboxValue = model.TextboxValue,
                VendorId = model.VendorId,
                BankListValue = model.BankListValue,
            });
        }

        public async Task<bool> UpdateDueAsync(VendorDueDiligenceModel model)
            => await ModifyDueDiligence(model);


        public async Task<bool> DeleteDueAsync(int dueId)
            => await ModifyDueDiligence(new() { DesignId = dueId });



        public async Task<bool> AddDueDesignGrid(DueDiligenceGridModel gridModel)
        {
            return await ModifyDueGrid(new()
            {
                Id = 0,
                DueDesignId = gridModel.DueDesignId,
                Column1 = gridModel.Column1,
                Column2 = gridModel.Column2,
                Column3 = gridModel.Column3,
                Column4 = gridModel.Column4,
                Column5 = gridModel.Column5,
                VendorId = gridModel.VendorId,
            });
        }


        private async Task<bool> ModifyDueGrid(DueDiligenceGridModel gridModel)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_DueDiligenceGridData_IUD @DueDiligenceGridDataId,@DueDiligenceDesignId,
                                                                                         @Column1,@Column2,@Column3,@Column4,@Column5,@VendorId";

                command.Parameters.AddWithValue(command, "@DueDiligenceGridDataId", gridModel.Id);
                command.Parameters.AddWithValue(command, "@DueDiligenceDesignId", gridModel.DueDesignId);
                command.Parameters.AddWithValue(command, "@Column1", gridModel.Column1);
                command.Parameters.AddWithValue(command, "@Column2", gridModel.Column2);
                command.Parameters.AddWithValue(command, "@Column3", gridModel.Column3);
                command.Parameters.AddWithValue(command, "@Column4", gridModel.Column4);
                command.Parameters.AddWithValue(command, "@Column5", gridModel.Column5);
                command.Parameters.AddWithValue(command, "@VendorId", gridModel.VendorId);


                bool result = await command.ExecuteNonQueryAsync() > 0;
                return result;
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

        public async Task<List<DueDiligenceGrid>> GetDueDiligenceGridAsync(int dueDesignId, int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_DueDiligenceGridData_Load @dueDesignId,@vendorId";
                command.Parameters.AddWithValue(command, "@dueDesignId", dueDesignId);
                command.Parameters.AddWithValue(command, "@vendorId", vendorId);

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

        public async Task<List<DeliveryTerms>> GetDeliveryTermsAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_DeliveryTerms_List";

                List<DeliveryTerms> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<DeliveryTerms>());

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

        public async Task<List<VendorBankDetail>> GetVendorBankDetailsAsync(int vendorid)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorBank_Load @vendorId";
                command.Parameters.AddWithValue(command, "@vendorId", vendorid);

                List<VendorBankDetail> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<VendorBankDetail>());

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
                Weight = reader.Get<decimal>("Weight"),
                Disabled = reader.Get<bool>("Disabled")
            };
        }




        public async Task<CompanyInfo> GetCompanyInfoAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_GET_VENDOR @VENDOR_ID";
                command.Parameters.AddWithValue(command, "@VENDOR_ID", vendorId);

                CompanyInfo result = new();
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    result = reader.GetByEntityStructure<CompanyInfo>();

                return result;
            }
        }

        public async Task<List<VendorNDA>> GetNDAAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorNDA_load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<VendorNDA> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<VendorNDA>());

                return result;
            }
        }

        public async Task<bool> AddNDAAsync(VendorNDA ndas)
        {
            return await ModifyNDA(new()
            {
                VendorNDAId = ndas.VendorNDAId,
                VendorId = ndas.VendorId,
                BusinessUnitId = ndas.BusinessUnitId
            });
        }


        public async Task<bool> DeleteNDAAsync(int vendorId)
            => await ModifyNDA(new() { VendorId = vendorId});


        private async Task<bool> ModifyNDA(VendorNDA nda)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorNDA_ID @VendorNDAId,
                                                             @VendorId,
                                                             @BusinessUnitId";


                command.Parameters.AddWithValue(command, "@VendorNDAId", nda.VendorNDAId);
                command.Parameters.AddWithValue(command, "@VendorId", nda.VendorId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", nda.BusinessUnitId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> AddCOBCAsync(VendorCOBC cobc)
        {
            return await ModifyCOBC(new()
            {
                VendorCOBCId = cobc.VendorCOBCId,
                VendorId = cobc.VendorId,
                BusinessUnitId = cobc.BusinessUnitId
            });
        }

        public async Task<bool> DeleteCOBCAsync(int id)
            => await ModifyCOBC(new() { VendorId = id });


        private async Task<bool> ModifyCOBC(VendorCOBC cobc)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorCOBC_ID @VendorCOBCId,
                                                              @VendorId,
                                                              @BusinessUnitId";


                command.Parameters.AddWithValue(command, "@VendorCOBCId", cobc.VendorCOBCId);
                command.Parameters.AddWithValue(command, "@VendorId", cobc.VendorId);
                command.Parameters.AddWithValue(command, "@BusinessUnitId", cobc.BusinessUnitId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateDueDesignGrid(DueDiligenceGridModel gridModel)
            => await ModifyDueGrid(gridModel);

        public async Task<bool> DeleteDueDesignGrid(int id)
            => await ModifyDueGrid(new() { Id = id });

        public async Task<List<VendorCOBC>> GetCOBCAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorCOBC_load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<VendorCOBC> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<VendorCOBC>());

                return result;
            }
        }

        public async Task<Prequalification> GetPrequalificationAsync(int vendorid)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorPrequalification_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorid);

                Prequalification result = new();
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    result = reader.GetByEntityStructure<Prequalification>();

                return result;
            }
        }

        public async Task<List<Application.Entities.SupplierEvaluation.VendorPrequalification>> GetVendorPrequalificationAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorPrequalificationCategory_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<Application.Entities.SupplierEvaluation.VendorPrequalification> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<Application.Entities.SupplierEvaluation.VendorPrequalification>());

                return result;
            }
        }

        public async Task<List<VendorBuCategory>> GetVendorBuCategoriesAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorBusinessCategory_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<VendorBuCategory> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<VendorBuCategory>());

                return result;
            }

        }

        public async Task<List<DueDiligenceValue>> GetVendorDuesAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorDueDiligence_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<DueDiligenceValue> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<DueDiligenceValue>());

                return result;
            }
        }

        public async Task<List<VendorProductService>> GetVendorProductServices(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorProductServices_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<VendorProductService> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<VendorProductService>());

                return result;
            }
        }

        public async Task<List<PrequalificationDesign>> GetPrequalificationDesignsAsync(int categoryId, Language language)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_PrequalificationDesign_Load @CategoryId,@LanguageCode";

                command.Parameters.AddWithValue(command, "@CategoryId", categoryId);
                command.Parameters.AddWithValue(command, "@LanguageCode", language.ToString());

                List<PrequalificationDesign> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<PrequalificationDesign>());

                return result;
            }
        }

        public async Task<List<VendorPrequalificationValues>> GetPrequalificationValuesAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorPrequalification_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<VendorPrequalificationValues> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<VendorPrequalificationValues>());

                return result;
            }
        }

        public async Task<List<Application.Entities.SupplierEvaluation.PrequalificationGridData>> GetPrequalificationGridAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_PrequalificationAllGridData_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<Application.Entities.SupplierEvaluation.PrequalificationGridData> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<Application.Entities.SupplierEvaluation.PrequalificationGridData>());

                return result;
            }
        }

        public async Task<bool> AddPrequalification(VendorPrequalificationValues value)
        {
            return await ModifyPrequalificationAsync(new()
            {
                VendorPrequalificationId = 0,
                PrequalificationDesignId = value.PrequalificationDesignId,
                VendorId = value.VendorId,
                TextareaValue = value.TextareaValue,
                TextboxValue = value.TextboxValue,
                CheckboxValue = value.CheckboxValue,
                RadioboxValue = value.RadioboxValue,
                IntValue = value.IntValue,
                DecimalValue = value.DecimalValue,
                DateTimeValue = value.DateTimeValue,
                Scoring = value.Scoring,
            });
        }

        public Task<bool> UpdatePrequalification(VendorPrequalificationValues value)
            => ModifyPrequalificationAsync(value);

        public Task<bool> DeletePrequalification(int vendorPreId)
            => ModifyPrequalificationAsync(new() { VendorPrequalificationId = vendorPreId });

        private async Task<bool> ModifyPrequalificationAsync(VendorPrequalificationValues values)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorPrequalification_IUD @VendorPrequalificationId,
                                                                           @PrequalificationDesignId,
                                                                           @VendorId,
                                                                           @TextboxValue,
                                                                           @TextareaValue,
                                                                           @CheckboxValue,
                                                                           @RadioboxValue,
                                                                           @IntValue,
                                                                           @DecimalValue,
                                                                           @DateTimeValue,
                                                                           @Scoring";


                command.Parameters.AddWithValue(command, "@VendorPrequalificationId", values.VendorPrequalificationId);
                command.Parameters.AddWithValue(command, "@PrequalificationDesignId", values.PrequalificationDesignId);
                command.Parameters.AddWithValue(command, "@VendorId", values.VendorId);
                command.Parameters.AddWithValue(command, "@TextboxValue", values.TextboxValue);
                command.Parameters.AddWithValue(command, "@TextareaValue", values.TextareaValue);
                command.Parameters.AddWithValue(command, "@CheckboxValue", values.CheckboxValue);
                command.Parameters.AddWithValue(command, "@RadioboxValue", values.RadioboxValue);
                command.Parameters.AddWithValue(command, "@IntValue", values.IntValue);
                command.Parameters.AddWithValue(command, "@DecimalValue", values.DecimalValue);
                command.Parameters.AddWithValue(command, "@DateTimeValue", values.DateTimeValue);
                command.Parameters.AddWithValue(command, "@Scoring", values.Scoring);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> AddPreGridAsync(Application.Entities.SupplierEvaluation.PrequalificationGridData grid)
        {
            return await ModifyPreGridAsync(new Application.Entities.SupplierEvaluation.PrequalificationGridData()
            {
                PreqqualificationGridDataId = 0,
                PreqqualificationDesignId = grid.PreqqualificationDesignId,
                Column1 = grid.Column1,
                Column2 = grid.Column2,
                Column3 = grid.Column3,
                Column4 = grid.Column4,
                Column5 = grid.Column5,
            });
        }

        public async Task<bool> UpdatePreGridAsync(Application.Entities.SupplierEvaluation.PrequalificationGridData grid)
            => await ModifyPreGridAsync(grid);


        public async Task<bool> DeletePreGridAsync(int preGridId)
            => await ModifyPreGridAsync(new() { PreqqualificationGridDataId = preGridId });


        public async Task<bool> ModifyPreGridAsync(Application.Entities.SupplierEvaluation.PrequalificationGridData gridData)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_PrequalificationGridData_IUD @PreqqualificationGridDataId,
                                                                             @PreqqualificationDesignId,
                                                                             @Column1,
                                                                             @Column2,
                                                                             @Column3,
                                                                             @Column4,
                                                                             @Column5,
                                                                             @VendorId";



                command.Parameters.AddWithValue(command, "@PreqqualificationGridDataId", gridData.PreqqualificationGridDataId);
                command.Parameters.AddWithValue(command, "@PreqqualificationDesignId", gridData.PreqqualificationDesignId);
                command.Parameters.AddWithValue(command, "@Column1", gridData.Column1);
                command.Parameters.AddWithValue(command, "@Column2", gridData.Column2);
                command.Parameters.AddWithValue(command, "@Column3", gridData.Column3);
                command.Parameters.AddWithValue(command, "@Column4", gridData.Column4);
                command.Parameters.AddWithValue(command, "@Column5", gridData.Column5);
                command.Parameters.AddWithValue(command, "@VendorId", gridData.VendorId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> AddPrequalificationCategoryAsync(PrequalificationCategoryData data)
             => await ModifyPrequalificationCategorySaveAsync(data);

        public async Task<bool> DeletePrequalificationCategoryAsync(int vendorId)
            => await ModifyPrequalificationCategorySaveAsync(new PrequalificationCategoryData { VendorId = vendorId });

        public async Task<bool> ModifyPrequalificationCategorySaveAsync(PrequalificationCategoryData data)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorPrequalificationCategory_ID @VendorId,
                                                                                  @PrequalificationCategoryId";



                command.Parameters.AddWithValue(command, "@VendorId", data.VendorId);
                command.Parameters.AddWithValue(command, "@PrequalificationCategoryId", data.PrequalificationCategoryId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> AddVendorBusinessCategoryAsync(VendorBusinessCategoryData data)
            => await ModifyVendorBusinessCategorySaveAsync(data);

        public async Task<bool> DeleteVendorBusinessCategoryAsync(int vendorId)
            => await ModifyVendorBusinessCategorySaveAsync(new VendorBusinessCategoryData { VendorId = vendorId });

        public async Task<bool> ModifyVendorBusinessCategorySaveAsync(VendorBusinessCategoryData data)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorBusinessCategory_ID @VendorId,
                                                                          @BusinessCategoryId";



                command.Parameters.AddWithValue(command, "@VendorId", data.VendorId);
                command.Parameters.AddWithValue(command, "@BusinessCategoryId", data.BusinessCategoryId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> AddRepresentedCompany(Application.Models.VendorRepresentedCompany data)
            => await ModifyRepresentedCategorySaveAsync(data);

        public async Task<bool> DeleteRepresentedCompanyAsync(int vendorId)
            => await ModifyRepresentedCategorySaveAsync(new Application.Models.VendorRepresentedCompany { VendorId = vendorId });

        public async Task<bool> ModifyRepresentedCategorySaveAsync(Application.Models.VendorRepresentedCompany data)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorRepresentedCompany_ID @VendorId,
                                                                            @RepresentedCompanyName";



                command.Parameters.AddWithValue(command, "@VendorId", data.VendorId);
                command.Parameters.AddWithValue(command, "@RepresentedCompanyName", data.RepresentedCompanyName);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> AddRepresentedProductAsync(RepresentedProductData data)
             => await ModifyRepresentedProductSaveAsync(data);

        public async Task<bool> DeleteRepresentedProductAsync(int vendorId)
            => await ModifyRepresentedProductSaveAsync(new RepresentedProductData { VendorId = vendorId });

        public async Task<bool> ModifyRepresentedProductSaveAsync(RepresentedProductData data)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorRepresentedProducts_ID @VendorId,
                                                                            @RepresentedProductName";



                command.Parameters.AddWithValue(command, "@VendorId", data.VendorId);
                command.Parameters.AddWithValue(command, "@RepresentedProductName", data.RepresentedProductName);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> AddProductServiceAsync(ProductServiceData productService)
          => await ModifyProductServiceAsync(productService);

        public async Task<bool> DeleteProductServiceAsync(int id)
          => await ModifyProductServiceAsync(new ProductServiceData { VendorId = id });

        public async Task<bool> ModifyProductServiceAsync(ProductServiceData data)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorProductServices_ID @VendorId,
                                                                            @ProductServiceId";



                command.Parameters.AddWithValue(command, "@VendorId", data.VendorId);
                command.Parameters.AddWithValue(command, "@ProductServiceId", data.ProductServiceId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public Task<bool> AddPequalificationCategoryAsync(PrequalificationCategory category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePequalificationCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ModifyPrequalificationCategoryAsync(PrequalificationCategory data)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_VendorPrequalificationCategory_ID @VendorId,
                                                                            @PrequalificationCategoryId";



                command.Parameters.AddWithValue(command, "@VendorId", data.Id);
                command.Parameters.AddWithValue(command, "@PrequalificationCategoryId", data.Id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<Application.Entities.Vendors.VendorRepresentedProduct> GetRepresentedProductAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorRepresentedProducts_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                Application.Entities.Vendors.VendorRepresentedProduct product = null;
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    product = reader.GetByEntityStructure<Application.Entities.Vendors.VendorRepresentedProduct>();

                return product;
            }
        }

        public async Task<Application.Entities.Vendors.VendorRepresentedCompany> GetRepresentedCompanyAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorRepresentedCompany_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                Application.Entities.Vendors.VendorRepresentedCompany company = null;
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    company = reader.GetByEntityStructure<Application.Entities.Vendors.VendorRepresentedCompany>();

                return company;
            }
        }


        public async Task<List<Application.Entities.Vendors.VendorUser>> GetVendorUsers(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorUsers_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<VendorUser> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<VendorUser>());

                return result;
            }
        }

        public async Task<List<Score>> Scores(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorScoring_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<Score> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<Score>());

                return result;
            }
        }

        public async Task<List<Shipment>> Shipments()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_Shipment_List";

                List<Shipment> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<Shipment>());

                return resultList;
            }
        }

        public async Task<List<WithHoldingTaxData>> WithHoldingTaxDatas()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_WithHoldingTax_List";

                List<WithHoldingTaxData> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<WithHoldingTaxData>());

                return resultList;
            }
        }

        public async Task<List<TaxData>> TaxDatas()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT * FROM VW_Tax_List";

                List<TaxData> resultList = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    resultList.Add(reader.GetByEntityStructure<TaxData>());

                return resultList;
            }
        }

        public async Task<bool> UpdateVendor(int userId, string taxId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_UpdateVendorByUser @TaxId,
                                                                            @UserId";



                command.Parameters.AddWithValue(command, "@TaxId", taxId);
                command.Parameters.AddWithValue(command, "@UserId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
