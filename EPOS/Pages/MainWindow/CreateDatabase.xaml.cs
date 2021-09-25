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

namespace EPOS.Pages
{
    /// <summary>
    /// Interaction logic for CreateDatabase.xaml
    /// </summary>
    public partial class CreateDatabase : Page
    {
        public CreateDatabase()
        {
            InitializeComponent();
        }
        System.Windows.Threading.DispatcherTimer LoadingTimer;
        Models.UserAccount CompanyInfo = null;
        public void CreateNewDatabase(Models.UserAccount CompanyInfo)
        {
            this.CompanyInfo = CompanyInfo;
            DBName.Text = "Epos_" + CompanyInfo.CompanyName.Replace(' ', '_');
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.Redirect<Pages.CreateAccount>("ReturnTO", new object[1] { CompanyInfo });
        }

        private void Cancel (object sender, RoutedEventArgs e)
        {
            CompanyInfo = null;
            MainWindow.Redirect<Pages.ConpanySelection>();
        }

        private async void Next(object sender, RoutedEventArgs e)
        {
            LoadingTimer = new System.Windows.Threading.DispatcherTimer();
            LoadingTimer.Tick += new EventHandler(LoadingTimer_Tick);
            LoadingTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            LoadingTimer.Start();
            LoadingAngle = 0;
            LoadingIcon.LayoutTransform = new RotateTransform(LoadingAngle);
            LoadingIcon.Visibility = Visibility.Visible;
            UserOrPassError.Visibility = Visibility.Hidden;
            var UserName = Username.Text;
            var PassWord = Password.Password;
            var DbName = DBName.Text;
            var Url = URL.Text;

            await Task.Run(() => { SetupDB(UserName, PassWord, DbName, Url); });
        }
        

        public void SetupDB(string Username, string Password, string DBName, string URL)
        {
            CompanyInfo.DBName = DBName;
            CompanyInfo.DBUsername = Username;
            CompanyInfo.DBPassword = Password;
            CompanyInfo.DBURL = URL;
            TillManager.Instance.CurrentUser = CompanyInfo;
            try
            {
                var DB = new DB.DBContext();
                DB.Products.Load();
            }
            catch
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Complate(false);
                }));
                return;
            }
            CompanyInfo.Save();
            Dispatcher.Invoke(new Action(() =>
            {
                Complate(true);
            }));
            /*
            var DB = Database.GetInstance<Mysql2>();
            if (TestConnection(DB, Username,Password,URL))
            {
                CompanyInfo.DBName = DBName;
                CompanyInfo.DBUsername = Username;
                CompanyInfo.DBPassword = Password;
                CompanyInfo.DBURL = URL;
                CompanyInfo.Save();
                TillManager.Instance.CurrentUser = CompanyInfo;
                if (!DB.DBExists(CompanyInfo.DBName))
                    CreateDB();
                else
                    UpdateDB();

                Dispatcher.Invoke(new Action(() =>
                {
                    Complate(true);
                }));
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Complate(false);
                }));
            }*/


        }

        private void Complate(bool Sus)
        {
            
            UserOrPassError.Visibility = Visibility.Hidden;
            LoadingIcon.Visibility = Visibility.Hidden;
            LoadingTimer.Stop();
            LoadingTimer = null;
            if (Sus)
            {
                MainWindow.Redirect(MainWindow.MainPageAfterLogin);
            }
            else
            {
                UserOrPassError.Visibility = Visibility.Visible;
            }
            
        }

        int LoadingAngle = 0;
        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            LoadingAngle += 45;
            LoadingIcon.LayoutTransform = new RotateTransform(LoadingAngle);
        }

        /*private void UpdateDB()
        {
            //throw new NotImplementedException();
            //var DB = Database.GetInstance<Mysql2>();
            //var Version = DB.Select("select * from `" + CompanyInfo.DBName + "`.`VersionControl`");

        }*/
