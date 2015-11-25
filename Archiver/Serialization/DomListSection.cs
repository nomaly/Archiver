using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Archiver
{
    public class CDomListSection : IEnumerable<CDomElement>
    {
        public const string NODE_ITEM = "Item";
        public const string NODE_LISTSECTION = "ListSection";

        private XmlElement m_listSectionElement;

        private CDomListSection(XmlElement element)
        {
            CArguments.ThrowIfArgumentNull(element, "element");
            m_listSectionElement = element;
        }

        public CDomSection CreateSection()
        {
            return CDomSection.CreateSection(m_listSectionElement, NODE_ITEM);
        }

        public IEnumerator<CDomElement> GetEnumerator()
        {
            return GetSectionList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<CDomSection> GetSectionList()
        {
            return CXmlHelper.GetElementsList(m_listSectionElement, NODE_ITEM).Select(CDomSection.OpenSection);
        }

        public static CDomListSection CreateListSection(XmlElement parentElement, String sectionName)
        {
            XmlElement listSectionElement = CXmlHelper.CreateElement(parentElement, NODE_LISTSECTION);
            CXmlHelper.CreateAttribute(listSectionElement, CDomElement.ATTR_NAME, sectionName);
            CXmlHelper.CreateAttribute(listSectionElement, CDomElement.ATTR_ISDOMELEMENT, true);

            return new CDomListSection(listSectionElement);
        }

        public static CDomListSection OpenListSection(XmlElement element)
        {
            return new CDomListSection(element);
        }
    }
}