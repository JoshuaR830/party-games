using System.Net;
using Chat.WordGame.WebHelpers;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.WordGame.WebHelpers
{
    public class WebRequestHelperTests
    {
        [Fact]
        public void TestRequest()
        {
            var url = "https://www.google.com";
            var webRequestHelper = new WebRequestHelper();
            
            var request = webRequestHelper.Create(url);

            request.Should().NotBeNull();
        }

        [Fact]
        public void TestResponse()
        {
            var url = "https://www.google.com";
            var webRequestHelper = new WebRequestHelper();

            var request = webRequestHelper.Create(url);
            var response = webRequestHelper.GetResponse(request);

            response.Should().BeOfType<HttpWebResponse>();
        }
    }
}