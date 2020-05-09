using System;
using System.IO;
using System.Text;
using Chat.WordGame.LocalDictionaryHelpers;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace PartyGamesTests.WordGame.LocalDictionaryHelpers
{
    public class FileHelperTests : IDisposable
    {
        private readonly string _filename;

        public FileHelperTests()
        {
            _filename = "test-dictionary.json";

            using (FileStream fs = File.Create(_filename))
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
        
        [Fact]
        public void ReadingAFileShouldReturnAnObject()
        {
            var fileHelper = new FileHelper();
            
            var response = fileHelper.ReadDictionary(_filename);
            
            response.Should().BeOfType<Dictionary>();
        }

        [Fact]
        public void ReadingAFileShouldReturnTheFileContent()
        {
            var fileHelper = new FileHelper();
            
            var response = fileHelper.ReadDictionary(_filename);
            var json = JsonConvert.SerializeObject(response);
            
            JToken.Parse(json).Should().BeEquivalentTo($@"
            {{
                ""Words"": [
                    {{""Word"":""Sheep"",""TemporaryDefinition"":""A fluffy animal that sits, eats grass, sits, eats grass and is commonly counted by children who can't sleep (they love the attention)."", ""PermanentDefinition"": ""An animal with a wool laden coat that lives on a farm""}},
                    {{""Word"":""Sloth"",""TemporaryDefinition"":""A sleepy animal that sleeps, sleeps and sleeps some more and is a common role model for teenagers who spend all their days sleeping. No sheep are counted anymore because there wouldn't be enough for all the sleeping that is done and given the environmental crisis, sloths are a more environmental choice."", ""PermanentDefinition"": ""An animal that likes sleeping""}},
                    {{""Word"":""Pelican"",""TemporaryDefinition"":""A large bird with one heck of a bill, I wonder if such a big bill causes problems? Adults find this relatable because they have entered the real world and also have big bills."", ""PermanentDefinition"": ""A bird with a big beak""}}
                ]
            }}");
        }

        public void Dispose()
        {
            if(File.Exists(_filename))
                File.Delete(_filename);
        }
    }
}