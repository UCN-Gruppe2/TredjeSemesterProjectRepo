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
using RestClientManagerNamespace;

namespace WebBookingInterface.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private static RestClient _client;

        public ReservationController()
        {
            _client = RestClientManager.GetInstance().RestClient;
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

        public ActionResult FindReservationsForEmployee()
        {
            return View();
        }

        public ActionResult FindReservationsForEmployee(int employeeID)
        {
            RestRequest request = new RestRequest("api/Reservation/GetReservationsByEmployeeID");

            List<Reservation> reservations = null;
            return View("EmployeeReservationsResult", reservations);
        }

        [HttpPost]
        public ActionResult Create(int treatmentID, int customerID, int employeeID, DateTime appointment_date, DateTime appointment_time)
        {
            DateTime appointment_dateTime = new DateTime(appointment_date.Year, appointment_date.Month, appointment_date.Day, appointment_time.Hour, appointment_time.Minute, 00);
            bool result;
            Exception exception;
            try
            {
                RestRequest reservationRequest = new RestRequest("/api/Reservation", Method.POST);
                var reservation_DTO = new Reservation_DTO(
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
                exception = e; // exception skulle logges, det kommer senere :))
                result = false;
            }
            return Json(result);
        }
    }
}