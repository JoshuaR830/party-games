using System.Collections.Generic;
using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.Pixenary
{
    public interface IWordCategoryHelper
    {
        List<WordData> GetAllWordsWithCategories();
        List<string> GetCategoryNames();
    }
}