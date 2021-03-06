﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using RESTfulService.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;

namespace DataTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ReservationTest
    {
        private ReservationController _reservationCtrl;
        private TreatmentController _treatmentCtrl;
        private EmployeeController _employeeCtrl;
        private Stopwatch _watch;
        private List<int> _categories;

        [TestInitialize]
        public void SetUp()
        {
            _reservationCtrl = new ReservationController();
            _treatmentCtrl = new TreatmentController();
            _employeeCtrl = new EmployeeController();
            _watch = new Stopwatch();
            DbCleanUp.CleanDB();
            InsertTestData.InsertData();

            _categories = new List<int>() { 1 };

            Treatment_DTO treatment = new Treatment_DTO(1, "Voks af ryg", "Vi benytter enten almindelig varm voks eller sugaring", 60, 699.95m, _categories);
            _treatmentCtrl.Post(treatment);
        }

        [TestMethod]
        public void TestCreateReservation1_Valid()
        {
            //Arrange
            Reservation_DTO newReservation = new Reservation_DTO(1, 1, 1, DateTime.Parse("26-11-2025 13:30"));

            //Act
            _watch.Start();
            IHttpActionResult addedReservationResult = _reservationCtrl.Post(newReservation);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedReservationResult, typeof(OkNegotiatedContentResult<Reservation>));
            Reservation addedReservationObj = ((OkNegotiatedContentResult<Reservation>)addedReservationResult).Content;

            Assert.AreEqual(newReservation.TreatmentID, addedReservationObj.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservationObj.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservationObj.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservationObj.StartTime);
            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);

        }

        [TestMethod]
        public void TestCreateReservation2_TimeAlreadyBooked()
        {
            //Arrange
            Reservation_DTO newReservation = new Reservation_DTO(1, 1, 1, DateTime.Parse("26-11-2025 13:30"));
            Reservation_DTO doubleReservation = new Reservation_DTO(1, 1, 1, DateTime.Parse("26-11-2025 13:30"));

            //Act
            _watch.Start();
            IHttpActionResult addedReservation = _reservationCtrl.Post(newReservation);
            IHttpActionResult addedReservationDouble = _reservationCtrl.Post(doubleReservation);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedReservationDouble, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedReservationDouble).StatusCode == System.Net.HttpStatusCode.Conflict);

            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateReservation3_TimeOverlap()
        {
            //Arrange
            Treatment_DTO treatment2 = new Treatment_DTO(1, "Voks af bryst", "Vi benytter enten almindelig varm voks eller sugaring", 60, 399.95m, _categories);
            _treatmentCtrl.Post(treatment2);

            Reservation_DTO newReservation = new Reservation_DTO(2, 1, 1, DateTime.Parse("26-02-2025 13:30"));
            Reservation_DTO doubleReservation = new Reservation_DTO(1, 1, 1, DateTime.Parse("26-02-2025 14:00"));

            //Act
            _watch.Start();
            IHttpActionResult addedReservationResult = _reservationCtrl.Post(newReservation);
            IHttpActionResult addedReservationDoubleResult = _reservationCtrl.Post(doubleReservation);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedReservationDoubleResult, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedReservationDoubleResult).StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateReservation4_IllegalEmployeeID()
        {
            //Arrange
            Reservation_DTO newReservation = new Reservation_DTO(1, 1, -1, DateTime.Parse("26-11-2025 13:30"));

            //Act
            _watch.Start();
            IHttpActionResult addedReservationResult = _reservationCtrl.Post(newReservation);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedReservationResult, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedReservationResult).StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [TestMethod]
        public void TestCreateReservation5_IllegalTreatmentID()
        {
            //Arrange
            Reservation_DTO newReservation = new Reservation_DTO(-1, 1, 1, DateTime.Parse("26-11-2025 13:30"));

            //Act
            _watch.Start();
            IHttpActionResult addedReservationResult = _reservationCtrl.Post(newReservation);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedReservationResult, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedReservationResult).StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestCreateReservation6_IllegalCustomerID()
        {
            //Arrange
            Reservation_DTO newReservation = new Reservation_DTO(1, -1, 1, DateTime.Parse("26-11-2025 13:30"));

            //Act
            _watch.Start();
            IHttpActionResult addedReservationResult = _reservationCtrl.Post(newReservation);
            _watch.Stop();

            //Assert
            Assert.IsInstanceOfType(addedReservationResult, typeof(NegotiatedContentResult<string>));
            Assert.IsTrue(((NegotiatedContentResult<string>)addedReservationResult).StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.IsTrue(_watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestFindReservationByEmployeeID1_Valid()
        {
            //Arrange
            int id = 1;

            //Act
            IHttpActionResult httpActionResult = _employeeCtrl.Reservations(id);

            //Assert
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<List<Reservation>>));
            List<Reservation> reservations = ((OkNegotiatedContentResult<List<Reservation>>)httpActionResult).Content;

            foreach (Reservation element in reservations)
            {
                Assert.AreEqual(id, element.EmployeeID);
            }
        }

        [TestMethod]
        public void TestFindReservationByEmployeeID2_NonFound()
        {
            //Arrange
            int id = 10;

            //Act
            IHttpActionResult httpActionResult = _employeeCtrl.Reservations(id);

            //Assert
            Assert.IsInstanceOfType(httpActionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void TestFindReservationByEmployeeID3_UnknownEmployee()
        {
            //Arrange
            int id = 35;

            //Act
            IHttpActionResult httpActionResult = _employeeCtrl.Reservations(id);

            //Assert
            Assert.IsInstanceOfType(httpActionResult, typeof(NotFoundResult));
        }
    }
}
