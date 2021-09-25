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
    /// Interaction logic for MixAndMatch.xaml
    /// </summary>
    public partial class MixAndMatch : Page
    {

        public MixAndMatch()
        {
            InitializeComponent();
            DB.DBContext.Instance.Products.Load();
            MixAndMatchGrid.DataContext = DB.DBContext.Instance.Products.Local;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Windows.Invertory.Redirect<Pages.Invertory.AddMixAndMatch>();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                var ID = int.Parse(((Button)sender).Tag.ToString());
                var Item = DB.DBContext.Instance.MixAndMatchs.Where(a => a.ID == ID).FirstOrDefault();
                if (Item == null)
                {
                    DB.DBContext.Instance.MixAndMatchs.Remove(Item);
                    DB.DBContext.Instance.SaveChanges();
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                Windows.Invertory.Redirect<Pages.Invertory.AddMixAndMatch>("Edit", new string[1] { ((Button)sender).Tag.ToString() });
            }
        }
    }
}
