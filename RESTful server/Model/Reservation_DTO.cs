using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Reservation_DTO
    {
        public int CompanyID;
        public int TreatmentID;
        public int CustomerID;
        public int EmployeeID;
        public DateTime StartTime;

        public Reservation_DTO(int companyID, int treatmentID, int customerID, int employeeID, DateTime startTime)
        {
            CompanyID = companyID;
            TreatmentID = treatmentID;
            CustomerID = customerID;
            EmployeeID = employeeID;
            StartTime = startTime;
        }
    }
}
