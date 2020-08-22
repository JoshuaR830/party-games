using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class WordServiceTests
    {
        private readonly IFileHelper _fileHelper;
        private readonly IFilenameHelper _filenameHelper;
        private readonly IAmazonLambda _lambda;

        private const string Filename = "";

        public WordServiceTests()
        {
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);
            
            _fileHelper = Substitute.For<IFileHelper>();
            _lambda = Substitute.For<IAmazonLambda>();
        }
        
        [Fact]
        public async Task WhenWordExistsInTheDictionaryThenTheWordServiceShouldReturnTrue()
        {
            var word = "sheep";
            
            _lambda.ClearReceivedCalls();
            
            var wordResponse = new WordResponseWrapper
            {
                IsSuccessful = true,
                WordResponse = new WordResponse
                {
                    Word = word,
                    Definition = "Multiple baaing animals",
                    Status = WordStatus.Temporary
                }
            };

            var json = JsonConvert.SerializeObject(wordResponse);

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            _lambda.InvokeAsync(Arg.Any<InvokeRequest>()).Returns(new InvokeResponse
            {
                Payload = stream
            });
            
            var wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            var response = await wordService.GetWordStatus(word);
            
            await _lambda.Received(1).InvokeAsync(Arg.Any<InvokeRequest>());
            response.Should().BeTrue();
        }
        
        [Fact]
        public async Task WhenWordIsPluralThenWordServiceShouldReturnTrue()
        {
            var word = "ducks";
            
            _lambda.ClearReceivedCalls();

            var wordResponse = new WordResponseWrapper
            {
                IsSuccessful = true,
                WordResponse = new WordResponse
                {
                    Word = word,
                    Definition = "Big quackers",
                    Status = WordStatus.Temporary
                }
            };
            
            var json = JsonConvert.SerializeObject(wordResponse);

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            _lambda.InvokeAsync(Arg.Any<InvokeRequest>()).Returns(new InvokeResponse
            {
                Payload = stream
            });

            var wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            var response = await wordService.GetWordStatus(word);
            
            await _lambda.Received(1).InvokeAsync(Arg.Any<InvokeRequest>());
            response.Should().BeTrue();
        }
        
        [Fact]
        public async Task WhenNotInLocalDictionaryThenWordServiceShouldReturnFalse()
        {
            var word = "sheeps";
            
            _lambda.ClearReceivedCalls();

            var wordResponse = new WordResponseWrapper
            {
                IsSuccessful = false,
                WordResponse = null
            };
            
            var json = JsonConvert.SerializeObject(wordResponse);

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            _lambda.InvokeAsync(Arg.Any<InvokeRequest>()).Returns(new InvokeResponse
            {
                Payload = stream
            });

            var wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            var response = await wordService.GetWordStatus(word);
            
            await _lambda.Received(1).InvokeAsync(Arg.Any<InvokeRequest>());
            response.Should().BeFalse();
        }
        
        [Fact]
        public async Task WhenWordIsNotAPluralButExistsInSingularThenWordServiceShouldReturnFalse()
        {
            var word = "sheeps";
            
            _lambda.ClearReceivedCalls();

            var wordResponse = new WordResponseWrapper
            {
                IsSuccessful = false,
                WordResponse = null
            };
            
            var json = JsonConvert.SerializeObject(wordResponse);

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            _lambda.InvokeAsync(Arg.Any<InvokeRequest>()).Returns(new InvokeResponse
            {
                Payload = stream
            });
            
            var wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            var response = await wordService.GetWordStatus(word);

            await _lambda.Received(1).InvokeAsync(Arg.Any<InvokeRequest>());
            response.Should().BeFalse();
        }
        
        [Fact]
        public async Task WhenAWordFormattedAsPluralDoesNotExistInTheSingularThenWordServiceShouldReturnFalse()
        {
            var word = "notawords";

            _lambda.ClearReceivedCalls();

            var wordResponse = new WordResponseWrapper
            {
                IsSuccessful = false,
                WordResponse = null
            };
            
            var json = JsonConvert.SerializeObject(wordResponse);

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            _lambda.InvokeAsync(Arg.Any<InvokeRequest>()).Returns(new InvokeResponse
            {
                Payload = stream
            });

            var wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            var response = await wordService.GetWordStatus(word);
            response.Should().BeFalse();
        }

        [Fact]
        public async Task WhenWordIsInTheDictionaryThenACallShouldBeMadeToGetTheDefinition()
        {
            var word = "sheep";
            
            _lambda.ClearReceivedCalls();

            var wordResponse = new WordResponseWrapper
            {
                IsSuccessful = true,
                WordResponse = new WordResponse
                {
                    Word = word,
                    Definition = "An absolutely baaing animal",
                    Status = WordStatus.Temporary
                }
            };
            
            var json = JsonConvert.SerializeObject(wordResponse);

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            _lambda.InvokeAsync(Arg.Any<InvokeRequest>()).Returns(new InvokeResponse
            {
                Payload = stream
            });
            
            var wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            var response = await wordService.GetDefinition(word);

            await _lambda.Received(1).InvokeAsync(Arg.Any<InvokeRequest>());
            response.Should().Be("An absolutely baaing animal");
        }
        
        [Fact]
        public async Task WhenWordIsNotInTheDictionaryThenNoCallShouldBeMadeToGetDefinition()
        {
            var word = "sheep";
            
            _lambda.ClearReceivedCalls();
            
            var wordResponse = new WordResponseWrapper
            {
                IsSuccessful = false,
                WordResponse = null
            };
            
            var json = JsonConvert.SerializeObject(wordResponse);

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            _lambda.InvokeAsync(Arg.Any<InvokeRequest>()).Returns(new InvokeResponse
            {
                Payload = stream
            });

            var wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            var response = await wordService.GetDefinition(word);
            
            await _lambda.Received(1).InvokeAsync(Arg.Any<InvokeRequest>());
            response.Should().Be(null);
        }
    }
}