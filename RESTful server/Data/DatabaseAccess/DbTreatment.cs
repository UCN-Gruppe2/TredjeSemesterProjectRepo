using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Model;

namespace DataAccess.DatabaseAccess
{
    public class DbTreatment : ITreatment
    {
        public bool DeleteTreatment()
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetCapableEmployees()
        {
            throw new NotImplementedException();
        }

        public Treatment GetTreatmentByID(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertTreatment(Treatment treatment)
        {
            throw new NotImplementedException();
        }

        public void SaveTreatment()
        {
            throw new NotImplementedException();
        }

        public bool UpdateTreatment()
        {
            throw new NotImplementedException();
        }

        List<Employee> ITreatment.GetCapableEmployees()
        {
            throw new NotImplementedException();
        }

        Treatment ITreatment.GetTreatmentByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
