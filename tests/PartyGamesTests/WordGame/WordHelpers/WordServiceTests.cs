using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using NSubstitute;

namespace PartyGamesTests.WordGame.WordHelpers
{
    public class WordServiceTests
    {
        private readonly IWordHelper _wordHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;

        public WordServiceTests()
        {
            _wordHelper = Substitute.For<IWordHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
        }

        // Test what happens when the word exists in the dictionary
        public void WhenWordExistsInTheDictionaryThenTheWordServiceShouldReturnTrue()
        {
            
        }

        // Test what happens when it is a plural
        // Test what happens when not in the dictionary when plural removed
        public void WhenWordIsPluralThenWordServiceShouldReturnTrue()
        {
            
        }
        
        // Test what happens if not in dictionary
        public void WhenNotInLocalDictionaryThenWordServiceShouldReturnFalse()
        {
            
        }
        
        
        // Test what happens when in the dictionary without plural
        public void WhenWordIsNotAPluralButExistsInSingularThenWordServiceShouldReturnFalse()
        {
            
        }
        
        // Check what happens if plural, not in dictionary
        // Check what happens if plural, in dictionary after and on website.
        public void WhenAWordFormattedAsPluralDoesNotExistInTheSingularThenWordServiceShouldReturnFalse()
        {
            
        }
    }
}