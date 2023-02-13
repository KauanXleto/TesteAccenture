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
            if (!string.IsNullOrWhiteSpace(txt1) && !string.IsNullOrWhiteSpace(text2))
                return (txt1?.Trim().ToLower() == text2?.Trim().ToLower());
            else
                return false;
        }

        public static bool ContainsString(string txt, string contains)
        {
            if (!string.IsNullOrWhiteSpace(txt))
                return (txt?.Trim().ToLower().Contains(contains?.Trim().ToLower())).Value;
            else
                return false;
        }
    }
}
