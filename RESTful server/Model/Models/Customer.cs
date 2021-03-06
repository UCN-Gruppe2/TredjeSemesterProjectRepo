﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Customer : Person
    {
        public int CustomerID { get; set; }
        public List<Reservation> Reservations { get; set; }
        //New Customer
        public Customer(string firstName, string lastName, string phone, string email, string address, int postalCode, string city) : base(firstName, lastName, phone, email, address, postalCode, city)
        {

        }

        //Existing customer
        public Customer(int customerID, string firstName, string lastName, string phone, string email, string address, int postalCode, string city) : base(firstName, lastName, phone, email, address, postalCode, city)
        {
            CustomerID = customerID;
        }
    }
}