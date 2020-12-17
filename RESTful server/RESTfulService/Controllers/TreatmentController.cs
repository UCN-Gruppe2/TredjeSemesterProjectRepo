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
                result = NotFound();
            }

            return result;
        }

        // POST: api/Treatment
        public IHttpActionResult Post([FromBody] Treatment_DTO value)
        {
            IHttpActionResult result;

            try
            {
                if (value.Duration > 0 && value.Price >= 0)
                {
                    var treatmentToAddObj = new Treatment(value.CompanyID, value.Name, value.Description, value.Duration, value.Price, value.TreatmentCategoryID);
                    Treatment treatmentAdded = _dbTreatment.InsertTreatmentToDatabase(treatmentToAddObj);

                    if (treatmentAdded != null)
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
                    //throw new ArgumentException("The Arguments provided were invalid.");
                    result = Content(HttpStatusCode.Conflict, "The Arguments provided were invalid.");
                }
            }
            catch (SqlException)
            {
                //Company does not exist
                //  throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not insert data to database.", sqlE));
                result = InternalServerError();
            }
            catch (ArgumentException)
            {
                //Invalid arguments
                //    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ae));
                result = BadRequest();
            }
            catch (AlreadyExistsException)
            {
                //Already exists
                //    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, alreadyExistsException));
                result = Conflict();
            }

            return result;
        }
    }
}
