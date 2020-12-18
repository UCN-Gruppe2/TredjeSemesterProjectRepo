using Model;
using Newtonsoft.Json;
using RestClientManagerNamespace;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBookingInterface.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        RestClient _client;

        public EmployeeController()
        {
            _client = RestClientManager.GetInstance().RestClient;
        }

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeReservationsResult(int employeeID)
        {
            RestRequest request = new RestRequest("api/Employee/", Method.GET);
            request.AddParameter("employeeID", employeeID);
            RestResponse response = (RestResponse)_client.Execute(request);

            ActionResult viewToReturn;
            ViewBag.EmployeeID = employeeID;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.EmployeeID = employeeID;
                ViewBag.Reservations = JsonConvert.DeserializeObject<List<Reservation>>(response.Content);
                viewToReturn = View();
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