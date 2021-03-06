﻿using BaseMessages.Commands;
using System;
using System.Collections.Generic;

namespace OrderEntry.Commands
{
    public interface SubmitOrder : ICommand
    {
        string OrderId { get; set; }
        string CustomerId { get; set; }
        List<Product> Products { get; set; }
    }
}
