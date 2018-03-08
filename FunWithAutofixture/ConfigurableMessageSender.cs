using System.Linq;
using System.Collections.Generic;
using System;

namespace FunWithAutofixture
{
    public class ConfigurableMessageSender
    {
        private readonly ILogger _logger;
        private readonly List<Message> _messages = new List<Message>();
        private readonly MessageSenderConfiguration _messageSenderConfiguration;

        public IReadOnlyCollection<Message> MessageBuffer { get { return _messages; } }

        public ConfigurableMessageSender(ILogger logger, GlobalConfiguration configuration)
        {
            _logger = logger;
            _messageSenderConfiguration = configuration.CommunicationConfiguration.MessageSenderConfiguration;
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
            if (string.IsNullOrEmpty(_messageSenderConfiguration.ServiceAddress))
            {
                throw new Exception("shit.");
            }
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

    public class MessageSenderConfiguration
    {
        public string ServiceAddress { get; internal set; }
        public string Username { get; internal set; }
        public string Password { get; internal set; }

        public MessageSenderConfiguration(string serviceAddress, string username, string password)
        {
            ServiceAddress = serviceAddress;
            Username = username;
            Password = password;
        }
    }
}
