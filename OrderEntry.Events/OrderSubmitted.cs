using EventLib;
using System.Collections.Generic;

namespace OrderEntry.Events
{
    public interface OrderSubmitted : IEvent
    {
        string OrderId { get; set; }
        string CustomerId { get; set; }
        List<Product> Products { get; set; }
    }
}
