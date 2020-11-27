using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using RESTfulService;
using RESTfulService.Controllers;

namespace DataTest
{
    [TestClass]
    public class TreatmentTest
    {
        public TreatmentController TreatmentCtrl;
        public Stopwatch watch;

        [TestInitialize]
        public void SetUp()
        {
            TreatmentCtrl = new TreatmentController();
            watch = new Stopwatch();
        }

        [TestCleanup]
        public void CleanUp()
        {
            DbCleanUp.CleanDB();
        }

        [TestMethod]
        public void TestCreateTreatment1_Valid()
        {
            //Arrange
            Treatment treatment = new Treatment(3, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m); //m = decimal 

            //Act
            watch.Start();
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);
            watch.Stop();

            //Assert
            Assert.AreEqual(treatment.Name, addedTreatment.Name);
            Assert.AreEqual(treatment.Description, addedTreatment.Description);
            Assert.AreEqual(treatment.Duration, addedTreatment.Duration);
            Assert.AreEqual(treatment.Price, addedTreatment.Price);
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateTreatment2_AlreadyExists()
        {
            //Arrange
            Treatment treatment = new Treatment(3, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m);
            Treatment treatmentDouble = new Treatment(3, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m);

            //Act
            watch.Start();
            Treatment addedTreatment = TreatmentCtrl.Post(treatment); //Hvad returnerer den??
            Treatment addedTreatmentDouble = TreatmentCtrl.Post(treatmentDouble);
            watch.Stop();

            //Assert
            Assert.AreEqual(treatment.Name, addedTreatment.Name);
            Assert.AreEqual(treatment.Description, addedTreatment.Description);
            Assert.AreEqual(treatment.Duration, addedTreatment.Duration);
            Assert.AreEqual(treatment.Price, addedTreatment.Price);
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateTreatment3_IllegalDuration()
        {
            //Arrange
            Treatment treatment = new Treatment(3, "Dameklip, lang hår", "Vi klipper langt hår på damer", -30, 499.95m);

            //Act
            watch.Start();
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);
            watch.Stop();

            //Assert
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateTreatment4_IllegalPrice()
        {
            //Arrange
            Treatment treatment = new Treatment(3, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, -499.95m);

            //Act
            watch.Start();
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);
            watch.Stop();

            //Assert
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestFindTreatmentByID_Valid()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void TestFindTreatmentByID_UnknownID()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void TestFindEmployeesOfTreatment_Valid()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void TestFindEmployeesOfTreatment_NoEmployees()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void TestFindEmployeesOfTreatment_UnknownID()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void TestFindAvailableTreatments_Valid()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void TestFindAvailableTreatments_NonIsValid()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
