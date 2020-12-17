using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IDbTreatment
    {
        Treatment GetTreatmentByID(int id);
        Treatment InsertTreatmentToDatabase(Treatment treatment);
        void UpdateTreatmentsInCategory(TreatmentCategory category);
    }
}
