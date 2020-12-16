﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess.DatabaseAccess;
using System.Data.SqlClient;
//using DataAccess.Interfaces;

namespace RESTfulService.Controllers
{
    [Authorize]
    public class ReservationController : ApiController
    {
        private DbReservation _dbReservation = new DbReservation();
        private DbTreatment _dbTreatment = new DbTreatment();

        // POST: api/Reservation
        [Authorize]
        [HttpPost]
        public Reservation Post([FromBody] Reservation_DTO reservation_DTO)
        {
            //try
            //{
            if (reservation_DTO.CustomerID < 0)
            {
                var exceptionToThrow = new ArgumentException("The CustomerID is not valid.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, exceptionToThrow.Message));
            }
            else if (reservation_DTO.EmployeeID < 0)
            {
                var exceptionToThrow = new ArgumentException("The EmployeeID is not valid.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, exceptionToThrow.Message));
            }
            else if (reservation_DTO.TreatmentID < 0)
            {
                var exceptionToThrow = new ArgumentException("The Treatment is not valid.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, exceptionToThrow.Message));
            }
            else if (reservation_DTO.StartTime.ToLocalTime().CompareTo(DateTime.Now) < 0)
            {
                var exceptionToThrow = new ArgumentException("The start-time is not valid.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, exceptionToThrow.Message));
            }
            try
            {
                Treatment treatmentToUse = _dbTreatment.GetTreatmentByID(reservation_DTO.TreatmentID);
                Reservation reservationToAdd = new Reservation(treatmentToUse, reservation_DTO.CustomerID, reservation_DTO.EmployeeID, reservation_DTO.StartTime);
                return _dbReservation.InsertReservationToDatabase(reservationToAdd);
            }
            catch (SqlException)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not insert data into database."));
            }
            catch (NullReferenceException)
            {
                var exceptionToThrow = new NullReferenceException($"The Treatment with the ID ({reservation_DTO.TreatmentID}) was not found.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, exceptionToThrow.Message));
            }
            catch (ArgumentException)
            {
                var exceptionToThrow = new ArgumentException("There occurred a conflict with the selected time.");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, exceptionToThrow.Message));
            }
        }

        //// PUT: api/Reservation/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/Reservation/5
        //public void Delete(int id)
        //{
        //}
    }
}
