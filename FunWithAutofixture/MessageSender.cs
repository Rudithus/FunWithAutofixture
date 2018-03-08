using System.Linq;
using System.Collections.Generic;

namespace FunWithAutofixture
{
    public class MessageSender
    {
        private readonly ILogger _logger;
        private readonly List<Message> _messages = new List<Message>();
        public IReadOnlyCollection<Message> MessageBuffer { get { return _messages; } }

        public MessageSender(ILogger logger)
        {
            _logger = logger;
        }

        public void SendAll()
        {
            _messages.ForEach(m => Send(m));
            _messages.Clear();
        }
        private void Send(Message message)
        {
            if (message.IsImportant)
            {
                _logger.LogAsImportant(message.ToAddress);
            }
            else
            {
                _logger.LogMessage(message.ToAddress);
            }
        }
        public void Add(Message message)
        {
            _messages.Add(message);
        }
        public int UnsentMessageCount
        {
            get { return MessageBuffer.Count; }
        }
        public void SendLimited(int numberOfMessagesToSend)
        {
            var messageList = _messages.Take(numberOfMessagesToSend).ToList();
            messageList.ForEach(m => Send(m));
            _messages.RemoveRange(0, numberOfMessagesToSend);
        }
    }
}
