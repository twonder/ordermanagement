﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Events
{
    public class DelinquentOrderTimeout
    {
        public DateTime DateOccurred { get; set; }
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public double Amount { get; set; }
    }
}
