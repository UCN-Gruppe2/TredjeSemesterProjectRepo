using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Employee : Person
    {
        //New Customer
        public Employee(string firstName, string lastName, string phone, string address, string postalCode, string city) : base(firstName, lastName, phone, address, postalCode, city)
        {

        }

        //Existing customer
        public Employee(int employeeID, string firstName, string lastName, string phone, string address, string postalCode, string city, int id) : base(firstName, lastName, phone, address, postalCode, city, id)
        {
            ID = id;
        }

    }
}