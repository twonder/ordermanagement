using System;

namespace BaseMessages.Events
{
    public interface IEvent : IMessage
    {
        DateTime Occurred { get; set; }
    }
}
