namespace SolaERP.Persistence.TemporaryModel
{
    public class VM_RegistrationPending : VM_EmailTemplateBase
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string TemplateName()
        {
            return @"RegistrationPendingTemp.cshtml";
        }

        public string ImageName()
        {
            return @"registrationPending.png";
        }
    }
}
