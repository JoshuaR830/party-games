using System;
using System.Linq;
using Chat.Letters;
using Chat.WordGame.WordHelpers;

namespace Chat.RoomManager
{
    public class WordManager
    {
        IWordService _wordService;
        private IFilenameHelper _filenameHelper;
        
        public string Word { get; }
        public string Definition { get; }
        public int Score { get; }
        public bool isValid { get; }


        public WordManager(IWordService wordService, string word)
        {
            _wordService = wordService;
            Word = word;
            Score = CalculateWordScore();
        }

        private int CalculateWordScore()
        {
            var alphabet = new Alphabet();
            var score = 0;
            foreach (var letter in Word)
            {
                var chosenLetter = alphabet.LettersList.Where(x => x.Letter.ToLower() == Convert.ToString(letter).ToLower()).ToList();
               
                if (!chosenLetter.Any())
                    continue;
                
                score += chosenLetter[0].Score;
            }

            return score;
        }

        private void GetDefinition()
        {
            // _wordService.GetDefinition(, word);
        }

        public void ChangeValidity(bool status)
        {
            throw new NotImplementedException();
        }
    }
}