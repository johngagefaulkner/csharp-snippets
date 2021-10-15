using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Snippets.Dictionaries;

namespace Snippets.Helpers
{
    public class StringAssistant
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
            {
                throw new ArgumentNullException(nameof(clearText));
            }

            var securePassword = new SecureString();

            foreach (var c in clearText)
            {
                securePassword.AppendChar(c);
            }

            securePassword.MakeReadOnly();
            return securePassword;
        }

        /// <summary>
        /// Splits a string into an array like the opposite of a StringBuilder.
        /// </summary>
        /// <param name="_inputStr">The string you want to split.</param>
        /// <returns>An array of strings.</returns>
        public static string[] SplitStringByNewLines(string _inputStr)
        {
            return _inputStr.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Splits a file path (absolute or relative) using the system's directory separator character.
        /// </summary>
        /// <param name="_inputPath">The file path you want to split.</param>
        /// <returns>An array of strings. For example, "C:\Users" would return "C:" and "Users".</returns>
        public static string[] SplitPathByDirectories(string _inputPath)
        {
            return _inputPath.Split(Path.DirectorySeparatorChar);
        }
    }
}