/*
        private void CreateDB()
        {

            /* v1.01
            var DB = Database.GetInstance<Mysql2>();
            DB.CreateDB(CompanyInfo.DBName);
            var SQL = "CREATE TABLE `" + CompanyInfo.DBName + "`.`VersionControl` ( `ID` INT NOT NULL AUTO_INCREMENT , `TableName` TEXT NOT NULL , `Version` INT NOT NULL , UNIQUE (`ID`)) ENGINE = InnoDB;"
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"Products\",1);"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`Products` ( `ID` INT NOT NULL AUTO_INCREMENT,`Enabled` boolean NOT NULL DEFAULT 0 , `Name` Text NOT NULL , `SKU` VARCHAR(10) NOT NULL, `MixMatch` VARCHAR(10) default \"\" , `Cost` FLOAT NOT NULL DEFAULT 0, `Price` FLOAT NOT NULL DEFAULT 0, `Attribute` TEXT NOT NULL, `Catagory` VARCHAR(255) NOT NULL DEFAULT \"Unknown\", `Added` DATE NOT NULL , `NumberOfSales` INT NOT NULL DEFAULT 0, `WarrantyType` INT NOT NULL DEFAULT 0, UNIQUE (`ID`)) ENGINE = InnoDB;"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`SKUs` ( `ID` VARCHAR(20) NOT NULL, `ProductID` INT NOT NULL,`Enabled` boolean NOT NULL DEFAULT 0 , PRIMARY KEY (`ID`)) ENGINE = InnoDB;"
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"SKUs\",1);"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`Attributes` ( `ID` INT NOT NULL AUTO_INCREMENT, `ProductID` INT NOT NULL , `Attributes` TEXT NOT NULL , UNIQUE(`ID`)) ENGINE = InnoDB; "
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"Attributes\",1);"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`Orders` ( `ID` INT NOT NULL AUTO_INCREMENT,`InvoiceNumber` int Not Null , `TotalCost` FLOAT NOT NULL , `TotalPrice` FLOAT NOT NULL , `Ordered` DATETIME NOT NULL DEFAULT NOW() , `NumberOfItems` INT NOT NULL , UNIQUE (`ID`)) ENGINE = InnoDB;"
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"Orders\",1);"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`OrderedProducts` ( `ID` INT NOT NULL AUTO_INCREMENT, `Name` TEXT NOT NULL , `QTY` INT NOT NULL , `HasAttributes` BOOLEAN NULL , `Price` FLOAT NOT NULL, `Cost` FLOAT NOT NULL , `Discount` FLOAT NOT NULL , `OrderID` INT NOT NULL, `WarrantyLenght` INT NOT NULL, `WarrantyName` Text NOT NULL, UNIQUE (`ID`)) ENGINE = InnoDB; "
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"OrderedProducts\",1);"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`OrderedAttributes` ( `ID` INT NOT NULL AUTO_INCREMENT, `OrderID` INT NOT NULL,`ProductID` INT NOT NULL , `ItemNumber` INT NOT NULL , `Name` TEXT NULL , `Value` TEXT NOT NULL , UNIQUE (`ID`)) ENGINE = InnoDB; "
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"OrderedAttributes\",1);"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`mixandmatches` ( `ID` INT NOT NULL AUTO_INCREMENT,`QTY` INT NOT NULL, `Name` TEXT NOT NULL , `SellAT` FLOAT NOT NULL , `ItemSpecific` BOOLEAN NOT NULL, `Enabled` BOOLEAN NOT NULL, UNIQUE (`ID`)) ENGINE = InnoDB;"
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"mixandmatches\",1);"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`ProductMixAndMatch` ( `ID` INT NOT NULL AUTO_INCREMENT, `ProductID` INT NOT NULL, `MixAndMatchID` INT NOT NULL, UNIQUE (`ID`)) ENGINE = InnoDB;"
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"ProductMixAndMatch\",1);"
                   + "CREATE TABLE `" + CompanyInfo.DBName + "`.`WarrantyTypes` ( `ID` INT NOT NULL AUTO_INCREMENT, `Name` TEXT NOT NULL , `Lenght` int NOT NULL ,`Type` INT NOT NULL, UNIQUE (`ID`)) ENGINE = InnoDB;"
                   + "INSERT INTO `" + CompanyInfo.DBName + "`.`VersionControl`(`TableName`, `Version`) VALUES (\"WarrantyTypes\",1);";
            System.IO.File.WriteAllText("Test.txt", SQL);

            DB.Query(
                SQL

            );
            DB.Query("use `" + CompanyInfo.DBName + "`");
             * /

            // V2

        }


       / * private bool TestConnection(Database DB, string Username, string Password, string URL)
        {
            if (DB.Open(URL, Username, Password))
            {
                if (DB.IsValid())
                {
                    DB.Close();
                    return true;
                }
            }
            return false;
        }*/
    }
}

