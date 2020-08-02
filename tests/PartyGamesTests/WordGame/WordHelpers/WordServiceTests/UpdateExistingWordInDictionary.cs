using System;
using System.IO;
using Amazon.Lambda;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class UpdateExistingWordInDictionary : IDisposable
    {
        const string Filename = "./file-with-existing-words.json";

        private const string TempDefinition = "A fluffy animal that sits, eats grass, sits, eats grass and is commonly counted by children who can't sleep (they love the attention).";
        private const string PermDefinition = "An animal with a wool laden coat that lives on a farm";
        private readonly FileHelper _fileHelper;
        private IFilenameHelper _filenameHelper;
        private readonly WordService _wordService;
        private readonly IAmazonLambda _lambda;
        
        public UpdateExistingWordInDictionary()
        {
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper
                .GetGuessedWordsFilename()
                .Returns(Filename);
            _filenameHelper.GetDictionaryFilename().Returns(Filename);

            _lambda = Substitute.For<IAmazonLambda>();

            TestFileHelper.Create(Filename);
            _fileHelper = new FileHelper(_filenameHelper);
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            
        }
        
        [Fact]
        public void WhenWordExistsUpdateDefinitionThePermanentDefinitionShouldBeUpdated()
        {
            var newDefinition = "Cloud like fluffy animals";
            var word = "sheep";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = word,
                    PermanentDefinition = newDefinition,
                    TemporaryDefinition = TempDefinition,
                    Status = WordStatus.Permanent
                });
        }

        [Fact]
        public void WhenWordDoesNotExistButUpdateHappensNothingShouldBeChanged()
        {
            var newDefinition = "This is not a definition";
            var word = "Not a word";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary
                .Words
                .Count
                .Should()
                .Be(8);

            dictionary.Words.Should().Contain(x => x.Word == "sheep");
            dictionary.Words.Should().Contain(x => x.Word == "sloth");
            dictionary.Words.Should().Contain(x => x.Word == "pelican");
            dictionary.Words.Should().Contain(x => x.Word == "lion");
            dictionary.Words.Should().Contain(x => x.Word == "boxing");
            dictionary.Words.Should().Contain(x => x.Word == "dodo");
            dictionary.Words.Should().Contain(x => x.Word == "unicorn");
            dictionary.Words.Should().Contain(x => x.Word == "dinosaur");
        }

        [Fact]
        public void WhenUpdateHappensAndDefinitionIsEmptyDefinitionShouldNotBeChanged()
        {
            var newDefinition = "";
            var word = "sheep";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = word,
                    PermanentDefinition = PermDefinition,
                    TemporaryDefinition = TempDefinition,
                    Status = WordStatus.Permanent
                });
        }

        [Fact]
        public void WhenUpdateHappensButWordIsEmptyNothingShouldBeChanged()
        {
            var newDefinition = "A cloud like fluffy thing";
            var word = "";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary
                .Words
                .Count
                .Should()
                .Be(8);

            dictionary.Words.Should().Contain(x => x.Word == "sheep");
            dictionary.Words.Should().Contain(x => x.Word == "sloth");
            dictionary.Words.Should().Contain(x => x.Word == "pelican");
            dictionary.Words.Should().Contain(x => x.Word == "lion");
            dictionary.Words.Should().Contain(x => x.Word == "boxing");
            dictionary.Words.Should().Contain(x => x.Word == "dodo");
            dictionary.Words.Should().Contain(x => x.Word == "unicorn");
            dictionary.Words.Should().Contain(x => x.Word == "dinosaur");
        }

        [Fact]
        public void WhenUpdateRequestedButBothWordAndDefinitionAreEmptyNothingShouldBeChanged()
        {
            var newDefinition = "";
            var word = "";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary
                .Words
                .Count
                .Should()
                .Be(8);

            dictionary.Words.Should().Contain(x => x.Word == "sheep");
            dictionary.Words.Should().Contain(x => x.Word == "sloth");
            dictionary.Words.Should().Contain(x => x.Word == "pelican");
            dictionary.Words.Should().Contain(x => x.Word == "lion");
            dictionary.Words.Should().Contain(x => x.Word == "boxing");
            dictionary.Words.Should().Contain(x => x.Word == "dodo");
            dictionary.Words.Should().Contain(x => x.Word == "unicorn");
            dictionary.Words.Should().Contain(x => x.Word == "dinosaur");
        }

        [Fact]
        public void WhenCapitalisationIsBlockCapitalTheDefinitionShouldStillBeUpdated()
        {
            var newDefinition = "Big and fluffy animal";
            var word = "SHEEP";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = "sheep",
                    PermanentDefinition = newDefinition,
                    TemporaryDefinition = TempDefinition,
                    Status = WordStatus.Permanent
                });
        }
        
        [Fact]
        public void WhenCapitalisationIsLowerTheDefinitionShouldBeUpdated()
        {
            var newDefinition = "Eats grass";
            var word = "sheep";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = "sheep",
                    PermanentDefinition = newDefinition,
                    TemporaryDefinition = TempDefinition,
                    Status = WordStatus.Permanent
                });
        }
        
        [Fact]
        public void WhenCapitalisationIsSentenceTheDefinitionShouldBeUpdated()
        {
            var newDefinition = "If clouds could baa they'd be sheep";
            var word = "Sheep";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = "sheep",
                    PermanentDefinition = newDefinition,
                    TemporaryDefinition = TempDefinition,
                    Status = WordStatus.Permanent
                });
        }

        public void Dispose()
        {
            if(File.Exists(Filename))
                File.Delete(Filename);
            
            WordDictionaryGetter.WordDictionary.Remove(Filename);
        }
    }
}