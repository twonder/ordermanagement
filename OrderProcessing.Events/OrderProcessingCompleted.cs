﻿using BaseMessages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Events
{
    public interface OrderProcessingCompleted : IOrderEvent
    {
        double Price { get; set; }
        DateTime ScheduledDate { get; set; }
    }
}
