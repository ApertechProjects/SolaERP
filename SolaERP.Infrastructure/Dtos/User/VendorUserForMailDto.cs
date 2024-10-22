using SolaERP.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.User
{
    public class VendorUserForMailDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
    }
}
