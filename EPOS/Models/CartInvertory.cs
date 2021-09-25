using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPOS
{
    public class CartInvertory : INotifyPropertyChanged
    {
        public double TotalPrice
        {

            get {
                // double d = Math.Round( * 100.0) / 100.0;
                return (Math.Ceiling(((float)QTY * Price) * 100) / 100);//Math.Round((float)(QTY * Price), 2, MidpointRounding.AwayFromZero);
            }
        }

        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                NotifyPropertyChanged("ID");
            }
        }
        public int QTY {
            get { return _QTY; }
            set
            {
                _QTY = value;
                NotifyPropertyChanged("QTY");
            }
        }

        public List<Dictionary<string,string>> Attributes
        {
            get { return _Attributes; }
            set
            {
                _Attributes = value;
                NotifyPropertyChanged("Attributes");
            }
        }

        public string MixMatch
        {
            get { return _MixMatch; }
            set
            {
                _MixMatch = value;
                NotifyPropertyChanged("MixMatch");
            }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public float Price
        {
            get { return _Price; }
            set
            {
                _Price = value;
                NotifyPropertyChanged("Price");
            }
        }

        public float Cost
        {
            get { return _Cost; }
            set
            {
                _Cost = value;
                NotifyPropertyChanged("Cost");
            }
        }

        public string WarrantyName
        {
            get { return WarrantyName; }
        }

        public bool HasAttrubute { get { return _Attributes.Count > 0; } }

        public float Discount {
            get { return _Discount; }
            set { _Discount = value;NotifyPropertyChanged("Discount"); }
        }

        public int _WarrantyType = -1;
        public string _WarrantyName = "";
        public int _WarrantyLenght = 0;
        private int _ID = 0;
        private int _QTY = 0;
        private List<Dictionary<string, string>> _Attributes = new List<Dictionary<string, string>>();
        private string _MixMatch = "";
        private string _Name = "";
        private float _Price = 0;
        private float _Cost = 0;
        private float _Discount = 0;



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}