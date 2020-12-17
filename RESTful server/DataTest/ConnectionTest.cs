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
        public void InitialiseBeforeEachMethod()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
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