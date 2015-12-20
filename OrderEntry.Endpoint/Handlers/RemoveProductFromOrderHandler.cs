using NServiceBus;
using OrderEntry.Commands;
using OrderEntry.Events;
using System;

namespace OrderEntry.Endpoint
{
    public class RemoveProductFromOrderHandler : IHandleMessages<RemoveProductFromOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(RemoveProductFromOrder message)
        {
            Bus.Publish<ProductRemovedFromOrder>(p =>
            {
                p.OrderId = message.OrderId;
                p.ProductId = message.ProductId;
                p.CustomerId = message.CustomerId;
                p.Occurred = DateTime.Now;
            });

            Console.WriteLine("Product removed to order");
            Console.WriteLine("Order Id: " + message.OrderId);
            Console.WriteLine("Customer: " + message.CustomerId);
            Console.WriteLine("ProductId: " + message.ProductId);
        }
    }
}
