namespace FunWithAutofixture
{
    public class Message
    {
        public string ToAddress { get; internal set; }
        public string Content { get; internal set; }
        public bool IsImportant { get; internal set; }
        public Message(string toAddress, string content, bool isImportant)
        {
            ToAddress = toAddress;
            Content = content;
            IsImportant = isImportant;
        }

    }
}
