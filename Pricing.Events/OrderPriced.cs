using BaseMessages.Events;

namespace Pricing.Events
{
    public interface OrderPriced : IOrderEvent
    {
        double Price { get; set; }
    }
}
