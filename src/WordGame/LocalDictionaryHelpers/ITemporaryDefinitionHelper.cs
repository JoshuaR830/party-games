namespace Chat.WordGame.LocalDictionaryHelpers
{
    public interface ITemporaryDefinitionHelper
    {
        void AutomaticallySetTemporaryDefinitionForWord(WordDictionary wordDictionary, string word, string temporaryDefinition);
    }
}