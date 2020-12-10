using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
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

        public ActionResult Create(Treatment_DTO treatment_DTO)
        {
            return Json(_client);
        }
    }
}