using NServiceBus;
using NServiceBus.Saga;
using OrderEntry.Events;
using OrderProcessing.Commands;
using OrderProcessing.Events;
using Pricing.Events;
using Scheduling.Events;
using System;

namespace OrderProcessing.Backend
{
    public class NewOrderPolicyData : ContainSagaData
    {
        [Unique]
        public virtual string OrderId { get; set; }
        public virtual double? OrderPrice { get; set; }
        public virtual DateTime ScheduledDate { get; set; }
        public virtual int NumberOfCancelTimeouts { get; set; }
    }

    public class NewOrderPolicy : Saga<NewOrderPolicyData>,
        IAmStartedByMessages<OrderSubmitted>,
        IHandleMessages<CancelOrder>,
        IAmStartedByMessages<OrderPriced>,
        IAmStartedByMessages<OrderScheduled>,
        IHandleTimeouts<OrderCancelTimeout>,
        IHandleTimeouts<DelinquentOrderTimeout>
    {
        private const int SecondsToWaitForCancel = 30;
        private const int HoursUntilDelinquent = 4;
        private const int MaxCancelRetries = 2;

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<NewOrderPolicyData> mapper)
        {
            mapper.ConfigureMapping<OrderSubmitted>(order => order.OrderId).ToSaga(policy => policy.OrderId);
            mapper.ConfigureMapping<CancelOrder>(order => order.OrderId).ToSaga(policy => policy.OrderId);
            mapper.ConfigureMapping<OrderPriced>(order => order.OrderId).ToSaga(policy => policy.OrderId);
            mapper.ConfigureMapping<OrderScheduled>(order => order.OrderId).ToSaga(policy => policy.OrderId);
        }
        public void Handle(OrderSubmitted message)
        {
            Data.OrderId = message.OrderId;
            Data.NumberOfCancelTimeouts = 0;

            // schedule the cancel order timeout
            RequestTimeout(TimeSpan.FromSeconds(SecondsToWaitForCancel), new OrderCancelTimeout
            {
                CustomerId = message.CustomerId,
                OrderId = message.OrderId,
                DateOccurred = DateTime.Now
            });

            // make sure that order isn't lost when pricing or scheduling don't respond
            RequestTimeout(TimeSpan.FromHours(HoursUntilDelinquent), new DelinquentOrderTimeout
            {
                CustomerId = message.CustomerId,
                OrderId = message.OrderId,
                DateOccurred = DateTime.Now
            });
        }

        public void Handle(OrderScheduled message)
        {
            Data.OrderId = message.OrderId;
            Data.ScheduledDate = message.ScheduledDate;

            Console.WriteLine("Order scheduled date: " + message.ScheduledDate.ToString("MMMM dd, yyyy") + ".");
            Console.WriteLine("Order Id: " + Data.OrderId);
            Console.WriteLine("---------------------------------");
        }

        public void Handle(OrderPriced message)
        {
            Data.OrderId = message.OrderId;
            Data.OrderPrice = message.Price;

            Console.WriteLine("Order price: $" + message.Price + ".");
            Console.WriteLine("Order Id: " + Data.OrderId);
            Console.WriteLine("---------------------------------");
        }

        public void Timeout(OrderCancelTimeout state)
        {
            Console.WriteLine("---Order Timeout Expired---");
            CompleteIfDone();

            if (!IsComplete() && Data.NumberOfCancelTimeouts < MaxCancelRetries)
            {
                Data.NumberOfCancelTimeouts++;

                // do cancel order timeout again
                RequestTimeout(TimeSpan.FromSeconds(SecondsToWaitForCancel / 2), state);
            }
        }

        public void Timeout(DelinquentOrderTimeout state)
        {
            Console.WriteLine("The order is deliquent, someone needs to investigate.");
            Console.WriteLine("Order Id: " + Data.OrderId);
            Console.WriteLine("---------------------------------");
        }

        public void Handle(CancelOrder message)
        {
            Bus.Publish<OrderCancelled>(o =>
            {
                o.OrderId = Data.OrderId;
                o.Occurred = DateTime.Now;
            });

            Console.WriteLine("Order Cancelled");
            Console.WriteLine("Order Id: " + Data.OrderId);
            Console.WriteLine("---------------------------------");

            MarkAsComplete();
        }

        private bool IsComplete()
        {
            return Data.ScheduledDate != null && Data.OrderPrice != null;
        }

        private void CompleteIfDone()
        {
            if (!IsComplete()) return;

            Bus.Publish<OrderProcessingCompleted>(o =>
            {
                o.OrderId = Data.OrderId;
                o.Price = (double)Data.OrderPrice;
                o.ScheduledDate = Data.ScheduledDate;
                o.Occurred = DateTime.Now;
            });

            Console.WriteLine("Order Completed!");
            Console.WriteLine("Order Id: " + Data.OrderId);
            Console.WriteLine("Price: $" + Data.OrderPrice);
            Console.WriteLine("ScheduledDate: " + Data.ScheduledDate.ToString("MMMM dd, yyyy"));
            Console.WriteLine("Occurred: " + DateTime.Now);
            Console.WriteLine("---------------------------------");

            MarkAsComplete();
        }
    }
}
