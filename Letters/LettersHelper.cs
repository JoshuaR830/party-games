using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Chat.Letters {
    public class LettersHelper
    {
        public List<string> Letters { get; }

        public LettersHelper()
        {
            this.Letters = GetLetters();
        }

        public List<string> GetLetters()
        {
            System.Console.WriteLine("Get");
            var alphabet = new Alphabet();
            var highFreq = alphabet.LettersList.Where(x => x.Frequency > 8).ToList();
            var randomFreq = alphabet.LettersList;
            var lowFreq = alphabet.LettersList.Where(x => x.Frequency < 2).ToList();

            var alphabetList = new List<string>();
            var temp = new List<Alphabet.LetterData>();
            
            var rand = new Random();

            var pos = 0;

            for (var i = 0; i < 8; i++)
            {
                if (i < 3) 
                {
                    temp = alphabet.LettersList.Where(x => x.Frequency > 7).ToList();
                }
                else if (i < 6)
                {
                    temp = alphabet.LettersList.Where(x => x.Frequency > 3).ToList();
                }
                else 
                {
                    temp = alphabet.LettersList;
                }

                do
                {
                    System.Console.WriteLine("Do");
                    System.Console.WriteLine(temp[pos].Letter);
                    pos = rand.Next(0, temp.Count);
                } while(temp[pos].AlreadyUsed == true);

                if(temp[pos].Repeatable == false)
                {
                    temp[pos].SetUsed();
                }

                alphabetList.Add(temp[pos].Letter);
            }

            return alphabetList;
        }

        public void SaveWordsToFile(List<WordAcceptability> words)
        {
            TextWriter writer = null;
            try
            {
                var json = JsonConvert.SerializeObject(words);
                writer = new StreamWriter("./word-list.json", false);
                writer.Write(json);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public List<WordAcceptability> ReadWordsFromFile()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader("./word-list.json");
                var fileContent = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<WordAcceptability>>(fileContent);
            }
            finally
            {
                if (reader != null)        
                    reader.Close();
            }
        }
    }

    public class WordStatusHelper
    {
        public List<WordAcceptability> Words { get; private set; }

        public WordStatusHelper()
        {
            this.Words = new List<WordAcceptability>();
        }

        public bool GetWordStatus(string word)
        {
            if (ContainsWord(word))
            {
                var selectedWord = this.Words.Where(x => x.Word == word).First();
                return selectedWord.IsAcceptable;
            }

            return FindUnknownWordStatus(word);
        }

        public bool ContainsWord(string word)
        {
            var letters = new LettersHelper();
            this.Words = letters.ReadWordsFromFile();
            Console.WriteLine("Hello there");
            System.Console.WriteLine(this.Words);
            Console.WriteLine(this.Words.Any(x => x.Word == word));
            return this.Words.Any(x => x.Word == word);
        }

        public bool FindUnknownWordStatus(string word)
        {
            if(ContainsWord(word))
                return false;

            var validationHelper = new WordValidationHelper();
            var isValidWord = validationHelper.MakeWebRequest(word);
            this.Words.Add(new WordAcceptability(word, isValidWord));

            var letters = new LettersHelper();
            letters.SaveWordsToFile(this.Words);

            return isValidWord;
        }
    }

    public class WordAcceptability
    {
        public bool IsAcceptable { get; private set; }
        public string Word { get; private set; }

        public WordAcceptability(string word, bool isAcceptable)
        {
            this.IsAcceptable = isAcceptable;
            this.Word = word;
        }
    }


    public class Alphabet
    {
        public List<LetterData> LettersList { get; }
        
        public Alphabet() {
            this.LettersList = new List<LetterData>();
            this.GenerateAlphabet();
        }

        public void GenerateAlphabet()
        {
            this.LettersList.Add(new LetterData("A", 8.12, true));
            this.LettersList.Add(new LetterData("B", 1.49, true));
            this.LettersList.Add(new LetterData("C", 9.10, false));
            this.LettersList.Add(new LetterData("D", 4.32, true));
            this.LettersList.Add(new LetterData("E", 12.02, true));
            this.LettersList.Add(new LetterData("F", 2.30, true));
            this.LettersList.Add(new LetterData("G", 2.03, true));
            this.LettersList.Add(new LetterData("H", 5.92, true));
            this.LettersList.Add(new LetterData("I", 7.31, true));
            this.LettersList.Add(new LetterData("J", 0.10, false));
            this.LettersList.Add(new LetterData("K", 0.69, false));
            this.LettersList.Add(new LetterData("L", 3.98, true));
            this.LettersList.Add(new LetterData("M", 2.61, true));
            this.LettersList.Add(new LetterData("N", 6.95, true));
            this.LettersList.Add(new LetterData("O", 7.68, true));
            this.LettersList.Add(new LetterData("P", 1.82, true));
            this.LettersList.Add(new LetterData("Q", 0.11, false));
            this.LettersList.Add(new LetterData("R", 6.02, true));
            this.LettersList.Add(new LetterData("S", 6.28, true));
            this.LettersList.Add(new LetterData("T", 9.10, true));
            this.LettersList.Add(new LetterData("U", 2.88, true));
            this.LettersList.Add(new LetterData("V", 1.11, false));
            this.LettersList.Add(new LetterData("W", 2.09, true));
            this.LettersList.Add(new LetterData("X", 0.17, false));
            this.LettersList.Add(new LetterData("Y", 2.11, true));
            this.LettersList.Add(new LetterData("Z", 0.07, false));
        }

        public class LetterData {
            public string Letter { get; }
            public double Frequency { get; }
            public bool Repeatable { get; }
            public bool AlreadyUsed { get; private set; }
            public int Score { get; }

            public LetterData(string letter, double frequency, bool repeatable) {
                this.Letter = letter;
                this.Frequency = frequency;
                this.Score = Convert.ToInt32((100 - frequency)/100);
                this.Repeatable = repeatable;
            }

            public void SetUsed()
            {
                this.AlreadyUsed = true;
            }
        }
    }
}