namespace Chat.WordGame.LocalDictionaryHelpers
{
    public interface ITemporaryDefinitionHelper
    {
        void AutomaticallySetTemporaryDefinitionForWord(string filename, string word, string temporaryDefinition);
    }
}