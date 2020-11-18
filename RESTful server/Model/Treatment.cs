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
        public double Price { get; set; }
        public int ID { get; set; }

        private List<Employee> _employees;

        // to create new Treatment object.
        public Treatment(string name, string description, int durationMinutes, double price)
        {
            Name = name;
            Description = description;
            Duration = durationMinutes;
            Price = price;
            _employees = new List<Employee>();
        }

        // to create object of already existing Object.
        public Treatment(int id, string name, string description, int durationMinutes, double price, List<Employee> employees)
        {
            ID = id;
            Name = name;
            Description = description;
            Duration = durationMinutes;
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