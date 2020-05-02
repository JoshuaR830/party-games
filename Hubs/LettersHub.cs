using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Chat.Letters;

namespace Chat.Hubs
{
    public class LettersHub : Hub
    {
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task ChooseLetters(string group)
        {
            System.Console.WriteLine("Hi");
            var lettersHelper = new LettersHelper();
            var letters = lettersHelper.Letters;
            Console.WriteLine(letters);
            await Clients.Group(group).SendAsync("LettersForGame", JsonConvert.SerializeObject(letters));
        }

        public async Task IsValidWord(string word, string group)
        {
            var wordValidationHelper = new WordValidationHelper();
            var isValid = wordValidationHelper.ValidateWord(word);
            await Clients.Group(group).SendAsync("WordStatusResponse", isValid);
        }
    }

   
}