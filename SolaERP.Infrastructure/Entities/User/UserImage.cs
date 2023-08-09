using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.User
{
    public class UserImage : BaseEntity
    {
        public string UserPhoto { get; set; }
        public string SignaturePhoto { get; set; }
    }
}
