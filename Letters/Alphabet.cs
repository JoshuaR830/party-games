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