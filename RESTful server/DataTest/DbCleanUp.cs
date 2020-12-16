using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTest
{
    static class DbCleanUp
    {
        private static readonly string[] cleanupCommands = {
         "delete from Reservation;",
         "delete from Employee;",
         "delete from Customer;",
         "delete from CategoryOfTreatments;",
         "delete from Treatment;",
         "delete from TreatmentCategory;",
         "delete from Company;",
         "DBCC CHECKIDENT ('Reservation', RESEED, 0);",
         "DBCC CHECKIDENT ('Treatment', RESEED, 0);",
         "DBCC CHECKIDENT ('Company', RESEED, 0);",
         "DBCC CHECKIDENT ('Employee', RESEED, 0);",
         "DBCC CHECKIDENT ('Customer', RESEED, 0);",
         "DBCC CHECKIDENT ('TreatmentCategory', RESEED, 0);"
        };

        public static void CleanDB()
        {
            foreach (string command in cleanupCommands)
            {
                _executeCommand(command);
            }
        }

        private static void _executeCommand(string commandString)
        {
            string _connString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            Console.Write($"Running command: '{commandString}'... ");
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = commandString;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
            Console.Write($"Finished command!\r\n");
        }
    }
}
