using System;
using System.Collections.Generic;
using System.Xml;
using Archiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class DomContainerTest
    {
        [TestMethod]
        public void TestContainer()
        {
            TPersonForSerialization person = CreatePerson();

            CDomContainer container = CDomContainer.CreateContainer("Person");
            TPersonForSerialization.Serialize(person, container);
            Console.WriteLine(container.SerializeToString());

            XmlDocument doc = CXmlHelper.LoadDocument(container.SerializeToString());
            CDomContainer deserializedContainer = new CDomContainer(doc);
            TPersonForSerialization deserializedEmployee = TPersonForSerialization.Deserialize(deserializedContainer);

            Console.WriteLine(deserializedContainer.SerializeToString());
        }

        public static TPersonForSerialization CreatePerson()
        {
            return new TPersonForSerialization()
            {
                FirstName = "Petr",
                LastName = "Petrovich",
                Age = 43,
                Height = 183,
                Weight = 101,

                EmployeeInfo = new TEmployeeInfo()
                {
                    CompanyName = "ACME ltd",
                    EmployeeId = 234345,
                    Position = "Engeneer"
                },

                PersonChildrens = new List<TPersonChildren>()
                {
                    new TPersonChildren()
                    {
                        Age = 1,
                        Gender = TGender.Female,
                        Name = "Alice"
                    },
                    new TPersonChildren()
                    {
                        Age = 5,
                        Gender = TGender.Male,
                        Name = "Bob"
                    }
                }
            };
        }
    }
}
