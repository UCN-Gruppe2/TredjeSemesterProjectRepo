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
        private TreatmentController _treatmentCtrl;
        private Stopwatch _watch;
        private Treatment_DTO _treatment;
        private List<int> _categories;

        [TestInitialize]
        public void SetUp()
        {
            _treatmentCtrl = new TreatmentController();
            _watch = new Stopwatch();
            _treatment = new Treatment_DTO(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m, _categories);
            _categories = new List<int>() { 1 };
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
            
            //Act
            _watch.Start();
            IHttpActionResult addedTreatmentResult = _treatmentCtrl.Post(_treatment);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedTreatmentResult, typeof(OkNegotiatedContentResult<Treatment>));
            Treatment addedTreatmentObj = ((OkNegotiatedContentResult<Treatment>)addedTreatmentResult).Content;

            Assert.AreEqual(_treatment.Name, addedTreatmentObj.Name);
            Assert.AreEqual(_treatment.Description, addedTreatmentObj.Description);
            Assert.AreEqual(_treatment.Duration, addedTreatmentObj.Duration);
            Assert.AreEqual(_treatment.Price, addedTreatmentObj.Price);
            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateTreatment2_AlreadyExists()
        {
            //Arrange

            //Act
            _watch.Start();
            IHttpActionResult addedTreatmentResult = _treatmentCtrl.Post(_treatment);
            IHttpActionResult addedTreatmentDoubleResult = _treatmentCtrl.Post(_treatment);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedTreatmentResult, typeof(OkNegotiatedContentResult<Treatment>));
            Assert.IsInstanceOfType(addedTreatmentDoubleResult, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateTreatment3_IllegalDuration()
        {
            //Arrange
            Treatment_DTO treatment = _treatment;
            treatment.Duration = -30;

            //Act
            _watch.Start();
            IHttpActionResult addedTreatment = _treatmentCtrl.Post(treatment);

            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedTreatment, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedTreatment).StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateTreatment4_IllegalPrice()
        {
            //Arrange
            Treatment_DTO treatment = _treatment;
            treatment.Price = -499.95m;

            //Act
            _watch.Start();
            IHttpActionResult addedTreatment = _treatmentCtrl.Post(treatment);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedTreatment, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedTreatment).StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestFindTreatmentByID1_Valid()
        {
            //Arrange
            int id = 1;

            //Act
            IHttpActionResult treatmentFound = _treatmentCtrl.Get(id);

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
            IHttpActionResult found = _treatmentCtrl.Get(id);

            //Assert
            Assert.IsInstanceOfType(found, typeof(NegotiatedContentResult<string>));
        }
    }
}
