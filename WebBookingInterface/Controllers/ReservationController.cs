using DataAccess.DatabaseAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebBookingInterface.Controllers
{
    public class ReservationController : Controller
    {
        private RestClient _client;

        public ReservationController()
        {
            _client = new RestClient("https://localhost:44388/");
            string authToken = _getToken();
            _client.AddDefaultHeader("Authentication", $"Bearer {authToken}");
        }

        private string _getToken()
        {
            var request = new RestRequest("/Token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("userName", "mail@marcuslc.com"); //Note to self: brug Web.config i stedet for!
            request.AddParameter("password", "Password1!"); //Samme her
            var responseAsParsedJSON = JObject.Parse(_client.Execute(request).Content);

            return responseAsParsedJSON["access_token"].ToString();
        }

        // GET: Reservation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult FindPersonalReservations()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(int companyID, int treatmentID, int customerID, int employeeID, DateTime appointment_date, DateTime appointment_time)
        {
            DateTime appointment_dateTime = new DateTime(appointment_date.Year, appointment_date.Month, appointment_date.Day, appointment_time.Hour, appointment_time.Minute, 00);
            bool result;
            Exception exception;
            try
            {
                RestRequest reservationRequest = new RestRequest("/api/Reservation", Method.POST);
                reservationRequest.AddHeader("Content-type", "application/json");
                var reservation_DTO = new Reserveration_DTO()
                {
                    CompanyID = companyID, 
                    EmployeeID = employeeID,
                    CustomerID = customerID,
                    TreatmentID = treatmentID,
                    Appointment_dateTime = appointment_date
                };

                //var reservationJSOnObject = JObject.FromObject(reservation_DTO);
                reservationRequest.AddJsonBody(reservation_DTO);
                //reservationRequest.Body = new RequestBody()
                //{
                //    ContentType = "application/json",
                //    Value = reservation_DTO
                //};
                //reservationRequest.RequestFormat = DataFormat.Json;
                //reservationRequest.AddParameter("companyID", companyID);
                //reservationRequest.AddParameter("treatmentID", treatmentID);
                //reservationRequest.AddParameter("customerID", customerID);
                //reservationRequest.AddParameter("employeeID", employeeID);
                //reservationRequest.AddParameter("appointment_dateTime", appointment_dateTime);
                var response = _client.Execute(reservationRequest);



                //Treatment treatmentToUse = _dbTreatment.GetTreatmentByID(treatmentID);
                //Reservation reservationToAdd = new Reservation(treatmentToUse, customerID, employeeID, appointment_dateTime);
                //_dbReservation.InsertReservationToDatabase(reservationToAdd);
                result = response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                exception = e;
                result = false;
            }
            return Json(result);
        }
    }
}