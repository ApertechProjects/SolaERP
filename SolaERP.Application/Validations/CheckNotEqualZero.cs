using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Validations
{
    public static class CheckNotEqualZero
    {
        public static bool NotEqualZero(List<int> ints)
        {
            return ints.All(i => i != 0);
        }
    }
}
