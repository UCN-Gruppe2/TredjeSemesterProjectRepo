using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using RESTfulService.Controllers;
using System;
using System.Collections.Generic;
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

       [TestInitialize]
       public void SetUp()
        {
            ReservationCtrl = new ReservationController();
            TreatmentCtrl = new TreatmentController();
            Watch = new Stopwatch();
            DbCleanUp.CleanDB();
        }

        [TestMethod]
        public void TestCreateReservation1_Valid()
        {
            //Arrange
            Customer customer = new Customer(1, "Hans", "Larsen", "12345678", "Banegårdsgade 3", "9000", "Aalborg");
            Treatment treatment = new Treatment("Voks af ryg", "Vi benytter enten almindelig varm voks eller sugaring", 60, 699.95m);
            Employee employee = new Employee(12, "Sanne", "Liane", "87654321", "Bygade 32", "9000", "Aalborg");
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
            Customer customer = new Customer(1, "Hans", "Larsen", "12345678", "Banegårdsgade 3", "9000", "Aalborg");
            Treatment treatment = new Treatment(23, "Voks af ryg", "Vi benytter enten almindelig varm voks eller sugaring", 60, 699.95m);
            Employee employee = new Employee(12, "Sanne", "Liane", "87654321", "Bygade 32", "9000", "Aalborg");
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);

            Customer customer2 = new Customer(4, "William", "Jensen", "43215678", "Banegårdsgade 12", "9000", "Aalborg");
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
        public void TestCreateReservation3_ObjectAlreadyExists()
        {
            //Arrange
            Customer customer = new Customer(1, "Hans", "Larsen", "12345678", "Banegårdsgade 3", "9000", "Aalborg");
            Treatment treatment = new Treatment(23, "Voks af ryg", "Vi benytter enten almindelig varm voks eller sugaring", 60, 699.95m);
            Employee employee = new Employee(12, "Sanne", "Liane", "87654321", "Bygade 32", "9000", "Aalborg");
            Reservation newReservation = new Reservation(treatment, customer.CustomerID, employee.EmployeeID, DateTime.Parse("26-11-2020 13:30"));
            Reservation doubleReservation = new Reservation(treatment, customer.CustomerID, employee.EmployeeID, DateTime.Parse("26-11-2020 13:30"));

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

            //Act

            //Assert

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateReservation5_NoTreatment()
        {
            //Arrange

            //Act

            //Assert

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateReservation6_NoCustomer()
        {
            //Arrange

            //Act

            //Assert

        }
    }
}
