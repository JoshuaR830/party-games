using System.Net;

namespace Chat.WordGame.WebHelpers
{
    public interface IWebRequestHelper
    {
        WebRequest Create(string url);
        HttpWebResponse GetResponse(WebRequest request);
    }
}