using Authentication.Events;
using NServiceBus;
using Infrastructure.EventStore;

namespace Authentication.Endpoint
{
    public class Recorder : IHandleMessages<IAuth>
    {
        public void Handle(IAuth message)
        {
            var stream = new StreamRepository("authentication");
            stream.RecordEvent(message);
        }
    }
}
