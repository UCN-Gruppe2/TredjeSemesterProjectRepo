using Model;
using RESTfulService.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace DataTest
{
    static class InsertTestData
    {
        private static readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        public static void InsertData()
        {
            _insertCompany();
            _insertCategory();
            _insertTreatment();
            _insertCustomer();
            _insertEmployee();
            _insertReservation();
        }

        private static void _insertCompany()
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

        private static void _insertCategory()
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

        private static void _insertTreatment()
        {
            TreatmentController treatmentCtrl = new TreatmentController();
            //List<int> Categories = new List<TreatmentCategory>();
            //Categories.Add(new TreatmentCategory(1));
            Treatment_DTO treatment = new Treatment_DTO(1, "Stor fed klip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m, new List<int> { 1 });


            treatmentCtrl.Post(treatment);
        }

        private static void _insertCustomer()
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

        private static void _insertEmployee()
        {
            string employeeSQL = "INSERT INTO Employee (companyID, firstName, lastName, phone, email, address, postalCode, city) VALUES (1, 'Sanne', 'Liane', '87654321', 'hej@ucn.dk', 'Bygade 32', 9000, 'Aalborg');";
            string employeeSQL2 = "INSERT INTO Employee (companyID, firstName, lastName, phone, email, address, postalCode, city) VALUES (1, 'Lene', 'Sorensen', '87651234', 'dav@ucn.dk', 'Bygade 32', 9000, 'Aalborg');";
            string resultSQL = $"{employeeSQL} {employeeSQL2}";
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

        private static void _insertReservation()
        {
            ReservationController reservationCtrl = new ReservationController();
            TreatmentController treatmentCtrl = new TreatmentController();

            IHttpActionResult treatmentResult = treatmentCtrl.Get(1);
            Treatment treatment = ((OkNegotiatedContentResult<Treatment>)treatmentResult).Content;
            Reservation_DTO reservation1 = new Reservation_DTO(
                startTime: DateTime.Parse("31-12-2025 23:59"),
                employeeID: 1,
                customerID: 1,
                treatmentID: treatment.ID
            //(treatment, 1, 1, DateTime.Parse("26-11-2010 13:30")
            );

            Reservation_DTO reservation2 = new Reservation_DTO(
                startTime: DateTime.Parse("25-10-2026 17:30"),
                employeeID: 1,
                customerID: 1,
                treatmentID: treatment.ID
            );

            //Reservation reservation2 = new Reservation(treatment, 1, 1, DateTime.Parse("25-10-2021 17:30"));

            //reservationCtrl.Post((Reservation_DTO)reservation1);
            reservationCtrl.Post(reservation1);
            reservationCtrl.Post(reservation2);
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
