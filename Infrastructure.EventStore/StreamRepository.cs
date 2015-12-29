using EventStore.ClientAPI;
using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Infrastructure.EventStore
{
    public class StreamRepository
    {
        private string _streamId;

        public StreamRepository(string streamId)
        {
            _streamId = streamId;
        }

        public void RecordEvent(object eventMessage)
        {
            var fullName = eventMessage.GetType().FullName.Replace("__impl", "");
            var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));

            // Don't forget to tell the connection to connect!
            connection.ConnectAsync().Wait();

            var myEvent = new EventData(Guid.NewGuid(),
                                        fullName,
                                        true,
                                        Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(eventMessage)),
                                        Encoding.UTF8.GetBytes(""));

            connection.AppendToStreamAsync(_streamId, ExpectedVersion.Any, myEvent).Wait();
        }
    }
}
