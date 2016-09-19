using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMMClient
{
    public static class StringExtender
    {
        public static bool HasText(this String str)
        {
            return (str == null ? false : !str.Trim().Equals(String.Empty));
        }
    }
}
