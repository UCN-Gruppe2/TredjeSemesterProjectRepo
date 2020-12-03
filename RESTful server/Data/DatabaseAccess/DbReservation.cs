﻿using Data.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace DataAccess.DatabaseAccess
{
    public class DbReservation : IDbReservation
    {
        private string _connectionString;

        public DbReservation()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public Reservation GetReservationByID(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Reservation WHERE id = @id";
                Reservation result = conn.Query<Reservation>(sqlString, new { id = id }).FirstOrDefault();
                return result;
            }
        }

        public List<Reservation> GetReservationsByCustomerID(int id)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                string customerCheck = "SELECT * FROM Customer WHERE id = @id";
                var queryResult = conn.Query<int>(customerCheck, new { id });
                bool hasExisting = queryResult.Any();

                if  (hasExisting)
                {
                    string sqlString = "SELECT * FROM Reservation WHERE customerID = @id";
                    List<Reservation> results = (List<Reservation>)conn.Query<Reservation>(sqlString, new { id = id });
                    return results;
                }
                else
                {
                    throw new ArgumentException("Customer not found.");
                }
            }
        }

        public List<Reservation> GetReservationsByEmployeeID(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string employeeCheck = "SELECT * FROM Employee WHERE id = @id";
                var queryResult = conn.Query<int>(employeeCheck, new { id });
                bool hasExisting = queryResult.Any();

                if(hasExisting)
                {
                    string sqlString = "SELECT * FROM Reservation WHERE employeeID = @id";
                    List<Reservation> results = (List<Reservation>)conn.Query<Reservation>(sqlString, new { id = id });
                    return results;
                }
                else
                {
                    throw new ArgumentException("Employee not found.");
                }
                
            }
        }

        public void SaveReservation()
        {
            throw new NotImplementedException();
        }

        public bool DeleteReservation()
        {
            throw new NotImplementedException();
        }

        public bool UpdateReservation()
        {
            throw new NotImplementedException();
        }

        public Employee GetEmployee()
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer()
        {
            throw new NotImplementedException();
        }

        public Treatment GetTreatment()
        {
            throw new NotImplementedException();
        }

        public Reservation InsertReservationToDatabase(Reservation reservation)
        {
            int treatmentID = reservation.TreatmentID;
            int customerID = reservation.CustomerID;
            int employeeID = reservation.EmployeeID;
            DateTime startTime = reservation.StartTime;
            DateTime endTime = reservation.EndTime;

            //Implicit transaction med IsolationLevel
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable,
                Timeout = TimeSpan.FromSeconds(15) //<-- Timeout to prevent gridlocks, or any other type of blockage.
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (var conn = new SqlConnection(_connectionString))
                {

                    //SQL statement, hvis der returneres >0 findes der allerede reservation(er) i det ønskede tidsrum.
                    string findExistingReservation = "SELECT * FROM Reservation WHERE employeeID = @employeeID AND (" +
                        "(startTime <= @startTime AND endTime > @startTime)" +
                        "OR (startTime >= @startTime AND startTime < @endTime)" +
                        "OR (endTime > @startTime AND startTime < @endTime)" + 
                        "OR (endTime >= @endTime AND startTime < @endTime)" + 
                        "OR (endTime <= @endTime AND endTime > @startTime))";

                    var queryResult = conn.Query<int>(findExistingReservation, new { employeeID, startTime, endTime });
                    bool hasExisting = queryResult.Any();

                    if (!hasExisting)
                    {
                        string queryString = "INSERT INTO Reservation (treatmentID, customerID, employeeID, startTime, endTime) VALUES (@treatmentID, @customerID, @employeeID, @startTime, @endTime); " +
                            "SELECT SCOPE_IDENTITY()";

                        var id = conn.ExecuteScalar<int>(queryString, new
                        {
                            treatmentID,
                            customerID,
                            employeeID,
                            startTime,
                            endTime
                        });

                        var result = conn.Query<Reservation>("SELECT * FROM Reservation WHERE id = @id", new { id = id }).FirstOrDefault();
                        scope.Complete();
                        return result;
                    }
                    else
                    {
                        throw new ArgumentException("The reservation could not be inserted.");
                    }
                }

            }
        }

        //Not for use!
        public Reservation _InsertReservationToDatabase(Reservation reservation)
        {
            int treatmentID = reservation.TreatmentID;
            int customerID = reservation.CustomerID;
            int employeeID = reservation.EmployeeID;
            DateTime startTime = reservation.StartTime;
            DateTime endTime = reservation.EndTime;

            //Implicit transaction med IsolationLevel
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable,
                Timeout = TimeSpan.FromSeconds(15) //<-- Timeout to prevent gridlocks, or any other type of blockage.
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (var conn = new SqlConnection(_connectionString))
                {

                    //SQL statement, hvis der returneres >0 findes der allerede reservation(er) i det ønskede tidsrum.
                    string betterString = "INSERT INTO Reservation (treatmentID, customerID, employeeID, startTime, endTime) " +
                        "VALUES (@treatmentID, @customerID, @employeeID, @startTime, @endTime) " +
                        "IF(SELECT COUNT(1) FROM Reservation WHERE(employeeID = @employeeID AND(" +
                        "(startTime >= @startTime AND endTime < @startTime)" +
                        "OR (startTime <= @startTime AND startTime < @endTime)" +
                        "OR (endTime >= @endTime AND endTime > @startTime)))) = 0" +
                        "SELECT SCOPE_IDENTITY()";

                    string bestString = "IF EXISTS(SELECT * FROM Reservation WHERE(employeeID = @employeeID AND(" +
                        "(startTime >= @startTime AND endTime < @startTime)" +
                        "OR (startTime <= @startTime AND startTime < @endTime)" +
                        "OR (endTime >= @endTime AND endTime > @startTime)))) = 0 " +
                        "BEGIN INSERT INTO Reservation (treatmentID, customerID, employeeID, startTime, endTime) " +
                        "VALUES (@treatmentID, @customerID, @employeeID, @startTime, @endTime)" +
                        "SELECT SCOPE_IDENTITY()";


                    var id = conn.ExecuteScalar<int>(betterString, new
                        {
                            treatmentID,
                            customerID,
                            employeeID,
                            startTime,
                            endTime
                        });

                        var result = conn.Query<Reservation>("SELECT * FROM Reservation WHERE id = @id", new { id = id }).FirstOrDefault();
                        scope.Complete();
                        return result;
                }
            }
        }
    }
}