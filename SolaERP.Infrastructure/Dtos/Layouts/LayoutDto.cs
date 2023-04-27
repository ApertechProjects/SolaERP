using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace SolaERP.Application.Dtos.Layout
{
    public class LayoutDto
    {
        public string Key { get; set; }
        public string Filebase64 { get; set; }
        [JsonIgnore]
        public byte[] Layout
        {
            get => Encoding.UTF8.GetBytes(Filebase64);
            set
            {
                this.Filebase64 = Convert.ToBase64String(value);
            }
        }
        public int TabIndex { get; set; }
    }
}
