﻿namespace BaseMessages.Events
{
    public interface IOrderEvent : IEvent
    {
        string OrderId { get; set; }
        string CustomerId { get; set; }
    }
}
