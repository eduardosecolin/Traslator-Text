using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Traslator
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Hello World!";
            Console.WriteLine("Original Text: " + text);
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Traslated Text: " + Util.TraslateText(text));
            Console.ReadKey();
        }
    }

    public class Util
    {
        public static string TraslateText(string text)
        {
            string url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             "en", "pt-BR", Uri.EscapeUriString(text));
            HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;

            var jsonData = JsonConvert.DeserializeObject<List<dynamic>>(result);

            var translationItems = jsonData[0];

            string translation = "";

            foreach (object item in translationItems)
            {
                IEnumerable translationLineObject = item as IEnumerable;
                IEnumerator translationLineString = translationLineObject.GetEnumerator();
                translationLineString.MoveNext();
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }

            if (translation.Length > 1) { translation = translation.Substring(1); };

            return translation;
        }
    }
}
