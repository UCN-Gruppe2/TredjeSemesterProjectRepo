using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;

namespace DataTest
{
    [TestClass]
    public class ConnectionTest
    {
        SqlConnection connection;

        [TestInitialize]
        public void InitialiseBeforeEachMethods()
        {
      //      string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            string connectionString = "server = hildur.ucn.dk; User Id=dmaa0919_1072100;Password=Password1!; Database = dmaa0919_1072100";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        [TestMethod]
        public void TestDoWeHaveTheConnectionToDatabase()
        {
            Assert.AreEqual(System.Data.ConnectionState.Open, connection.State);
        }

        [TestCleanup]
        public void CleanUp()
        {
            connection.Dispose();
        }

    }
}