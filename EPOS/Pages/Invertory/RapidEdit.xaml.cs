using System;
using System.Collections.Generic;
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
using System.Data.Entity;
using System.Collections.ObjectModel;
using EPOS.DB.Models;

namespace EPOS.Pages.Invertory
{
    public class WarrantyDropDown
    {
        public int ID { get; set; }
        public List<DB.Models.WarrantyType> Values { get; set; }
        public string Value { get; set; }
    }
    /// <summary>
    /// Interaction logic for RapidEdit.xaml
    /// </summary>
    public class RapidEditViewModel
    {
        private bool Edited = false;
        public int ID
        {
            get { return _ID; }
            set
            {
                if (!Edited && _ID != value)
                    Edited = true;
                _ID = value;
            }
        }
        public string Name
        {
            get { return _Name; }
            set
            {
                if (!Edited && _Name != value)
                    Edited = true;
                _Name = value;
            }
        }
        public float Price
        {
            get { return _Price; }
            set
            {
                if (!Edited && _Price != value)
                    Edited = true;
                _Price = value;
            }
        }
        public float Cost
        {
            get { return _Cost; }
            set
            {
                if (!Edited && _Cost != value)
                    Edited = true;
                _Cost = value;
            }
        }
        public string SKU
        {
            get { return _SKU; }
            set
            {
                if (!Edited && _SKU != value)
                    Edited = true;
                _SKU = value;
            }
        }
        public string Catagory
        {
            get { return _Catagory; }
            set
            {
                if (!Edited && _Catagory != value)
                    Edited = true;
                _Catagory = value;
            }
        }
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                if (!Edited && _Enabled != value)
                    Edited = true;
                _Enabled = value;
            }
        }
        public int WarrantyType
        {
            get { return _WarrantyType; }
            set
            {
                if (!Edited && _WarrantyType != value)
                    Edited = true;
                _WarrantyType = value;
            }
        }
        public string Attributes
        {
            get { return _Attributes; }
            set
            {
                if (!Edited && _Attributes != value)
                    Edited = true;
                _Attributes = value;
            }
        }




        private int _ID = -1;

        private string _Name = "";
        private float _Price = 0;
        private float _Cost = 0;
        private string _SKU = "";
        private string _Attributes = "";
        private string _Catagory = "";
        private bool _Enabled = false;
        private int _WarrantyType = 0;


        public bool HasEdited() { return Edited; }
        public void ResetEdited()
        {
            Edited = false;
        }
        public void Saved(bool SaveDB = true) {
            DB.Models.Product Product = null;
            bool NeedAdd = false;
            if (_ID != -1)
                Product = DB.DBContext.Instance.Products.Where(a => a.ID == _ID).FirstOrDefault();

            if (Product == null)
            {
                Product = new DB.Models.Product();
                NeedAdd = true;
            }

            Product.Name = _Name;
            Product.Price = _Price;
            Product.WarrantyType = _WarrantyType;
            Product.Cost = _Cost;
            Product.Price = _Price;
            Product.Enabled = _Enabled;
            Product.Catagory = _Catagory;
            Product.SKU = _SKU;
            Product.Attribute = _Attributes;
            if(Product.Catagory == "")
            {
                Product.Catagory = "Unknown";
            }
            if (NeedAdd)
            {
                DB.DBContext.Instance.Products.Add(Product);
            }

            try
            {
                DB.DBContext.Instance.SaveChanges();
                _ID = Product.ID;
            }
            catch (Exception)
            {

                MessageBox.Show("Error Saving Changes");
                return;
            }
            



            var Skus = _SKU.Split(new char[2] { ' ', ',' });

            DB.DBContext.Instance.SKUs.RemoveRange(DB.DBContext.Instance.SKUs.Where(a => a.ProductID == _ID));// SkuTable.Select("ProductID = " + ID);

            if (Skus.Length > 0)
            {
                //var Temp = Database.GetInstance<Mysql>().Select(SkuCMD);

                foreach (var Sku in Skus)
                {
                    var row = new DB.Models.SKU()
                    {
                        ID = Sku,
                        ProductID = _ID
                    };
                    DB.DBContext.Instance.SKUs.Add(row);
                }
            }


            DB.DBContext.Instance.SKUs.RemoveRange(DB.DBContext.Instance.SKUs.Where(a => a.ProductID == _ID));// SkuTable.Select("ProductID = " + ID);


            var att = _Attributes.Split(new char[1] { ',' });

            if (att.Length > 0)
            {
                //var Temp = Database.GetInstance<Mysql>().Select(SkuCMD);

                foreach (var Attribute in att)
                {
                    var Row = new DB.Models.Attribute()
                    {
                        Attributes = Attribute,
                        ProductID =_ID
                    };
                    DB.DBContext.Instance.Attributes.Add(Row);
                }
            }
            DB.DBContext.Instance.SaveChanges();
            
            Edited = false;

        }
    }
    public class RapidEditViewModelController
    {
        public ObservableCollection<RapidEditViewModel> Products;
        public ObservableCollection<RapidEditViewModel> ProductsFiltered;
        bool ShowingAll = true;
        DataGrid Grid;
        public RapidEditViewModelController(DataGrid Grid)
        {
            this.Grid = Grid;
            DB.DBContext.Instance.Products.Load();
            DB.DBContext.Instance.Attributes.Load();
            DB.DBContext.Instance.SKUs.Load();

            Products = new ObservableCollection<RapidEditViewModel>(DB.DBContext.Instance.Products.Select(a => new RapidEditViewModel() {ID = a.ID, Catagory = a.Catagory, Cost = a.Cost, Enabled = a.Enabled, Name = a.Name, Price = a.Price, WarrantyType = a.WarrantyType,SKU = a.SKU,Attributes = a.Attribute  }));
            for (int i = 0; i < Products.Count; i++)
            {
                Products[i].ResetEdited();
            }
            Grid.ItemsSource = Products;
        }

        public void SaveChanges(DataGrid Grid)
        {
            foreach (var item in Products)
            {
                if (item.HasEdited())
                {
                    item.Saved(false);
                }
            }
            DB.DBContext.Instance.SaveChanges();
            Grid.Items.Refresh();
        }

        public void FilterAmount(float num, bool MoreThan)
        {
            LastText = "";
            ProductsFiltered = new ObservableCollection<RapidEditViewModel>();
            foreach (var item in Products)
            {
                if (MoreThan)
                {
                    if (num < item.Price)
                        ProductsFiltered.Add(item);
                }
                else
                {
                    if (num > item.Price)
                        ProductsFiltered.Add(item);
                }
            }
            Grid.ItemsSource = ProductsFiltered;
            ShowingAll = false;
        }
        string LastText = "";

        public void FilterByText(string Text)
        {
            ObservableCollection<RapidEditViewModel> Collection = null;
            if (LastText != Text && LastText.Contains(Text))
                Collection = ProductsFiltered;
            else
                Collection = Products;
            ProductsFiltered = new ObservableCollection<RapidEditViewModel>();
            foreach (var item in Collection)
            {
                if (item.Name.Contains(Text))
                    ProductsFiltered.Add(item);
            }
            Grid.ItemsSource = ProductsFiltered;
            ShowingAll = false;
        }

        public void ResetView()
        {
            LastText = "";
            if (!ShowingAll)
                Grid.ItemsSource = Products;

            ShowingAll = true;
        }

        public void remove(Product item)
        {
            RapidEditViewModel ToRemove = null;
            foreach (var item2 in Products)
            {
                if (item.ID == item2.ID)
                {
                    ToRemove = item2;
                }
            }

            remove(ToRemove);
        }

        public void remove(RapidEditViewModel item)
        {
            if (item != null)
            {
                Products.Remove(item);
                if (!ShowingAll)
                    ProductsFiltered.Remove(item);
            }
        }
    }
    public partial class RapidEdit : Page
    {

        public RapidEditViewModelController Controller;
        public RapidEdit()
        {
            InitializeComponent();
            Windows.Invertory.Instance.Title = "Rapid Product Edit";
            Controller = new RapidEditViewModelController(gridEmployees);

            gridEmployees.AutoGeneratingColumn += GridEmployees_AutoGeneratingColumn;
            gridEmployees.CellEditEnding += GridEmployees_CellEditEnding;
        }


        private void GridEmployees_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox Textbox;
            try
            {
                Textbox = (TextBox)e.EditingElement;
            }
            catch (Exception)
            {
                return;
            }
            var Item = (RapidEditViewModel)e.Row.Item;

            if (e.Column.Header.ToString() == "Catagory")
            {
                
                var Array = Textbox.Text.Split(new char[1] { ',' });
                Textbox.Text = "";
                foreach (var item in Array)
                {
                    Textbox.Text += item.Trim();
                }
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Controller.SaveChanges(gridEmployees);
        }


        private void FixTable(System.Data.DataTable Table)
        {
            for (int i = Table.Rows.Count-1; i > -1; i--)
            {
                if(Table.Rows[i].HasErrors)
                {
                    MessageBox.Show("Error: Changes With Error Has Been Removed");
                    Table.Rows[i].RejectChanges();
                }
            }
        }

        private void GridEmployees_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ID")
            {
                  e.Cancel = true;
            }
            else if(e.PropertyName == "Name")
            {
                e.Column.Width = 200;
            }
            else if (e.PropertyName == "SKU")
            {
                e.Column.Width = 100;
            }
            else if(e.PropertyName == "Catagory")
            {
            
            }
        }

        private long getMaxID()
        {
            return DB.DBContext.Instance.ProductsAI;

        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (gridEmployees == null)
                return;
            if (FilterText.Text == "")
            {
                Controller.ResetView();
                return;
            }

            string Type = ((ListBoxItem)FilterSelection.SelectedItem).Content.ToString();
            if (Type == "Price LessThan")
            {
                float Num;
                if (float.TryParse(FilterText.Text, out Num))
                {
                    Controller.FilterAmount(Num, false);
                }

            }
            else if (Type == "Price MoreThan")
            {
                float Num;
                if (float.TryParse(FilterText.Text, out Num))
                {
                    Controller.FilterAmount(Num, true);
                }
            }
            else
                Controller.FilterByText(FilterText.Text);
            //    ((System.Data.DataView)gridEmployees.DataContext).RowFilter = "Price < " + FilterText.Text + "";
            //
            //    ((System.Data.DataView)gridEmployees.DataContext).RowFilter = "Price > " + FilterText.Text + "";
            //else
            //    ((System.Data.DataView)gridEmployees.DataContext).RowFilter = Type + " Like '%" + FilterText.Text + "%'";
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterText.Text = "";
            Filter_TextChanged(null, null);

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                int ID = int.Parse(((Button)sender).Tag.ToString());
                var Item = DB.DBContext.Instance.Products.Where(a => a.ID == ID).FirstOrDefault(); //Table.Select("ID=" + ((Button)sender).Tag.ToString());
                if (Item == null)
                    return;

                DB.DBContext.Instance.SKUs.RemoveRange(DB.DBContext.Instance.SKUs.Where(a => a.ProductID == Item.ID));
                DB.DBContext.Instance.Attributes.RemoveRange(DB.DBContext.Instance.Attributes.Where(a => a.ProductID == Item.ID));
                DB.DBContext.Instance.Products.Remove(Item);
                Controller.remove(Item);
            }
        }
    }
}

/*
  <DataGridTemplateColumn Header="Warranty">
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <ComboBox ItemsSource="{Binding Test}" SelectedValuePath="%" SelectedValue="{Binding TestValue}" >
                                <Popup x:Name="Popup">
                                    <Grid x:Name="DropDown">
                                        <Border x:Name="DropDownBorder"/>
                                        <ScrollViewer>
                                            <!-- Replace this with a DataGrid -->
                                            <StackPanel IsItemsHost="True" KeyboardNavigation.DirectionalNavigation="Contained" />
                                        </ScrollViewer>
                                    </Grid>
                                </Popup>
                            </ComboBox>
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
*/