using Model;
using RESTfulService.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTest
{
    static class InsertTestData
    {
        private static readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        public static void InsertData()
        {
            InsertCompany();
            InsertCategory();
            InsertTreatment();
            InsertCustomer();
            InsertEmployee();
        }

        private static void InsertCompany()
        {
            string companySQL = "INSERT INTO COMPANY (CVR, name, phone, address, postalCode, city, openingTime, closingTime) VALUES (123, 'Buthlers Service', 88888888, 'Danmarksgade 50', 9000, 'Aalborg', '08:00', '17:30');";
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = companySQL;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void InsertCategory()
        {
            string categorySQL = "INSERT INTO TreatmentCategory (name) VALUES ('haveservice')";
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = categorySQL;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void InsertTreatment()
        {
            TreatmentController treatmentCtrl = new TreatmentController();
            Treatment treatment = new Treatment(1, "Stor fed klip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m);
            List<TreatmentCategory> Categories = new List<TreatmentCategory>();
            Categories.Add(new TreatmentCategory(1, "Klip"));
            treatmentCtrl.Post(treatment, Categories);
        }

        private static void InsertCustomer()
        {
            string customerSQL = "INSERT INTO Customer (firstName, lastName, phone, email, address, postalCode, city) VALUES ('Hans', 'Larsen', '12345678', 'yo@ucn.dk', 'Banegårdsgade 3', 9000, 'Aalborg');";
            string customerSQL2 = "INSERT INTO Customer (firstName, lastName, phone, email, address, postalCode, city) VALUES ('William', 'Jensen', '43215678', 'whatis@updog.com', 'Banegårdsgade 12', 9000, 'Aalborg');";
            string resultSQL = $"{customerSQL} {customerSQL2}";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = resultSQL;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void InsertEmployee()
        {
            string employeeSQL = "INSERT INTO Employee (companyID, firstName, lastName, phone, email, address, postalCode, city) VALUES (1, 'Sanne', 'Liane', '87654321', 'hej@ucn.dk', 'Bygade 32', 9000, 'Aalborg');";
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = employeeSQL;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void _executeMultipleStatements(string[] statements)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                foreach (string statement in statements)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = statement;
                        command.Prepare();
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
