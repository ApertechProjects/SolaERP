namespace SolaERP.Infrastructure.Models
{
    public class GroupMenuPrivilegeListModel
    {

        //If user can do this operations for specific menus then add the menu id for its own privilege list
        public List<int> Create { get; set; } = new();
        public List<int> Edit { get; set; } = new();
        public List<int> Delete { get; set; } = new();
        public List<int> Export { get; set; } = new();

        public List<int> GetAllUnionMenuIds()
        {
            return (Create.Union(Edit).ToList().Union(Delete).ToList().Union(Export)).ToList();
        }
    }
}
