using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace DataTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SortedLinkListTester
    {
        private SortedLinkedList<int> _instance;

        [TestInitialize]
        public void Setup()
        {
            _instance = new SortedLinkedList<int>();
        }

        [TestMethod]
        public void AddTest()
        {
            _instance.Clear();
            int prevCount = _instance.Count;
            Assert.AreEqual(prevCount, 0);

            _instance.Add(1);
            int valueAfterValueOneAdded = _instance.Count;
            Assert.AreEqual(1, valueAfterValueOneAdded);

            _instance.Add(3);
            int valueAfterValueThreeAdded = _instance.Count;
            Assert.AreEqual(2, valueAfterValueThreeAdded);

            _instance.Add(2);
            int valueAfterValueTwoAdded = _instance.Count;
            Assert.AreEqual(3, valueAfterValueTwoAdded);
        }

        [TestMethod]
        public void ClearTest()
        {
            _instance.Clear();
            //int previousCount = _instance.Count;
            _instance.Add(1);
            _instance.Clear();
            int currentCount = _instance.Count;

            Assert.AreEqual(0, currentCount);
        }

        [TestMethod]
        public void CopyToTest()
        {
            var localInstance = new SortedLinkedList<string>(); //Skift til global instans
            int numberOfValuesToAdd = 50;

            localInstance.Clear();
            for (int i = numberOfValuesToAdd; i > 0; i--)
            {
                localInstance.Add(i.ToString());
            }

            string[] endArray = new string[numberOfValuesToAdd];
            localInstance.CopyTo(endArray, 0);

            bool foundNull = false;
            for (int i = 0; i < numberOfValuesToAdd; i++)
            {
                System.Diagnostics.Debug.WriteLine($"{i}: {endArray[i]}");
                if (endArray[i] == null)
                {
                    foundNull = true;
                }
            }

            Assert.IsFalse(foundNull);
        }
    }
}
