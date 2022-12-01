using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.User
{
    public class UserUpdatePasswordDto
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPasswordHash { get; set; }
    }
}
