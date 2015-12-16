using System;
using NServiceBus;
using OrderEntry.Events;
using Pricing.Events;
using System.Threading;

namespace Pricing.Endpoint
{
    public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
    {
        public IBus Bus { get; set; }
        public void Handle(OrderSubmitted message)
        {
            throw new NullReferenceException();

            // go look up the price
            Random random = new Random();
            var price = Math.Round(random.NextDouble() * (10000 - 1000) + 1000, 2);
            Thread.Sleep(5000);

            // now that we got the date, publish the event
            Bus.Publish<OrderPriced>(o =>
            {
                o.OrderId = message.OrderId;
                o.Price = price;
                o.Occurred = DateTime.Now;
            });
            Console.WriteLine("Order priced!");
            Console.WriteLine("Order Id: " + message.OrderId);
            Console.WriteLine("Price: $" + price);
            Console.WriteLine("---------------------------------");
        }
    }
}
