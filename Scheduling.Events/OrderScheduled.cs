using EventLib;
using System;

namespace Scheduling.Events
{
    public interface OrderScheduled : IEvent
    {
        string OrderId { get; set; }
        DateTime ScheduledDate { get; set; }
    }
}
