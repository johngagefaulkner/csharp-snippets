using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snippets.Helpers
{
    public class Converters
    {
        public static string GetFilePathWithoutQuotes(string inputFilePath)
        {
            char quoteChar = '"';
            string rev1 = inputFilePath.Replace(quoteChar.ToString(), "'");
            string rev2 = "'" + rev1.Trim() + "'";
            return rev2.Trim();
        }
    }
}
