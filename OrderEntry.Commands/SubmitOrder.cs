using EventLib;
using System;
using System.Collections.Generic;

namespace OrderEntry.Commands
{
    public interface SubmitOrder : ICommand
    {
        string CustomerId { get; set; }
        List<Product> Products { get; set; }
    }
}
