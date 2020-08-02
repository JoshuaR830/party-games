using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.WordGame.WordHelpers;

namespace Chat.RoomManager
{
    public class UserWordGame
    {
        private readonly IWordService _wordService;
        public Dictionary<string, WordManager> WordList { get; }

        public UserWordGame(IWordService wordService)
        {
            _wordService = wordService;
            WordList = new Dictionary<string, WordManager>();
        }

        public async Task AddWordToList(string word)
        {
            var definition = await _wordService.GetDefinition(word);
            var status = await _wordService.GetWordStatus(word);
            WordList.Add(word, new WordManager(word, definition, status));
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