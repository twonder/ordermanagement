using BaseMessages.Events;

namespace OrderEntry.Events
{
    public interface IProductEvent : IOrderEvent
    {
        string ProductId { get; set; }
        string CustomerId { get; set; }
    }
}
