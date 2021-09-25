using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Epos.Models
{
    public class ProductForCart : INotifyPropertyChanged
    {
        private int _qty = 0;
        public int QTY { get { return _qty; } set { _qty = value; NotifyPropertyChanged(); NotifyPropertyChanged("TotalPriceFormatted"); } }
        public string MixAndMatch { get; set; }

        public string TotalPriceFormatted { get { return "£" + TotalPrice; } }
        public double TotalPrice{ get { return Price * QTY; } }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public bool SnNeed { get; set; }
        public string SN { get; set; }

        private double _price = 0;
        public double Price { get { return _price; } set { _price = value; NotifyPropertyChanged(); NotifyPropertyChanged("TotalPriceFormatted"); } }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static ProductForCart LoadFromProduct(Product product)
        {
            return new ProductForCart()
            {
                Id = product.Id,
                Price = product.Price,
                ProductName = product.ProductName,
                SnNeed = product.SnNeed,
            };
        }
    }
}
