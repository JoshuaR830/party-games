using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Chat.Letters {
    public class LettersHelper
    {
        public List<LetterScore> Letters { get; }

        public LettersHelper()
        {
            this.Letters = GetLetters();
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

            var pos = 0;

            for (var i = 0; i < 8; i++)
            {
                if (i < 3) 
                {
                    temp = alphabet.LettersList
                        .Where(x => x.Frequency > 7)
                        .Where(x => x.CurrentCount < x.MaxCount)
                        .ToList();
                }
                else if (i < 6)
                {
                    temp = alphabet.LettersList
                        .Where(x => x.Frequency > 3)
                        .Where(x => x.Frequency < 7)
                        .Where(x => x.CurrentCount < x.MaxCount)
                        .ToList();
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

                temp[pos].CurrentCount ++;
                if(temp[pos].Repeatable == false)
                {
                    temp[pos].SetUsed();
                }

                alphabetList.Add(new LetterScore(temp[pos].Letter, temp[pos].Score));
            }

            alphabetList = new List<LetterScore>();
            alphabetList.Add(new LetterScore("T", 1));
            alphabetList.Add(new LetterScore("A", 1));
            alphabetList.Add(new LetterScore("X", 1));
            alphabetList.Add(new LetterScore("I", 1));
            alphabetList.Add(new LetterScore("S", 1));
            alphabetList.Add(new LetterScore("E", 1));
            alphabetList.Add(new LetterScore("G", 1));
            alphabetList.Add(new LetterScore("W", 1));


            return alphabetList;
        }
    }

    public class LetterScore
    {
        public string Letter {get; }
        public int Score { get; }

        public LetterScore(string letter, int score)
        {
            this.Letter = letter;
            this.Score = score;
        }
    }
}