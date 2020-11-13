using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Reservation
    {
        public int ID { get; set; }
        public int TreatmentID { get; set; }
        public int CustomerID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Reservation(int treatmentID, int customerID, DateTime startTime)
        {
            TreatmentID = treatmentID;
            CustomerID = customerID;
            StartTime = startTime;
        }
        public Reservation(int id, int treatmentID, int customerID, DateTime startTime)
        {
            ID = id;
            TreatmentID = treatmentID;
            CustomerID = customerID;
            StartTime = startTime;
        }

        public DateTime GetEndTime()
        {
            //Get duration on treatment
            //Get StartTime of reservation
            //Calculate EndTime
        }
    }
}