using System;
using System.Net;
using Chat.WordGame.WebHelpers;
using FluentAssertions;
using Xunit;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace PartyGamesTests.WordGame.WebHelpers
{
    public class WebDictionaryRequestHelperTests
    {
        [Fact]
        public void WhenWordDoesNotExistExistenceRequestShouldReturnFalse()
        {
            var url = "https://www.dictionary.com/browse";
            var word = "word-that-doesnt-exist";
            var webRequestHelper = Substitute.For<IWebRequestHelper>();

            webRequestHelper
                .Create($"{url}/{word}")
                .Throws(new Exception());

            var webDictionaryRequestHelper = new WebDictionaryRequestHelper(webRequestHelper);
            var response = webDictionaryRequestHelper.MakeExistenceRequest(word);

            response.Should().BeFalse();
        }

        [Fact]
        public void WhenWordExistsExistenceRequestShouldReturnTrue()
        {
            var url = "https://www.dictionary.com/browse";
            var word = "word-that-doesnt-exist";
            var webRequestHelper = Substitute.For<IWebRequestHelper>();

            webRequestHelper
                .Create(url)
                .Returns(WebRequest.CreateDefault(new Uri(url)));
            
            var webDictionaryRequestHelper = new WebDictionaryRequestHelper(webRequestHelper);
            var response = webDictionaryRequestHelper.MakeExistenceRequest(word);

            response.Should().BeTrue();
        }
    }
}