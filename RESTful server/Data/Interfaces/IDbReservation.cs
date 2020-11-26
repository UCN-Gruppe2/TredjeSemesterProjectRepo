using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    interface IDbReservation
    {
        Reservation GetReservationByID(int id);
        void SaveReservation();
        bool DeleteReservation();
        bool UpdateReservation();
        Employee GetEmployee();
        Customer GetCustomer();
        Treatment GetTreatment();
        Reservation InsertReservationToDatabase(Reservation reservation);
    }
}
