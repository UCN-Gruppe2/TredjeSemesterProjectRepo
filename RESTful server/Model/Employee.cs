using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Employee : Person
    {
        public int EmployeeID { get; set; }
        public int CompanyID { get; set; }
        //New Customer
        public Employee(string firstName, string lastName, string phone, int companyID, string address, int postalCode, string city) : base(firstName, lastName, phone, address, postalCode, city)
        {
            CompanyID = companyID;
        }

        //Existing customer
        public Employee(int employeeID, string firstName, string lastName, string phone, int companyID, string address, int postalCode, string city) : base(firstName, lastName, phone, address, postalCode, city)
        {
            EmployeeID = employeeID;
            CompanyID = companyID;
        }

    }
}