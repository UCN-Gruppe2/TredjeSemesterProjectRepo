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
         "delete from Treatment;",
         "DBCC CHECKIDENT ('Reservation', RESEED, 0);",
         "DBCC CHECKIDENT ('Treatment', RESEED, 0);"
        };

        public static void CleanDB()
        {
            foreach (string command in cleanupCommands)
            {
                ExecuteCommand(command);
            }
        }

        private static void ExecuteCommand(string commandString)
        {
            string _connString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
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
        }
    }
}
