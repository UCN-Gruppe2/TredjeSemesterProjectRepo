using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class TreatmentCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Treatment> Treatments { get; set; }

        public TreatmentCategory(string name)
        {
            Name = name;
            Treatments = new List<Treatment>();
        }

        public TreatmentCategory (int id, string name)
        {
            ID = id;
            Name = name;
            Treatments = new List<Treatment>();
        }

        public void AddTreatment(Treatment toAdd)
        {
            Treatments.Add(toAdd);
        }
    }
}