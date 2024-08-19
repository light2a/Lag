using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkteaForFree
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ItemsSummary { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
