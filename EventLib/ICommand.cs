using System;

namespace BaseMessages.Commands
{
    public interface ICommand : IMessage
    {
        DateTime DateSent { get; set; }
    }
}
