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
        private DBTreatmentCategory _dbTreatmentCategory = new DBTreatmentCategory();

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
        public Treatment Post([FromBody] Treatment_DTO value)
        {
            Treatment treatmentAdded = null;
            if (value.Duration > 0 && value.Price > 0)
            {
                try
                {
                    var treatmentToAddObj = new Treatment(value.CompanyID, value.Name, value.Description, value.Duration, value.Price, value.TreatmentCategoryID);
                    treatmentAdded = _dbTreatment.InsertTreatmentToDatabase(treatmentToAddObj);
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

            if (treatmentAdded != null && value.TreatmentCategoryID != null)
            {
                foreach (int categoryID in value.TreatmentCategoryID)
                {
                    _dbTreatmentCategory.AddCategoryToTreatment(treatmentAdded.ID, categoryID);
                }
                treatmentAdded.TreatmentCategoryID = value.TreatmentCategoryID;
            }
            return treatmentAdded;
        }

        // PUT: api/Treatment/5
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE: api/Treatment/5
        public void Delete(int id)
        {
        }
    }
}
