namespace SolaERP.Application.Dtos.Menu
{
    public class GroupMenuWithPrivillageIdListDto
    {
        public List<int> Create { get; set; } = new();
        public List<int> Edit { get; set; } = new();
        public List<int> Delete { get; set; } = new();
        public List<int> Export { get; set; } = new();
    }
}
