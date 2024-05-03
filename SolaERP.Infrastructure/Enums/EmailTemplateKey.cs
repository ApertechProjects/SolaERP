namespace SolaERP.Application.Enums
{
    public enum EmailTemplateKey
    {
        VER,  //verification Email
        RGA, //Registration Pending For Approval for single user
        RP,  //Registration Pending for group
        REQP, //Request is pending
        REQA, //Request is approved
        REQR, //Request is rejected
        REQH, //Request is held
        UP, //User is pending
        UA, //User Approved
        UR, //User Reject
        UH //User Held
    }


    public enum EmailTemplate
    {
        Verification,  //verification Email
        RegistrationPendingForApproval, //Registration Pending For Approval for single user
        RequestPending, //Request is pending
        RequestApproved, //Request is approved
        RequestRejected, //Request is rejected
        RequestHeld, //Request is held
        UserPending, //User is pending
        UserApproved, //User Approved
        UserRejected, //User Reject
        UserHeld //User Held
    }
}
