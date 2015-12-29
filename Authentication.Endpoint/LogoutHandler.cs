using System;
using Authentication.Commands;
using NServiceBus;
using Authentication.Events;

namespace Authentication.Endpoint
{
    public class LogoutHandler : IHandleMessages<Logout>
    {
        public IBus Bus { get; set; }

        public void Handle(Logout message)
        {
            Bus.Publish<LoggedOut>(l =>
            {
                l.Id = message.Id;
                l.Occurred = DateTime.Now;
            });
        }
    }
}
