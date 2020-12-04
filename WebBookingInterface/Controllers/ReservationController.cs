using DataAccess.DatabaseAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBookingInterface.Controllers
{
    public class ReservationController : Controller
    {

        private DbReservation _dbReservation;
        private DbTreatment _dbTreatment;

        public ReservationController()
        {
            _dbReservation = new DbReservation();
            _dbTreatment = new DbTreatment();
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
                Treatment treatmentToUse = _dbTreatment.GetTreatmentByID(treatmentID);
                Reservation reservationToAdd = new Reservation(treatmentToUse, customerID, employeeID, appointment_dateTime);
                _dbReservation.InsertReservationToDatabase(reservationToAdd);
                result = true;
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