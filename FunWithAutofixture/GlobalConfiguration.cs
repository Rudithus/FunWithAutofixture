namespace FunWithAutofixture
{
    public class GlobalConfiguration
    {
        public GlobalConfiguration(CommunicationConfiguration communicationConfiguration, DatabaseConfiguration databaseConfiguration, RouteConfiguration routeConfiguration)
        {
            CommunicationConfiguration = communicationConfiguration;
            DatabaseConfiguration = databaseConfiguration;
            RouteConfiguration = routeConfiguration;
        }

        public CommunicationConfiguration CommunicationConfiguration { get; internal set; }
        public DatabaseConfiguration DatabaseConfiguration { get; internal set; }
        public RouteConfiguration RouteConfiguration { get; internal set; }
    }

    public class DatabaseConfiguration
    {
    }

    public class RouteConfiguration
    {
    }

    public class CommunicationConfiguration
    {
        public CommunicationConfiguration(MessageSenderConfiguration messageSenderConfiguration, EmailSenderConfiguration emailSenderConfiguration, SmsSenderConfiguration smsSenderConfiguration)
        {
            MessageSenderConfiguration = messageSenderConfiguration;
            EmailSenderConfiguration = emailSenderConfiguration;
            SmsSenderConfiguration = smsSenderConfiguration;
        }

        public MessageSenderConfiguration MessageSenderConfiguration { get; internal set; }
        public EmailSenderConfiguration EmailSenderConfiguration { get; internal set; }
        public SmsSenderConfiguration SmsSenderConfiguration { get; internal set; }

    }

    public class SmsSenderConfiguration
    {
        public SmsSenderConfiguration(string setting1, string setting2, string setting3)
        {
            Setting1 = setting1;
            Setting2 = setting2;
            Setting3 = setting3;
        }

        public string Setting1 { get; internal set; }
        public string Setting2 { get; internal set; }
        public string Setting3 { get; internal set; }
    }

    public class EmailSenderConfiguration
    {
        public EmailSenderConfiguration(string setting1, string setting2, string setting3)
        {
            Setting1 = setting1;
            Setting2 = setting2;
            Setting3 = setting3;
        }

        public string Setting1 { get; internal set; }
        public string Setting2 { get; internal set; }
        public string Setting3 { get; internal set; }
    }
}
