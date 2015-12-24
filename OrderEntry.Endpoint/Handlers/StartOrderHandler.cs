using System;
using NServiceBus;
using OrderEntry.Commands;
using OrderEntry.Events;

namespace OrderEntry.Endpoint
{
    public class StartOrderHandler : IHandleMessages<StartOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(StartOrder message)
        {
            Bus.Publish<OrderStarted>(o =>
            {
                o.OrderId = message.OrderId;
                o.CustomerId = message.CustomerId;
                o.Occurred = DateTime.Now;
            });

            Console.WriteLine("Order started!");
            Console.WriteLine("Order Id: " + message.OrderId);
            Console.WriteLine("Customer: " + message.CustomerId);
        }
    }
}
