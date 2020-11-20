using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Treatment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public int ID { get; set; }

        private List<Employee> _employees;

        // to create new Treatment object.
        public Treatment(string name, string description, int duration, decimal price)
        {
            Name = name;
            Description = description;
            Duration = duration;
            Price = price;
            _employees = new List<Employee>();
        }

        //To create a new Treatment object from database without any employees.
        public Treatment(int id, string name, string description, int duration, decimal price)
        {
            ID = id;
            Name = name;
            Description = description;
            Duration = duration;
            Price = Math.Round(price, 2);
            _employees = new List<Employee>();
        }

        // to create object of Treatment which already exists in database.
        public Treatment(int id, string name, string description, int duration, decimal price, List<Employee> employees)
        {
            ID = id;
            Name = name;
            Description = description;
            Duration = duration;
            Price = price;
            _employees = employees;
        }

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        public List<Employee> getAllEmployees()
        {
            return _employees;
        }
    }
}