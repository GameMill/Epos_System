using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EPOS.Models
{
    [System.Serializable]
    public class UserAccount
    {
        public string CompanyName = "";
        public string Username = "";
        public string Password = "";

        public string DBName = "";
        public string DBUsername = "";
        public string DBPassword = "";
        public string DBURL = "";

        public string AddressLine1 = "";
        public string AddressLine2 = "";
        public string City = "";
        public string State = "";
        public string Postcode = "";
        public string Vat = "";
        public string Phone = "";
        public string Fax = "";
        public string Email = "";
        public string Website = "";
        public bool CanSave = true;

        public string ComPort = "";

        [NonSerialized]
        private static byte[] key = { 17, 22, 13, 64, 95, 46, 67, 28 };
        [NonSerialized]
        private static byte[] iv = { 52, 45, 51, 12, 102, 21, 1, 57 };



        public int Invoice = 0;
        public int Order = 0;
        public UserAccount()
        {

        }
        public UserAccount(
            string CompanyName = "",
            string DBName = "",
            string DBUsername = "",
            string DBPassword = "",
            string Username = "",
            string Password = "",
            string DBURL = "",
            string AddressLine1 = "",
            string AddressLine2 = "",
            string City = "",
            string State = "",
            string Postcode = "",
            string Vat = "",
            string Phone = "",
            string Fax = "",
            string Email = "",
            string Website = "",
            int Invoice = 0,
            int Order = 0
            )
        {
            this.CompanyName = CompanyName;
            this.DBName = DBName;
            this.DBUsername = DBUsername;
            this.DBPassword = DBPassword;
            this.Username = Username;
            this.Password = Password;
            this.DBURL = DBURL;
            this.AddressLine1 = AddressLine1;
            this.AddressLine2 = AddressLine2;
            this.City = City;
            this.State = State;
            this.Postcode = Postcode;
            this.Vat = Vat;
            this.Phone = Phone;
            this.Fax = Fax;
            this.Email = Email;
            this.Website = Website;
            this.Invoice = Invoice;
            this.Order = Order;
            
        }
    
        public void Save()
        {
            string path = Pages.ConpanySelection.DataPath + "Accounts\\" + CompanyName.Replace(' ', '_') + ".dat";

            System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider();

            // Encryption
            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            using (var cryptoStream = new System.Security.Cryptography.CryptoStream(fs, des.CreateEncryptor(key, iv), System.Security.Cryptography.CryptoStreamMode.Write))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // This is where you serialize the class
                formatter.Serialize(cryptoStream, this);
            }
        }

        public static Models.UserAccount Load(string CompanyName)
        {
            if (TillManager.Instance.CurrentUser != null)
                if (TillManager.Instance.CurrentUser.CompanyName == CompanyName)
                    return TillManager.Instance.CurrentUser;

            string path = Pages.ConpanySelection.DataPath + "Accounts\\" + CompanyName.Replace(' ', '_') + ".dat";
            System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider();

            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            using (var cryptoStream = new System.Security.Cryptography.CryptoStream(fs, des.CreateDecryptor(key, iv), System.Security.Cryptography.CryptoStreamMode.Read))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // This is where you deserialize the class
                return (Models.UserAccount)formatter.Deserialize(cryptoStream);
            }
        }

        public void DeleteAccount()
        {
            System.IO.File.Delete(Pages.ConpanySelection.DataPath + "Accounts\\" + CompanyName.Replace(' ', '_') + ".dat");

        }

    }
}
