﻿using System;
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
        private DBTreatmentCategory _dbTreatmentCategory;

        public DbTreatment()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            _dbTreatmentCategory = new DBTreatmentCategory();
        }

        public Treatment GetTreatmentByID(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return GetTreatmentByID(id, conn);
            }
        }

        public Treatment GetTreatmentByID(int id, SqlConnection connection)
        {
            string sqlString = "SELECT * FROM Treatment WHERE id = @id";
            List<int> categoryIDs = _dbTreatmentCategory.GetCategoryIDByTreatmentID(id, connection);
            Treatment result = connection.Query<Treatment>(sqlString, new { id = id }).FirstOrDefault();
            if (result != null)
            {
                result.TreatmentCategoryID = categoryIDs;
            }
            return result;
        }


        public Treatment InsertTreatmentToDatabase(Treatment treatment)
        {
            int companyID = treatment.CompanyID;
            string name = treatment.Name.Trim();
            string description = treatment.Description.Trim();
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

                        var result = this.GetTreatmentByID(id, conn);
                        scope.Complete();
                        return result;
                    }
                    else
                    {
                        throw new AlreadyExistsException("A treatment with the specified values already exists in the database.");
                    }
                }
            }
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
