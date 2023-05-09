namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_RegistrationPending : VM_EmailTemplateBase
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string TemplateName()
        {
            return @"RegistrationPending.cshtml";
        }
    }
}
