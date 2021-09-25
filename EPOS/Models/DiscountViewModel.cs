using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPOS.Models
{
    public class DiscountViewModel
    {
        public int ID { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public int QTY { get; set; } = 1;
    }
}
