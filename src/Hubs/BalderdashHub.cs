using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Hubs
{
    public class BalderdashHub : Hub
    {
        public async Task StartUp()
        {
            // ToDo: add the game to the server and set up anything that needs to be set
            // ToDo: Trigger off the back of login
        }

        public async Task SetUpUser()
        {
            // ToDo: connect user name to server
            // ToDO: connect user to group server for group updates
            
            // ToDo: add user to the game list
        }

        public async Task BalderdashScores()
        {
            // ToDo: need to take who is giving answer and who they chose
            // ToDo: server needs to work out the scores
        }

        public async Task BalderdashDiscardSimilar()
        {
            // ToDo: if number discarded = 2 start next round
            // ToDo: call reset game
        }

        public async Task ResetGame()
        {
            // ToDo: keep scores, keep player order but update dasher and clear screens
        }

        public async Task PassToDasher()
        {
            // ToDo: send text and who
            // ToDo: server knows who dasher is so can send it easily
            // ToDo: keep a record of all of a rounds guesses - send all back to the dasher in random order
            // ToDo: object would look like {"User": "Name", "Data": "This is my guess"}
            // ToDo: When user presses submit it will send to the server
            // ToDo: Keep track of the number of guesses in server 
        }
    }
}