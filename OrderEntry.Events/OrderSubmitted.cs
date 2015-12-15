using EventLib;
using System.Collections.Generic;

namespace OrderEntry.Events
{
    public interface OrderSubmitted : IOrderEvent
    {
        string CustomerId { get; set; }
        List<Product> Products { get; set; }
    }
}
