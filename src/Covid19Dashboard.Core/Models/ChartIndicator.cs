﻿using System;

namespace Covid19Dashboard.Core.Models
{
    public class ChartIndicator
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public object Value { get; set; }
    }
}
