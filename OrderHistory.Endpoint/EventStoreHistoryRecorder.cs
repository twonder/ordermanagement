using BaseMessages.Events;
using EventStore.ClientAPI;
using Infrastructure.EventStore;
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
            var stream = new StreamRepository("order-history");
            stream.RecordEvent(message);

            var fullName = message.GetType().FullName.Replace("__impl", "");
            Console.WriteLine("Recording history: " + message.OrderId);
            Console.WriteLine(fullName);
            Console.WriteLine("----------------------------------------------");
        }
    }
}
