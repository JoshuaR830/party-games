using System.Xml.Linq;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public interface IFileHelper
    {
        Dictionary ReadDictionary();
    }
}