
namespace OrderHistory.Endpoint
{
    using NServiceBus;
    using System;
    
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            Console.Title = "Order History";

            configuration.EndpointName("OrderManagement.OrderHistory");

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<JsonSerializer>();

            // specify what the commands and events can be recognized by
            configuration.Conventions().DefiningCommandsAs(e => e.Namespace != null & e.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(e => e.Namespace != null & e.Namespace.EndsWith("Events"));
        }
    }
}
