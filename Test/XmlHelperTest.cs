using System;
using Archiver;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class XmlHelperTest
    {
        [TestMethod]
        public void Test()
        {
            String sampleValue = "Inner sample value";
            String sampleNode = "FirstNode";

            XmlDocument doc = CXmlHelper.CreateDocument("TestDocument");

            CXmlHelper.CreateElement(doc.DocumentElement, sampleNode, sampleValue);
            CXmlHelper.GetSingleElement(doc.DocumentElement, sampleNode);

            String innerTest = CXmlHelper.GetElementAsString(doc.DocumentElement, sampleNode);

            Assert.IsTrue(String.Equals(innerTest, sampleValue, StringComparison.Ordinal));
        }
    }
}
