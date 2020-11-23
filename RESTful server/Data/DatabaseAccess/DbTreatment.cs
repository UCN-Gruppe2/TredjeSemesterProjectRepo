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
            throw new NotImplementedException();
        }

        public Treatment InsertTreatmentToDatabase(Treatment treatment)
        {
            string name = treatment.Name;
            string description = treatment.Description;
            int duration = treatment.Duration;
            decimal price = treatment.Price;

            using (var conn = new SqlConnection(_connectionString))
            {
                string checkString = "SELECT name, description, duration, price FROM Treatment WHERE (name = @name AND description = @description AND " +
                    "duration = @duration AND price = @price)";
                Treatment existingTreatment = conn.Query<Treatment>(checkString, new { name = name, description = description, duration = duration, price = price }).FirstOrDefault();
                if(existingTreatment == null) { 
                    string queryString = "INSERT INTO Treatment (name, description, duration, price) VALUES (@name, @description, @duration, @price); " +
                        "SELECT SCOPE_IDENTITY()";

                    var id = conn.ExecuteScalar<int>(queryString, new
                    {
                        name, description, duration, price
                    });
                    return conn.Query<Treatment>("SELECT * FROM Treatment WHERE Id = @id", new { id=id }).FirstOrDefault();
                }
                else
                {
                    throw new ArgumentException("A treatment with the specified values already exists in the database.");
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
    }
}
