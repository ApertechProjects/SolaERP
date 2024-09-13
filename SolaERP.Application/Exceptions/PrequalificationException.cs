using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Exceptions
{
    public class PrequalificationException : Exception
    {
        public PrequalificationException(string message) : base(message)
        {

        }
    }
}
