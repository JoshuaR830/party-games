using System;
using System.Linq;
using Chat.Letters;

namespace Chat.RoomManager
{
    public class WordManager
    {
        public string Word { get; }
        public string Definition { get; }
        public int Score { get; }
        public bool IsValid { get; private set; }

        public WordManager(string word, string definition, bool status)
        {
            Word = word;
            Definition = definition;
            Score = CalculateWordScore();
            IsValid = status;
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

        public void ChangeValidity(bool status)
        {
            IsValid = status;
        }
    }
}