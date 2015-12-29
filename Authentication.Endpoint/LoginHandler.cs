using System;
using Authentication.Commands;
using NServiceBus;
using Authentication.Events;

namespace Authentication.Endpoint
{
    public class LoginHandler : IHandleMessages<Login>
    {
        public IBus Bus { get; set; }

        public void Handle(Login message)
        {
            Bus.Publish<LoggedIn>(l =>
            {
                l.Id = message.Id;
                l.Occurred = DateTime.Now;
            });
        }
    }
}
