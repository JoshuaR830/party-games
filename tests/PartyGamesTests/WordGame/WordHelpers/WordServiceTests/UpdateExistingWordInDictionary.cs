using System.IO;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class UpdateExistingWordInDictionary
    {
        const string Filename = "./file-with-existing-words.json";

        private const string TempDefinition = "A fluffy animal that sits, eats grass, sits, eats grass and is commonly counted by children who can't sleep (they love the attention).";
        private const string PermDefinition = "An animal with a wool laden coat that lives on a farm";
        private IWordExistenceHelper _wordExistenceHelper;
        private IWordHelper _wordHelper;
        private IWordDefinitionHelper _wordDefinitionHelper;
        private readonly FileHelper _fileHelper;
        private readonly WordService _wordService;
        
        public UpdateExistingWordInDictionary()
        {
            TestFileHelper.Create(Filename);
            _fileHelper = new FileHelper();
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            
        }
        
        [Fact]
        public void WhenWordExistsUpdateDefinitionThePermanentDefinitionShouldBeUpdated()
        {
            var newDefinition = "A cloud like fluffy animal";
            var word = "sheep";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            
            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = word,
                    PermanentDefinition = newDefinition,
                    TemporaryDefinition = TempDefinition
                });
        }

        [Fact]
        public void WhenWordDoesNotExistButUpdateHappensNothingShouldBeChanged()
        {
            var newDefinition = "This is not a definition";
            var word = "Not a word";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            
            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary
                .Words
                .Count
                .Should()
                .Be(3);

            dictionary.Words.Should().Contain(x => x.Word == "sheep");
            dictionary.Words.Should().Contain(x => x.Word == "sloth");
            dictionary.Words.Should().Contain(x => x.Word == "pelican");
        }

        [Fact]
        public void WhenUpdateHappensAndDefinitionIsEmptyDefinitionShouldNotBeChanged()
        {
            var newDefinition = "";
            var word = "sheep";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            
            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = word,
                    PermanentDefinition = PermDefinition,
                    TemporaryDefinition = TempDefinition
                });
        }

        [Fact]
        public void WhenUpdateHappensButWordIsEmptyNothingShouldBeChanged()
        {
            var newDefinition = "A cloud like fluffy animal";
            var word = "";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            
            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary
                .Words
                .Count
                .Should()
                .Be(3);

            dictionary.Words.Should().Contain(x => x.Word == "sheep");
            dictionary.Words.Should().Contain(x => x.Word == "sloth");
            dictionary.Words.Should().Contain(x => x.Word == "pelican");
        }

        [Fact]
        public void WhenUpdateRequestedButBothWordAndDefinitionAreEmptyNothingShouldBeChanged()
        {
            var newDefinition = "";
            var word = "";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary
                .Words
                .Count
                .Should()
                .Be(3);

            dictionary.Words.Should().Contain(x => x.Word == "sheep");
            dictionary.Words.Should().Contain(x => x.Word == "sloth");
            dictionary.Words.Should().Contain(x => x.Word == "pelican");
        }

        [Fact]
        public void WhenCapitalisationIsBlockCapitalTheDefinitionShouldStillBeUpdated()
        {
            var newDefinition = "A cloud like fluffy animal";
            var word = "SHEEP";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            
            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = "sheep",
                    PermanentDefinition = newDefinition,
                    TemporaryDefinition = TempDefinition
                });
        }
        
        [Fact]
        public void WhenCapitalisationIsLowerTheDefinitionShouldBeUpdated()
        {
            var newDefinition = "A cloud like fluffy animal";
            var word = "sheep";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            
            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = "sheep",
                    PermanentDefinition = newDefinition,
                    TemporaryDefinition = TempDefinition
                });
        }
        
        [Fact]
        public void WhenCapitalisationIsSentenceTheDefinitionShouldBeUpdated()
        {
            var newDefinition = "A cloud like fluffy animal";
            var word = "Sheep";

            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            
            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary
                .Words
                .Should()
                .ContainEquivalentOf(new WordData
                {
                    Word = "sheep",
                    PermanentDefinition = newDefinition,
                    TemporaryDefinition = TempDefinition
                });
        }
    }
}