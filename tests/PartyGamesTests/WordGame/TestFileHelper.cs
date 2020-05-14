using System.IO;
using System.Text;

namespace PartyGamesTests.WordGame
{
    public static class TestFileHelper
    {
        public static void Create(string filename)
        {
            using (FileStream fs = File.Create(filename))
            {
                var content = new UTF8Encoding(true).GetBytes($@"{{ 
                    ""Words"" : [
                        {{""Word"":""Sheep"",""TemporaryDefinition"":""A fluffy animal that sits, eats grass, sits, eats grass and is commonly counted by children who can't sleep (they love the attention)."", ""PermanentDefinition"": ""An animal with a wool laden coat that lives on a farm""}},
                        {{""Word"":""Sloth"",""TemporaryDefinition"":""A sleepy animal that sleeps, sleeps and sleeps some more and is a common role model for teenagers who spend all their days sleeping. No sheep are counted anymore because there wouldn't be enough for all the sleeping that is done and given the environmental crisis, sloths are a more environmental choice."", ""PermanentDefinition"": ""An animal that likes sleeping""}},
                        {{""Word"":""Pelican"",""TemporaryDefinition"":""A large bird with one heck of a bill, I wonder if such a big bill causes problems? Adults find this relatable because they have entered the real world and also have big bills."", ""PermanentDefinition"": ""A bird with a big beak""}}
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