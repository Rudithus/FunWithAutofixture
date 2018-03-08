using Xunit;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using AutoFixture.Xunit2;
using FluentAssertions;

namespace FunWithAutofixture
{
    public class HappyTest
    {

        [Theory]
        [AutoMoqData]
        public void AddMessage_ShouldAddMessagesToBuffer(
            Message message1,
            Message message2,
            MessageSender sut)
        {
            sut.Add(message1);
            sut.Add(message2);

            sut.UnsentMessageCount.Should().Be(2);
            sut.MessageBuffer.Should().Contain(message1);
            sut.MessageBuffer.Should().Contain(message2);
        }

        [Theory]
        [AutoMoqData]
        public void SendMessage_ShouldBeLogged_BufferShouldBeEmpty(
           [Frozen]Mock<ILogger> logger,
            Message message1,
            Message message2,
            MessageSender sut)
        {
            message1.IsImportant = true;
            message2.IsImportant = false;

            sut.Add(message1);
            sut.Add(message2);

            sut.SendAll();

            logger.Verify(l => l.LogMessage(It.IsAny<string>()), Times.Once);
            logger.Verify(l => l.LogAsImportant(It.IsAny<string>()), Times.Once);

            sut.MessageBuffer.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void SendLimited_ShouldSendExactNumberOfMesssages_ShouldRemoveExactNumberOfMessagesFromBuffer(
            [Frozen]Mock<ILogger> logger,
            Message message1,
            Message message2,
            Message message3,
            MessageSender sut)
        {
            message1.IsImportant = true;
            message2.IsImportant = false;

            sut.Add(message1);
            sut.Add(message2);
            sut.Add(message3);

            sut.SendLimited(2);

            logger.Verify(l => l.LogMessage(It.IsAny<string>()), Times.Once);
            logger.Verify(l => l.LogAsImportant(It.IsAny<string>()), Times.Once);
            sut.MessageBuffer.Count.Should().Be(1);
        }
    }



    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() =>
            new Fixture()
                .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
