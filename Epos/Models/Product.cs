using System;
using System.Collections.Generic;
using System.Text;

namespace Epos.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public bool SnNeed { get; set; }
        public double Price { get; set; }
        
    }
}
