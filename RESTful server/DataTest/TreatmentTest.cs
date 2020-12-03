using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using RESTfulService;
using RESTfulService.Controllers;
using System.Collections;

namespace DataTest
{
    [TestClass]
    public class TreatmentTest
    {
        public TreatmentController TreatmentCtrl;
        public Stopwatch watch;
        public List<TreatmentCategory> Categories;

        [TestInitialize]
        public void SetUp()
        {
            TreatmentCtrl = new TreatmentController();
            watch = new Stopwatch();

            Categories = new List<TreatmentCategory>();
            Categories.Add(new TreatmentCategory(1, "Klip"));
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
            Treatment treatment = new Treatment(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m); //m = decimal 

            //Act
            watch.Start();
            Treatment addedTreatment = TreatmentCtrl.Post(treatment, Categories);
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
            Treatment treatment = new Treatment(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m);
            Treatment treatmentDouble = new Treatment(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 499.95m);

            //Act
            watch.Start();
            Treatment addedTreatment = TreatmentCtrl.Post(treatment, Categories); //Hvad returnerer den??
            Treatment addedTreatmentDouble = TreatmentCtrl.Post(treatmentDouble, Categories);
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
            Treatment treatment = new Treatment(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", -30, 499.95m);

            //Act
            watch.Start();
            Treatment addedTreatment = TreatmentCtrl.Post(treatment, Categories);
            watch.Stop();

            //Assert
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateTreatment4_IllegalPrice()
        {
            //Arrange
            Treatment treatment = new Treatment(1, "Dameklip, lang hår", "Vi klipper langt hår på damer", 30, -499.95m);

            //Act
            watch.Start();
            Treatment addedTreatment = TreatmentCtrl.Post(treatment, Categories);
            watch.Stop();

            //Assert
            Assert.IsTrue(watch.ElapsedMilliseconds < 2500);
        }

        [TestMethod]
        public void TestFindTreatmentByID1_Valid()
        {
            //Arrange
            int id = 1;

            //Act
            Treatment found = TreatmentCtrl.Get(id);

            //Assert
            Assert.AreEqual(id, found.ID);
        }

        [TestMethod]
        public void TestFindTreatmentByID2_UnknownID()
        {
            //Arrange
            int id = 35;

            //Act
            Treatment found = TreatmentCtrl.Get(id);

            //Assert
            Assert.IsNull(found);
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
