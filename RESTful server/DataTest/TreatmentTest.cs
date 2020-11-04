using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess.Interfaces;
using DataAccess.DatabaseAccess;
using RESTfulService;

namespace DataTest
{
    [TestClass]
    public class TreatmentTest
    {
        ITreatment _treatmentDb;


        [TestInitialize]
        public void InitializeBeforeEachMethod()
        {
            _treatmentDb = new DbTreatment();
        }

        [TestMethod]
        public void TestCreateTreatmentValid()
        {
        }

        [TestMethod]
        public void TestCreateTreatmentInvalid()
        {
        }

        [TestMethod]
        public void TestCreateTreatmentAlreadyExists()
        {
        }
    }
}
