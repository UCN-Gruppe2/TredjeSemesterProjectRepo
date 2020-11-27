using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using RESTfulService.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

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
        public Customer customer;
        public Customer customer2;
        public Treatment treatment;
        public Employee employee;

       [TestInitialize]
       public void SetUp()
        {
            ReservationCtrl = new ReservationController();
            TreatmentCtrl = new TreatmentController();
            Watch = new Stopwatch();
            DbCleanUp.CleanDB();

            customer = new Customer("Hans", "Larsen", "12345678", "Banegårdsgade 3", "9000", "Aalborg");
            customer2 = new Customer(4, "William", "Jensen", "43215678", "Banegårdsgade 12", "9000", "Aalborg");
            employee = new Employee("Sanne", "Liane", "87654321", 1, "Bygade 32", "9000", "Aalborg");

            //Not for database
            //Just for not repeating to often
            treatment = new Treatment("Voks af ryg", "Vi benytter enten almindelig varm voks eller sugaring", 60, 699.95m);
        }

        [TestMethod]
        public void TestCreateReservation1_Valid()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);
            Reservation newReservation = new Reservation(addedTreatment, customer.CustomerID, employee.EmployeeID, DateTime.Parse("26-11-2020 13:30"));

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
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);

            Treatment treatment2 = new Treatment(25, "Voks af bryst", "Vi benytter enten almindelig varm voks eller sugaring", 30, 399.95m);
            Treatment addedTreatment2 = TreatmentCtrl.Post(treatment2);

            Reservation newReservation = new Reservation(addedTreatment, customer.CustomerID, employee.EmployeeID, DateTime.Parse("26-11-2020 13:30"));
            Reservation doubleReservation = new Reservation(addedTreatment2, customer2.CustomerID, employee.EmployeeID, DateTime.Parse("26-11-2020 13:30"));

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
        public void TestCreateReservation3_NoEmployee()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);
            Reservation newReservation = new Reservation(addedTreatment, customer.CustomerID, -1, DateTime.Parse("26-11-2020 13:30"));

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
        public void TestCreateReservation4_NoTreatment()
        {
            //Arrange
            Reservation newReservation = new Reservation(treatment, customer.CustomerID, employee.EmployeeID, DateTime.Parse("26-11-2020 13:30"));

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
        public void TestCreateReservation5_NoCustomer()
        {
            //Arrange
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);
            Reservation newReservation = new Reservation(addedTreatment, -1 , employee.EmployeeID, DateTime.Parse("26-11-2020 13:30"));

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
    }
}
