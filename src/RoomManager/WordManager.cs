using System;
using System.Linq;
using Chat.Letters;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat.RoomManager
{
    public class WordManager
    {
        readonly IWordService _wordService;
        private readonly IFilenameHelper _filenameHelper;
        
        public string Word { get; }
        public string Definition { get; }
        public int Score { get; }
        public bool IsValid { get; private set; }


        public WordManager(IWordService wordService, IFilenameHelper filenameHelper, string word)
        {
            _wordService = wordService;
            Word = word;
            _filenameHelper = filenameHelper;
            Definition = GetDefinition();
            Score = CalculateWordScore();
            IsValid = SetInitialValidity();
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

        private string GetDefinition()
        {
            return _wordService.GetDefinition(_filenameHelper.GetDictionaryFilename(), Word);
        }

        private bool SetInitialValidity()
        {
            return _wordService.GetWordStatus(_filenameHelper.GetDictionaryFilename(), Word);
        }
        
        public void ChangeValidity(bool status)
        {
            IsValid = status;
        }
    }
}