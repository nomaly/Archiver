using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Archiver
{
    [Serializable]
    public class CNotFoundExceptionException : Exception
    {
        public CNotFoundExceptionException()
        {
        }

        public CNotFoundExceptionException(string message) : base(message)
        {
        }

        public CNotFoundExceptionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CNotFoundExceptionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    public static class CXmlHelper
    {
        public static XmlDocument CreateDocument(String rootElementName)
        {
            CArguments.ThrowIfNot(!string.IsNullOrEmpty(rootElementName), "rootElementName");

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement(rootElementName);
            doc.AppendChild(root);

            return doc;
        }

        public static XmlDocument LoadDocument(String xml)
        {
            CArguments.ThrowIfNot(!String.IsNullOrEmpty(xml), "xml");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        public static void SetElement(XmlElement parentElement, String attrName, String value)
        {
            XmlElement childElement = GetSingleElement(parentElement, attrName);
            childElement.InnerText = value;
        }

        public static void SetAttribute(XmlElement element, String attrName, String value)
        {
            CArguments.ThrowIfArgumentNull(element, "element");
            CArguments.ThrowIfNot(!String.IsNullOrEmpty(attrName), "attrName");

            XmlAttribute attr = element.GetAttributeNode(attrName);
            if(attr == null)
                throw new CNotFoundExceptionException($"XML attribute '{attrName}' was not found");

            attr.InnerText = value;
        }

        public static String FindAttrAsString(XmlElement element, String attrName)
        {
            String value = null;
            TryToGetAttrAsStringInternal(element, attrName, out value);
            return value;
        }

        public static String FindAttrAsString(XmlElement element, String attrName, out bool attrExists)
        {
            String value = null;
            attrExists = TryToGetAttrAsStringInternal(element, attrName, out value);
            return value;
        }

        public static Boolean? FindAttrAsBoolean(XmlElement element, String attrName, Boolean? defaultValue)
        {
            String value = FindAttrAsString(element, attrName);
            if (value == null)
                return defaultValue;
            return (Boolean?) Boolean.Parse(value);
        }

        public static Boolean FindAttrAsBoolean(XmlElement element, String attrName, Boolean defaultValue = false)
        {
            String value = FindAttrAsString(element, attrName);
            if (value == null)
                return defaultValue;

            return Boolean.Parse(value);
        }

        public static Int64 GetAttributeAsInt64(XmlElement element, String attrName)
        {
            return Int64.Parse(GetAttributeAsString(element, attrName));
        }

        public static Int32 GetAttributeAsInt32(XmlElement element, String attrName)
        {
            return Int32.Parse(GetAttributeAsString(element, attrName));
        }
        
        public static String GetAttributeAsString(XmlElement element, String attrName)
        {
            CArguments.ThrowIfNot(!String.IsNullOrEmpty(attrName), "attrName");

            String attrValue = FindAttrAsString(element, attrName);
            if(attrValue == null)
                throw new ArgumentNullException($"Attribute '{attrName}' not found");

            return element.GetAttribute(attrName);
        }

        public static XmlAttribute CreateAttribute(XmlElement parentElement, String attrName, bool attrValue)
        {
            return CreateAttribute(parentElement, attrName, Convert.ToString(attrValue, CultureInfo.InvariantCulture));
        }

        public static XmlAttribute CreateAttribute(XmlElement parentElement, String attrName, Int32 attrValue)
        {
            return CreateAttribute(parentElement, attrName, Convert.ToString(attrValue, CultureInfo.InvariantCulture));
        }

        public static XmlAttribute CreateAttribute(XmlElement parentElement, String attrName, Int64 attrValue)
        {
            return CreateAttribute(parentElement, attrName, Convert.ToString(attrValue, CultureInfo.InvariantCulture));
        }

        public static XmlAttribute CreateAttribute(XmlElement parentElement, String attrName, String attrValue)
        {
            XmlAttribute newAttr = CreateAttribute(parentElement, attrName);
            newAttr.InnerText = attrValue;
            return newAttr;
        }

        public static XmlAttribute CreateAttribute(XmlElement parentElement, String attrName)
        {
            CArguments.ThrowIfArgumentNull(parentElement, "parentElement");
            CArguments.ThrowIfNot(!String.IsNullOrEmpty(attrName), "attrName");

            XmlDocument doc = parentElement.OwnerDocument;
            XmlAttribute newAttr = doc.CreateAttribute(attrName);

            parentElement.Attributes.Append(newAttr);

            return newAttr;
        }



        public static Int64 GetElementAsInt64(XmlElement parentElement, String elementName)
        {
            return Int64.Parse(GetAttributeAsString(parentElement, elementName));
        }

        public static Int32 GetElementAsInt32(XmlElement parentElement, String elementName)
        {
            return Int32.Parse(GetAttributeAsString(parentElement, elementName));
        }

        public static String GetElementAsString(XmlElement parentElement, String elementName)
        {
            String value;
            if(!TryToGetSingleElementAsStringInternal(parentElement, elementName, out value))
                throw new CNotFoundExceptionException($"Xml element '{elementName}' not found");

            return value;
        }

        public static String FindElementAsString(XmlElement parentElement, String elementName)
        {
            CArguments.ThrowIfArgumentNull(parentElement, "parentElement");
            CArguments.ThrowIfNot(String.IsNullOrEmpty(elementName), "elementName");

            XmlElement element = FindSingleElement(parentElement, elementName);
            return element.InnerText;
        }

        public static IEnumerable<XmlElement> GetElementsList(XmlElement parentElement, string elementName)
        {
            return parentElement.GetElementsByTagName(elementName).Cast<XmlElement>();
        }

        public static XmlElement GetSingleElement(XmlElement parentElement, string elementName)
        {
            XmlElement childElement = FindSingleElement(parentElement, elementName);
            if(childElement == null)
                throw new CNotFoundExceptionException($"Xml element '{elementName}' was not found");

            return childElement;
        }
        
        public static XmlElement CreateElement(XmlElement parentElement, string elementName)
        {
            return CreateElementInternal(parentElement, elementName, null);
        }

        public static XmlElement CreateElement(XmlElement parentElement, string elementName, string value)
        {
            return CreateElementInternal(parentElement, elementName, value);
        }

        private static XmlElement CreateElementInternal(XmlElement parentElement, string elementName, string value)
        {
            CArguments.ThrowIfArgumentNull(parentElement, "parentElement");
            CArguments.ThrowIfNot(!String.IsNullOrEmpty(elementName), "elementName");

            XmlDocument doc = parentElement.OwnerDocument;
            XmlElement newElement = doc.CreateElement(elementName);

            if (!String.IsNullOrEmpty(value))
                newElement.InnerText = value;

            parentElement.AppendChild(newElement);

            return newElement;
        }

        public static XmlElement FindSingleElement(XmlElement parentElement, string elementName)
        {
            CArguments.ThrowIfArgumentNull(elementName, "elementName");
            CArguments.ThrowIfNot(!String.IsNullOrEmpty(elementName), "elementName");

            return parentElement.SelectSingleNode(elementName) as XmlElement;
        }

        private static bool TryToGetSingleElementAsStringInternal(XmlElement parentElement, string elementName, out String value)
        {
            XmlElement element = FindSingleElement(parentElement, elementName);

            value = null;
            if (element == null)
                return false;

            value = element.InnerText;

            return true;
        }

        public static XmlAttribute FindAttribute(XmlElement parentElement, String attrName)
        {
            CArguments.ThrowIfArgumentNull(parentElement, "element");
            CArguments.ThrowIfNot(!String.IsNullOrEmpty(attrName), "attrName");

            return parentElement.GetAttributeNode(attrName);
        }

        private static bool TryToGetAttrAsStringInternal(XmlElement element, String attrName, out String value)
        {
            XmlAttribute attr = FindAttribute(element, attrName);

            value = attr?.Value;

            return value != null;
        }
    }
}