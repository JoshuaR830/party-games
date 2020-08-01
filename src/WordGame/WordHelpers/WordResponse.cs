namespace Chat.WordGame.WordHelpers
{
    public class WordResponseWrapper
    {
        public bool IsSuccessful { get; set; }
        public WordResponse WordResponse { get; set; }
    }
    
    public class WordResponse
    {
        public string Word { get; set; }
        public string Definition { get; set; }
        public WordStatus Status { get; set; }
    }
}