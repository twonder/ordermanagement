using BaseMessages.Events;
using System;

namespace Scheduling.Events
{
    public interface OrderScheduled : IOrderEvent
    {
        DateTime ScheduledDate { get; set; }
    }
}
