
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Customer : Person
    {
        public List<Reservation> Reservations { get; set; }
        //New Customer
        public Customer(string firstName, string lastName, string phone, string address, string postalCode, string city) : base(firstName, lastName, phone, address, postalCode, city)
        {
            
        }

        //Existing customer
        public Customer(int customerID, string firstName, string lastName, string phone, string address, string postalCode, string city, int id) : base(firstName, lastName, phone, address, postalCode, city, id)
        {
           
        }
    }
}