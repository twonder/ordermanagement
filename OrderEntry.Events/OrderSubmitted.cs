using BaseMessages.Events;
using System.Collections.Generic;

namespace OrderEntry.Events
{
    public interface OrderSubmitted : IOrderEvent
    {
        List<Product> Products { get; set; }
    }
}
