using EventLib;

namespace OrderProcessing.Events
{
    public interface OrderCancelled : IEvent
    {
        string OrderId { get; set; }
    }
}
