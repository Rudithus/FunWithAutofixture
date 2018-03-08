using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using System.Linq;
using Xunit;

namespace FunWithAutofixture
{
    public class SlightlyHappyTest
    {
        private readonly MessageSender _sut;
        private readonly IFixture _fixture;
        private readonly Mock<ILogger> _logger;

        public SlightlyHappyTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _logger = _fixture.Freeze<Mock<ILogger>>();
            _sut = _fixture.Create<MessageSender>();
        }

        [Fact]
        public void AddMessage_ShouldAddMessagesToBuffer()
        {
            var message1 = _fixture.Create<Message>();
            var message2 = _fixture.Create<Message>();

            _sut.Add(message1);
            _sut.Add(message2);

            _sut.UnsentMessageCount.Should().Be(2);
            _sut.MessageBuffer.Should().Contain(message1);
            _sut.MessageBuffer.Should().Contain(message2);
        }

        [Fact]
        public void SendMessage_ShouldBeLogged_BufferShouldBeEmpty()
        {
            var imporantMessage = _fixture.Create<Message>();
            imporantMessage.IsImportant = true;
            var unimportantMessage = _fixture.Create<Message>();
            unimportantMessage.IsImportant = false;

            _sut.Add(imporantMessage);
            _sut.Add(unimportantMessage);

            _sut.SendAll();

            _logger.Verify(l => l.LogMessage(It.IsAny<string>()), Times.Once);
            _logger.Verify(l => l.LogAsImportant(It.IsAny<string>()), Times.Once);

            _sut.MessageBuffer.Should().BeEmpty();
        }

        [Fact]
        public void SendLimited_ShouldSendExactNumberOfMesssages_ShouldRemoveExactNumberOfMessagesFromBuffer()
        {
            var messages = _fixture.CreateMany<Message>(3).ToList();

            messages[0].IsImportant = true;
            messages[1].IsImportant = false;

            messages.ForEach(m => _sut.Add(m));

            _sut.SendLimited(2);


            _logger.Verify(l => l.LogMessage(It.IsAny<string>()), Times.Once);
            _logger.Verify(l => l.LogAsImportant(It.IsAny<string>()), Times.Once);

            _sut.MessageBuffer.Count.Should().Be(1);
        }
    }
}
