using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace MISA.Web02.Core.Utilities
{
    // Hàm tiện ích JSON
    public static class JsonExtensions
    {
        // Truyển đổi các trường trong array > CamelCase 
        public static void Capitalize(this JArray jArr)
        {
            foreach (var x in jArr.Cast<JToken>().ToList())
            {
                var childObj = x as JObject;
                if (childObj != null)
                {
                    childObj.Capitalize();
                    continue;
                }
                var childArr = x as JArray;
                if (childArr != null)
                {
                    childArr.Capitalize();
                    continue;
                }
            }
        }
        // Truyển đổi các trường trong object > CamelCase 
        public static void Capitalize(this JObject jObj)
        {

            foreach (var kvp in jObj.Cast<KeyValuePair<string, JToken>>().ToList())
            {
                jObj.Remove(kvp.Key);
                var newKey = kvp.Key.Capitalize();
                var childObj = kvp.Value as JObject;
                if (childObj != null)
                {
                    childObj.Capitalize();
                    jObj.Add(newKey, childObj);
                    return;
                }
                var childArr = kvp.Value as JArray;
                if (childArr != null)
                {
                    childArr.Capitalize();
                    jObj.Add(newKey, childArr);
                    return;
                }
                jObj.Add(newKey, kvp.Value);
            }
        }

        // Hàm truyển đổi dạng CamelCase 
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("empty string");
            }

            string result = Regex.Replace(str, "_[a-z]", delegate (Match m)
            {
                return m.ToString().TrimStart('_').ToUpper();
            });

            result = char.ToUpper(result[0]) + result.Substring(1);

            return result;
        }
    }
}
