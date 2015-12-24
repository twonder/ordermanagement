using System;
using NServiceBus;
using OrderEntry.Events;
using Scheduling.Events;
using System.Threading;

namespace Scheduling.Endpoint
{
    public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
    {
        public IBus Bus { get; set; }
        public void Handle(OrderSubmitted message)
        {
            // go look up the schedule date, it takes a little bit
            Thread.Sleep(6000);

            var scheduledDate = DateTime.Now.AddDays(7);

            // now that we got the date, publish the event
            Bus.Publish<OrderScheduled>(o =>
            {
                o.OrderId = message.OrderId;
                o.CustomerId = message.CustomerId;
                o.ScheduledDate = scheduledDate;
                o.Occurred = DateTime.Now;
            });
            Console.WriteLine("Order scheduled!");
            Console.WriteLine("Order Id: " + message.OrderId);
            Console.WriteLine("Scheduled Date: " + scheduledDate.ToString("MMMM dd, yyyy"));
            Console.WriteLine("---------------------------------");
        }
    }
}
