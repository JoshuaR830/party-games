using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat.DatabasePopulator
{
    public class AddItemsToDatabase : IAddItemsToDatabase
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly IFilenameHelper _fileNameHelper;
        private readonly IFileHelper _fileHelper;

        public AddItemsToDatabase(IAmazonDynamoDB dynamoDb, IFilenameHelper fileNameHelper, IFileHelper fileHelper)
        {
            _dynamoDb = dynamoDb;
            _fileNameHelper = fileNameHelper;
            _fileHelper = fileHelper;
        }

        public async Task SetDatabaseToUpdate()
        {
            var wordDictionary = _fileHelper.ReadDictionary(_fileNameHelper.GetDictionaryFilename());
            var count = 0;
            Console.WriteLine("Start");
            foreach (var wordData in wordDictionary.Words)
            {
                if (count == 12)
                {
                    System.Threading.Thread.Sleep(1000);
                    count = 0;
                    Console.WriteLine("");
                }
                Console.WriteLine(wordData.Word);
                await UpdateDatabase(wordData.Word, wordData.TemporaryDefinition, wordData.PermanentDefinition, wordData.Status, wordData.Category);
                count++;
            }
        }

        public async Task UpdateDatabase(string word, string tempDefinition, string permDefinition, WordStatus status, WordCategory category)
        {

            if (string.IsNullOrEmpty(word))
                return;
            
            if(status == WordStatus.DoesNotExist)
                return;

            var item = new Dictionary<string, AttributeValue>();
            
            item.Add("Word", new AttributeValue { S = word });

            
            if(!string.IsNullOrEmpty(tempDefinition))
                item.Add("TemporaryDefinition", new AttributeValue { S = tempDefinition });
            
            if(!string.IsNullOrEmpty(permDefinition))
                item.Add("PermanentDefinition", new AttributeValue { S = permDefinition });
            
            item.Add("Status", new AttributeValue { S = (status == 0? WordStatus.Temporary.ToString() : status.ToString())});

            var wordRequest = new PutItemRequest
            {
                TableName = "WordTable",
                Item = item
            };
            
            await _dynamoDb.PutItemAsync(wordRequest);

            if (category == WordCategory.None)
                return;
            
            var categoryRequest = new PutItemRequest
            {
                TableName = "CategoryTable",
                Item = new Dictionary<string, AttributeValue>
                {
                    {"Word", new AttributeValue { S = word }},
                    {"Category", new AttributeValue { S = category.ToString() }},
                }
            };

            
            await _dynamoDb.PutItemAsync(categoryRequest);
        }
    }
}