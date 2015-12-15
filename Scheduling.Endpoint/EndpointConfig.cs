
namespace Scheduling.Endpoint
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using System;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            Console.Title = "Scheduling";

            configuration.EndpointName("OrderManagement.Scheduling");

            configuration.UsePersistence<NHibernatePersistence>()
                .ConnectionString(@"data source=.\SQLEXPRESS;Database=OrderManagement.Scheduling;Integrated Security=SSPI");
            configuration.UseSerialization<JsonSerializer>();

            // specify what the commands and events can be recognized by
            configuration.Conventions().DefiningCommandsAs(e => e.Namespace != null & e.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(e => e.Namespace != null & e.Namespace.EndsWith("Events"));
        }
    }
}
