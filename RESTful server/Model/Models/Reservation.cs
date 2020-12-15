using Newtonsoft.Json;
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
        public int EmployeeID { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Constructor for a new object
        /// </summary>
        /// <param name="treatment"></param>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <param name="startTime"></param>
        public Reservation(Treatment treatment, int customerID, int employeeID, DateTime startTime)
        {
            TreatmentID = treatment.ID;
            CustomerID = customerID;
            EmployeeID = employeeID;
            StartTime = startTime;
            EndTime = GetEndTime(treatment.Duration);
        }

        /// <summary>
        /// Constructs an existing Reservation
        /// </summary>
        /// <param name="id">ReservationID</param>
        /// <param name="treatmentID"></param>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        [JsonConstructor]
        public Reservation(int id, System.Int32 treatmentID, System.Int32 customerID, System.Int32 employeeID, System.DateTime startTime, System.DateTime endTime)
        {
            ID = id;
            TreatmentID = treatmentID;
            CustomerID = customerID;
            EmployeeID = employeeID;
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime GetEndTime(int duration)
        {
            DateTime endTimeCalc = StartTime.AddMinutes(duration);
            return endTimeCalc;
        }
    }
}