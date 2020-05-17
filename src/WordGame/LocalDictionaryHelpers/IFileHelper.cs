using System.Xml.Linq;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public interface IFileHelper
    {
        Dictionary ReadDictionary(string filename);
        void WriteDictionary(string filename, Dictionary dictionary);
        string ReadFile(string filename);
        void WriteFile(string filename, object data);
    }
}