using System.Collections.Generic;
using Chat.WordGame.WordHelpers;

namespace Chat.WordGame
{
    public class GuessedWords
    {
        public List<GuessedWord> Words { get; set; }

        public GuessedWords()
        {
            Words = new List<GuessedWord>();
        }

        public void AddWord(string word, WordStatus status )
        {
            Words.Add(new GuessedWord(word, status));
        }
    }

    public class GuessedWord
    {
        public string Word { get; }
        public WordStatus Status { get;  }

        public GuessedWord(string word, WordStatus status)
        {
            Word = word;
            Status = status;
        }
    }
}