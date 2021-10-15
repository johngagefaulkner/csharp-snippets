using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Snippets.Helpers
{
    public class Converters
    {        
        // Reads JSON from the provided file path.
        // Converts the contents to the provided type.
        public static T ConvertFromJsonFile<T>(string json)
        {
            string tmpJson;
            using (StreamReader sr = new StreamReader(json))
            {
                tmpJson = sr.ReadToEnd();
            }

            return System.Text.Json.Deserialize<T>(tmpJson);
        }

        // Converts the provided string to the provided type.
        public static T ConvertFromJsonString<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }
}
