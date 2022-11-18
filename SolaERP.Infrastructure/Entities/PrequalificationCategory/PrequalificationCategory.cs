using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.PrequalificationCategory
{
    public class PrequalificationCategory : BaseEntity
    {
        public int PrequalificationCategoryId { get; set; }
        public string PrequalificationCategoryName { get; set; } //PrequalificationCategory db name

    }
}
