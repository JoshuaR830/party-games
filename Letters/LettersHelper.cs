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
    }
}