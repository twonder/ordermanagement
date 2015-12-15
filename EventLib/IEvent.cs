using System;

namespace EventLib
{
    public interface IEvent : IMessage
    {
        DateTime Occurred { get; set; }
    }
}
