namespace Chat.Letters
{
    public class WordAcceptability
    {
        public bool IsAcceptable { get; private set; }
        public string Word { get; private set; }

        public WordAcceptability(string word, bool isAcceptable)
        {
            this.IsAcceptable = isAcceptable;
            this.Word = word;
        }
    }

    public class Dictionary
    {
        
    }
}