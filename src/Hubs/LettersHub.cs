using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Amazon.Lambda;
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
        private readonly IAmazonLambda _amazonLambda;

        public LettersHub(IWordService wordService, IFileHelper fileHelper, IFilenameHelper filenameHelper, IJoinRoomHelper joinRoomHelper, IGameManager gameManager, IAmazonLambda amazonLambda)
        {
            _wordService = wordService;
            _fileHelper = fileHelper;
            _filenameHelper = filenameHelper;
            _joinRoomHelper = joinRoomHelper;
            _gameManager = gameManager;
            _amazonLambda = amazonLambda;

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
            var isValid = await _wordService.GetWordStatus(word);
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
            var definition = await _wordService.GetDefinition(word);
            await Clients.Group(group).SendAsync("ReceiveDefinition", definition, word);
        }
        
        public async Task GetCrowdSourceDefinition(string group, string word)
        {
            var definition = await _wordService.GetDefinition(word);
            var category = await _wordService.GetCategory(word);
            await Clients.Group(group).SendAsync("ReceiveDefinition", definition, word);
            await Clients.Group(group).SendAsync("ReceiveCategory", category);
        }

        public async Task AddWordToDictionary(string group, string newWord, string newDefinition, int category = 0)
        {
            _wordService.AmendDictionary(_filenameHelper.GetDictionaryFilename(), newWord, newDefinition, (WordCategory) category);
            _wordService.ToggleIsWordInDictionary(_filenameHelper.GetDictionaryFilename(), newWord, true);
            _wordService.AddWordToGuessedWords(_filenameHelper.GetDictionaryFilename(), _filenameHelper.GetGuessedWordsFilename(), newWord);
            await Clients.Group(group).SendAsync("DefinitionUpdated", newWord);
        }

        public void UpdateDictionary(string group, string word, string definition, int category = 0)
        {
            Console.WriteLine(word);
            _wordService.UpdateExistingWord(_filenameHelper.GetDictionaryFilename(), word, definition, (WordCategory) category);
        }

        public void UpdateCategory(string group, string word, int category)
        {
            _wordService.UpdateCategory(_filenameHelper.GetDictionaryFilename(), word, (WordCategory) category);
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
                Rooms.RoomsList[roomId].Word.GetLetters();
                Console.WriteLine(name);
            }

            Console.WriteLine("Word game 1");
            if (Rooms.RoomsList[roomId].Word == null)
            {
                Console.WriteLine("Word game 2");
                _gameManager.SetupNewGame(roomId, name, (GameType) gameId);
                if (Rooms.RoomsList[roomId].Word == null)
                    return;
                
                Rooms.RoomsList[roomId].Word.GetLetters();
            }
        }

        public async Task SetupNewUser(string roomId, string name)
        {
            Console.WriteLine("New user");
            
            // Hit the lambda on login to get it up and running for when it will actually be needed (avoid the boot times later)
            await _wordService.GetWordStatus("WarmingUpTheLambda");
            if (!Rooms.RoomsList[roomId].Users.ContainsKey(name))
            {
                var game = Rooms.RoomsList[roomId].Word;
                _gameManager.SetUpNewWordUser(roomId, name, game);
                Console.WriteLine(name);
            }

            if (Rooms.RoomsList[roomId].Users[name].WordGame == null)
            {
                var game = Rooms.RoomsList[roomId].Word;
                _gameManager.SetUpNewWordUser(roomId, name, game);
            }
            
            var loggedInUsers = Rooms.RoomsList[roomId].Users.Select(x => x.Key).ToList().OrderBy(x => x); 
            await Clients.Group(roomId).SendAsync("LoggedInUsers", loggedInUsers);
        }
        
        public async Task ServerIsValidWord(string word, string roomId, string name)
        {
            var isSuccessful = await _wordService.GetWordStatus(word);
            _wordService.AddWordToGuessedWords(_filenameHelper.GetDictionaryFilename(), _filenameHelper.GetGuessedWordsFilename(), word);
            await Rooms.RoomsList[roomId].Users[name].WordGame.AddWordToList(word);
            await Clients.Group(name).SendAsync("WordStatusResponse", isSuccessful, word);
        }

        public async Task GetUserData(string roomId, string name)
        {
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                Console.WriteLine("Get user data");
                var letters = Rooms.RoomsList[roomId].Word.Letters;
                var words = Rooms.RoomsList[roomId].Users[user.Value.Name].WordGame.WordList.Values;
                var serializedLetters = JsonConvert.SerializeObject(letters);
                var serializedWords = JsonConvert.SerializeObject(words);
                var letterCount = letters.Count;
                await Clients.Group(user.Value.Name).SendAsync("ReceiveUserData", serializedLetters, serializedWords, letterCount, words.Count);
            }
        }

        public async Task ResetGame(string roomId, string word, int gameId)
        {
            var game = Rooms.RoomsList[roomId].Word;
            _gameManager.ResetWordGame(roomId);
            
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                if (user.Value.WordGame == null)
                {
                    Console.WriteLine($"No word game for {user.Key}");
                    SetupNewUser(roomId, user.Key);
                }
                else
                {
                    Console.WriteLine($"Yes there is a word game for {user.Key}");
                    _gameManager.ResetWordGameForUser(roomId, user.Value.Name);
                }
            }
        }

        public void SaveUpdatesToFile()
        {
            _wordService.UpdateDictionaryFile();
            _wordService.UpdateGuessedWordsFile();
        }
    }
}