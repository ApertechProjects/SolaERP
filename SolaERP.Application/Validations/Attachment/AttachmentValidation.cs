using FluentValidation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Validations.AttachmentValidation
{
    public class AttachmentValidation : AbstractValidator<AttachmentSaveModel>
    {
        public AttachmentValidation()
        {
            RuleFor(x => x.SourceId).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.ExtensionType).NotEmpty().WithMessage("Please, enter {PropertyName}");
            RuleFor(x => x.SourceTypeId).NotEmpty().WithMessage("Please, enter {PropertyName}");
        }

    }
}
