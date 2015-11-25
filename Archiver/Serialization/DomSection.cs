using System;
using System.Xml;

namespace Archiver
{
    public class CDomSection : CDomElement
    {
        public const string NODE_SECTION = "Section";
        private XmlDocument m_doc;

        public String Name { get; }

        private CDomSection(XmlElement element) : base(element)
        {
            Name = CXmlHelper.GetAttributeAsString(m_writingSection, CDomElement.ATTR_NAME);
        }

        public static CDomSection CreateSection(XmlElement parentElement, String sectionName)
        {
            XmlElement sectionElement = CXmlHelper.CreateElement(parentElement, NODE_SECTION);
            CXmlHelper.CreateAttribute(sectionElement, CDomElement.ATTR_NAME, sectionName);
            CXmlHelper.CreateAttribute(sectionElement, ATTR_ISDOMELEMENT, true);

            return new CDomSection(sectionElement);
        }

        public static CDomSection OpenSection(XmlElement element)
        {
            return new CDomSection(element);
        }
    }
}