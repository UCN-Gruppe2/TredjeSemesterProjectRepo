﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess.DatabaseAccess;

namespace RESTfulService.Controllers
{
    public class ReservationController : ApiController
    {
        private DbReservation _dbReservation = new DbReservation();

        // GET: api/Reservation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Reservation/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Reservation
        public Reservation Post([FromBody] Reservation value)
        {
            Reservation reservationAdded = null;
            //try
            //{
            if(value.CustomerID < 0)
            {
                throw new ArgumentException("The CustomerID doesn't  exist.");
            }
            else if(value.EmployeeID < 0)
            {
                throw new ArgumentException("The EmployeeID doesn't  exist.");
            }
            else if(value.TreatmentID < 0)
            {
                throw new ArgumentException("The TreatmentID doesn't  exist.");
            }
            else
            {
                reservationAdded = _dbReservation.InsertReservationToDatabase(value);
            }
            //}
            //catch (ArgumentException e)
            //{
            //    throw e;
            //}
            return reservationAdded;
        }

        // PUT: api/Reservation/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Reservation/5
        public void Delete(int id)
        {
        }
    }
}