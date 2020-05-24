namespace Chat.WordGame.LocalDictionaryHelpers
{
    public interface ITemporaryDefinitionHelper
    {
        void AutomaticallySetTemporaryDefinitionForWord(Dictionary dictionary, string word, string temporaryDefinition);
    }
}