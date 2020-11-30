using Data.Interfaces;
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
            throw new NotImplementedException();
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
                IsolationLevel = IsolationLevel.RepeatableRead
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (var conn = new SqlConnection(_connectionString))
                {

                    //SQL statement, hvis der returneres >0 findes der allerede reservation(er) i det ønskede tidsrum.
                    string findExistingReservation = "SELECT COUNT(1) FROM Reservation WHERE (employeeID = @employeeID AND (" +
                        "(startTime >= @startTime AND endTime < @startTime)" +
                        "OR (startTime <= @startTime AND startTime < @endTime)" +
                        "OR endTime >= @endTime AND endTime > @startTime))";

                    var queryResult = conn.Query<int>(findExistingReservation, new { employeeID, startTime, endTime });
                    bool hasExisting = queryResult.First<int>() > 0;

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
                        scope.Complete();
                        return conn.Query<Reservation>("SELECT * FROM Reservation WHERE id = @id", new { id = id }).FirstOrDefault();
                    }
                    else
                    {
                        throw new ArgumentException("The reservation could not be inserted.");
                    }
                }

            }
        }
    }
}