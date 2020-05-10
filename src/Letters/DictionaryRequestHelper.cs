using System;
using System.IO;
using System.Net;
using System.Text;

namespace Chat.Letters
{
    public class DictionaryRequestHelper
    {
        public bool MakeWebRequest(string url)
        {
            try {
                var request = (HttpWebRequest)WebRequest.Create($"https://www.dictionary.com/browse/{url}");
                request.Method = "HEAD";
                var response = (HttpWebResponse)request.GetResponse();
                return true;
            } catch (WebException e) {
                System.Console.WriteLine(e);
                return false;
            } catch
            {
                return false;
            }
        }

        public string MakeDefinitionRequest(string url)
        {
            try {
                var request = (HttpWebRequest)WebRequest.Create($"https://www.collinsdictionary.com/dictionary/english/{url}");

                var responseText = "";

                
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    var encoding = Encoding.GetEncoding(response.CharacterSet);
                    
                    using (var responseStream = response.GetResponseStream())
                    using (var reader = new StreamReader(responseStream, encoding))
                        responseText = reader.ReadToEnd();
                }
                
                Console.WriteLine("Definition: ");
                Console.WriteLine(responseText);
                return responseText;
                
            } catch (Exception e) {
                System.Console.WriteLine(e);
                return "";
            }
        }

        public bool IsPluralReal(string word, string shortenedWord)
        {
            var textContent = this.MakeDefinitionRequest(shortenedWord);
            if (textContent.ToLower().Contains("word forms"))
            {
                Console.WriteLine("Contains word forms");
                return textContent.Contains(word);
            }

            return false;
        }
    }
}