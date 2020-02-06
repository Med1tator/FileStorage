using System;
using System.Text;

namespace FileStorage.WebApi.Utilities
{
    public class StringUtilities
    {
        public static string GenerateString(int len, bool? containsNumber = true, bool? containsLowerCase = true, bool? containsUpperCase = true, bool? containsSpecialChar = true)
        {
            if (len <= 0)
                return string.Empty;

            var charsSb = new StringBuilder();
            if (containsNumber.GetValueOrDefault())
                charsSb.Append("0123456789");

            if (containsLowerCase.GetValueOrDefault())
                charsSb.Append("abcdefhijkmnprstwxyz");

            if (containsUpperCase.GetValueOrDefault())
                charsSb.Append("ABCDEFGHJKMNPQRSTWXYZ");

            if (containsSpecialChar.GetValueOrDefault())
                charsSb.Append("!@#$%*&_");

            var chars = charsSb.ToString();

            var rdm = new Random();
            var result = string.Empty;
            for (int i = 0; i < len; i++)
            {
                result += chars[rdm.Next(0, chars.Length)];
            }

            return result;
        }
    }
}
