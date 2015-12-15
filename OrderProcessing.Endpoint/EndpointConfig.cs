
namespace OrderProcessing.Endpoint
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using System;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            Console.Title = "Order Processing";

            configuration.EndpointName("OrderManagement.OrderProcessing");

            configuration.UsePersistence<NHibernatePersistence>()
                .ConnectionString(@"data source=.\SQLEXPRESS;Database=OrderProcessing;Integrated Security=SSPI");
            configuration.UseSerialization<JsonSerializer>();

            // specify what the commands and events can be recognized by
            configuration.Conventions().DefiningCommandsAs(e => e.Namespace != null & e.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(e => e.Namespace != null & e.Namespace.EndsWith("Events"));
        }
    }
}
