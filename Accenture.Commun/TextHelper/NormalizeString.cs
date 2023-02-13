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
            if (!string.IsNullOrWhiteSpace(txt))
                return RemoveAccent(txt.Trim().ToLower());
            else
                return "";
        }
        public static string PrepareToSqlInsert(string txt)
        {
            if (!string.IsNullOrWhiteSpace(txt))
                return RemoveAccent(txt?.Trim().Replace("'", "''"));
            else
                return "";
        }
        public static string ConditionToSq(string txt)
        {
            if (!string.IsNullOrWhiteSpace(txt))
                return RemoveAccent(PrepareToSqlInsert(txt).Replace("[", "_").Replace("]", "_"));
            else
                return "";
        }
        public static string WhereLikeToSql(string txt)
        {
            if (!string.IsNullOrWhiteSpace(txt))
                return (!string.IsNullOrWhiteSpace(txt) ? string.Format("%{0}%", ConditionToSq(txt)) : "");
            else
                return "";
        }

        public static string RemoveAccent(string txt)
        {
            if (!string.IsNullOrWhiteSpace(txt))
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
            else
                return "";
        }
    }
}
