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

            using (var conn = new SqlConnection(_connectionString))
            {
                //string checkStringStartTime = "SELECT treatmentID, customerID, employeeID, startTime, endTime "
                    //+ "FROM Reservation WHERE (employeeID = @employeeID AND startTime = @startTime AND endTime = @endTime)"; 
                                                                                                                                //startTime = 13:30 | endTime = 14:00
                                                                                                                               //@startTime = 13:45 | @endTime = 13:55
                                                                                                                               //Result = DEN MÅ IKKE! >:(
                //Det her er strengt forbudt!
                string findExistingReservation = "SELECT COUNT(1) FROM Reservation WHERE (employeeID = @employeeID AND ("+
                    "(startTime >= @startTime AND endTime < @startTime)" + 
                    "OR (startTime <= @startTime AND startTime < @endTime)" +
                    "OR endTime >= @endTime AND endTime > @startTime))";

                //Det her må den gerne :::-----))))))
                //"(startTime < @startTime AND endTime > @startTime)" + //Hvis startTime sker før @startTime, så skal den eksisterende endTime ske før @startTime. 
                //    "OR (startTime > @startTime AND startTime >= @endTime)" + //
                //    "OR endTime < @endTime AND endTime <= @startTime))"; //

                //var sqlSelectReader = conn.ExecuteReader(checkStringStartTime, new { employeeID, startTime, endTime });
                var queryResult = conn.Query<int>(findExistingReservation, new { employeeID, startTime, endTime });
                bool hasExisting = queryResult.First<int>() > 0;

                if (!hasExisting)
                {
                    //sqlSelectReader.Close();
                    string queryString = "INSERT INTO Reservation (treatmentID, customerID, employeeID, startTime, endTime) VALUES (@treatmentID, @customerID, @employeeID, @startTime, @endTime); " +
                        "SELECT SCOPE_IDENTITY()";

                    var id = conn.ExecuteScalar<int>(queryString, new {
                        treatmentID, customerID, employeeID, startTime, endTime
                    });
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
