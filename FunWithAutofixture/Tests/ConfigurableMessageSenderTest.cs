using Xunit;
using Moq;
using AutoFixture.Xunit2;
using System;

namespace FunWithAutofixture
{
    public class ConfigurableMessageSenderTest
    {
        [Fact]
        public void SendMessage_ServiceAddressIsNull_ThorwsException_Ugly()
        {
            var messageSenderConfiguration = new MessageSenderConfiguration("", "Necati", "Kunduz");
            var smsConfiguration = new SmsSenderConfiguration("", "", "");
            var emailConfiguration = new EmailSenderConfiguration("", "", "");
            var communicationConfiguration = new CommunicationConfiguration(messageSenderConfiguration, emailConfiguration, smsConfiguration);
            var databaseConfiguration = new DatabaseConfiguration();
            var routeConfiguration = new RouteConfiguration();
            var globalConfiguration = new GlobalConfiguration(communicationConfiguration, databaseConfiguration, routeConfiguration);

            var mockLogger = new Mock<ILogger>();
            var sut = new ConfigurableMessageSender(mockLogger.Object, globalConfiguration);
            var message = new Message("", "", true);


            Assert.Throws<Exception>(() => sut.Add(message));
        }

        [Theory]
        [AutoMoqData]
        public void SendMessage_ServiceAddressIsNull_ThorwsException_Beauty(
            [Frozen]MessageSenderConfiguration messageSenderConfiguration,
            Message message,
            ConfigurableMessageSender sut)
        {
            messageSenderConfiguration.ServiceAddress = "";

            Assert.Throws<Exception>(() => sut.Add(message));
        }
    }
}
