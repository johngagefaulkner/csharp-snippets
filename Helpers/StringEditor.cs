using System;
using Snippets.Dictionaries;

namespace Snippets.Helpers
{
    public class StringEditor
    {
        // Easily remove Quotation Marks (") and Apostrophes (') from a string.
        public static string RemoveQuotes(string inputStr, bool alsoRemoveApostrophes = false)
        {
            string _result = inputStr.Replace(Punctuation.GetString(Punctuation.PunctuationMark.QuotationMark), "");

            if (alsoRemoveApostrophes == true)
            {
                _result = _result.Replace(Punctuation.GetString(Punctuation.PunctuationMark.Apostrophe), "");
            }

            return _result.Trim();
        }
    }
}
