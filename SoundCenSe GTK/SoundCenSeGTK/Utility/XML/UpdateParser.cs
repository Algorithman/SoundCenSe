using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;


namespace SoundCenSeGTK
{
    public class UpdateParser
    {
        public static List<DirectoryData> Parse(string filename)
        {
            XmlDocument doc = new XmlDocument();
            TextReader tr = new StreamReader(filename);
            string xmldata = tr.ReadToEnd().Replace("<?xml version=\"1.1\"", "<?xml version=\"1.0\"");
            tr.Close();
            doc.LoadXml(xmldata);

            List<DirectoryData> data = new List<DirectoryData>();

            XmlNodeList nl = doc.GetElementsByTagName("autoUpdater");
            XmlNode main = nl.Item(0);
            string relpath = "";

            foreach (XmlNode dirnode in main.ChildNodes)
            {
                ParseDir(dirnode, data, relpath);
            }
            return data;
        }

        private static void ParseDir(XmlNode dirnode, List<DirectoryData> data, string relpath)
        {
            bool ignore = dirnode.ParseBoolAttribute("ignore", false);
            if (!ignore)
            {
                if (dirnode.LocalName == "directory")
                {
                    // Recursive into subfolders
                    string dName = dirnode.Attributes.GetNamedItem("name").Value;
                    string relpathNew = Path.Combine(relpath, dName);
                    foreach (XmlNode newNode in dirnode.ChildNodes)
                    {
                        ParseDir(newNode, data, relpathNew);
                    }
                }
                else if (dirnode.LocalName == "file")
                {
                    // Generate File data
                    DirectoryData dd = new DirectoryData();
                    dd.Filename = dirnode.Attributes.GetNamedItem("name").Value;
                    dd.RelativePath = relpath;
                    dd.Size = dirnode.ParseLongAttribute("size", -1);
                    dd.SHA1 = dirnode.Attributes.GetNamedItem("sha1").Value.ToLower();
                    data.Add(dd);
                }
            }
        }

        public static string SHA1Checksum(string filename)
        {
            string check = "";
            if (!File.Exists(filename))
            {
                return "";
            }
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    using (SHA1CryptoServiceProvider crypto = new SHA1CryptoServiceProvider())
                    {
                        check = BitConverter.ToString(crypto.ComputeHash(fs)).Replace("-", "").ToLower();
                    }
                }
            }
            catch (Exception)
            {
            }

            return check;
        }
    }
}

