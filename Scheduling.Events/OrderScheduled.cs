using EventLib;
using System;

namespace Scheduling.Events
{
    public interface OrderScheduled : IOrderEvent
    {
        DateTime ScheduledDate { get; set; }
    }
}
