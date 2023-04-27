namespace SolaERP.Application.Dtos.Menu
{
    public class GroupMenuResponseDto
    {
        public List<MenuWithPrivilagesDto> Menus { get; set; } = new();
        public GroupMenuWithPrivillageIdListDto PrivillageList { get; set; } = new();
    }
}
