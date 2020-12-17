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


        public ActionResult EmployeeReservationsResult(int employeeID)
        {
            RestRequest request = new RestRequest("api/Employee/", Method.GET);
            request.AddParameter("employeeID", employeeID);
            RestResponse response = (RestResponse)_client.Execute(request);

            ActionResult viewToReturn;

            //GUTTER, I SKAL FANDME FÅ FIXET DET HER, VI ER FOR DUMME TIL DET!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.EmployeeID = employeeID;
                ViewBag.Reservations = JsonConvert.DeserializeObject<List<Reservation>>(response.Content);
                viewToReturn = View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(int treatmentID, int customerID, int employeeID, DateTime appointment_date, string appointment_time)
        {
            DateTime appointment_timeObj = DateTime.Parse(appointment_time);
            if (appointment_timeObj.Minute % 30 != 0) throw new Exception("Illegal minute... How did You get here?? Begone, hacker! callPoliceOn(this.User);");

            DateTime appointment_dateTime = new DateTime(appointment_date.Year, appointment_date.Month, appointment_date.Day, appointment_timeObj.Hour, appointment_timeObj.Minute, 00);

            RestRequest reservationRequest = new RestRequest("/api/Reservation", Method.POST);
            var reservation_DTO = new Reservation_DTO(
                employeeID: employeeID,
                customerID: customerID,
                treatmentID: treatmentID,
                startTime: appointment_dateTime
            );

            reservationRequest.AddJsonBody(reservation_DTO);
            var response = _client.Execute(reservationRequest);

            ActionResult viewToReturn;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.Reservation = JsonConvert.DeserializeObject<Reservation>(response.Content);
                viewToReturn = View("SuccessView");
            }
            else
            {
                ViewBag.ExceptionAsString = response.Content;
                viewToReturn = View("FailView");
            }

            return viewToReturn;
        }
    }
}