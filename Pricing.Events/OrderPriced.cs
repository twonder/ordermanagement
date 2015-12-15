using EventLib;

namespace Pricing.Events
{
    public interface OrderPriced : IEvent
    {
        string OrderId { get; set; }
        double Price { get; set; }
    }
}
