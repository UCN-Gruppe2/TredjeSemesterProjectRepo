using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using RESTfulService.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace DataTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ReservationTest
    {
        public ReservationController ReservationCtrl;
        public TreatmentController TreatmentCtrl;
        public Stopwatch Watch;
        public Customer Customer;
        public Customer Customer2;
        public Treatment Treatment;
        public Employee Employee;
        public List<TreatmentCategory> Categories;

        [TestInitialize]
        public void SetUp()
        {
            ReservationCtrl = new ReservationController();
            TreatmentCtrl = new TreatmentController();
            Watch = new Stopwatch();
            DbCleanUp.CleanDB();
            InsertTestData.InsertData();

            Categories = new List<TreatmentCategory>();
            Categories.Add(new TreatmentCategory(1, "Klip"));
 
            //Not for database
            //Just for not repeating to often
            Treatment = new Treatment(1, "Voks af ryg", "Vi benytter enten almindelig varm voks eller sugaring", 60, 699.95m);
        }

        [TestMethod]
        public void TestCreateReservation1_Valid()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment, Categories);
            Reservation newReservation = new Reservation(addedTreatment, 1, 1, DateTime.Parse("30-11-2021 13:30"));

            //Act
            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.AreEqual(newReservation.EndTime, addedReservation.EndTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateReservation2_TimeAlreadyBooked()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment, Categories);

            Treatment treatment2 = new Treatment(1, "Voks af bryst", "Vi benytter enten almindelig varm voks eller sugaring", 30, 399.95m);
            Treatment addedTreatment2 = TreatmentCtrl.Post(treatment2, Categories);

            Reservation newReservation = new Reservation(addedTreatment, 1, 1, DateTime.Parse("26-11-2021 13:30"));
            Reservation doubleReservation = new Reservation(addedTreatment2, 2, 1, DateTime.Parse("26-11-2021 13:30"));

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
            Assert.AreEqual(newReservation.EndTime, addedReservation.EndTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateReservation3_TimeOverlap()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment, Categories);

            Treatment treatment2 = new Treatment(1, "Voks af bryst", "Vi benytter enten almindelig varm voks eller sugaring", 60, 399.95m);
            Treatment addedTreatment2 = TreatmentCtrl.Post(treatment2, Categories);

            Reservation newReservation = new Reservation(addedTreatment, 1, 1, DateTime.Parse("26-11-2021 13:30"));
            Reservation doubleReservation = new Reservation(addedTreatment2, 2, 1, DateTime.Parse("26-11-2021 14:00"));

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
            Assert.AreEqual(newReservation.EndTime, addedReservation.EndTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateReservation4_NoEmployee()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment, Categories);
            Reservation newReservation = new Reservation(addedTreatment, 1, -1, DateTime.Parse("26-11-2021 13:30"));

            //Act

            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.AreEqual(newReservation.EndTime, addedReservation.EndTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void TestCreateReservation5_NoTreatment()
        {
            //Arrange
            Reservation newReservation = new Reservation(Treatment, 1, 1, DateTime.Parse("30-11-2021 13:30"));

            //Act

            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.AreEqual(newReservation.EndTime, addedReservation.EndTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateReservation6_NoCustomer()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment, Categories);
            Reservation newReservation = new Reservation(addedTreatment, -1, 1, DateTime.Parse("26-11-2021 13:30"));

            //Act

            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.AreEqual(newReservation.EndTime, addedReservation.EndTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateReservation7_DateInThePast()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(Treatment, Categories);
            Reservation newReservation = new Reservation(addedTreatment, 1, 1, DateTime.Parse("26-11-2020 13:30"));

            //Act

            Watch.Start();
            Reservation addedReservation = ReservationCtrl.Post(newReservation);
            Watch.Stop();

            //Assert
            Assert.AreEqual(newReservation.TreatmentID, addedReservation.TreatmentID);
            Assert.AreEqual(newReservation.CustomerID, addedReservation.CustomerID);
            Assert.AreEqual(newReservation.EmployeeID, addedReservation.EmployeeID);
            Assert.AreEqual(newReservation.StartTime, addedReservation.StartTime);
            Assert.AreEqual(newReservation.EndTime, addedReservation.EndTime);
            Assert.IsTrue(Watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestFindReservationByID1_Valid()
        {
            //Arrange
            int id = 1;

            //Act
            Reservation found = ReservationCtrl.GetReservationByID(id);

            //Assert
            Assert.AreEqual(id, found.ID);
        }

        [TestMethod]
        public void TestFindReservationByID2_NonExists()
        {
            //Arrange
            int id = 35;

            //Act
            Reservation found = ReservationCtrl.GetReservationByID(id);

            //Assert
            Assert.IsNull(found);
        }

        //Udarbejdet med TDD
        //Test fuldt skrevet først, dernæst controller, så DbReservation
        [TestMethod]
        public void TestFindReservationByCustomerID1_Valid()
        {
            //Arrange
            int id = 1;

            //Act
            List<Reservation> founds = ReservationCtrl.GetReservationsByCustomerID(id);

            //Assert
            foreach (Reservation element in founds)
            {
                Assert.AreEqual(id, element.CustomerID);
            }
        }

        [TestMethod]
        public void TestFindReservationByCustomerID2_NonFound()
        {
            //Arrange
            int id = 2;

            //Act
            List<Reservation> founds = ReservationCtrl.GetReservationsByCustomerID(id);

            //Assert
            Assert.IsTrue(founds.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFindReservationByCustomerID3_UnknownCustomer()
        {
            //Arrange
            int id = 35;

            //Act
            List<Reservation> founds = ReservationCtrl.GetReservationsByCustomerID(id);

            //Assert
            Assert.IsTrue(founds.Count == 0);
        }

        [TestMethod]
        public void TestFindReservationByEmployeeID1_Valid()
        {
            //Arrange
            int id = 1;

            //Act
            List<Reservation> founds = ReservationCtrl.GetReservationsByEmployeeID(id);

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
            List<Reservation> founds = ReservationCtrl.GetReservationsByEmployeeID(id);

            //Assert
            Assert.IsTrue(founds.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFindReservationByEmployeeID3_UnknownEmployee()
        {
            //Arrange
            int id = 35;

            //Act
            List<Reservation> founds = ReservationCtrl.GetReservationsByEmployeeID(id);

            //Assert
            Assert.IsTrue(founds.Count == 0);
        }
    }
}
