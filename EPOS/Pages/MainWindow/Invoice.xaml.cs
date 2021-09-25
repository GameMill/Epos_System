using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EPOS.Pages
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : Page
    {
        public Models.UserAccount Account;
        PrinterService Printer;
        bool ReturnTOSelectionPage = false;

        public Invoice()
        {
            InitializeComponent();
            //DataViewer.SourceUpdated += DataViewer_SourceUpdated;
            // DataViewer.TargetUpdated += DataViewer_TargetUpdated;
            DataViewer.RowEditEnding += DataViewer_RowEditEnding;
            //DataViewer.AddingNewItem += DataViewer_AddingNewItem;
            //this.TextInput += Invoice_TextInput;
            /*
            for (int i = 0; i < 50000; i++)
            {
                var a = new DB.Models.Product()
                {
                    Added = DateTime.Now,
                    Name = "this is a test - " + i,
                    Enabled = true,
                    Price = i,
                    SKU = "test-" + i,
                    Cost = i / 2,
                    Catagory = "Unknown-Test",
                    WarrantyType = 0,
                };
                DB.DBContext.Instance.Products.Add(a);
                DB.DBContext.Instance.SaveChanges();
                var b = new DB.Models.SKU()
                {
                    Enabled = true,
                    ProductID = a.ID,
                    ID = "test-" + i
                };
                DB.DBContext.Instance.SKUs.Add(b);
                DB.DBContext.Instance.SaveChanges();
            }*/

            SizeChanged += Invoice_SizeChanged;
            inventoryView = new InvertoryViewModel();
            inventoryView.CartChanged += InventoryView_CartChanged;

            ((MainWindow)Application.Current.MainWindow).DataContext = inventoryView;
            inventoryView.CartItems.CollectionChanged += CartItems_CollectionChanged;
            UpdateTotal();
            UpdateInvoiceNumber();
            Account = TillManager.Instance.CurrentUser;
            Printer = new PrinterService(Account.ComPort);
            //var DB = Database.GetInstance<Mysql>();
           /* if (DB.IsValid() == false)
            {
                if(!DB.Open(Account.DBURL, Account.DBUsername, Account.DBPassword, Account.DBName))
                {
                    MessageBox.Show(DB.ErrorMessage);
                    ReturnTOSelectionPage = true;
                    return;
                }
            }*/
            if (DB.DBContext.Instance != null)
            {
                PergeCatagory();
            }
            else
            {
                MessageBox.Show("Unknown Error: Unable To Connect To Database");
            }


            SizeChanged += Invoice_SizeChanged1;
        }

        

        public double TotalDiscount()
        {
            double Total = 0;
            foreach (Models.DiscountViewModel item in Discount.Items)
            {
                Total += item.Price;
            }
            return Total;
        }

        public void UpdateMixMatch(CartInvertory Item)
        {
            //if (Item == null)
            //    return;
            if (Item.MixMatch == "")
                return;

            //var MixMatch = Database.GetInstance<Mysql>();
            //var Match = MixMatch.Select("select MixAndMatchID From `productmixandmatch` where ProductID=" + Item.ID.ToString());
            var Match = DB.DBContext.Instance.ProductMixAndMatchs.Where(a => a.ProductID == Item.ID).Select(a=>a.MixAndMatchID).ToList();

            if (Match.Count > 0)
            {
                int ItemQtyRemaning = Item.QTY;

                var MixMatchSystemItem = DB.DBContext.Instance.MixAndMatchs.Where(a => Match.Contains(a.ID)).ToList(); //MixMatch.Select("SELECT * FROM `mixandmatch` where Enabled=1 and ItemSpecific=1 and " + OrString + " order by QTY desc");
                if (MixMatchSystemItem.Count > 0)
                {
                    // var System = MixMatchSystemItem.Rows[0];
                    foreach (var item in MixMatchSystemItem)
                    {
                        var NumberOFMatchs = DeductMixMatch(item.QTY,ref ItemQtyRemaning);
                        if (NumberOFMatchs > 0)
                        {
                            bool Found = false;
                            for (int i = 0; i < Discount.Items.Count; i++)
                            {
                                var Temp = ((Models.DiscountViewModel)Discount.Items[i]);
                                if (Temp.ID == item.ID)
                                {
                                    Found = true;
                                    if (Temp.QTY == NumberOFMatchs)
                                        return;
                                    Temp.QTY = NumberOFMatchs;
                                    Temp.Price = Item.TotalPrice - NumberOFMatchs * item.SellAT;
                                }
                            }
                            if (!Found)
                            {
                                Discount.Items.Add(
                                    new Models.DiscountViewModel()
                                    {
                                        ID = item.ID,
                                        Price = Item.TotalPrice - NumberOFMatchs * item.SellAT,
                                        QTY = NumberOFMatchs,
                                        Name = item.Name,
                                    }
                                    );
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Discount.Items.Count; i++)
                            {
                                if (((Models.DiscountViewModel)Discount.Items[i]).ID == item.ID)
                                {
                                    Discount.Items.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }
                }

                /*MixMatchSystemItem = MixMatch.Select("SELECT QTY,Name,SellAT FROM `mixandmatch` where Enabled=1 and ItemSpecific=0 and " + OrString + " order by QTY desc");
                if (MixMatchSystemItem.Rows.Count > 0)
                {
                }*/

            }
        }

        private int DeductMixMatch(int RequiredQTY,ref int itemQtyRemaning,int NumberOfMatchs=0)
        {
            if(itemQtyRemaning >= RequiredQTY)
            {
                itemQtyRemaning -= RequiredQTY;
                return DeductMixMatch(RequiredQTY, ref itemQtyRemaning, NumberOfMatchs + 1);
            }
            return NumberOfMatchs;
        }

        private void InventoryView_CartChanged(object sender, CartChangedEventArgs e,PropertyChangedEventArgs a)
        {
            if (e.PropertyName == "ADD")
            {
                if (e.Item._WarrantyType == -1)
                {
                    //Database.GetInstance<Mysql>().Select("select * from `WarrantyTypes`");
                }
                UpdateMixMatch(e.Item);
            }
            else if(e.PropertyName == "Edited")
            {
                if(e.e.PropertyName == "QTY")
                {
                    UpdateMixMatch(e.Item);
                }
            }
            else if (e.PropertyName == "Remove")
            {

            }
            UpdateTotal();
        }


        private void Invoice_SizeChanged1(object sender, SizeChangedEventArgs e)
        {
            Discount_Loaded(Discount, new RoutedEventArgs());
        }

        public void PergeCatagory()
        {
            InvertoryMenu.Items.Clear();
            Dictionary<string, MenuItem> TempArray = new Dictionary<string, MenuItem>();
            /*
             var Table = DB.Select("SELECT ID,Name,Price,Catagory FROM `"+TillManager.Instance.CurrentUser.DBName+"`.products  where Enabled=1 ORDER BY Catagory ASC");
             foreach (System.Data.DataRow item in Table.Rows)
             {
                 PergeCatagoryRow(ref TempArray, item, 1);
             }*/
            int NumberOFItems = 0;
            foreach (var item in DB.DBContext.Instance.Products.Where(Product => Product.Enabled == true).OrderBy(a => a.Catagory).ToList())
            {
                if (NumberOFItems == 500)
                    break;
                PergeCatagoryRow(ref TempArray, item, 1);

                NumberOFItems++;
            }

            TempArray = null;
            InvertoryMenu.Items.Add(new Separator());
            var Menuitem = new MenuItem() { Header = "Refresh Menu" };
            Menuitem.Click += RefreshMenu_Click;
            InvertoryMenu.Items.Add(Menuitem);
        }

        private void RefreshMenu_Click(object sender, RoutedEventArgs e)
        {
            PergeCatagory();
        }

        public void PergeCatagoryRow(ref Dictionary<string, MenuItem> TempArray, DB.Models.Product item, int Level)
        {

            var Cat = item.Catagory.Split('-');
            if (Cat.Length > 0)
            {
                var CurrentLevelString = "";
                var PerentLevelString = "";
                for (int i = 0; i < Level; i++)
                {
                    if (CurrentLevelString != "")
                        CurrentLevelString += "-";

                    CurrentLevelString += new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Cat[i].ToLower());
                    if (i < Level - 1)
                    {
                        if (PerentLevelString != "")
                            PerentLevelString += "-";
                        PerentLevelString += new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Cat[i].ToLower());
                    }
                }
                if (!TempArray.ContainsKey(CurrentLevelString) && CurrentLevelString != "")
                {
                    TempArray[CurrentLevelString] = new MenuItem();
                    TempArray[CurrentLevelString].Header = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Cat[Level - 1].ToLower());

                    if (PerentLevelString == "")
                        InvertoryMenu.Items.Add(TempArray[CurrentLevelString]);
                    else
                        TempArray[PerentLevelString].Items.Add(TempArray[CurrentLevelString]);
                }
                if (Level < Cat.Length)
                {
                    PergeCatagoryRow(ref TempArray, item, Level + 1);
                }
                else
                {
                    var Menuitem = new MenuItem();
                    Menuitem.Header = item.Name;
                    Menuitem.Name = "Menuitem_ID_" +item.ID;
                    Menuitem.Click += InvertoryMenu_Click;

                    if (CurrentLevelString == "")
                        InvertoryMenu.Items.Add(Menuitem);
                    else
                        TempArray[CurrentLevelString].Items.Add(Menuitem);
                }
            }
            else
            {
                var Menuitem = new MenuItem();
                Menuitem.Header = item.Name;
                Menuitem.Name = "Menuitem_ID_" + item.ID;
                Menuitem.Click += InvertoryMenu_Click;
                InvertoryMenu.Items.Add(Menuitem);
            }
        }

        public void UpdateTotal()
        {
            for (int i = inventoryView.CartItems.Count - 1; i > 0; i--)
            {
                if (inventoryView.CartItems[i].QTY == 0)
                {
                    inventoryView.CartItems.RemoveAt(i);
                }
            }

            TotalPrice.Text = "£" + (Math.Ceiling((inventoryView.TotalPrice - (float)TotalDiscount()) * 100) / 100).ToString();//Math.Round(_TotalPrice, 2, MidpointRounding.AwayFromZero).ToString();
        }
        public void UpdateInvoiceNumber()
        {
            CurrentInvoice.Text = "Invoice: " + TillManager.Instance.CurrentUser.Invoice.ToString();
        }


       

        private void DataViewer_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.DataViewer.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= DataViewer_RowEditEnding;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).RowEditEnding += DataViewer_RowEditEnding;
            }
            else return;
            UpdateTotal();
            //MessageBox.Show("Updated3");
        }


        InvertoryViewModel inventoryView;
        private void Invoice_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //DataViewer.Width = ListViewer.ActualWidth - 25;

            var GridView = ((GridView)lvInvertory.View);
            var QTY = GridView.Columns[0];
            var Name = GridView.Columns[1];
            var Monery = GridView.Columns[2];
            Name.Width = lvInvertory.ActualWidth - QTY.ActualWidth - Monery.ActualWidth;


        }

        private void InvertoryItem_Click(object sender, RoutedEventArgs e)
        {
            inventoryView.Add(new CartInvertory() { QTY = 1, Name = "Yes It Works", Price = 300.50f });
        }

        private void InvertoryMenu_Click(object sender, RoutedEventArgs e)
        {
            var Name = ((MenuItem)sender).Name.Split('_');
            int ID = int.Parse(Name[2]);
            //Database.GetInstance<Mysql>().Select("SELECT * FROM `epos_l.a_systems`.products where ID=" + Name[2]).Rows[0]

            var Product = DB.DBContext.Instance.Products.Where(a => a.ID == ID).FirstOrDefault();
            if (Product != null)
                AddItem(Product);


        }
        public void AddItem(DB.Models.Product Item)
        {
            var ITEM = inventoryView.Add(new CartInvertory()
            {
                ID = Item.ID,
                Price = Item.Price,
                Cost = Item.Cost,
                Attributes = new List<Dictionary<string, string>>(),
                QTY = 1,
                Name = Item.Name,

            });
            if(Item.WarrantyType != 0 && ITEM.QTY == 1)
            {
                var Warraty = DB.DBContext.Instance.WarrantyTypes.Where(a => a.ID == Item.WarrantyType).FirstOrDefault();
                if(Warraty != null)
                {
                    ITEM.Name += Environment.NewLine + Warraty.Name;
                    ITEM._WarrantyLenght = Warraty.Lenght;
                    ITEM._WarrantyName = Warraty.Name;
                }
            }
            if(Item.Attribute != "")
            {
                Windows.Attribute.Open(ITEM, Item.Attribute.Split(',').ToList());
            }
        }
        /*public void AddItemViaDataRow(System.Data.DataRow Item)
        {
            var ID = (int)Item["ID"];
            var Name = (string)Item["Name"];
            var Cost = (float)Item["Cost"];
            var Price = (float)Item["Price"];
            var SKU = (string)Item["SKU"];
            var MixMatch = (string)Item["MixMatch"];
            var WarrantyID = (int)Item["WarrantyType"];
            var WarrantyType = 0;
            string WarrantyName = "";
            int WarrantyLenght = 0;
            if(WarrantyID != 0)
            {
                var Warrantys = Database.GetInstance<Mysql>().Select("Select * from `WarrantyTypes` where ID=" + WarrantyID.ToString());
                if (Warrantys != null)
                {
                    WarrantyName = Warrantys.Rows[0]["Name"].ToString();
                    WarrantyLenght = (int)Warrantys.Rows[0]["Lenght"];
                    WarrantyType = (int)Warrantys.Rows[0]["Type"];
                }

            }



            var attributes = Database.GetInstance<Mysql>().Select("Select Attributes from Attributes where ProductID=" + ID.ToString());
            if (attributes.Rows.Count == 0)
            {
                inventoryView.Add(new CartInvertory()
                {
                    ID = ID,
                    Name = Name,
                    Price = Price,
                    //Attributes = (string)Item["SKU"],
                    QTY = 1,
                    Cost = Cost,
                    MixMatch = MixMatch,
                    _WarrantyType = WarrantyType,
                    _WarrantyLenght = WarrantyLenght,
                    _WarrantyName = WarrantyName
                });
            }
            else
            {
                var Attub = new List<string>();
                foreach (System.Data.DataRow row in attributes.Rows)
                {
                    Attub.Add((string)row[0]);
                }
                var item = new CartInvertory()
                {
                    ID = ID,
                    Name = Name,
                    Price = Price,
                    //Attributes = (string)Item["SKU"],
                    QTY = 1,
                    Cost = Cost,
                    MixMatch = MixMatch,
                    _WarrantyType = WarrantyType,
                    _WarrantyLenght = WarrantyLenght,
                    _WarrantyName = WarrantyName
                };
                item = inventoryView.Add(item);
                Windows.Attribute.Open(item, Attub);
            }
        }*/

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            //inventoryView.RemoveBySKU(inventoryView.CartItems[1].SKU);
            MainWindow.Redirect<Pages.Dashboard>();
        }

        private void lvInvertory_Loaded(object sender, RoutedEventArgs e)
        {
            Invoice_SizeChanged(null, null);

        }

        private void AddNewItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var Text = AddNewItem.Text;
                if (Text == "")
                {
                    Windows.Invertory.Open(this);
                }
                else
                {
                    bool Found = false;
                    var SKU = DB.DBContext.Instance.SKUs.Where(a => a.ID == Text.ToLower()).FirstOrDefault();
                    if (SKU != null)
                    {
                        var Product = DB.DBContext.Instance.Products.Where(a => a.ID == SKU.ProductID && a.Enabled == true).FirstOrDefault();
                        if (Product != null)
                        {
                            AddItem(Product);
                            Found = true;
                        }
                    }
                    if (!Found)
                    {
                        var Product = DB.DBContext.Instance.Products.Where(a => a.Name.ToLower() == Text.ToLower() && a.Enabled == true).FirstOrDefault();
                        if (Product != null)
                            AddItem(Product);
                    }

                    AddNewItem.Text = "";
                }
            }
        }


        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (inventoryView.CartItems.Count == 0)
                return;

            var Number = TillManager.Instance.CurrentUser.Invoice;

            var Order = new DB.Models.Order()
            {
                InvoiceNumber = Number,
                NumberOfItems = inventoryView.NumberOFItems,
                TotalCost = inventoryView.TotalCost,
                TotalPrice = inventoryView.TotalPrice,
                Ordered = DateTime.Now,
            };
            DB.DBContext.Instance.Orders.Add(Order);
            DB.DBContext.Instance.SaveChanges();




            foreach (var item in inventoryView.CartItems)
            {
                var Product = new DB.Models.OrderedProduct()
                {
                    Name = item.Name.Split(new string[1] { "\r\n" }, StringSplitOptions.None)[0],
                    QTY = item.QTY,
                    HasAttributes = item.HasAttrubute,
                    Price = item.Price,
                    Discount = 0,
                    OrderID = Order.ID,
                    Cost = item.Cost,
                    WarrantyLenght = item._WarrantyLenght,
                    WarrantyName = item._WarrantyName
                };
                DB.DBContext.Instance.OrderedProducts.Add(Product);
                DB.DBContext.Instance.SaveChanges();

                if (item.HasAttrubute)
                {
                    for (int i = 0; i < item.Attributes.Count; i++)
                    {
                        foreach (var Att in item.Attributes[i])
                        {
                            var Attr = new DB.Models.OrderedAttribute()
                            {
                                ProductID = Product.ID,
                                OrderID = Order.ID,
                                ItemNumber = i,
                                Name = Att.Key,
                                Value = Att.Value
                            };
                            DB.DBContext.Instance.OrderedAttributes.Add(Attr);
                        }
                    }
                    DB.DBContext.Instance.SaveChanges();
                }
            }

            Printer.Open();

            Printer.WriteLine("Receipt: " + Number + (new String(' ', 8 - Number.ToString().Length)) + "              " + DateTime.Now.ToShortDateString());
            Printer.WriteLine("                               " + DateTime.Now.ToString("h:mm tt"));
            Printer.WriteLine();
            Printer.WriteLine(" " + TillManager.Instance.CurrentUser.CompanyName);
            Printer.WriteLine(" " + TillManager.Instance.CurrentUser.AddressLine1);
            if (TillManager.Instance.CurrentUser.AddressLine2 != "")
                Printer.WriteLine(" " + TillManager.Instance.CurrentUser.AddressLine2);
            Printer.WriteLine(" " + TillManager.Instance.CurrentUser.City + ", " + TillManager.Instance.CurrentUser.Postcode);
            Printer.WriteLine();
            Printer.WriteLine("QTY     Description       Price    Total  ");
            Printer.WriteLine("------------------------------------------");
            foreach (var item in inventoryView.CartItems)
            {
                DB.DBContext.Instance.Products.Where(a => a.ID == item.ID).First().NumberOfSales += item.QTY;

                var Array = item.Name.Split(new string[1] { "\r\n" }, StringSplitOptions.None);

                string Item = item.QTY + (new String(' ', 3 - item.QTY.ToString().Length)) + " ";

                if (Array[0].Length > 21)
                    Item += Array[0].Substring(0, 18) + "...";
                else
                    Item += Array[0] + (new String(' ', 21 - Array[0].Length)) + " ";

                Item += item.Price.ToString() + (new String(' ', 7 - item.Price.ToString().Length)) + " ";
                Item += item.TotalPrice.ToString() + (new String(' ', 7 - item.TotalPrice.ToString().Length));
                Printer.WriteLine(Item);

                if (Array.Length > 1)
                {
                    for (int i = 1; i < Array.Length; i++)
                    {
                        Printer.WriteLine(Array[i], 1);
                        if (item._WarrantyType != 0)
                        {
                            Printer.WriteLine("Warranty: "+item._WarrantyName, 1);
                        }
                        
                    }
                }
            }

            Printer.WriteLine("------------------------------------------");
            Printer.WriteLine("                      SubTotal: £" + inventoryView.TotalPrice);
            Printer.WriteLine("                      Discount: £" + inventoryView.Discount);
            Printer.WriteLine("                           Vat: £0.00 ");
            Printer.WriteLine("                         Total: £" + inventoryView.TotalPrice);
            Printer.WriteLine("                          Paid: £" + inventoryView.TotalPrice);
            Printer.WriteLine("                     Remaining: £0.00");
            if (Note.Text != "")
            {
                Printer.WriteLine("------------------------------------------");
                Printer.WriteLine("Seller Note", 1);
                Printer.Write(Note.Text + "\r\n");
                Printer.WriteLine("------------------------------------------");
            }

            //Printer.Barcode("1234", 0, "\u0050");
            //Printer.Barcode128("TestTestTe", 0, "\u0050");

            //Printer.WriteLine("£" + inventoryView.TotalPrice.ToString(),1,true,true,true,2,true);
            Printer.Close();
            DB.DBContext.Instance.SaveChanges();

            inventoryView.CartItems.Clear();


            TillManager.Instance.CurrentUser.Invoice++;
            TillManager.Instance.CurrentUser.Save();

            Note.Text = "";
            UpdateInvoiceNumber();
        }

        private void CartItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateTotal();
        }

        private void OpenInventory_Click(object sender, RoutedEventArgs e)
        {
            Windows.Invertory.Open(this);
        }
        private void EmptyCart_Click(object sender, RoutedEventArgs e)
        {
            inventoryView.CartItems.Clear();
        }

        private void Rapid_Invertory_Edit_Click(object sender, RoutedEventArgs e)
        {
            Windows.Invertory.Open(this, true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Printer.Open();
            Printer.DisableSmooth = true;
            Printer.ChangeFont(1);
            Printer.WriteLine("L.A Systems", 1, false, 5, true);
            Printer.WriteLine(new string('-', 56));
            Printer.WriteLine("Tel: " + TillManager.Instance.CurrentUser.Phone, 1, false, 3, true);
            Printer.WriteLine(new string('-', 56));
            Printer.BarcodeTest();
            Printer.Close();

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutBox1 box = new AboutBox1();
            box.Show();

        }

        private void Discount_Loaded(object sender, RoutedEventArgs e)
        {
            ((GridView)Discount.View).Columns[1].Width = Discount.ActualWidth - 370;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MixMatch_Click(object sender, RoutedEventArgs e)
        {
            Windows.Invertory.Open(this, false,"",true);
        }

        private void WarrantyEditor_Click(object sender, RoutedEventArgs e)
        {
            Windows.Invertory.Open(this, false, "",false, true);
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            Windows.Invertory.Open(this, false, "", false, false, true);

        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if (ReturnTOSelectionPage)
            {
                MainWindow.Redirect<Pages.ConpanySelection>();
            }
        }
    }



    public class InvertoryViewModel
    {
        public System.ComponentModel.ICollectionView CartInvertory { get; private set; }

        private System.Collections.ObjectModel.ObservableCollection<EPOS.CartInvertory> _myCollection = new System.Collections.ObjectModel.ObservableCollection<EPOS.CartInvertory>();
        public System.Collections.ObjectModel.ObservableCollection<EPOS.CartInvertory> CartItems
        {
            get { return _myCollection; }
        }

        public float TotalCost
        {
            get
            {
                float Cost = 0;
                foreach (var item in CartItems)
                {
                    Cost += item.Cost * (float)item.QTY;
                }
                return Cost;
            }
        }

        /*public void AddByTable(System.Data.DataRow Row)FF
        {
            Add(new EPOS.CartInvertory()
            {
                Name = (string)Row["Name"],
                Price = (float)Row["Price"],
                ID = (int)Row["ID"],
                MixMatch = (string)Row["MixMatch"],
                Cost = (float)Row["Cost"],
                QTY = 1,
                //SKU = (string)Row["SKU"],
            });
        }*/

        public EPOS.CartInvertory Add(EPOS.CartInvertory Item)
        {
            foreach (var item in _myCollection)
            {
                if (item.ID == Item.ID)
                {
                    item.QTY += Item.QTY;
                    if (Item.HasAttrubute)
                    {
                        item.Name += "\r\nItem " + item.QTY;
                        foreach (var Att in Item.Attributes[0])
                        {
                            item.Attributes.Add(Item.Attributes[0]);
                            item.Name += "\r\n" + Att.Key + ": " + Att.Value;

                        }
                    }
                    NotifyPropertyChanged("ADD", item);
                    return item;
                }
            }
            Item.PropertyChanged += Item_PropertyChanged;
            _myCollection.Add(Item);
            NotifyPropertyChanged("ADD",Item);
            return Item;
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Edited",(CartInvertory)sender,e);
        }

        public void Empty()
        {
            _myCollection.Clear();
            NotifyPropertyChanged("Remove");
        }

        public void RemoveBySKU(string SKU)
        {
            for (int i = 0; i < _myCollection.Count; i++)
            {
                /*if (_myCollection[i].SKU == SKU)
                {
                    _myCollection.RemoveAt(i);
                    NotifyPropertyChanged("Remove");
                    return;
                }*/
            }
        }

        public List<EPOS.CartInvertory> _CartInvertory = new List<EPOS.CartInvertory>();
        public int NumberOFItems
        {
            get
            {
                int Total = 0;
                foreach (var item in CartItems)
                {
                    Total += item.QTY;
                }
                return Total;
            }
        }

        public float TotalPrice
        {
            get
            {
                float Total = 0;
                foreach (var item in CartItems)
                {
                    Total += (((float)item.QTY) * item.Price);
                }
                return Total;
            }
        }

        public float Discount
        {
            get
            {
                float Total = 0;
                foreach (var item in CartItems)
                {
                    Total += (item.Discount);
                }
                return Total;
            }
        }

        public delegate void CartChangedEventHandler(InvertoryViewModel sender, CartChangedEventArgs e, PropertyChangedEventArgs a = null);

        public event CartChangedEventHandler CartChanged;

        private void NotifyPropertyChanged(string propertyName,CartInvertory Item = null, PropertyChangedEventArgs e=null)
        {
            if (CartChanged != null)
            {
                CartChanged(this,new CartChangedEventArgs(propertyName,Item,e));
            }
        }

        public InvertoryViewModel()
        {
            /* _myCollection.Add(new EPOS.CartInvertory
             {
                 SKU = "0",
                 Name = "Test",
                 Price = 0.05f,
                 QTY = 1,
             });
             _myCollection.Add(new EPOS.CartInvertory
             {
                 SKU = "1",
                 Name = "Test2",
                 Price = 19.90f,
                 QTY = 1,
             });
             _myCollection.Add(new EPOS.CartInvertory
             {
                 SKU = "2",
                 Name = "Test3",
                 Price = 50.05f,
                 QTY = 1,
             });
             _myCollection.Add(new EPOS.CartInvertory
             {
                 SKU = "3",
                 Name = "Test4",
                 Price = 1f,
                 QTY = 1,
             });*/

            /* _CartInvertory = new List<EPOS.CartInvertory>
                                       {
                                           new EPOS.CartInvertory
                                               {
                                                   SDK = 0,
                                                   Name = "Test",
                                                   Price = 0.05f,
                                                   QTY = 1,
                                               },
                                           new EPOS.CartInvertory
                                               {
                                                   SDK = 0,
                                                   Name = "Test2",
                                                   Price = 19.90f,
                                                   QTY = 1,
                                               },
                                           new EPOS.CartInvertory
                                               {
                                                   SDK = 0,
                                                   Name = "Test3",
                                                   Price = 50.05f,
                                                   QTY = 1,
                                               },
                                           new EPOS.CartInvertory
                                               {
                                                   SDK = 0,
                                                   Name = "Test4",
                                                   Price = 1f,
                                                   QTY = 1,
                                               },
                                       };

              CartInvertory = CollectionViewSource.GetDefaultView(_CartInvertory);*/
        }

    }

    public class CartChangedEventArgs
    {
        public CartChangedEventArgs(string propertyName,CartInvertory Item = null, PropertyChangedEventArgs e = null) { this.PropertyName = propertyName;this.Item = Item;this.e = e; }
        public virtual string PropertyName { get; }
        public virtual CartInvertory Item { get; } = null;
        public PropertyChangedEventArgs e { get; } = null;
    }
}