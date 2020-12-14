using Dapper;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataAccess.DatabaseAccess
{
    public class DBTreatmentCategory : IDbTreatmentCategory
    {
        private string _connectionString;
        public DBTreatmentCategory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public bool AddCategoryToTreatment(int treatmentID, int treatmentCategoryID)
        {
            bool result;
            try
            {
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.RepeatableRead,
                    Timeout = TimeSpan.FromSeconds(15) //<-- Timeout to prevent gridlocks, or any other type of blockage.
                };

                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        string insertStatement = "INSERT INTO CategoryOfTreatments (treatmentID, categoryID) VALUES (@treatmentID, @categoryID)";
                        int numberOfRowsAffected = conn.Execute(insertStatement, new
                        {
                            treatmentID = treatmentID,
                            categoryID = treatmentCategoryID
                        });

                        result = numberOfRowsAffected >= 1; //Den skulle gerne være 1... 
                    }
                }
            }
            catch (SqlException)
            {
                result = false; //Der kom en fejl, hvilket kan betyde at den allerede eksisterer.
            }

            return result;
        }

        public List<int> GetCategoryIDByTreatmentID(int treatmentID, SqlConnection connection)
        {
            string selectStatement = "SELECT categoryID FROM CategoryOfTreatments INNER JOIN Treatment ON CategoryOfTreatments.treatmentID = Treatment.id";
            List<int> listOfIDs = (List<int>)connection.Query<int>(selectStatement);

            return listOfIDs;
        }

        public List<int> GetCategoryIDByTreatmentID(int treatmentID)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead,
                Timeout = TimeSpan.FromSeconds(15) //<-- Timeout to prevent gridlocks, or any other type of blockage.
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    return GetCategoryIDByTreatmentID(treatmentID, conn);
                }
            }
        }
    }
}
