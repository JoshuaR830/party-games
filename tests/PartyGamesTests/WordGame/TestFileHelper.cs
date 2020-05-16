using System.IO;
using System.Text;
using Chat.WordGame.WordHelpers;

namespace PartyGamesTests.WordGame
{
    public static class TestFileHelper
    {
        public const string SheepTemporaryDefinition = "A fluffy animal that sits, eats grass, sits, eats grass and is commonly counted by children who can't sleep (they love the attention).";
        public const string SheepPermanentDefinition = "An animal with a wool laden coat that lives on a farm";
        public const string SlothTemporaryDefinition = "A sleepy animal that sleeps, sleeps and sleeps some more and is a common role model for teenagers who spend all their days sleeping. No sheep are counted anymore because there wouldn't be enough for all the sleeping that is done and given the environmental crisis, sloths are a more environmental choice.";
        public const string SlothPermanentDefinition = "An animal that likes sleeping";
        public const string PelicanTemporaryDefinition = "A large bird with one heck of a bill, I wonder if such a big bill causes problems? Adults find this relatable because they have entered the real world and also have big bills.";
        public const string PelicanPermanentDefinition = "A bird with a big beak";
        public const string LionPermanentDefinition = "A big cat that inhabits a jungle, the male has a very large beared";
        public const string LionTemporaryDefinition = "King of the jungle";
        public const string BoxingTemporaryDefinition = "Put on those mitts and start punching";
        public const string DodoPermanentDefinition = "A big dead bird";
        public const string UnicornTemporaryDefinition = "A magical beast with a horn";
        
        public static void Create(string filename)
        {
            using (FileStream fs = File.Create(filename))
            {
                var content = new UTF8Encoding(true).GetBytes($@"{{ 
                    ""Words"" : [
                        {{""Word"":""sheep"",""TemporaryDefinition"":""{SheepTemporaryDefinition}"", ""PermanentDefinition"":""{SheepPermanentDefinition}"", ""Status"": {(int)WordStatus.Permanent}}},
                        {{""Word"":""sloth"",""TemporaryDefinition"":""{SlothTemporaryDefinition}"", ""PermanentDefinition"":""{SlothPermanentDefinition}"", ""Status"": {(int)WordStatus.Permanent}}},
                        {{""Word"":""pelican"",""TemporaryDefinition"":""{PelicanTemporaryDefinition}"", ""PermanentDefinition"":""{PelicanPermanentDefinition}"", ""Status"": {(int)WordStatus.Permanent}}},
                        {{""Word"":""lion"",""TemporaryDefinition"":""{LionTemporaryDefinition}"", ""PermanentDefinition"":""{LionPermanentDefinition}"", ""Status"": {(int)WordStatus.Temporary}}},
                        {{""Word"":""boxing"",""TemporaryDefinition"":""{BoxingTemporaryDefinition}"", ""PermanentDefinition"": null, ""Status"": {(int)WordStatus.Suffix}}},
                        {{""Word"":""dodo"",""TemporaryDefinition"": null, ""PermanentDefinition"": ""{DodoPermanentDefinition}"", ""Status"": {(int)WordStatus.DoesNotExist}}},
                        {{""Word"":""unicorn"",""TemporaryDefinition"":""{UnicornTemporaryDefinition}"", ""PermanentDefinition"": null, ""Status"": {(int)WordStatus.DoesNotExist}}},
                       {{""Word"":""dinosaur"",""TemporaryDefinition"": null, ""PermanentDefinition"": null, ""Status"": {(int)WordStatus.DoesNotExist}}}
                    ]
                }}");

                fs.Write(content, 0, content.Length);
            }
        }

        public static string Read(string filename)
        {
            if (File.Exists(filename))
            {
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    return json;
                }
            }

            return null;
        }
    }
}