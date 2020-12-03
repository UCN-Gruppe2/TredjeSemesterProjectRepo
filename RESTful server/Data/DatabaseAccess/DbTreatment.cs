using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Model;
using Dapper;
using System.Transactions;

namespace DataAccess.DatabaseAccess
{
    public class DbTreatment : IDbTreatment
    {
        private string _connectionString;

        public DbTreatment()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public bool DeleteTreatment()
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetCapableEmployees()
        {
            throw new NotImplementedException();
        }

        public Treatment GetTreatmentByID(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Treatment WHERE id = @id";
                Treatment result = conn.Query<Treatment>(sqlString, new { id = id }).FirstOrDefault();
                return result;
            }
        }

        public Treatment InsertTreatmentToDatabase(Treatment treatment)
        {
            int companyID = treatment.CompanyID;
            string name = treatment.Name;
            string description = treatment.Description;
            int duration = treatment.Duration;
            decimal price = treatment.Price;

            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead,
                Timeout = TimeSpan.FromSeconds(15) //<-- Timeout to prevent gridlocks, or any other type of blockage.
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string checkString = "SELECT companyID, name, description, duration, price FROM Treatment WHERE (companyID = @companyID AND name = @name AND description = @description AND " +
                        "duration = @duration AND price = @price)";

                    var sqlSelectReader = conn.ExecuteReader(checkString, new { companyID, name, description, duration, price });

                    if (!sqlSelectReader.Read())
                    {
                        sqlSelectReader.Close();
                        string queryString = "INSERT INTO Treatment (companyID, name, description, duration, price) VALUES (@companyID, @name, @description, @duration, @price); " +
                                "SELECT SCOPE_IDENTITY()";

                        var id = conn.ExecuteScalar<int>(queryString, new
                        {
                            companyID,
                            name,
                            description,
                            duration,
                            price
                        });

                        //scope.Complete();
                        var result = conn.Query<Treatment>("SELECT * FROM Treatment WHERE id = @id", new { id = id }).FirstOrDefault();
                        scope.Complete();
                        return result;
                    }
                    else
                    {
                        throw new ArgumentException("A treatment with the specified values already exists in the database.");
                    }
                }
            }
        }

        public void SaveTreatment()
        {
            throw new NotImplementedException();
        }

        public bool UpdateTreatment()
        {
            throw new NotImplementedException();
        }

        List<Employee> IDbTreatment.GetCapableEmployees()
        {
            throw new NotImplementedException();
        }

        Treatment IDbTreatment.GetTreatmentByID(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTreatmentsInCategory(TreatmentCategory category)
        {
            int categoryID = category.ID;
            string name = category.Name;
            List<Treatment> treatments = category.Treatments;

            foreach (Treatment t in treatments)
            {
                int treatmentID = t.ID;
                using (var conn = new SqlConnection(_connectionString))
                {
                    string checkString = "SELECT categoryID, treatmentID FROM CategoryOfTreatments WHERE (categoryID = @categoryID AND treatmentID = @treatmentID)";

                    var sqlSelectReader = conn.ExecuteReader(checkString, new { categoryID = categoryID, treatmentID = treatmentID });

                    if (!sqlSelectReader.Read())
                    {
                        sqlSelectReader.Close();
                        string queryString = "INSERT INTO CategoryOfTreatments (treatmentID, categoryID) VALUES (@treatmentID, @categoryID);";
                        //Console.WriteLine("INSERT INTO CategoryOfTreatments (treatmentID, categoryID) VALUES ({0}, {1})", treatmentID, categoryID);
                        conn.ExecuteReader(queryString, new
                        {
                            treatmentID,
                            categoryID
                        });
                    }
                }
            }
        }
    }
}
