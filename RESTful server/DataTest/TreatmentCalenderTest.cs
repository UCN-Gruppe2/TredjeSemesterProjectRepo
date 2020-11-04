using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace DataTest
{
    /// <summary>
    /// Summary description for TreatmentCalenderTest
    /// </summary>
    [TestClass]
    public class TreatmentCalenderTest
    {
        private HourSpan _testHours;

        public TreatmentCalenderTest()
        {
            _testHours = new HourSpan(00, 00);

            //TestContext = new TreatmentCalender();
        }

        [TestMethod]
        public void TestHourSpan()
        {
            int expectedDif = 60;
            int actualDif = _testHours.CompareTo(new HourSpan(01, 00));

            Assert.AreEqual(expectedDif, actualDif);
        }
    }
}
