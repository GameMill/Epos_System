using Epos.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Epos.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public List<Models.ProductForCart> Cart { get; set; } = new List<Models.ProductForCart>();
        public bool HasCheckoutComplate { get; set; } = false;
        public DateTime Order { get; set; }
        public DateTime LastUpdated { get; set; }

        internal Sale toSale()
        {
            return new Sale()
            {
                Cart = new ObservableCollection<ProductForCart>(Cart),
                InvoiceNumber = Id,
                HasCheckoutComplate = HasCheckoutComplate,
                DateSold = Order
            };
        }
    }
}
