using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accenture.Commun.TextHelper
{
    public static class StringCompare
    {
        public static bool CompareString(string txt1, string text2)
        {
            return (txt1?.Trim().ToLower() == text2?.Trim().ToLower());
        }

        public static bool ContainsString(string txt, string contains)
        {
            return (txt?.Trim().ToLower().Contains(contains?.Trim().ToLower())).Value;
        }
    }
}
