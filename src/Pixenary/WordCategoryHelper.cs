using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat.Pixenary
{
    public class WordCategoryHelper : IWordCategoryHelper
    {
        private IFilenameHelper _fileNameHelper;
        private readonly IFileHelper _fileHelper;

        public WordCategoryHelper(IFileHelper fileHelper, IFilenameHelper fileNameHelper)
        {
            _fileHelper = fileHelper;
            _fileNameHelper = fileNameHelper;
        }
        
        public List<WordData> GetAllWordsWithCategories()
        {
            return _fileHelper.ReadDictionary(_fileNameHelper.GetDictionaryFilename())
                .Words
                .Where(x => x.Category != WordCategory.None)
                .ToList();
        }

        public List<string> GetCategoryNames()
        {
            return Enum.GetNames(typeof(WordCategory))
                .Where(x => x != "None")
                .ToList();
        }
    }
}