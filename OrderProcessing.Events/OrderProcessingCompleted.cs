using BaseMessages.Events;
using System;

namespace OrderProcessing.Events
{
    public interface OrderProcessingCompleted : IOrderEvent
    {
        double Price { get; set; }
        DateTime ScheduledDate { get; set; }
    }
}
