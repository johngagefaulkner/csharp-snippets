using System;
using Snippets.Dictionaries;

namespace Snippets.Helpers
{
    public class StringEditor
    {
        public static string RemoveQuotes(string inputStr, bool alsoRemoveApostrophes = false)
        {
            string _result = inputStr.Replace(Punctuation.GetString(Punctuation.PunctuationMark.QuotationMark), "");

            if (alsoRemoveApostrophes == true)
            {
                _result = _result.Replace(Punctuation.GetString(Punctuation.PunctuationMark.Apostrophe), "");
            }

            return _result.Trim();
        }

        public static string AddQuotes(string inputStr, bool useSingleQuotes = false)
        {
            if (useSingleQuotes == true)
            {
                return "'" + inputStr.Trim() + "'";
            }

            else
            {
                string _str = Punctuation.PunctuationMark.QuotationMark.ToString() + inputStr.Trim() + Punctuation.PunctuationMark.QuotationMark.ToString();
                return _str.Trim();
            }
        }
        
        public static string ConvertFromSecureString(SecureString secureString)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public static SecureString ConvertToSecureString(string clearText)
        {
            if (clearText == null)
                throw new ArgumentNullException(nameof(clearText));

            var securePassword = new SecureString();

            foreach (var c in clearText)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}
