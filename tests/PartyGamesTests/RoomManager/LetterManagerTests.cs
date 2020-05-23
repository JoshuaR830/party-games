using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class LetterManagerTests
    {
        private readonly LetterManager _letterManager;

        public LetterManagerTests()
        {
            var shuffleHelper = new ShuffleHelper<string>();
            _letterManager = new LetterManager(shuffleHelper);
        }
        
        [Fact]
        public void TheLetterShouldBeSet()
        {
            _letterManager.SetLetter();
            
            _letterManager
                .Letter
                .Should()
                .NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void TheNextLetterShouldBeDifferent()
        {
            _letterManager.SetLetter();
            var numLettersUsed = _letterManager.NumLettersUsed;
            var letter = _letterManager.Letter;
            
            _letterManager.SetLetter();

            _letterManager.Letter.Should().Be(_letterManager.Alphabet[numLettersUsed]);
            _letterManager.NumLettersUsed.Should().Be(numLettersUsed + 1);
            _letterManager.Letter.Should().NotBe(letter);
        }
        
        [Fact]
        public void WhenMoreLettersHaveBeenUsedThanTheInitialNumberOfLettersStartAgain()
        {
            var alphabet1 = _letterManager.Alphabet;
            var iterations = alphabet1.Count;


            for (var i = 0; i < iterations + 1; i++)
            {
                _letterManager.SetLetter();
            }

            var alphabet2 = _letterManager.Alphabet;

            alphabet1.Should().NotEqual(alphabet2);

            _letterManager.NumLettersUsed.Should().Be(1);
            
            _letterManager
                .Letter
                .Should()
                .NotBeNullOrWhiteSpace();
        }
    }
}