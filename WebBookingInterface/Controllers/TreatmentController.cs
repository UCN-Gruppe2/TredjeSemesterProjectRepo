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
    [Authorize]
    public class TreatmentController : Controller
    {
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
        public ActionResult Create(int companyID, string name, string description, string duration, int price, string treatmentCategoryID)
        {
            int durationAsInt = int.Parse(duration);
          

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
            treatmentRequest.AddHeader("Authorization", $"Bearer {Request.Cookies["token"].Value}");

            Treatment_DTO treatmentTransferObj = new Treatment_DTO(
                companyID: companyID,
                name: name,
                description: description,
                duration: durationAsInt,
                price: price,
                treatmentCategoryID: categoriesID
            );

            treatmentRequest.AddJsonBody(treatmentTransferObj);
            var response = RestClientManager.Client.Execute(treatmentRequest);

            ActionResult viewToReturn;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.treatment = JsonConvert.DeserializeObject<Treatment>(response.Content);
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