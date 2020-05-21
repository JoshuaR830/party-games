using System;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Chat.Letters;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat.Hubs
{
    public class LettersHub : Hub
    {
        private readonly IWordService _wordService;
        private readonly IFileHelper _fileHelper;
        private readonly IJoinRoomHelper _joinRoomHelper;
        private readonly IRoomHelper _roomHelper;

        private const string DictionaryFilename = "./word-dictionary.json";
        private const string GuessedWordsFilename = "./words-guessed.json";

        public LettersHub(IWordService wordService, IFileHelper fileHelper, IJoinRoomHelper joinRoomHelper, IRoomHelper roomHelper)
        {
            _wordService = wordService;
            _fileHelper = fileHelper;
            _joinRoomHelper = joinRoomHelper;
            _roomHelper = roomHelper;

            if (!File.Exists(DictionaryFilename))
                File.Create(DictionaryFilename);
            
            if (!File.Exists(GuessedWordsFilename))
                File.Create(GuessedWordsFilename);
        }

        public async Task AddToGroup(string groupName)
        {
            // ToDo: Use a real name as entered by user
            _joinRoomHelper.CreateRoom(groupName, "User name");
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task ChooseLetters(string group)
        {
            var lettersHelper = new LettersHelper();
            var letters = lettersHelper.Letters;
            Console.WriteLine(letters);
            await Clients.Group(group).SendAsync("LettersForGame", JsonConvert.SerializeObject(letters));
        }

        public async Task IsValidWord(string word, string group)
        {          
            var isValid = _wordService.GetWordStatus(DictionaryFilename, word);
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, word);
            await Clients.Group(group).SendAsync("WordStatusResponse", isValid, word);
        }

        public async Task WordTicked(string word, string group, bool newStatus)
        {
            Console.WriteLine(word);
            _wordService.ToggleIsWordInDictionary(DictionaryFilename, word, newStatus);
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, word);
            await Clients.Group(group).SendAsync("TickWord", word);
        }

        public async Task GetDefinition(string group, string word)
        {
            var definition = _wordService.GetDefinition(DictionaryFilename, word);
            await Clients.Group(group).SendAsync("ReceiveDefinition", definition, word);
        }

        public async Task AddWordToDictionary(string group, string newWord, string newDefinition)
        {
            _wordService.AmendDictionary(DictionaryFilename, newWord, newDefinition);
            _wordService.ToggleIsWordInDictionary(DictionaryFilename, newWord, true);
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, newWord);
            await Clients.Group(group).SendAsync("DefinitionUpdated", newWord);
        }

        public void UpdateDictionary(string group, string word, string definition)
        {
            Console.WriteLine(word);
            Console.WriteLine(definition);
            _wordService.UpdateExistingWord(DictionaryFilename, word, definition);
        }

        public async Task GetGuessedWords(string group)
        {
            var wordData = _fileHelper.ReadFile(GuessedWordsFilename);
            await Clients.Group(group).SendAsync("ReceiveGuessedWord", wordData);
        }
    }
}