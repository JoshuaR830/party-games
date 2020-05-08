namespace Chat.WordGame.WebHelpers
{
    public interface IWebDictionaryRequestHelper
    {
        bool MakeExistenceRequest(string word);
        string MakeContentRequest(string word);
    }
}