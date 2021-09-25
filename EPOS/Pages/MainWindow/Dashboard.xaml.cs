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

namespace EPOS.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Printer = new PrinterService("COM6");
            Printer.Open();
            /*Printer.WriteLine("Printer Barcode Test",0,true, true, true);
            Printer.Barcode("8091", 1, false, "\u0050");
            Printer.Barcode("8091", 2, false, "\u0050");
            Printer.Barcode("8091", 1, true, "\u0050");
            Printer.Close();
            Printer.Open();
            
            Printer.WriteLine("Small Test");
            Printer.WriteLine("DoubleStrike Test", 0,true);
            Printer.WriteLine("Large Test", 1, false, true);
            Printer.WriteLine("Bold Test", 2, false, false, true);
            Printer.WriteLine("Bold Test", 2, false, false, true);
            Printer.WriteLine("Underline 1", 0, false, false, false, 1);
            Printer.WriteLine("Underline 2", 0, false, false, false, 2);
            Printer.WriteLine("Reverse Mode", 0, false, false, false, 0,true);*/

            /*Printer.WriteLine("Font Type 0");
            Printer.WriteLine("z23456789012345678901234567890123456789012|34"+"\u0023");
            Printer.WriteLine("z23456789012345678901234567890123456789012", 0, true);
            Printer.WriteLine("z23456789012345678901234567890123456789012", 0, false);
            Printer.WriteLine("z23456789012345678901234567890123456789012", 0, false, false, true);
            Printer.BlankLine(1);
            Printer.ChangeFont(1);
            Printer.WriteLine("Font Type 2");
            Printer.WriteLine("z23456789012345678901234567890123456789012", 0, true);
            Printer.WriteLine("z23456789012345678901234567890123456789012", 0, false, true);
            Printer.WriteLine("z23456789012345678901234567890123456789012", 0, false, false, true);
            Printer.ChangeFont(0);
            Printer.WriteLine("Test", 1, false, false, false, 0, true);*/
            Printer.Close();
        }

        private void OpenInvoice(object sender, RoutedEventArgs e)
        {
            MainWindow.Redirect<Pages.Invoice>();
        }
    }
}
