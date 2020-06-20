using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Chat.RoomManager;
using Microsoft.EntityFrameworkCore;

namespace Chat.Letters {
    public class LettersHelper : ILettersHelper
    {
        private readonly ShuffleHelper<Alphabet.LetterData> _shuffleHelper;
        public List<LetterScore> Letters { get; }

        public LettersHelper()
        {
            _shuffleHelper = new ShuffleHelper<Alphabet.LetterData>();
            Letters = GetLetters();
        }

        public List<LetterScore> GetLetters()
        {
            System.Console.WriteLine("Get");
            var alphabet = new Alphabet();
            var highFreq = alphabet.LettersList.Where(x => x.Frequency > 8).ToList();
            var randomFreq = alphabet.LettersList;
            var lowFreq = alphabet.LettersList.Where(x => x.Frequency < 2).ToList();

            var alphabetList = new List<LetterScore>();
            var temp = new List<Alphabet.LetterData>();
            
            var rand = new Random();

            for (var i = 0; i < 8; i++)
            {
                if (i < 3) 
                {
                    temp = alphabet.LettersList
                        .Where(x => x.Frequency > 7)
                        .Where(x => x.CurrentCount < x.MaxCount)
                        .Where(x => !x.AlreadyUsed)
                        .ToList();
                }
                else if (i < 6)
                {
                    temp = alphabet.LettersList
                        .Where(x => x.Frequency > 3)
                        .Where(x => x.Frequency < 7)
                        .Where(x => x.CurrentCount < x.MaxCount)
                        .Where(x => !x.AlreadyUsed)
                        .ToList();
                }
                else 
                {
                    temp = alphabet.LettersList
                        .Where(x => x.CurrentCount < x.MaxCount)
                        .Where(x => !x.AlreadyUsed)
                        .ToList();
                }

                temp = _shuffleHelper.ShuffleList(temp);
                
                var chosenLetter = temp.FirstOrDefault();

                if (chosenLetter == null)
                {
                    i--;
                    continue;
                }

                chosenLetter.CurrentCount ++;
                if(chosenLetter.Repeatable == false)
                {
                    chosenLetter.SetUsed();
                }

                alphabetList.Add(new LetterScore(chosenLetter.Letter, chosenLetter.Score));
            }

            // alphabetList = new List<LetterScore>();
            // alphabetList.Add(new LetterScore("T", 1));
            // alphabetList.Add(new LetterScore("A", 1));
            // alphabetList.Add(new LetterScore("X", 1));
            // alphabetList.Add(new LetterScore("I", 1));
            // alphabetList.Add(new LetterScore("S", 1));
            // alphabetList.Add(new LetterScore("N", 1));
            // alphabetList.Add(new LetterScore("A", 1));
            // alphabetList.Add(new LetterScore("S", 1));
            
            if (alphabetList.Any(x => x.Letter == "Q"))
            {
                var num = rand.Next(0, 5);
                var letterU = alphabet.LettersList.First(x => x.Letter == "U");
                alphabetList[num].UpdateLetter(letterU.Letter, letterU.Score);
            }

            return alphabetList;
        }
    }

    public class LetterScore
    {
        public string Letter {get; private set; }
        public int Score { get; private set; }

        public LetterScore(string letter, int score)
        {
            this.Letter = letter;
            this.Score = score;
        }

        public void UpdateLetter(string letter, int score)
        {
            this.Letter = letter;
            this.Score = score;
        }
    }
}