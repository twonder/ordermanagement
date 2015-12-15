
namespace OrderEntry.Endpoint
{
    using AutoMapper;
    using NServiceBus;
    using Commands;
    using System;
    using Events;
    using NServiceBus.Persistence;

    
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            Console.Title = "Order Entry";

            configuration.EndpointName("OrderManagement.OrderEntry");

            configuration.UsePersistence<NHibernatePersistence>()
                .ConnectionString(@"data source=.\SQLEXPRESS;Database=OrderManagement.OrderEntry;Integrated Security=SSPI");
            configuration.UseSerialization<JsonSerializer>();

            // specify what the commands and events can be recognized by
            configuration.Conventions().DefiningCommandsAs(e => e.Namespace != null & e.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(e => e.Namespace != null & e.Namespace.EndsWith("Events"));

            Mapper.CreateMap<SubmitOrder, OrderSubmitted>();
            Mapper.CreateMap<Commands.Product, Events.Product>();
        }
    }
}
