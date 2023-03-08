using SolaERP.Business.Dtos.EntityDtos.UserMenu;

namespace SolaERP.Business.Dtos.Wrappers
{
    public class GroupMenuExternalDataWrapper
    {
        public List<UserMenu_Load> MenuItems { get; set; }
        public MenuGroupWRP SelectedMenuList { get; set; }
    }
}
