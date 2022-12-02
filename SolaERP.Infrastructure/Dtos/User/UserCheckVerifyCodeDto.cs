using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.User
{
    public class UserCheckVerifyCodeDto
    {
        public string Email { get; set; }
        public string VerifyCode { get; set; }
    }
}
