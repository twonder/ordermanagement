using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLib
{
    public interface IOrderEvent : IEvent
    {
        string OrderId { get; set; }
    }
}
