using System;
using System.Collections.Generic;
using System.IO;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace PartyGamesTests.WordGame.LocalDictionaryHelpers.FileHelper
{
    public class FileHelperTests : IDisposable
    {
        private readonly string _filename;
        private readonly string _filenameToCreate;

        public FileHelperTests()
        {
            _filename = "test-dictionary.json";
            _filenameToCreate = "created-new-file.json";

            TestFileHelper.Create(_filename);
        }
        
        [Fact]
        public void ReadingAFileShouldReturnAnObject()
        {
            var fileHelper = new Chat.WordGame.LocalDictionaryHelpers.FileHelper();
            
            var response = fileHelper.ReadDictionary(_filename);
            
            response.Should().BeOfType<Dictionary>();
        }

        [Fact]
        public void ReadingAFileShouldReturnTheFileContent()
        {
            var fileHelper = new Chat.WordGame.LocalDictionaryHelpers.FileHelper();

            var json = TestFileHelper.Read(_filename);
            
            JToken.Parse(json).Should().BeEquivalentTo($@"
            {{
                ""Words"": [
                    {{""Word"":""sheep"",""TemporaryDefinition"":""A fluffy animal that sits, eats grass, sits, eats grass and is commonly counted by children who can't sleep (they love the attention)."", ""PermanentDefinition"": ""An animal with a wool laden coat that lives on a farm"", ""Status"": {(int)WordStatus.Permanent}}},
                    {{""Word"":""sloth"",""TemporaryDefinition"":""A sleepy animal that sleeps, sleeps and sleeps some more and is a common role model for teenagers who spend all their days sleeping. No sheep are counted anymore because there wouldn't be enough for all the sleeping that is done and given the environmental crisis, sloths are a more environmental choice."", ""PermanentDefinition"": ""An animal that likes sleeping"",  ""Status"": {(int)WordStatus.Permanent}}},
                    {{""Word"":""pelican"",""TemporaryDefinition"":""A large bird with one heck of a bill, I wonder if such a big bill causes problems? Adults find this relatable because they have entered the real world and also have big bills."", ""PermanentDefinition"": ""A bird with a big beak"", ""Status"": {(int)WordStatus.Permanent}}},
                    {{""Word"":""lion"",""TemporaryDefinition"":""{TestFileHelper.LionTemporaryDefinition}"", ""PermanentDefinition"": ""{TestFileHelper.LionPermanentDefinition}"", ""Status"": {(int)WordStatus.Temporary}}},
                    {{""Word"":""boxing"",""TemporaryDefinition"":""{TestFileHelper.BoxingTemporaryDefinition}"", ""PermanentDefinition"": null, ""Status"": {(int)WordStatus.Suffix}}},
                    {{""Word"":""dodo"",""TemporaryDefinition"": null, ""PermanentDefinition"": ""{TestFileHelper.DodoPermanentDefinition}"", ""Status"": {(int)WordStatus.DoesNotExist}}},
                    {{""Word"":""unicorn"",""TemporaryDefinition"":""{TestFileHelper.UnicornTemporaryDefinition}"", ""PermanentDefinition"": null, ""Status"": {(int)WordStatus.DoesNotExist}}},
                    {{""Word"":""dinosaur"",""TemporaryDefinition"": null, ""PermanentDefinition"": null, ""Status"": {(int)WordStatus.DoesNotExist}}}
                ]
            }}");
        }

        [Fact]
        public void WhenWritingToAFileIfItDoesNotExistItShouldBeCreated()
        {
	        var fileHelper = new Chat.WordGame.LocalDictionaryHelpers.FileHelper();
	        File.Exists(_filenameToCreate).Should().BeFalse();
			fileHelper.WriteDictionary(_filenameToCreate, new Dictionary());
			File.Exists(_filenameToCreate).Should().BeTrue();
        }

		[Fact]
        public void WhenWritingContentTheContentShouldActuallyBeDeserializableToDictionary()
        {
			var fileHelper = new Chat.WordGame.LocalDictionaryHelpers.FileHelper();

			var dictionary = new Dictionary
			{
				Words = new List<WordData>
				{
					new WordData
					{
						Word = "sheep",
						PermanentDefinition = "An animal with a wool laden coat",
						TemporaryDefinition = "A fluffy animal that sits",
						Status = WordStatus.Permanent
					},
					new WordData
					{
						Word = "sloth",
						PermanentDefinition = "An animal that likes sleeping",
						TemporaryDefinition = "A sleepy animal that sleeps",
						Status = WordStatus.DoesNotExist
					}
				}
			};

			fileHelper.WriteDictionary(_filename, dictionary);

			using (StreamReader r = new StreamReader(_filename))
			{
				string json = r.ReadToEnd();
				var dictionaryContent = JsonConvert.DeserializeObject<Dictionary>(json);
				dictionaryContent.Should().BeOfType<Dictionary>();
				JToken.Parse(JsonConvert.SerializeObject(dictionaryContent)).Should().BeEquivalentTo($@"
				{{
					""Words"": [
						{{
							""Word"": ""sheep"",
							""PermanentDefinition"": ""An animal with a wool laden coat"",
							""TemporaryDefinition"": ""A fluffy animal that sits"",
							""Status"": {(int)WordStatus.Permanent},
							""Category"" : 0
						}},
						{{
							""Word"": ""sloth"",
							""PermanentDefinition"": ""An animal that likes sleeping"",
							""TemporaryDefinition"": ""A sleepy animal that sleeps"",
							""Status"": {(int)WordStatus.DoesNotExist},
							""Category"" : 0
						}}
					]
				}}");
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