namespace Chat.WordGame.WebHelpers
{
    public class WordHelper : IWordHelper
    {
        private readonly IWebDictionaryRequestHelper _webDictionaryRequestHelper;

        public WordHelper(IWebDictionaryRequestHelper webDictionaryRequestHelper)
        {
            _webDictionaryRequestHelper = webDictionaryRequestHelper;
        }

        public bool CheckWordEndingExists(string word)
        {
            var responseText = _webDictionaryRequestHelper.MakeContentRequest(word);
            return responseText.Contains(word);
        }
    }
}