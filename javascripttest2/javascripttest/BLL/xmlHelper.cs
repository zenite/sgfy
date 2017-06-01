using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace javascripttest
{
    public class xmlHelper
    {
        public static void SerializeToXml(object srcObject, Type type, string xmlFilePath, string xmlRootName)
        {
            if (srcObject != null && !string.IsNullOrEmpty(xmlFilePath))
            {
                type = type != null ? type : srcObject.GetType();

                using (StreamWriter sw = new StreamWriter(xmlFilePath))
                {
                    XmlSerializer xs = string.IsNullOrEmpty(xmlRootName) ?
                        new XmlSerializer(type) :
                        new XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                    xs.Serialize(sw, srcObject);
                }
            }
        }

        public static DataSet getXmlData()
        {
            string path = "server_list.xml";
            DataSet ds = new DataSet();
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                ds.ReadXml(sr);
                sr.Close();
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        /// <summary>
        /// 保存或更新服务器信息
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="paraValue"></param>
        public static void addOrEditElement(string paraName, string paraValue)
        {
            string xml_path = "basic.xml";
            XmlDocument document = new XmlDocument();
            document.Load(xml_path);
            XmlNode node = document.SelectSingleNode("basic");
            try
            {
                if (node.SelectSingleNode("setting") == null)
                {
                    XmlElement childNode = document.CreateElement("setting");
                    childNode.SetAttribute(paraName, paraValue);
                    node.AppendChild(childNode);
                }
                else
                {
                    XmlElement childNode = (XmlElement)node.SelectSingleNode("setting");
                    childNode.SetAttribute(paraName, paraValue);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                document.Save(xml_path);
            }          
            
        }
        /// <summary>
        /// 获取特定值段的属性
        /// </summary>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public static string getElementValue(string paraName)
        {
            string xml_path = "basic.xml";
            XmlDocument document = new XmlDocument();
            document.Load(xml_path);
            //XmlNamespaceManager xnsm=new XmlNamespaceManager(document.NameTable);
            
            XmlElement element = (XmlElement)document.SelectSingleNode("basic").SelectSingleNode("setting");
            string returnvalue = element.GetAttribute(paraName);
            return returnvalue;
        }


        public static void saveConfig(string paraName, string paraValue)
        {
            xmlHelper.addOrEditElement(paraName, paraValue);
        }
        public static string getConfig(string paraName)
        {
            return xmlHelper.getElementValue(paraName);
        }



    }
}
