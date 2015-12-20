using BaseMessages.Events;
using EventStore.ClientAPI;
using NServiceBus;
using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace OrderHistory.Endpoint
{
    public class EventStoreHistoryRecorder : IHandleMessages<IOrderEvent>
    {
        public void Handle(IOrderEvent message)
        {
            var fullName = message.GetType().FullName.Replace("__impl", "");
            var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));

            // Don't forget to tell the connection to connect!
            connection.ConnectAsync().Wait();

            var myEvent = new EventData(Guid.NewGuid(), 
                                        fullName, 
                                        true, 
                                        Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(message)), 
                                        Encoding.UTF8.GetBytes(""));

            connection.AppendToStreamAsync("orders-stream", ExpectedVersion.Any, myEvent).Wait();

            Console.WriteLine("Recording history: " + message.OrderId);
            Console.WriteLine(fullName);
            Console.WriteLine("----------------------------------------------");
        }
    }
}
