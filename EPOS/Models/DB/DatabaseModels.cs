using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPOS.DB.Models
{
    public class Attribute
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string Attributes { get; set; }
    }
    public class MixAndMatch
    {
        public int ID { get; set; }
        public int QTY { get; set; }
        public string Name { get; set; }
        public float SellAT { get; set; }
        public bool ItemSpecific { get; set; }
        public bool Enabled { get; set; }
    }
    public class OrderedAttribute
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int ItemNumber { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class OrderedProduct
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int QTY { get; set; }
        public bool HasAttributes { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public int OrderID { get; set; }
        public float Cost { get; set; }
        public int WarrantyLenght { get; set; } = 0;
        public string WarrantyName { get; set; } = "";
        public bool InWarranty(DateTime Ordered) { return Ordered.AddDays(WarrantyLenght) > DateTime.Now; }
    }
    public class Order
    {
        public int ID { get; set; }
        public int InvoiceNumber { get; set; }
        public float TotalCost { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Ordered { get; set; }
        public int NumberOfItems { get; set; }
    }
    public class ProductMixAndMatch
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int MixAndMatchID { get; set; }
    }
    public class Product
    {
        public int ID { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string MixMatch { get; set; }
        public float Cost { get; set; }
        public float Price { get; set; }
        public string Attribute { get; set; }
        public string Catagory { get; set; }
        public DateTime Added { get; set; }
        public int NumberOfSales { get; set; }
        public int WarrantyType { get; set; }
    }
    public class SKU
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string ID { get; set; }
        public int ProductID { get; set; }
        public bool Enabled { get; set; }
    }
    public class VersionControl
    {
        public int ID { get; set; }
        public string TableName { get; set; }
        public int Version { get; set; }
    }
    public class WarrantyType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Lenght { get; set; }
        public int Type { get; set; }
    }
}
