using System.Collections.Generic;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat.RoomManager
{
    public class UserWordGame
    {
        private readonly IWordService _wordService;
        private readonly IFilenameHelper _filenameHelper;
        public Dictionary<string, WordManager> WordList { get; }

        public UserWordGame(IWordService wordService, IFilenameHelper filenameHelper)
        {
            _wordService = wordService;
            _filenameHelper = filenameHelper;
            WordList = new Dictionary<string, WordManager>();
        }

        public void AddWordToList(string word)
        {
            WordList.Add(word, new WordManager(_wordService, _filenameHelper, word));
        }

        public void ChangeWordStatus(string word, bool status)
        {
            WordList[word].ChangeValidity(status);
        }
        
        public void ResetWordList()
        {
            WordList.Clear();
        }
    }
}