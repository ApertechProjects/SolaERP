using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Services
{
    public interface IEmailService
    {
        bool ValidateEmail(string email);

    }
}
