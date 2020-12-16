﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IDbTreatment
    {
        //void InsertTreatmentToDatabase(Treatment treatment);
        Treatment GetTreatmentByID(int id);
        //void SaveTreatment();
        //bool DeleteTreatment();
        //bool UpdateTreatment();
        //List<Employee> GetCapableEmployees();
        Treatment InsertTreatmentToDatabase(Treatment treatment);
    }
}
