using DataAccess.DatabaseAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RESTfulService.Controllers
{
    public class TreatmentController : ApiController
    {
        private DbTreatment _dbTreatment = new DbTreatment();

        // GET: api/Treatment
        public IEnumerable<string> GetAll()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Treatment/5
        public Treatment Get(int id)
        {
            Treatment found = _dbTreatment.GetTreatmentByID(id);
            return found;
        }

        // POST: api/Treatment
        public Treatment Post([FromBody] Treatment value, List<TreatmentCategory> categories)
        {
            Treatment treatmentAdded = null;
            if (value.Duration > 0 && value.Price > 0)
            {
                try
                {
                    treatmentAdded = _dbTreatment.InsertTreatmentToDatabase(value);
                }
                catch (ArgumentException e)
                {
                    throw e;
                }
            }
            else
            {
                throw new ArgumentException();
            }

            if(treatmentAdded != null)
            {
                foreach(TreatmentCategory element in categories)
                {
                    element.AddTreatment(treatmentAdded);
                    _dbTreatment.UpdateTreatmentsInCategory(element);
                }
            }
            return treatmentAdded;
        }

            // PUT: api/Treatment/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE: api/Treatment/5
        public void Delete(int id)
        {
        }
    }
}
