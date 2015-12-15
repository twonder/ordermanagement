
namespace OrderEntry.Endpoint
{
    using AutoMapper;
    using NServiceBus;
    using Commands;
    using System;
    using Events;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            Console.Title = "Order Entry";

            configuration.EndpointName("OrderManagement.OrderEntry");

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<JsonSerializer>();

            // specify what the commands and events can be recognized by
            configuration.Conventions().DefiningCommandsAs(e => e.Namespace != null & e.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(e => e.Namespace != null & e.Namespace.EndsWith("Events"));

            Mapper.CreateMap<SubmitOrder, OrderSubmitted>();
            Mapper.CreateMap<Commands.Product, Events.Product>();
        }
    }
}
