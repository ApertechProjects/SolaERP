namespace SolaERP.Business.Dtos.Wrappers
{
    public class MenuGroupWRP
    {
        public MenuGroupWRP()
        {
            Create = new();
            Edit = new();
            Delete = new();
            Export = new();
        }
        public List<int> Create { get; set; }
        public List<int> Edit { get; set; }
        public List<int> Delete { get; set; }
        public List<int> Export { get; set; }


        public List<int> GetMenuIds()
        {
            return (Create.Union(Edit).ToList().Union(Delete).ToList().Union(Export)).ToList();
        }
    }
}
