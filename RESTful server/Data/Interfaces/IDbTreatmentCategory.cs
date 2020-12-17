using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    interface IDbTreatmentCategory
    {
        void AddCategoryToTreatment(int treatmentID, int treatmentCategoryID);
        List<int> GetCategoryIDByTreatmentID(int treatmentID, SqlConnection connection);
    }
}
