using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.GridLayout
{
    public class GridLayout : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LayoutName { get; set; }
        public string LayoutData { get; set; }
    }
}
