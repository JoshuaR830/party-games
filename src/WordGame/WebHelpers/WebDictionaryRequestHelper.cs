using System;
using System.IO;
using System.Text;

namespace Chat.WordGame.WebHelpers
{
    public class WebDictionaryRequestHelper : IWebDictionaryRequestHelper
    {
        const string ExistenceRequestUrlBase = "https://www.dictionary.com/browse";
        const string ContentRequestUrlBase = "https://www.collinsdictionary.com/dictionary/english";
        readonly IWebRequestHelper _webRequestHelper;

        public WebDictionaryRequestHelper(IWebRequestHelper webRequestHelper)
        {
            _webRequestHelper = webRequestHelper;
        }

        public bool MakeExistenceRequest(string word)
        {
            var url = $"{ExistenceRequestUrlBase}/{word}";
            try
            {
                var request = _webRequestHelper.Create(url);
                _webRequestHelper.GetResponse(request);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public string MakeContentRequest(string word)
        {

            var url = $"{ContentRequestUrlBase}/{word}";
            try
            {
                var request = _webRequestHelper.Create(url);
                string responseText;
                
                using (var response = _webRequestHelper.GetResponse(request))
                {
                    var encoding = Encoding.UTF8;
                    
                    try 
                    {
                        encoding = Encoding.GetEncoding(response.CharacterSet);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to retrieve {e}");
                    }
                    
                    using (var responseStream = response.GetResponseStream())
                    using (var reader = new StreamReader(responseStream ?? throw new Exception(), encoding))
                        responseText = reader.ReadToEnd();
                }

                return responseText;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}