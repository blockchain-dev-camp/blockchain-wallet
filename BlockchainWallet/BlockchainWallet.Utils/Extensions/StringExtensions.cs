using System.Text.RegularExpressions;

namespace BlockchainWallet.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveControllerSuffix(this string text)
        {
            text = Regex.Replace(text, "controller", "", RegexOptions.IgnoreCase);

            return text;
        }

        public static string RemovePointSeparator(this string str)
        {
            return str.Replace(".", "").Replace(",", "");
        }
    }
}
