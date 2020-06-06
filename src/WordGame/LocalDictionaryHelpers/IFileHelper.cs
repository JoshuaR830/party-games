using System.Xml.Linq;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public interface IFileHelper
    {
        WordDictionary ReadDictionary(string filename);
        void WriteDictionary(string filename, WordDictionary wordDictionary);
        string ReadFile(string filename);
        void WriteFile(string filename, object data);
    }
}