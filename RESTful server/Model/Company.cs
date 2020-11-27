using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Company
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CVR { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Webpage { get; set; }

        public Company(string name, int cvr, string phone, string address, int postalcode, string city, string webpage)
        {
            Name = name;
            CVR = cvr;
            Phone = phone;
            Address = address;
            PostalCode = postalcode;
            City = city;
            Webpage = webpage;
        }
    }
}