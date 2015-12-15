using EventLib;

namespace Pricing.Events
{
    public interface OrderPriced : IOrderEvent
    {
        double Price { get; set; }
    }
}
