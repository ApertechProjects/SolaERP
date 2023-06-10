using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlSupplierEvaluationRepository : ISupplierEvaluationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlSupplierEvaluationRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        private async Task<bool> ModifyDueDiligence(VendorDueDiligenceModel vendorDueDiligence)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorDueDiligence_IUD @VendorDueDiligenceId,
                                                                       @DueDiligenceDesignId,@VendorId
                                                                      ,@TextboxValue,@TextareaValue
                                                                      ,@CheckboxValue,@RadioboxValue
                                                                      ,@IntValue,@DecimalValue
                                                                      ,@DateTimeValue,@AgreementValue";

                command.Parameters.AddWithValue(command, "@VendorDueDiligenceId", vendorDueDiligence.VendorDueDiligenceId);
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


        public async Task<bool> AddDueDesignAsync(VendorDueDiligenceModel model)
        {
            return await ModifyDueDiligence(new()
            {
                VendorDueDiligenceId = 0,
                DueDiligenceDesignId = model.DueDiligenceDesignId,
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
            });
        }

        public async Task<bool> UpdateDueDesignAsync(VendorDueDiligenceModel model)
            => await ModifyDueDiligence(model);


        public async Task<bool> DeleteDueDesignAsync(int dueId)
            => await ModifyDueDiligence(new() { DueDiligenceDesignId = dueId });



        public async Task<bool> AddDueDesignGrid(DueDiligenceGridModel gridModel)
        {
            return await ModifyDueGrid(new()
            {
                DueDesignId = 0,
                Column1 = gridModel.Column1,
                Column2 = gridModel.Column2,
                Column3 = gridModel.Column3,
                Column4 = gridModel.Column4,
                Column5 = gridModel.Column5,
            });
        }


        private async Task<bool> ModifyDueGrid(DueDiligenceGridModel gridModel)
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

        public async Task<List<DueDiligenceGrid>> GetDueDiligenceGridAsync(int dueDesignId)
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

        public async Task<List<VendorBankDetail>> GetVondorBankDetailsAsync(int vendorid)
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
            };
        }


        private async Task<bool> ModifyPrequalification(VendorPreInputModel model)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorPrequalification_IUD @VendorPrequalificationId,@PrequalificationDesignId
                                                                           @VendorId,@TextboxValue,@TextareaValue,@CheckboxValue,
                                                                           @RadioboxValue,@IntValue,@DecimalValue,@DateTimeValue,@Scoring";

                command.Parameters.AddWithValue(command, "@VendorPrequalificationId", model.PrequealificationId);
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


        public async Task<bool> DeleteNDAAsync(int ndaId)
            => await ModifyNDA(new() { VendorNDAId = ndaId });


        private async Task<bool> ModifyNDA(VendorNDA nda)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorNDA_ID @VendorNDAId,
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
            => await ModifyCOBC(new() { VendorCOBCId = id });


        private async Task<bool> ModifyCOBC(VendorCOBC cobc)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"EXEC SP_VendorCOBC_ID @VendorCOBCId,
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

        public async Task<List<VendorPrequalification>> GetVendorPrequalificationAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorPrequalificationCategory_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<VendorPrequalification> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<VendorPrequalification>());

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

        public async Task<List<VendorDueDiligence>> GetVendorDuesAsync(int vendorId)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_VendorDueDiligence_Load @VendorId";
                command.Parameters.AddWithValue(command, "@VendorId", vendorId);

                List<VendorDueDiligence> result = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    result.Add(reader.GetByEntityStructure<VendorDueDiligence>());

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
    }
}
