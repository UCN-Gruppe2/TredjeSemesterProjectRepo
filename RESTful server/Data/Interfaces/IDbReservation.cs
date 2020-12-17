using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    interface IDbReservation
    {
        List<Reservation> GetReservationsByEmployeeID(int id);
        Reservation InsertReservationToDatabase(Reservation reservation);
    }
}
