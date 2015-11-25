using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace Archiver
{
    public class CDomContainer : CDomElement
    {
        private const string NODE_DOMCONTAINER = "DomContainer";

        public XmlDocument OwnerDocument { get; }

        public CDomContainer() :this("NewContainer") { }

        public CDomContainer(string rootName) 
            : this(CXmlHelper.CreateDocument(NODE_DOMCONTAINER))
        {
            CXmlHelper.CreateAttribute(m_writingSection, CDomElement.ATTR_NAME, rootName);
            CXmlHelper.CreateAttribute(m_writingSection, CDomElement.ATTR_ISDOMELEMENT, true);
        }

        public CDomContainer(XmlDocument doc) : base(doc.DocumentElement)
        {
            CArguments.ThrowIfArgumentNull(doc, "doc");

            OwnerDocument = doc;
        }

        public CDomSection CreateSection(String name)
        {
            CheckElementExists(name);
            return CDomSection.CreateSection(OwnerDocument.DocumentElement, name);
        }

        public CDomSection GetSection(String name)
        {
            var sections = CXmlHelper.GetElementsList(OwnerDocument.DocumentElement, CDomSection.NODE_SECTION);
            var foundElement = sections.FirstOrDefault(x => CXmlHelper.FindAttrAsBoolean(x, ATTR_ISDOMELEMENT) == true);
            if(foundElement == null)
                throw new CNotFoundExceptionException($"Section with name '{name}' was not found");

            return CDomSection.OpenSection(foundElement);
        }

        public CDomListSection CreateListSection(String name)
        {
            CheckElementExists(name);
            return CDomListSection.CreateListSection(m_writingSection, name);
        }

        public String SerializeToString()
        {
            return OwnerDocument.InnerXml;
        }

        public static CDomContainer CreateContainer(String rootNodeName)
        {
            return new CDomContainer(rootNodeName);
        }

        private void CheckElementExists(string name)
        {
            XmlElement element = CXmlHelper.FindSingleElement(m_writingSection, name);
            if (element != null)
                throw new InvalidOperationException($"Section with name '{name}' already exists");
        }
    }
}