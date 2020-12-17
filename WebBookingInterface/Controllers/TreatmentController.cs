using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestClientManagerNamespace;
using RestSharp;

namespace WebBookingInterface.Controllers
{
    public class TreatmentController : Controller
    {
        RestClient _client;

        public TreatmentController()
        {
            _client = RestClientManager.GetInstance().RestClient;
        }

        // GET: Treatment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(int companyID, string name, string description, int duration, int price, string treatmentCategoryID)
        {
            List<int> categoriesID = new List<int>();
            foreach (string part in treatmentCategoryID.Trim().Split(','))
            {
                int parsedValue = -1;
                if (int.TryParse(part, out parsedValue))
                {
                    categoriesID.Add(parsedValue);
                }
            }

            RestRequest treatmentRequest = new RestRequest("/api/Treatment", Method.POST);

            Treatment_DTO treatmentTransferObj = new Treatment_DTO(
                companyID: companyID,
                name: name,
                description: description,
                duration: duration,
                price: price,
                treatmentCategoryID: categoriesID
            );

            treatmentRequest.AddJsonBody(treatmentTransferObj);
            var response = _client.Execute(treatmentRequest);

            ActionResult viewToReturn;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.treatment =  JsonConvert.DeserializeObject<Treatment>(response.Content);
                viewToReturn = View("SuccessView");
            }
            else
            {
                ViewBag.ExceptionAsString = response.Content;
                viewToReturn = View("FailView");
            }

            return viewToReturn;
        }
    }
}