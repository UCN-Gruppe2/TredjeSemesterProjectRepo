using System;
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

        [TestInitialize]
        public void SetUp()
        {
            TreatmentCtrl = new TreatmentController();
        }

        [TestMethod]
        public void TestCreateTreatment_Valid()
        {
            //Arrange
            Treatment treatment = new Treatment("Dameklip, lang hår", "Vi klipper langt hår på damer", 30, 599.99);

            //Act
            Treatment addedTreatment = TreatmentCtrl.Post(treatment);

            //Assert
            Assert.AreEqual(treatment.Name, addedTreatment.Name);
            Assert.AreEqual(treatment.Description, addedTreatment.Description);
            Assert.AreEqual(treatment.Duration, addedTreatment.Duration);
            Assert.AreEqual(treatment.Price, addedTreatment.Price);
        }

        [TestMethod]
        public void TestCreateTreatment_IDAlreadyExists()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void TestCreateTreatment_AlreadyExists()
        {
            //Arrange

            //Act

            //Assert
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
