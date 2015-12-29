using NServiceBus;
using NServiceBus.Persistence;
using System;

namespace Authentication.Endpoint
{
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            Console.Title = "Authentication";

            configuration.EndpointName("OrderManagement.Authentication");

            configuration.UsePersistence<NHibernatePersistence>()
                .ConnectionString(@"data source=.\SQLEXPRESS;Database=OrderManagement.Authentication;Integrated Security=SSPI");
            configuration.UseSerialization<JsonSerializer>();

            // specify what the commands and events can be recognized by
            configuration.Conventions().DefiningCommandsAs(e => e.Namespace != null & e.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(e => e.Namespace != null & e.Namespace.EndsWith("Events"));
        }
    }
}
