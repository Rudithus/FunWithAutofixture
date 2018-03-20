using FluentAssertions;
using Moq;
using Xunit;

namespace FunWithAutofixture.Tests
{
    public class UnHappyTest
    {
        private readonly MessageSender _sut;
        private readonly Mock<ILogger> _logger;
        public UnHappyTest()
        {
            _logger = new Mock<ILogger>();
            _sut = new MessageSender(_logger.Object);
        }

        [Fact]
        public void AddMessage_ShouldAddMessagesToBuffer()
        {
            var unimportantMessage = new Message("cozer@hurriyet.com.tr", "Awesome session bro!", false);
            var importantMessage = new Message("custa@hurriyet.com.tr", "Kadiköy sensiz çekilmiyor :(", true);

            _sut.Add(unimportantMessage);
            _sut.Add(importantMessage);

            _sut.UnsentMessageCount.Should().Be(2);
            _sut.MessageBuffer.Should().Contain(unimportantMessage);
            _sut.MessageBuffer.Should().Contain(importantMessage);
        }

        [Fact]
        public void SendMessage_ShouldBeLogged_BufferShouldBeEmpty()
        {
            var unimportantMessage = new Message("cozer@hurriyet.com.tr", "Awesome session bro!", false);
            var importantMessage = new Message("custa@hurriyet.com.tr", "Kadiköy sensiz çekilmiyor :(", true);

            _sut.Add(unimportantMessage);
            _sut.Add(importantMessage);

            _sut.SendAll();

            _logger.Verify(l => l.LogMessage(unimportantMessage.ToAddress), Times.Once);
            _logger.Verify(l => l.LogAsImportant(importantMessage.ToAddress), Times.Once);

            _sut.MessageBuffer.Should().BeEmpty();
        }

        [Fact]
        public void SendLimited_ShouldSendExactNumberOfMesssages_ShouldRemoveExactNumberOfMessagesFromBuffer()
        {
            var unimportantMessage1 = new Message("cozer@hurriyet.com.tr", "Awesome session bro!", false);
            var importantMessage = new Message("necati@necati.com.tr", "I wish there was an easier way of doing this", true);
            var unimportantMessage2 = new Message("custa@hurriyet.com.tr", "Kadiköy sensiz çekilmiyor :(", false);


            _sut.Add(unimportantMessage1);
            _sut.Add(importantMessage);
            _sut.Add(unimportantMessage2);

            _sut.SendLimited(2);

            _logger.Verify(l => l.LogMessage(unimportantMessage1.ToAddress), Times.Once);
            _logger.Verify(l => l.LogAsImportant(importantMessage.ToAddress), Times.Once);
            _sut.MessageBuffer.Count.Should().Be(1);
        }
    }
}
