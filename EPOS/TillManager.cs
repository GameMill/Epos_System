namespace EPOS
{
    class TillManager
    {
        public static TillManager Instance { get; } = new TillManager();
        public bool IsLogin { get { return CurrentUser != null; } }
        public Models.UserAccount CurrentUser { get; set; } /*= new Models.UserAccount()
        {
            AddressLine1 = "Fake AddressLine1",
            AddressLine2 = "Fake AddressLine2",
            City = "Fake City",
            CompanyName = "Fake Company",
            Email = "Fake@Fake",
            Fax = "0000-000-0000",
            Phone = "0000-000-0000",
            Postcode = "Fa51 5ke",
            State = "Fake State",
            Vat = "00000000000000",
            Invoice = 1000,
            Username = "Fake",
            Password = "Fake",
            Order = 1000,
            CanSave = false,
            ComPort = "Com6",
            DBName = "epos_test22",
            DBPassword = "456goldie@A1",
            DBURL = "192.168.0.10",
            DBUsername = "root"
        };*/
        private string Salt = "r9i32190rj21=9";

        

        public bool RequiresLogin(string CompanyName)
        {
            var User = GetUser(CompanyName);
            
            if (User.Username == "" && User.Password == "")
            {
                return false;
            }
            return true;
        }


        public void FakeLogin()
        {
            Instance.CurrentUser = new Models.UserAccount()
            {
                AddressLine1 = "Fake AddressLine1",
                AddressLine2 = "Fake AddressLine2",
                City = "Fake City",
                CompanyName = "Fake Company",
                Email = "Fake@Fake",
                Fax = "0000-000-0000",
                Phone = "0000-000-0000",
                Postcode = "Fa51 5ke",
                State = "Fake State",
                Vat = "00000000000000",
                Invoice = 1000,
                Username = "Fake",
                Password = "Fake",
                Order = 1000,
                CanSave = false,
                ComPort = "Com6",
                DBName = "epos_test22",
                DBPassword = "456goldie@A1",
                DBURL = "192.168.0.10",
                DBUsername = "root"
            };
        }

        public string GetFileAddress(string Filename)
        {
            
            return System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Filename);
        }

        public bool Login(UserLoginModel UserInfo) // Add Redirect TO Login Page IF Failed
        {

            Models.UserAccount user = GetUser(UserInfo.ConpanyName);
            if (user.Username != UserInfo.UserName)
                return false;
            else if (user.Password != UserInfo.Password)
                return false;

            CurrentUser = user;
            return true;
        }

        public void Logout()// Add Redirect TO Login Page IF Failed
        {
            CurrentUser = null;
        }
        private string Encrypt(string str)
        {
            return Salt + str + Salt;
        }
        public Models.UserAccount GetUser(string CompanyName)
        {
            return Models.UserAccount.Load(CompanyName);
            //return new User() { UserName = "POS", Password = Encrypt("POS"), ConpanyName = "L.A System", AddressLine = "249 Liscard Road", Area = "Wallasey", Postcode = "Ch44 5th", PhoneNumber = "0151 201 1421", Website = "EcoIT.co" };
        }
    }
    class UserLoginModel
    {
        public string ConpanyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

