using System;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Chat.GameManager;
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
        private readonly IFilenameHelper _filenameHelper;
        private readonly IJoinRoomHelper _joinRoomHelper;
        private readonly IGameManager _gameManager;

        public LettersHub(IWordService wordService, IFileHelper fileHelper, IFilenameHelper filenameHelper, IJoinRoomHelper joinRoomHelper, IGameManager gameManager)
        {
            _wordService = wordService;
            _fileHelper = fileHelper;
            _filenameHelper = filenameHelper;
            _joinRoomHelper = joinRoomHelper;
            _gameManager = gameManager;

            if (!File.Exists(_filenameHelper.GetDictionaryFilename()))
                File.Create(_filenameHelper.GetDictionaryFilename());
            
            if (!File.Exists(_filenameHelper.GetGuessedWordsFilename()))
                File.Create(_filenameHelper.GetGuessedWordsFilename());
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
            var isValid = _wordService.GetWordStatus(_filenameHelper.GetDictionaryFilename(), word);
            _wordService.AddWordToGuessedWords(_filenameHelper.GetDictionaryFilename(), _filenameHelper.GetGuessedWordsFilename(), word);
            await Clients.Group(group).SendAsync("WordStatusResponse", isValid, word);
        }

        public async Task WordTicked(string word, string group, bool newStatus)
        {
            Console.WriteLine(word);
            _wordService.ToggleIsWordInDictionary(_filenameHelper.GetDictionaryFilename(), word, newStatus);
            _wordService.AddWordToGuessedWords(_filenameHelper.GetDictionaryFilename(), _filenameHelper.GetGuessedWordsFilename(), word);
            await Clients.Group(group).SendAsync("TickWord", word);
        }

        public async Task GetDefinition(string group, string word)
        {
            var definition = _wordService.GetDefinition(_filenameHelper.GetDictionaryFilename(), word);
            await Clients.Group(group).SendAsync("ReceiveDefinition", definition, word);
        }

        public async Task AddWordToDictionary(string group, string newWord, string newDefinition)
        {
            _wordService.AmendDictionary(_filenameHelper.GetDictionaryFilename(), newWord, newDefinition);
            _wordService.ToggleIsWordInDictionary(_filenameHelper.GetDictionaryFilename(), newWord, true);
            _wordService.AddWordToGuessedWords(_filenameHelper.GetDictionaryFilename(), _filenameHelper.GetGuessedWordsFilename(), newWord);
            await Clients.Group(group).SendAsync("DefinitionUpdated", newWord);
        }

        public void UpdateDictionary(string group, string word, string definition)
        {
            Console.WriteLine(word);
            Console.WriteLine(definition);
            _wordService.UpdateExistingWord(_filenameHelper.GetDictionaryFilename(), word, definition);
        }

        public async Task GetGuessedWords(string group)
        {
            var wordData = _fileHelper.ReadFile(_filenameHelper.GetGuessedWordsFilename());
            await Clients.Group(group).SendAsync("ReceiveGuessedWord", wordData);
        }

        public void RoundComplete(string group)
        {
            Console.WriteLine("Update");
            _wordService.UpdateDictionaryFile();
            _wordService.UpdateGuessedWordsFile();
        }
        
        
        // Server setup
        
        public async Task Startup(string roomId, string name, int gameId)
        {
            Console.WriteLine("New room 1");
            await Groups.AddToGroupAsync(Context.ConnectionId, name);

            if (!Rooms.RoomsList.ContainsKey(roomId))
            {
                Console.WriteLine("New room 2");
                _gameManager.SetupNewGame(roomId, name, (GameType) gameId);
                Rooms.RoomsList[roomId].WordGame.GetLetters();
                Console.WriteLine(name);
            }

            if (Rooms.RoomsList[roomId].WordGame == null)
            {
                _gameManager.SetupNewGame(roomId, name, (GameType) gameId);
                if (Rooms.RoomsList[roomId].WordGame == null)
                    return;
                
                Rooms.RoomsList[roomId].WordGame.GetLetters();
            }
        }

        public void SetupNewUser(string roomId, string name)
        {
            Console.WriteLine("New user");
            if (!Rooms.RoomsList[roomId].Users.ContainsKey(name))
            {
                var game = Rooms.RoomsList[roomId].WordGame;
                _gameManager.SetUpNewWordGameUser(roomId, name, game);
                Console.WriteLine(name);
            }

            if (Rooms.RoomsList[roomId].Users[name].WordGame == null)
            {
                var game = Rooms.RoomsList[roomId].WordGame;
                _gameManager.SetUpNewWordGameUser(roomId, name, game);
            }
        }
        
        public async Task ServerIsValidWord(string word, string roomId, string name)
        {
            var isValid = _wordService.GetWordStatus(_filenameHelper.GetDictionaryFilename(), word);
            _wordService.AddWordToGuessedWords(_filenameHelper.GetDictionaryFilename(), _filenameHelper.GetGuessedWordsFilename(), word);
            Rooms.RoomsList[roomId].Users[name].WordGame.AddWordToList(word);
            Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList[roomId].Users[name].WordGame.WordList));
            await Clients.Group(name).SendAsync("WordStatusResponse", isValid, word);
        }

        public async Task GetUserData(string roomId, string name)
        {
            Console.WriteLine("Get user data");
            var letters = Rooms.RoomsList[roomId].WordGame.Letters;
            var words = Rooms.RoomsList[roomId].Users[name].WordGame.WordList.Values;
            var serializedLetters = JsonConvert.SerializeObject(letters);
            var serializedWords = JsonConvert.SerializeObject(words);
            var letterCount = letters.Count;

            await Clients.Group(name).SendAsync("ReceiveUserData", serializedLetters, serializedWords, letterCount, words.Count);
        }

        public async Task ResetGame(string roomId, string word, int gameId)
        {
            var game = Rooms.RoomsList[roomId].WordGame;
            _gameManager.ResetWordGame(roomId);
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                _gameManager.ResetWordGameForUser(roomId, user.Value.Name);
            }
        }
    }
}