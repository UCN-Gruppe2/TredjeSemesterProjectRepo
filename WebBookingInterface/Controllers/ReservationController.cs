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
    [Authorize]
    public class ReservationController : Controller
    {
        private static RestClient _client;

        public ReservationController()
        {
            if (_client == null)
            {
                _client = new RestClient("https://localhost:44388/");
                string authToken = _getToken();
                _client.AddDefaultHeader("Authorization", $"Bearer {authToken}");
            }
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
                var reservation_DTO = new Reservation_DTO(
                    companyID: companyID,
                    employeeID: employeeID,
                    customerID: customerID,
                    treatmentID: treatmentID,
                    startTime: appointment_dateTime
                );

                reservationRequest.AddJsonBody(reservation_DTO);
                var response = _client.Execute(reservationRequest);

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