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
using System.Windows.Shapes;

namespace EPOS.Windows
{
    /// <summary>
    /// Interaction logic for Invertory.xaml
    /// </summary>
    public partial class Invertory : Window
    {
        public Pages.Invoice InvoicePage;
        public static Invertory Instance { get; private set; }


        private void Invertory_Closed(object sender, EventArgs e)
        {
            Instance = null;
        }

        public static void Redirect<T>(string Action = "", object[] Arg = null) where T : Page
        {
            Invertory.Instance.DataContext = null;
            Redirect((T)Activator.CreateInstance(typeof(T)), Action, Arg);
        }

        public static void Redirect(Type Page, string Action = "", object[] Arg = null)
        {
            Invertory.Instance.DataContext = null;
            Redirect((Page)Activator.CreateInstance(Page), Action, Arg);
        }

        public static void Redirect(Page TO, string Action = "", object[] Arg = null)
        {
            Invertory.Instance.Page.Content = TO.Content;

            if (Action == "")
                return;

            Type PageType = TO.GetType();

            var theMethod = PageType.GetMethod(Action);
            theMethod.Invoke(TO, Arg);
        }

        public static void Open(Pages.Invoice InvoicePage, bool RapidEdit = false, string SearchText = "", bool MixMatchEdit=false,bool WarrantyEdit = false,bool OrderViewer=false)
        {
            if (Instance != null)
            { Instance.Close(); }

            Instance = new Invertory();
            Instance.InvoicePage = InvoicePage;
            Instance.Closed += Instance.Invertory_Closed;

            if (RapidEdit)
            {
                Redirect<Pages.Invertory.RapidEdit>();
            }
            else if(MixMatchEdit)
            {
                Redirect<Pages.Invertory.MixAndMatch>();
            }
            else if(WarrantyEdit)
            {
                Redirect<Pages.Invertory.WarrantyEditor>();
            }
            else if(OrderViewer)
            {
                Redirect<Pages.Invertory.Orders>();
            }
            else
            {
                if (SearchText == "")
                    Redirect<Pages.Invertory.Main>();
                else
                    Redirect<Pages.Invertory.Main>("Filter", new object[1] { SearchText });
            }


            Instance.Show();
        }


        public Invertory()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //invertoryViewModel.Add(new CartInvertory() { Name = "From Invertory", Price = 50, QTY = 1, ID = 55 });
            //Close();
        }
    }
}
