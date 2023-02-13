using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accenture.Commun.TextHelper
{
    public static class NormalizeString
    {
        public static string StandardizeText(string txt)
        {
            return RemoveAccent(txt.Trim().ToLower());
        }
        public static string PrepareToSqlInsert(string txt)
        {
            return RemoveAccent(txt.Trim().Replace("'","''"));
        }
        public static string ConditionToSq(string txt)
        {
            return RemoveAccent(txt.Trim().Replace("'","''").Replace("[", "_").Replace("]", "_"));
        }
        public static string WhereLikeToSq(string txt)
        {
            return (!string.IsNullOrWhiteSpace(txt) ? string.Format("%{0}%", ConditionToSq(txt)) : "");
        }

        public static string RemoveAccent(string txt)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = txt.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            return sbReturn.ToString();
        }
    }
}
