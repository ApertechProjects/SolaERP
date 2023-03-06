namespace SolaERP.Infrastructure.Dtos.Layout
{
    public class LayoutDto
    {
        public int UserLayoutId { get; set; }
        public string Key { get; set; }
        public byte[] Layout { get; set; }
        public int TabIndex { get; set; }
    }
}
