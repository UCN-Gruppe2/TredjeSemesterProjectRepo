using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using RESTfulService;
using RESTfulService.Controllers;
using System.Collections;
using System.Web.Http;
using System.Web.Http.Results;

namespace DataTest
{
    [TestClass]
    public class TreatmentTest
    {
        private TreatmentController treatmentCtrl;
        private Stopwatch watch;
        private List<int> categories;

        [TestInitialize]
        public void SetUp()
        {
            treatmentCtrl = new TreatmentController();
            watch = new Stopwatch();

            categories = new List<int>() { 1 };
        }

        [TestCleanup]
        public void CleanUp()
        {
            DbCleanUp.CleanDB();
            InsertTestData.InsertData();
        }

        [TestMethod]
        public void TestCreateTreatment1_Valid()
        {
            //Arrange
            Treatment_DTO treatment = new Treatment_DTO(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m, categories); //m = decimal 
            //Act
            watch.Start();
            IHttpActionResult addedTreatmentResult = treatmentCtrl.Post(treatment);
            watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedTreatmentResult, typeof(OkNegotiatedContentResult<Treatment>));
            Treatment addedTreatmentObj = ((OkNegotiatedContentResult<Treatment>)addedTreatmentResult).Content;

            Assert.AreEqual(treatment.Name, addedTreatmentObj.Name);
            Assert.AreEqual(treatment.Description, addedTreatmentObj.Description);
            Assert.AreEqual(treatment.Duration, addedTreatmentObj.Duration);
            Assert.AreEqual(treatment.Price, addedTreatmentObj.Price);
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateTreatment2_AlreadyExists()
        {
            //Arrange
            Treatment_DTO treatment = new Treatment_DTO(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m, categories);

            //Act
            watch.Start();
            IHttpActionResult addedTreatmentResult = treatmentCtrl.Post(treatment);
            IHttpActionResult addedTreatmentDoubleResult = treatmentCtrl.Post(treatment);
            watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedTreatmentResult, typeof(OkNegotiatedContentResult<Treatment>));
            Assert.IsInstanceOfType(addedTreatmentDoubleResult, typeof(ConflictResult));
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateTreatment3_IllegalDuration()
        {
            //Arrange
            Treatment_DTO treatment = new Treatment_DTO(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", -30, 499.95m);

            //Act
            watch.Start();
            IHttpActionResult addedTreatment = treatmentCtrl.Post(treatment);

            watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedTreatment, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedTreatment).StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateTreatment4_IllegalPrice()
        {
            //Arrange
            Treatment_DTO treatment = new Treatment_DTO(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, -499.95m);

            //Act
            watch.Start();
            IHttpActionResult addedTreatment = treatmentCtrl.Post(treatment);
            watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedTreatment, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedTreatment).StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestFindTreatmentByID1_Valid()
        {
            //Arrange
            int id = 1;

            //Act
            IHttpActionResult treatmentFound = treatmentCtrl.Get(id);

            //Assert
            Assert.IsInstanceOfType(treatmentFound, typeof(OkNegotiatedContentResult<Treatment>));
            Assert.AreEqual(id, ((OkNegotiatedContentResult<Treatment>)treatmentFound).Content.ID);
        }

        [TestMethod]
        public void TestFindTreatmentByID2_UnknownID()
        {
            //Arrange
            int id = 35;

            //Act
            IHttpActionResult found = treatmentCtrl.Get(id);

            //Assert
            Assert.IsInstanceOfType(found, typeof(NotFoundResult));
        }
    }
}
