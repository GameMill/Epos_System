using Epos.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Epos.Models.ReceiptPrinter
{
    [System.Runtime.Serialization.DataContract]
    public class Sale
    {
        public Sale(Orders Sale)
        {
            foreach (var item in Sale.Cart)
            {
                Items.Add(new Row()
                {
                    ID = item.Id.ToString(),
                    Name = item.ProductName,
                    Price = item.Price,
                    Qty = item.QTY,
                });
            }
            InvoiceNumber = Sale.Id;
            DateTime = Sale.Order;

        }
        [System.Runtime.Serialization.DataMember]
        public int InvoiceNumber { get; set; }

        [System.Runtime.Serialization.DataMember]
        public List<Row> Items { get; set; } = new List<Row>();

        [System.Runtime.Serialization.DataMember]
        public DateTime DateTime { get; set; }


        public void Print()
        {
            using (TcpClient client = new TcpClient("192.168.0.13", 5157))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    using (var Writer = new System.IO.StreamWriter(stream))
                    {
                        Writer.AutoFlush = true;
                        Writer.Write(Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.Indented));
                        Writer.Write("<EOF>");
                    }
                }
            }
        }
    }

    public class Row
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string SN { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
    }
}
