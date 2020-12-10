using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Treatment_DTO
    {
        public int CompanyID;
        public string Name;
        public string Description;
        public int Duration;
        public decimal Price;

        public Treatment_DTO(int companyID, string name, string description, int duration, decimal price)
        {
            CompanyID = companyID;
            Name = name;
            Description = description;
            Duration = duration;
            Price = price;
        }
    }
}
