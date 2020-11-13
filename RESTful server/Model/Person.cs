using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int ID { get; set; }

        public Person(string firstName, string lastName, string phone, string address, string postalCode, string city)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Address = address;
            PostalCode = postalCode;
            City = city;
        }

        public Person(string firstName, string lastName, string phone, string address, string postalCode, string city, int id)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Address = address;
            PostalCode = postalCode;
            City = city;
            ID = id;
        }
        
        public override string ToString()
        {
            return $"{FirstName} {LastName}, Tlf.: {Phone}, Address: {Address}, {PostalCode} {City}";
        }
    }
}
