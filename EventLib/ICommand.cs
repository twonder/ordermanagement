using System;

namespace EventLib
{
    public interface ICommand : IMessage
    {
        DateTime DateSent { get; set; }
    }
}
