using System;
using System.Collections.Generic;

namespace Chat.Letters
 {
    public class Alphabet
    {
        public List<LetterData> LettersList { get; }
        
        public Alphabet() {
            this.LettersList = new List<LetterData>();
            this.GenerateAlphabet();
        }

        public void GenerateAlphabet()
        {
            this.LettersList.Add(new LetterData("A", 8.12, true, 2));
            this.LettersList.Add(new LetterData("B", 1.49, true, 2));
            this.LettersList.Add(new LetterData("C", 9.10, false, 2));
            this.LettersList.Add(new LetterData("D", 4.32, true, 2));
            this.LettersList.Add(new LetterData("E", 12.02, true, 2));
            this.LettersList.Add(new LetterData("F", 2.30, true, 2));
            this.LettersList.Add(new LetterData("G", 2.03, true, 1));
            this.LettersList.Add(new LetterData("H", 5.92, true, 2));
            this.LettersList.Add(new LetterData("I", 7.31, true, 2));
            this.LettersList.Add(new LetterData("J", 0.10, false, 1));
            this.LettersList.Add(new LetterData("K", 0.69, false, 1));
            this.LettersList.Add(new LetterData("L", 3.98, true, 1));
            this.LettersList.Add(new LetterData("M", 2.61, true, 2));
            this.LettersList.Add(new LetterData("N", 6.95, true, 2));
            this.LettersList.Add(new LetterData("O", 7.68, true, 2));
            this.LettersList.Add(new LetterData("P", 1.82, true, 2));
            this.LettersList.Add(new LetterData("Q", 0.11, false, 1));
            this.LettersList.Add(new LetterData("R", 6.02, true, 2));
            this.LettersList.Add(new LetterData("S", 6.28, true, 2));
            this.LettersList.Add(new LetterData("T", 9.10, true, 2));
            this.LettersList.Add(new LetterData("U", 2.88, true, 2));
            this.LettersList.Add(new LetterData("V", 1.11, false, 1));
            this.LettersList.Add(new LetterData("W", 2.09, true, 1));
            this.LettersList.Add(new LetterData("X", 0.17, false, 1));
            this.LettersList.Add(new LetterData("Y", 2.11, true, 1));
            this.LettersList.Add(new LetterData("Z", 0.07, false, 1));
        }

        public class LetterData {
            public string Letter { get; }
            public double Frequency { get; }
            public bool Repeatable { get; }
            public bool AlreadyUsed { get; private set; }
            public int Score { get; }
            public int CurrentCount { get; set; }
            public int MaxCount { get; }

            public LetterData(string letter, double frequency, bool repeatable, int maxCount) {
                this.Letter = letter;
                this.Frequency = frequency;
                this.Score = Convert.ToInt32((100 - frequency)/100);
                this.Repeatable = repeatable;
                this.CurrentCount = 0;
                this.MaxCount = maxCount;
            }

            public void SetUsed()
            {
                this.AlreadyUsed = true;
            }
        }
    }
 }