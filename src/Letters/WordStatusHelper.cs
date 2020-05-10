using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using static Chat.Letters.FileHelper;

namespace Chat.Letters
{
    public class WordStatusHelper
    {
        public List<WordStuff> Words { get; private set; }

        public WordStatusHelper()
        {
            var fileHelper = new FileHelper();
            this.Words = fileHelper.ReadWordsFromFile();
        }

        public void UpdateDictionary(string newWord, string newDefinition)
        {
            var fileHelper = new FileHelper();
            fileHelper.SaveNewWordToFile(this.Words, newWord, newDefinition);
        }

        public bool GetWordStatus(string word)
        {
            System.Console.WriteLine(word);
            word = word.ToLower();
            if (ContainsWord(word))
            {
                var wordData = this.Words.First(x => x.Word.ToLower() == word.ToLower());
                var definitionParts = wordData.Definition.Split(new Char[] {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '\n'}).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                // var definitionParts = wordData.Definition.Split(new Char[] {';'});

                Console.WriteLine(JsonConvert.SerializeObject(definitionParts));

                foreach(var part in definitionParts)
                {
                    System.Console.WriteLine(part);

                    if(part.ToLower() == "see")
                    {
                        continue;
                    }

                    if(part.ToLower().Contains("obs."))
                    {
                        System.Console.WriteLine("obs");
                    }

                    if(part.ToLower().Contains("scot."))
                    {
                        System.Console.WriteLine("Scot");
                    }

                    if(part.ToLower().Contains("archaic"))
                    {
                        System.Console.WriteLine("Archaic");
                    }

                    if(!part.Contains("Obs.") && !part.Contains("Scot.") && !part.Contains("Archaic"))
                    {
                        System.Console.WriteLine("Yeah looks good");
                        return true;
                    }

                    // if(!part.Contains("Scot."))
                    // {
                    //     System.Console.WriteLine("Scottish - so no");
                    //     return true;
                    // }

                    // if(!part.Contains("Archaic"))
                    // {
                    //     System.Console.WriteLine("Scottish - so no");
                    //     return true;
                    // }
                }
                // if(definitionParts[0].Contains("Obs."))
                // {
                //     System.Console.WriteLine("Obsolete");
                //     return false;
                // }
                // else if(wordData.Definition.Contains("Obs.")){
                //     System.Console.WriteLine("Would have been obsolete");
                // }

                // if(definitionParts[0].Contains("Scot."))
                // {
                //     System.Console.WriteLine("Scottish - so no");
                //     return false;
                // }
                // else if(wordData.Definition.Contains("Scot.")){
                //     System.Console.WriteLine("Would have been Scottish");
                // }


                // if(definitionParts[0].Contains("Archaic"))
                // {
                //     System.Console.WriteLine("Scottish - so no");
                //     return false;
                // }
                // else if(wordData.Definition.Contains("Archaic")){
                //     System.Console.WriteLine("Would have been archaic");
                // }

                // var selectedWord = this.Words.Where(x => x.Word == word).First();
                // return false;
            }

            System.Console.WriteLine("Does it end in S");
            if(word.EndsWith('s')) {
                System.Console.WriteLine("Ends with S");
                if(ContainsWord(word.Remove(word.Length - 1))){
                    var requestHelper = new DictionaryRequestHelper();
                    // return requestHelper.MakeWebRequest(word);
                    return requestHelper.IsPluralReal(word, word.Remove(word.Length - 1));
                }
            }

            if (word.Length > 2)
            {

                if (word.Substring(word.Length - 2) == "es")
                {
                    System.Console.WriteLine("Ends with es");
                    if (ContainsWord(word.Remove(word.Length - 2)))
                    {
                        var requestHelper = new DictionaryRequestHelper();
                        // return requestHelper.MakeWebRequest(word);
                        return requestHelper.IsPluralReal(word, word.Remove(word.Length - 2));
                    }
                }

                if (word.Substring(word.Length - 2) == "er")
                {
                    System.Console.WriteLine("Ends with er");
                    if (ContainsWord(word.Remove(word.Length - 2)))
                    {
                        var requestHelper = new DictionaryRequestHelper();
                        // return requestHelper.MakeWebRequest(word);
                        return requestHelper.IsPluralReal(word, word.Remove(word.Length - 2));
                    }
                }

                if (word.Substring(word.Length - 2) == "ed")
                {
                    System.Console.WriteLine("Ends with ed");
                    if (ContainsWord(word.Remove(word.Length - 2)))
                    {
                        var requestHelper = new DictionaryRequestHelper();
                        // return requestHelper.MakeWebRequest(word);
                        return requestHelper.IsPluralReal(word, word.Remove(word.Length - 2));
                    }
                }
            }

            if (word.Length > 3)
            {
                if (word.Substring(word.Length - 3) == "ing")
                {
                    System.Console.WriteLine("Ends with ing");
                    if (ContainsWord(word.Remove(word.Length - 3)))
                    {
                        var requestHelper = new DictionaryRequestHelper();
                        // return requestHelper.MakeWebRequest(word);
                        return requestHelper.IsPluralReal(word, word.Remove(word.Length - 3));
                    }
                }
            }

            return false;
            // return FindUnknownWordStatus(word);
        }

        public string GetDefinition(string word)
        {
            if(GetWordStatus(word))
            {
                var wordData = this.Words.First(x => x.Word.ToLower() == word.ToLower());

                return wordData.Definition;
            }

            return "";
        }

        public bool ContainsWord(string word)
        {
            System.Console.WriteLine(this.Words);
            
            return this.Words.Any(x => x.Word.ToLower() == word.ToLower());
        }

        // public bool FindUnknownWordStatus(string word)
        // {
        //     if(ContainsWord(word))
        //         return false;

        //     var validationHelper = new WordValidationHelper();
        //     var isValidWord = validationHelper.MakeWebRequest(word);
        //     this.Words.Add(new WordAcceptability(word, isValidWord));

        //     var fileHelper = new FileHelper();
        //     fileHelper.SaveWordsToFile(this.Words);

        //     return isValidWord;
        // }
    }
}