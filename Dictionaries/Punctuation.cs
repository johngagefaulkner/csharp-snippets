using System;
using System.Collections.Generic;

namespace Snippets.Dictionaries
{
    public class Punctuation
    {
        public enum PunctuationMark
        {
            Apostrophe = 0,
            Comma,
            ExclamationPoint,
            Hyphen,
            Period,
            QuotationMark,
            QuestionMark
        }

        private static char quoteChar = '"';
        private static Dictionary<PunctuationMark, string> InternalPunctuations = new()
        {
            { PunctuationMark.Apostrophe, "'" },
            { PunctuationMark.Comma, "," },
            { PunctuationMark.ExclamationPoint, "!" },
            { PunctuationMark.Hyphen, "-" },
            { PunctuationMark.Period, "." },
            { PunctuationMark.QuotationMark, quoteChar.ToString() },
            { PunctuationMark.QuestionMark, "?" }
        };

        public static char GetChar(PunctuationMark _mark)
        {
            var _char = Convert.ToChar(InternalPunctuations[_mark]);
            return _char;
        }

        public static string GetString(PunctuationMark _mark)
        {
            return InternalPunctuations[_mark];
        }
    }
}
