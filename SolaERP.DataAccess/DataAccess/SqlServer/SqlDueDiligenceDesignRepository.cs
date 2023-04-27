using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.Attachment;
using SolaERP.Application.Entities.DueDiligenceDesign;
using SolaERP.Application.Entities.VendorDueDiligence;
using SolaERP.Application.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlDueDiligenceDesignRepository : IDueDiligenceDesignRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlDueDiligenceDesignRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AddAsync(DueDiligenceDesign entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DueDiligenceDesign>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC dbo.SP_DueDiligenceDesign_Load";
                using var reader = await command.ExecuteReaderAsync();
                List<DueDiligenceDesign> dueDiligenceDesigns = new List<DueDiligenceDesign>();
                while (reader.Read())
                {
                    dueDiligenceDesigns.Add(GetFromReader(reader));
                }
                return dueDiligenceDesigns;
            }
        }

        public Task<DueDiligenceDesign> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DueDiligenceDesign entity)
        {
            throw new NotImplementedException();
        }

        private DueDiligenceDesign GetFromReader(IDataReader reader)
        {
            return new DueDiligenceDesign
            {
                AgreementText = reader.Get<string>("AgreementText"),
                GridColumnCount = reader.Get<int>("GridColumnCount"),
                GridRowLimit = reader.Get<int>("GridRowLimit"),
                HasAgreement = reader.Get<bool>("HasAgreement"),
                HasAttachment = reader.Get<bool>("HasAttachment"),
                HasBankList = reader.Get<bool>("HasBankList"),
                HasCheckbox = reader.Get<bool>("HasCheckBox"),
                HasDateTime = reader.Get<bool>("HasDateTime"),
                HasDecimal = reader.Get<bool>("HasDecimal"),
                HasGrid = reader.Get<bool>("HasGrid"),
                HasInt = reader.Get<bool>("HasInt"),
                HasRadiobox = reader.Get<bool>("HasRadioBox"),
                HasTextarea = reader.Get<bool>("HasTextarea"),
                HasTextbox = reader.Get<bool>("HasTextbox"),
                Label = reader.Get<string>("Label"),
                LineNo = reader.Get<int>("LineNo"),
                ParentCompanies = reader.Get<bool>("ParentCompanies"),
                Title = reader.Get<string>("Title"),
                Weight = reader.Get<int>("Weight"),
                DueDiligenceDesignId = reader.Get<int>("DueDiligenceDesignId"),
                Attachments = new Attachment
                {
                    AttachmentId = reader.Get<int>("AttachmentId"),
                    AttachmentSubTypeId = reader.Get<int>("AttachmentSubTypeId"),
                    AttachmentTypeId = reader.Get<int>("AttachmentTypeId"),
                    ExtensionType = reader.Get<string>("ExtensionType"),
                    FileData = reader.Get<byte[]>("FileData"),
                    FileName = reader.Get<string>("FileName"),
                    Reference = reader.Get<string>("Reference"),
                    Size = reader.Get<int>("Size"),
                    SourceId = reader.Get<int>("SourceId"),
                    SourceTypeId = reader.Get<int>("SourceTypeId"),
                    UploadDateTime = reader.Get<DateTime>("UploadDateTime")
                },
                VendorDueDiligence = new VendorDueDiligence
                {
                    AgreementValue = reader.Get<bool>("AgreementValue"),
                    CheckboxValue = reader.Get<bool>("CheckboxValue"),
                    DateTimeValue = reader.Get<DateTime>("DateTimeValue"),
                    DecimalValue = reader.Get<decimal>("DecimalValue"),
                    DueDiligenceDesignId = reader.Get<int>("DueDiligenceDesignId"),
                    IntValue = reader.Get<int>("IntValue"),
                    RadioboxValue = reader.Get<bool>("RadioboxValue"),
                    Scoring = reader.Get<decimal>("Scoring"),
                    TextareaValue = reader.Get<string>("TextareaValue"),
                    TextboxValue = reader.Get<string>("TextboxValue"),
                    VendorDueDiligenceId = reader.Get<int>("VendorDueDiligenceId"),
                    VendorId = reader.Get<int>("VendorId")
                }
            };
        }
    }
}
