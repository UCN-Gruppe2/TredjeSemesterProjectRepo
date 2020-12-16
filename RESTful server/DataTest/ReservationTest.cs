﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Newtonsoft.Json.Linq;
using RESTfulService.Controllers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace DataTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ReservationTest
    {
        private RestClient _client;
        public ReservationController ReservationCtrl;
        public TreatmentController TreatmentCtrl;
        public EmployeeController EmployeeCtrl;
        public Stopwatch Watch;
        public Treatment_DTO Treatment;
        public List<int> Categories;

        [TestInitialize]
        public void SetUp()
        {
            //_client = new RestClient("https://localhost:44388");
            //RestRequest request = new RestRequest("/Token", Method.POST);
            //request.AddParameter("grant_type", "password");
            //request.AddParameter("userName", "mail@marcuslc.com");
            //request.AddParameter("password", "Password1!");
            //var response = _client.Execute(request);

            //string accessToken = JObject.Parse(response.Content)["access_token"].ToString();
            //_client.AddDefaultHeader("Authorization", $"Bearer { accessToken }");

            ReservationCtrl = new ReservationController();
            TreatmentCtrl = new TreatmentController();
            EmployeeCtrl = new EmployeeController();
            Watch = new Stopwatch();
            DbCleanUp.CleanDB();
            InsertTestData.InsertData();

            Categories = new List<int>() { 1 };
            //Categories.Add(new TreatmentCategory(1, "Klip"));

            //Not for database
            //Just for not repeating to often
            Treatment = new Treatment_DTO(1, "Voks af ryg", "Vi benytter enten almindelig varm voks eller sugaring", 60, 699.95m, Categories);
        }

        [TestMethod]
        public void TestCreateReservation1_Valid()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment);
            Reservation_DTO newReservation = new Reservation_DTO(1, 1, 1, DateTime.Parse("26-11-2435 13:30"));

            //Act
            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void TestCreateReservation2_TimeAlreadyBooked()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment);

            Treatment_DTO treatment2 = new Treatment_DTO(1, "Voks af bryst", "Vi benytter enten almindelig varm voks eller sugaring", 30, 399.95m, Categories);
            RestRequest requestTreatment = new RestRequest("api/Treatment", Method.POST);
            requestTreatment.AddJsonBody(requestTreatment);
            _client.Execute(requestTreatment);
            //Treatment addedTreatment2 = TreatmentCtrl.Post(treatment2);

            Reservation_DTO newReservation = new Reservation_DTO(1, 1, 1, DateTime.Parse("26-11-2435 13:30"));
            Reservation_DTO doubleReservation = new Reservation_DTO(2, 1, 1, DateTime.Parse("26-11-2435 13:30"));

            //Act
            Watch.Start();
            //Reservation addedReservation = ReservationCtrl.Post(newReservation);
            //Reservation addedReservationDouble = ReservationCtrl.Post(doubleReservation);
            RestRequest addReservationRequest = new RestRequest("api/Reservation", Method.POST);
            addReservationRequest.AddJsonBody(newReservation);
            _client.Execute(addReservationRequest);
            RestRequest addReservationDoubleRequest = new RestRequest("api/Reservation", Method.POST);
            addReservationDoubleRequest.AddJsonBody(doubleReservation);
            _client.Execute(addReservationDoubleRequest);
            Watch.Stop();

            //Assert
            //Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            //Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            //Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            //Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void TestCreateReservation3_TimeOverlap()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment);

            Treatment_DTO treatment2 = new Treatment_DTO(1, "Voks af bryst", "Vi benytter enten almindelig varm voks eller sugaring", 60, 399.95m);
            Treatment addedTreatment2 = TreatmentCtrl.Post(treatment2);

            Reservation_DTO newReservation = new Reservation_DTO(2, 1, 1, DateTime.Parse("26-02-2435 13:30"));
            Reservation_DTO doubleReservation = new Reservation_DTO(2, 1, 1, DateTime.Parse("26-02-2435 14:00"));

            //Act
            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Reservation addedReservationDouble = ReservationCtrl.Post(doubleReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void TestCreateReservation4_IllegalEmployeeID()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment);
            Reservation_DTO newReservation = new Reservation_DTO(1, 1, -1, DateTime.Parse("26-11-2435 13:30"));

            //Act

            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void TestCreateReservation5_IllegalTreatmentID()
        {
            //Arrange
            Reservation_DTO newReservation = new Reservation_DTO(-1, 1, 1, DateTime.Parse("26-11-2435 13:30"));

            //Act

            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void TestCreateReservation6_IllegalCustomerID()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment);
            Reservation_DTO newReservation = new Reservation_DTO(1, -1, 1, DateTime.Parse("26-11-2435 13:30"));

            //Act

            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        //[TestMethod]
        //public void TestFindReservationByID1_Valid()
        //{
        //    //Arrange
        //    int id = 1;

        //    //Act
        //    List<Reservation> found = EmployeeCtrl.Reservations(id);

        //    //Assert
        //    Assert.IsTrue(found.Count > 0);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void TestFindReservationByID2_NonExists()
        //{
        //    //Arrange
        //    int id = 35;

        //    //Act
        //    List<Reservation> found = EmployeeCtrl.Reservations(id);

        //    //Assert
        //    Assert.IsTrue(found.Count == 0);
        //}

        //Udarbejdet med TDD
        //Test fuldt skrevet først, dernæst controller, så DbReservation
        //[TestMethod]
        //public void TestFindReservationByCustomerID1_Valid()
        //{
        //    //Arrange
        //    int id = 1;

        //    //Act
        //    List<Reservation> founds = EmployeeCtrl.Reservations(id);

        //    //Assert
        //    foreach (Reservation element in founds)
        //    {
        //        Assert.AreEqual(id, element.CustomerID);
        //    }
        //}

        //[TestMethod]
        //public void TestFindReservationByCustomerID2_NonFound()
        //{
        //    //Arrange
        //    int id = 2;

        //    //Act
        //    List<Reservation> founds = ReservationCtrl.GetReservationsByCustomerID(id);

        //    //Assert
        //    Assert.IsTrue(founds.Count == 0);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void TestFindReservationByCustomerID3_UnknownCustomer()
        //{
        //    //Arrange
        //    int id = 35;

        //    //Act
        //    List<Reservation> founds = ReservationCtrl.GetReservationsByCustomerID(id);

        //    //Assert
        //    Assert.IsTrue(founds.Count == 0);
        //}

        [TestMethod]
        public void TestFindReservationByEmployeeID1_Valid()
        {
            //Arrange
            int id = 1;

            //Act
            List<Reservation> founds = EmployeeCtrl.Reservations(id);

            //Assert
            foreach (Reservation element in founds)
            {
                Assert.AreEqual(id, element.EmployeeID);
            }
        }

        [TestMethod]
        public void TestFindReservationByEmployeeID2_NonFound()
        {
            //Arrange
            int id = 2;

            //Act
            List<Reservation> founds = EmployeeCtrl.Reservations(id);

            //Assert
            Assert.IsTrue(founds.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void TestFindReservationByEmployeeID3_UnknownEmployee()
        {
            //Arrange
            int id = 35;

            //Act
            List<Reservation> founds = EmployeeCtrl.Reservations(id);

            //Assert
            Assert.IsTrue(founds.Count == 0);
        }
    }
}
