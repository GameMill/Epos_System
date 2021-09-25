using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPOS
{
    public abstract class Database
    {
        private static Database Instance;
        public static T GetInstance<T>() where T : Database, new()
        {
            if (Instance == null)
                Instance = new T();
            return Instance as T;
        }
        public string ErrorMessage = "";
        public virtual bool Open() { return false; }
        public virtual bool Open(string URL, string Username, string Password, string DatabaseName="") { return false; }
        public virtual bool IsValid() { return false; }
        public virtual bool DBExists(string DBName="") { return false; }
        public virtual bool CreateDB(string DBName="") { return false; }
        public virtual void DeleteDB(string DBName="") { }
        public virtual bool Query(string CMD) { return false; }
        public virtual void Close() { }
    }   

    public class Mysql2 : Database
    {
        private MySql.Data.MySqlClient.MySqlConnection connection;

        private string URL, Username, Password, DatabaseName = "";


        public bool HasDBName { get { return DatabaseName != ""; } }

        public Mysql2()
        {
        }

        public void SaveChanges(System.Data.DataTable Table,string CMD)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter(CMD,connection);
            var Builder = new MySqlCommandBuilder(adapter);
            adapter.UpdateCommand = Builder.GetUpdateCommand();

            adapter.Update(Table);
        }
        private void FixTable()
        {
        }

        public override bool Open()
        {
            try
            {
                if (HasDBName)
                    connection = new MySql.Data.MySqlClient.MySqlConnection("server=" + URL + ";uid=" + Username + ";pwd=" + Password + ";database=" + DatabaseName + ";");
                else
                    connection = new MySql.Data.MySqlClient.MySqlConnection("server=" + URL + ";uid=" + Username + ";pwd=" + Password + ";");
                connection.Open();
                return true;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
        }


        public override bool Open(string URL, string Username, string Password, string DatabaseName = "")
        {
            this.URL = MySql.Data.MySqlClient.MySqlHelper.EscapeString(URL);
            this.Username = MySql.Data.MySqlClient.MySqlHelper.EscapeString(Username);
            this.Password = MySql.Data.MySqlClient.MySqlHelper.EscapeString(Password);
            this.DatabaseName = MySql.Data.MySqlClient.MySqlHelper.EscapeString(DatabaseName);
            return Open();
        }

        public override bool IsValid()
        {
            if (connection == null)
                return false;
            return connection.State == System.Data.ConnectionState.Open;
        }

       
        public override void Close()
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }


        public bool QueryMaker(string CMD, Dictionary<string,object> NameValues = null)
        {
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(CMD, connection);

            if(NameValues != null)
            foreach (var item in NameValues)
            {
                cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
            }


            return Query(cmd);
        }

        public System.Data.DataTable Select(string CMD)
        {
            System.Data.DataTable Table = new System.Data.DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(new MySqlCommand(CMD, connection));
            
            adapter.Fill(Table);
            return Table;
        }

        public override void DeleteDB(string DBName = "")
        {
            if(DBName != "")
                this.DatabaseName = DBName;

            if (!HasDBName)
            {
                return;
            }

            if (DBExists(DatabaseName))
            {
                QueryMaker("DROP DATABASE `"+DatabaseName+"`;");
               // Query("DROP DATABASE `" + DatabaseName + "`;");
            }
        }
        public override bool CreateDB(string DBName = "")
        {
            if (DBName != "")
                this.DatabaseName = DBName;

            if (!HasDBName)
            {
                return false;
            }

            if (!DBExists(DBName))
            {
                return QueryMaker("CREATE DATABASE IF NOT EXISTS `" + DatabaseName + "`;");// Query("CREATE DATABASE IF NOT EXISTS `" + DatabaseName + "`;");
            }
            return true;
        }

        public Dictionary<string, int> VersionControl = new Dictionary<string, int>{
            { "Products", 1 },
            //{ "key2", 0 }
        };
        public bool Query(MySqlCommand cmd,bool IgnoreError=false)
        {
            try
            {
                cmd.Connection = connection;
                int R = cmd.ExecuteNonQuery();
                if (!R.Equals(0))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                ErrorMessage = e.Message;
            }
            return false;
        }
        public override bool Query(string CMD)
        {
            return Query(new MySql.Data.MySqlClient.MySqlCommand(CMD, connection));
        }


        public override bool DBExists(string DBName="")
        {
            if (DBName != "")
                this.DatabaseName = DBName;
            try
            {
                List<string> QueryResult = new List<string>();
                MySqlCommand cmdName = new MySqlCommand("SHOW DATABASES LIKE '"+DatabaseName+"'", connection);

                MySqlDataReader reader = cmdName.ExecuteReader();
                while (reader.Read())
                {
                    QueryResult.Add(reader.GetString(0));
                }
                reader.Close();
                
                return QueryResult.Count > 0;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
        }
    }
}