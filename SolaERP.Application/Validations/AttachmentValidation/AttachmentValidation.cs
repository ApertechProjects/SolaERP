using FluentValidation;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Validations.AttachmentValidation
{
    public class AttachmentValidation : AbstractValidator<AttachmentSaveModel>
    {
        public AttachmentValidation()
        {
            RuleFor(x => x.AttachmentSubTypeId).NotEmpty().WithMessage("{PropertyName}: Please,enter {PropertyName}");
            RuleFor(x => x.SourceId).NotEmpty().WithMessage("{PropertyName}: Please,enter {PropertyName}");
            RuleFor(x => x.Reference).NotEmpty().WithMessage("{PropertyName}: Please,enter {PropertyName}");
            RuleFor(x => x.FileData).NotEmpty().WithMessage("{PropertyName}: Please,enter {PropertyName}");
            RuleFor(x => x.FileName).NotEmpty().WithMessage("{PropertyName}: Please,enter {PropertyName}");
            RuleFor(x => x.ExtensionType).NotEmpty().WithMessage("{PropertyName}: Please,enter {PropertyName}");
            RuleFor(x => x.AttachmentTypeId).NotEmpty().WithMessage("{PropertyName}: Please,enter {PropertyName}");
            RuleFor(x => x.SourceType).NotEmpty().WithMessage("{PropertyName}: Please,enter {PropertyName}");
        }

    }
}
