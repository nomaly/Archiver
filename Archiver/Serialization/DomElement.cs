using System;
using System.Linq;
using System.Xml;

namespace Archiver
{
    public abstract class CDomElement
    {
        public const string ATTR_ISDOMELEMENT = "IsDomElement";
        public const string ATTR_NAME = "Name";
        public const string NODE_PROPERTY = "Property";

        protected XmlElement m_writingSection;
        
        protected CDomElement(XmlElement element)
        {
            CArguments.ThrowIfArgumentNull(element, "element");

            m_writingSection = element;
        }
        
        public Int64 GetInt64(String key)
        {
            String value = GetString(key);
            return Int64.Parse(value);
        }

        public Int32 GetInt32(String key)
        {
            String value = GetString(key);
            return Int32.Parse(value);
        }

        public String GetString(String key)
        {
            XmlElement foundElement;
            if(!TryToFindElement(key, out foundElement))
                throw new CNotFoundExceptionException($"Property with name '{key}' not found");

            return foundElement.InnerText;
        }

        public void Set(String key, Int64 value)
        {
            Set(key, value.ToString());
        }

        public void Set(String key, Int32 value)
        {
            Set(key, value.ToString());
        }
        
        public void Set(String key, String value)
        {
            XmlElement foundElement;
            if (!TryToFindElement(key, out foundElement))
            {
                foundElement = CXmlHelper.CreateElement(m_writingSection, NODE_PROPERTY, value);
                CXmlHelper.CreateAttribute(foundElement, ATTR_NAME, key);
            }

            foundElement.InnerText = value;
        }

        private bool TryToFindElement(String name, out XmlElement foundElement)
        {
            var elements = CXmlHelper.GetElementsList(m_writingSection, NODE_PROPERTY);
            foundElement = elements.FirstOrDefault(x => CXmlHelper.FindAttrAsString(x, ATTR_NAME).Equals(name, StringComparison.Ordinal));

            if (foundElement == null)
                return false;

            return true;
        }
    }
}