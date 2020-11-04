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
        public List<Employee> Employees { get; set; }
        public int ID { get; set; }

        public Treatment(string name, string description, int durationMinutes, double price)
        {
            Name = name;
            Description = description;
            Duration = durationMinutes;
            Price = price;
            Employees = new List<Employee>();
        }

        public Treatment(int id, string name, string description, int durationMinutes, double price, List<Employee> employees)
        {
            ID = id;
            Name = name;
            Description = description;
            Duration = durationMinutes;
            Price = price;
            Employees = employees;
        }

        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
        }
    }
}