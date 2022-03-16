using System;
using System.Collections.Generic;
using System.Text;

namespace CSVReaderUpwork.ConsoleApp
{
    public class InputModel
    {
        public DateTime OrderDate { get; set; }
        public string Region { get; set; }
        public string Rep { get; set; }
        public string Item { get; set; }
        public int Units { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get { return UnitCost * Units; } }
    }
    
    public class OutputModel
    {
        public DateTime LastOrderDate { get; set; }
        public string Region { get; set; }
        public int TotalUnits { get; set; }
        public decimal TotalCost { get; set; }
    }
}
