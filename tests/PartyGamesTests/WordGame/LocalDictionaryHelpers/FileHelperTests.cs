using System;
using System.Collections.Generic;
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
        private readonly string _filenameToCreate;

        public FileHelperTests()
        {
            _filename = "test-dictionary.json";
            _filenameToCreate = "created-new-file.json";

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

        [Fact]
        public void WhenWritingToAFileIfItDoesNotExistItShouldBeCreated()
        {
	        var fileHelper = new FileHelper();
	        File.Exists(_filenameToCreate).Should().BeFalse();
			fileHelper.WriteDictionary(_filename, new Dictionary());
			File.Exists(_filenameToCreate).Should().BeTrue();
        }

		[Fact]
        public void WhenWritingContentTheContentShouldActuallyBeDeserializableToDictionary()
        {
			var fileHelper = new FileHelper();

			var dictionary = new Dictionary
			{
				Words = new List<WordData>
				{
					new WordData
					{
						Word = "Sheep",
						PermanentDefinition = "An animal with a wool laden coat",
						TemporaryDefinition = "A fluffy animal that sits"
					},
					new WordData
					{
						Word = "Sloth",
						PermanentDefinition = "An animal that likes sleeping",
						TemporaryDefinition = "A sleepy animal that sleeps"
					}
				}
			};

			fileHelper.WriteDictionary(_filename, dictionary);

			using (StreamReader r = new StreamReader(_filename))
			{
				string json = r.ReadToEnd();
				var dictionaryContent = JsonConvert.DeserializeObject<Dictionary>(json);
				dictionaryContent.Should().BeOfType<Dictionary>();
				JToken.Parse(JsonConvert.SerializeObject(dictionaryContent)).Should().BeEquivalentTo(@"
				{
					""Words"": [
						{
							""Word"": ""Sheep"",
							""PermanentDefinition"": ""An animal with a wool laden coat"",
							""TemporaryDefinition"": ""A fluffy animal that sits""
						},
						{
							""Word"": ""Sloth"",
							""PermanentDefinition"": ""An animal that likes sleeping"",
							""TemporaryDefinition"": ""A sleepy animal that sleeps""
						}
					]
				}");
			}
        }

        public void Dispose()
        {
            if(File.Exists(_filename))
                File.Delete(_filename);

            if (File.Exists(_filenameToCreate))
	            File.Delete(_filenameToCreate);
        }
    }
}