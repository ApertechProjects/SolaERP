using FluentValidation;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Validations.AttachmentValidation
{
    public class AttachmentValidation : AbstractValidator<AttachmentSaveModel>
    {
        public AttachmentValidation()
        {
            RuleFor(x => x.SourceId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Filebase64).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.FileName).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ExtensionType).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.SourceType).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }

    }
}
