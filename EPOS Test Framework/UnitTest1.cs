using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPOS_Test_Framework
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void MysqlDBValidConnection()
        {
            EPOS.DatabaseController DB = new EPOS.MysqlController();
            if (DB.Open("127.0.0.1", "root", "456goldie@A1"))
            {
                Assert.IsTrue(DB.DBExists("Test12321"), "Database 'Test12321' Not Found");
            }
            else
                Assert.Fail(DB.ErrorMessage);
            DB.Close();
        }

        [TestMethod]
        public void CreateDB()
        {
            EPOS.DatabaseController DB = new EPOS.MysqlController();
            if (DB.Open("127.0.0.1", "root", "456goldie@A1"))
            {
                if (!DB.CreateDB("Test12321"))
                {
                    Assert.Fail(DB.ErrorMessage);
                }
                else
                    DB.DeleteDB();


            }
            else
                Assert.Fail("Failed To Connect To DB");
            DB.Close();
        }
    }
}
