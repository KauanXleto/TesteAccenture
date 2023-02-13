using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Accenture.Commun.TextHelper
{
    public static class RegexValidator
    {
        public static Regex regexEmail = new Regex(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9_]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$");
        public static Regex regexPhone = new Regex(@"^\([1-9]{2}\) [2-9][0-9]{3}\-[0-9]{4,5}$");
        public static Regex regexCellPhone = new Regex(@"^(?:(?:\+|00)?(55)\s?)?(?:\(?([1-9][0-9])\)?\s?)?(?:((?:9\d|[6-9])\d{3})\-?(\d{4}))$");

        public static bool IsEmail(string text)
        {
            return regexEmail.IsMatch(text);
        }
        public static bool IsPhone(string text)
        {

            return (regexPhone.IsMatch(NormalizeString.StandardizeText(text)) || regexCellPhone.IsMatch(NormalizeString.StandardizeText(text)));
        }
    }
}
