using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.User
{
    public class UserUpdatePasswordDto
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
