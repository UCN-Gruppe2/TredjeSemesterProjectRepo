using DataAccess.DatabaseAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RESTfulService.Controllers
{
    [Authorize]
    public class TreatmentController : ApiController
    {
        private DbTreatment _dbTreatment = new DbTreatment();
        private DBTreatmentCategory _dbTreatmentCategory = new DBTreatmentCategory();

        // GET: api/Treatment/5
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult result;
            Treatment treatmentFound = _dbTreatment.GetTreatmentByID(id);
            if (treatmentFound != null)
            {
                result = Ok(treatmentFound);
            }
            else
            {
                result = Content(HttpStatusCode.NotFound, $"The id #{id} was not found!");
            }

            return result;
        }

        // POST: api/Treatment
        public IHttpActionResult Post([FromBody] Treatment_DTO value)
        {
            IHttpActionResult result;

            try
            {
             //   if () throw new ArgumentException("Illegal duration!");
                if (value.Duration > 0 && value.Price >= 0 && value.Name.Trim().Length > 0 && value.Duration % 30 == 0 && value.Duration <= 120)
                {
                    var treatmentToAddObj = new Treatment(value.CompanyID, value.Name, value.Description, value.Duration, value.Price, value.TreatmentCategoryID);
                    Treatment treatmentAdded = _dbTreatment.InsertTreatmentToDatabase(treatmentToAddObj);

                    if (treatmentAdded != null && value.TreatmentCategoryID != null)
                    {
                        foreach (int categoryID in value.TreatmentCategoryID)
                        {
                            _dbTreatmentCategory.AddCategoryToTreatment(treatmentAdded.ID, categoryID);
                        }
                        treatmentAdded.TreatmentCategoryID = value.TreatmentCategoryID;
                    }
                    result = Ok(treatmentAdded);
                }
                else
                {
                    result = Content(HttpStatusCode.Conflict, "The Arguments provided were invalid.");
                }
            }
            catch (SqlException)
            {
                result = result = Content(HttpStatusCode.InternalServerError, $"Data could not be inserted.");
            }
            catch (ArgumentException)
            {
                result = result = Content(HttpStatusCode.BadRequest, $"Invalid arguments provided.");
            }
            catch (AlreadyExistsException)
            {
                result = result = Content(HttpStatusCode.Conflict, $"The dataset already exists.");
            }

            return result;
        }
    }
}
