using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTfulService;
using RESTfulService.Controllers;

namespace DataTest
{
    [TestClass]
    public class TreatmentTest
    {
        TreatmentController TreatmentCtrl;

        [TestInitialize]
        public void SetUp()
        {
            TreatmentCtrl = new TreatmentController();
        }

        [TestMethod]
        public void TestCreateTreatment_Valid()
        {
            //Arrange

            //Act
            TreatmentCtrl.Post("hejsa"); //Parametre?

            //Assert
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
