using System.Net;

namespace Chat.Letters {
    public class WordValidationHelper
    {
        public bool ValidateWord(string word)
        {
            var statusHelper = new WordStatusHelper();
            return statusHelper.GetWordStatus(word);
        }

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
    }
}