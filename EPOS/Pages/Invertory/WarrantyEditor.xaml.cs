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
namespace EPOS.Pages.Invertory
{
    /// <summary>
    /// Interaction logic for WarrantyEditor.xaml
    /// </summary>
    public partial class WarrantyEditor : Page
    {
        public WarrantyEditor()
        {
            InitializeComponent();

            Windows.Invertory.Instance.Title = "Warranty Editor";
            DB.DBContext.Instance.WarrantyTypes.Load();

            gridEmployees.ItemsSource = DB.DBContext.Instance.WarrantyTypes.Local;
            gridEmployees.AutoGeneratingColumn += GridEmployees_AutoGeneratingColumn;
        }

        private void GridEmployees_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ID")
            {
                //System.Data.DataColumn myCol1 = ((System.Data.DataView)gridEmployees.ItemsSource).Table.Columns["ID"];
                //myCol1.AutoIncrement = true;
                //myCol1.AutoIncrementSeed = getMaxID();
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.DBContext.Instance.SaveChanges();
               // Database.GetInstance<Mysql>().SaveChanges(Table, CMD);
                MessageBox.Show("Saved Successfully");
                gridEmployees.Items.Refresh();
            }
            catch (Exception a)
            {
                MessageBox.Show("Error: "+a.InnerException.Message);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                var ID = int.Parse(((Button)sender).Tag.ToString());
                var Item = DB.DBContext.Instance.WarrantyTypes.Where(a => a.ID == ID).FirstOrDefault();
                if (Item != null)
                {
                    DB.DBContext.Instance.WarrantyTypes.Remove(Item);
                }
            }
        }
    }
}
