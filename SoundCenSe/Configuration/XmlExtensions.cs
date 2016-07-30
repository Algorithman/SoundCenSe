// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: XmlExtensions.cs
// 
// Last modified: 2016-07-30 19:37

#region Usings

using System.Collections.Generic;
using System.Xml;
using NLog;

#endregion

namespace SoundCenSe.Configuration
{
    public static class XmlExtensions
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        public static bool ParseBoolAttribute(this XmlNode node, string attributeName, bool defaultValue)
        {
            bool result = defaultValue;
            XmlNode boolNode = node.Attributes.GetNamedItem(attributeName);
            if (boolNode != null)
            {
                if (boolNode.Value.Equals("true"))
                {
                    result = true;
                }
                else if (boolNode.Value.Equals("false"))
                {
                    result = false;
                }
                else
                {
                    logger.Warn("XML Warning: " + attributeName + "'s value is not recognized, using default value " +
                                defaultValue);
                }
            }
            return result;
        }

        public static bool ParseBooleanTag(this XmlElement element, string nodeName)
        {
            XmlNode node = element.GetElementsByTagName(nodeName).Item(0);
            return node.ParseBoolAttribute("value", true);
        }

        public static float ParseFloatAttribute(this XmlNode node, string attributeName, float defaultValue)
        {
            float result = defaultValue;
            XmlNode floatNode = node.Attributes.GetNamedItem(attributeName);
            if (floatNode != null)
            {
                string text = floatNode.Value;
                if (!float.TryParse(text, out result))
                {
                    logger.Warn("XML Warning: " + attributeName + "'s value is not recognized, using default value " +
                                defaultValue);
                }
            }
            return result;
        }

        public static int ParseIntAttribute(this XmlNode node, string attributeName, int defaultValue)
        {
            int result = defaultValue;
            XmlNode intNode = node.Attributes.GetNamedItem(attributeName);
            if (intNode != null)
            {
                string text = intNode.Value;
                if (!int.TryParse(text, out result))
                {
                    logger.Warn("XML Warning: " + attributeName + "'s value is not recognized, using default value " +
                                defaultValue);
                }
            }
            return result;
        }

        public static long ParseLongAttribute(this XmlNode node, string attributeName, long defaultValue)
        {
            long result = defaultValue;
            XmlNode longNode = node.Attributes.GetNamedItem(attributeName);
            if (longNode != null)
            {
                string text = longNode.Value;
                if (!long.TryParse(text, out result))
                {
                    logger.Warn("XML Warning: " + attributeName + "'s value is not recognized, using default value " +
                                defaultValue);
                }
            }
            return result;
        }

        public static List<string> ParsePathList(this XmlNode node)
        {
            List<string> list = new List<string>();

            XmlNodeList items = node.ChildNodes;
            foreach (XmlNode n in items)
            {
                string name = n.LocalName;
                if (name == "item")
                {
                    list.Add(n.Attributes.GetNamedItem("path").Value);
                }
            }
            return list;
        }

        public static string ParseStringAttribute(this XmlNode node, string attributeName, string defaultValue)
        {
            string result = defaultValue;
            XmlNode stringNode = node.Attributes.GetNamedItem(attributeName);
            if (stringNode != null)
            {
                result = stringNode.Value;
            }
            return result;
        }
    }
}