using System;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Chat.Letters;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat.Hubs
{
    public class LettersHub : Hub
    {
        // readonly WordValidationHelper _wordValidationHelper = new WordValidationHelper();
        // readonly DictionaryRequestHelper _requestHelper = new DictionaryRequestHelper();
        private readonly IWordService _wordService;
        private readonly IFileHelper _fileHelper;

        private const string DictionaryFilename = "./word-dictionary.json";
        private const string GuessedWordsFilename = "./words-guessed.json";

        public LettersHub(IWordService wordService, IFileHelper fileHelper)
        {
            _wordService = wordService;
            _fileHelper = fileHelper;

            if (!File.Exists(DictionaryFilename))
                File.Create(DictionaryFilename);
            
            if (!File.Exists(GuessedWordsFilename))
                File.Create(GuessedWordsFilename);
        }

        public async Task AddToGroup(string groupName)
        {
            // var fileHelper =  new Letters.FileHelper();
            // fileHelper.CopyFileContent();

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
            // var isValid2 = _wordValidationHelper.ValidateWord(word);
            var isValid = _wordService.GetWordStatus(DictionaryFilename, word);
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, word);
            await Clients.Group(group).SendAsync("WordStatusResponse", isValid, word);
        }

        public async Task WordTicked(string word, string group)
        {
            Console.WriteLine(word);
            // _wordService.ToggleIsWordInDictionary(DictionaryFilename, word);
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, word);
            await Clients.Group(group).SendAsync("TickWord", word);
        }

        public async Task GetDefinition(string group, string word)
        {
            // var definition2 = _wordValidationHelper.GetDefinition(word);
            var definition = _wordService.GetDefinition(DictionaryFilename, word);
            // Console.WriteLine($"Old: {definition2}");
            Console.WriteLine($"New: {definition}");
            await Clients.Group(group).SendAsync("ReceiveDefinition", definition, word);
        }

        public async Task AddWordToDictionary(string group, string newWord, string newDefinition)
        {
            // var definition = _requestHelper.MakeDefinitionRequest(newWord);
            // _wordValidationHelper.UpdateDictionary(newWord, newDefinition);
            _wordService.AmendDictionary(DictionaryFilename, newWord, newDefinition);
            await Clients.Group(group).SendAsync("DefinitionUpdated", newWord);
        }
    }
}