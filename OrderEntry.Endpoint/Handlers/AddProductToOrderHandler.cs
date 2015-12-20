using System;
using NServiceBus;
using OrderEntry.Commands;
using OrderEntry.Events;

namespace OrderEntry.Endpoint
{
    public class AddProductToOrderHandler : IHandleMessages<AddProductToOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(AddProductToOrder message)
        {
            Bus.Publish<ProductAddedToOrder>(p =>
            {
                p.OrderId = message.OrderId;
                p.ProductId = message.ProductId;
                p.CustomerId = message.CustomerId;
                p.Occurred = DateTime.Now;
            });

            Console.WriteLine("Product added to order");
            Console.WriteLine("Order Id: " + message.OrderId);
            Console.WriteLine("Customer: " + message.CustomerId);
            Console.WriteLine("ProductId: " + message.ProductId);
        }
    }
}
