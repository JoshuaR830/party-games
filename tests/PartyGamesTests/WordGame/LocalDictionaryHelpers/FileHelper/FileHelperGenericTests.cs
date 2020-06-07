using System;
using System.IO;
using System.Net;
using Chat.WordGame;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.LocalDictionaryHelpers.FileHelper
{
    public class FileHelperGenericTests : IDisposable
    {
        public readonly Chat.WordGame.LocalDictionaryHelpers.FileHelper FileHelper;
        public const string Filename = "./generic-file-tests.json";
        
        public FileHelperGenericTests()
        {
            var filenameHelper = Substitute.For<IFilenameHelper>();
            
            filenameHelper
                .GetDictionaryFilename()
                .Returns(Filename);
            
            FileHelper = new Chat.WordGame.LocalDictionaryHelpers.FileHelper(filenameHelper);
        }

        [Fact]
        public void WhenWritingAnObjectToTheFileTheResponseShouldBeDeserializableToTheObjectWritten()
        {
            var data = new GuessedWords();
            data.AddWord("run", WordStatus.Permanent);
            data.AddWord("running", WordStatus.Suffix);

            FileHelper.WriteFile(Filename, data);

            var response = TestFileHelper.Read(Filename);
            
            var serializedObject = JsonConvert.SerializeObject(data);
            response.Should().BeEquivalentTo(serializedObject);
        }

        [Fact]
        public void WhenReadingAnObjectFromTheFileResponseShouldBeDeserializableToTheObjectWritten()
        {
            var data = new GuessedWords();
            data.AddWord("run", WordStatus.Permanent);
            data.AddWord("running", WordStatus.Suffix);

            TestFileHelper.CreateCustomFile(Filename, data);

            var response = FileHelper.ReadFile(Filename);
            
            var serializedObject = JsonConvert.SerializeObject(data);
            response.Should().BeEquivalentTo(serializedObject);
        }

        public void Dispose()
        {
            if (File.Exists(Filename))
                File.Delete(Filename);
        }
    }
}