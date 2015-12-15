using System;
using NServiceBus;
using OrderEntry.Commands;
using OrderEntry.Events;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace OrderEntry.Endpoint
{
    public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(SubmitOrder message)
        {
            var orderId = Guid.NewGuid().ToString(); 
            // maybe save it in the database

            Bus.Publish<OrderSubmitted>(o =>
            {
                o.OrderId = orderId;
                o.CustomerId = message.CustomerId;
                o.Products = Mapper.Map<List<Commands.Product>, List<Events.Product>>(message.Products);
                o.Occurred = DateTime.Now;
            });

            Console.WriteLine("Order submitted!");
            Console.WriteLine("Order Id: " + orderId);
            Console.WriteLine("Customer: " + message.CustomerId);
            Console.WriteLine("Products: " + String.Join(",", message.Products.Select(p => p.Name)));
            Console.WriteLine("---------------------------------");
        }
    }
}
