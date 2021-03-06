using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Treatment : Treatment_DTO
    {
        public int ID { get; set; }

        private List<Employee> _employees;

        // to create new Treatment object.
        public Treatment(int companyID, string name, string description, int duration, decimal price, List<int> treatmentCategoryID) : base (companyID, name, description, duration, price, treatmentCategoryID)
        {
            _employees = new List<Employee>();
        }

        //To create a new Treatment object from database without any employees.
        public Treatment(int id, int companyID, string name, string description, int duration, decimal price) : base ( companyID, name, description, duration, price, null) 
        {
            ID = id;
            _employees = new List<Employee>();
        }

        // to create object of Treatment which already exists in database.
        [JsonConstructor]
        public Treatment(int id, int companyID, string name, string description, int duration, decimal price, List<Employee> employees, List<int> treatmentCategoryID) : base (companyID, name, description, duration, price, treatmentCategoryID)
        {
            ID = id;
            _employees = employees;
        }

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employees;
        }
    }
}