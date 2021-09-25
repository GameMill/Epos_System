using Epos.Pages;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Epos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _instance = null;
        public static MainWindow Instance { get { return _instance; } }
        public static Models.Conpany Conpany { get; set; }
        public MainWindow()
        {
            _instance = this;
            DBContext.CoreDBContext.Instance.Database.Migrate();
            DBContext.ProductsDBContext.Instance.Database.Migrate();
            WindowState = WindowState.Maximized;
            InitializeComponent();
            this.Frame.Navigate(new ConpanySelection());
        }
    }
        
}
