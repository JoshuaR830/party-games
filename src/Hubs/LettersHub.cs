using System;
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
        readonly WordValidationHelper _wordValidationHelper = new WordValidationHelper();
        readonly DictionaryRequestHelper _requestHelper = new DictionaryRequestHelper();
        private readonly IWordService _wordService;
        private readonly IFileHelper _fileHelper;

        private const string Filename = "./word-dictionary.json";

        public LettersHub(IWordService wordService, IFileHelper fileHelper)
        {
            _wordService = wordService;
            _fileHelper = fileHelper;
        }

        public async Task AddToGroup(string groupName)
        {
            var fileHelper =  new Letters.FileHelper();
            fileHelper.CopyFileContent();
            // var fileHelper = new FileHelper();
            // var dictionaryContent = fileHelper.ReadDictionary();
            // fileHelper.WriteToDictionary(dictionaryContent);
            // var definition = _requestHelper.MakeDefinitionRequest("fish");

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
            var isValid = _wordValidationHelper.ValidateWord(word);
            await Clients.Group(group).SendAsync("WordStatusResponse", isValid, word);
            // oat
            // taxi
        }

        public async Task WordTicked(string word, string group)
        {
            Console.WriteLine(word);
            await Clients.Group(group).SendAsync("TickWord", word);
        }

        public async Task GetDefinition(string group, string word)
        {
            System.Console.WriteLine("Hello");
            var definition = _wordValidationHelper.GetDefinition(word);
            var definition2 = _wordService.GetDefinition(word);
            // var definition2 = _wordService.GetDefinition(word);
            Console.WriteLine("\n\nDefinition old");
            Console.WriteLine(definition);
            Console.WriteLine("\n\nDefinition new");
            Console.WriteLine(definition2);
            // Console.WriteLine(definition2);
            System.Console.WriteLine("Hi");
            await Clients.Group(group).SendAsync("ReceiveDefinition", definition, word);
        }

        public async Task AddWordToDictionary(string group, string newWord, string newDefinition)
        {
            // var definition = _requestHelper.MakeDefinitionRequest(newWord);
            _wordValidationHelper.UpdateDictionary(newWord, newDefinition);
            _wordService.AddNewWordToDictionary(Filename, newWord, newDefinition);
            await Clients.Group(group).SendAsync("DefinitionUpdated", newWord);
        }
    }
}