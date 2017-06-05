using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Threading.Tasks;

namespace javascripttest.Regular
{
    public  class SetConfig
    {
        private string rootname;
        private string filePath;
        public SetConfig(string name,string rootname)
        {
            string dir = System.Windows.Forms.Application.StartupPath;
            filePath=Path.Combine(dir,name+".xml");
            this.rootname = rootname;
            if (!File.Exists(filePath))
            {               
                CreateBasicXml();
            }
                
            
        }

        public void CreateBasicXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode declear = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(declear);
            XmlNode rootNode = xmlDoc.CreateElement(rootname);
            xmlDoc.AppendChild(rootNode);
            xmlDoc.Save(filePath);
        }
        public void setAttribute(object sender, EventArgs e)
        {
            entity.Node node = new entity.Node();
            if(sender is CheckBox)
            {
                CheckBox control = (CheckBox)sender;
                node.name = control.Name;
                node.state = control.Checked ? "true" : "false";
                node.controlType = "CheckBox";
            }
            else if (sender is TextBox)
            {
                TextBox control = (TextBox)sender;
                node.name = control.Name;
                node.state = control.Text;
                node.controlType = "TextBox";
            }
            else if (sender is ComboBox)
            {
                ComboBox control = (ComboBox)sender;
                node.name = control.Name;
                node.state = control.SelectedIndex.ToString();
                node.controlType = "ComboBox";
            }
            else if (sender is RadioButton)
            {
                RadioButton control = (RadioButton)sender;
                node.name = control.Name;
                node.state = control.Checked ? "true" : "false";
                node.controlType = "RadioButton";
            }
            xmlSetNode(node);
        }
        public void xmlSetNode(entity.Node node)
        {
            try
            {
                XElement rootele = XElement.Load(filePath);
                IEnumerable<XElement> xmleles = from target in rootele.Descendants("Control") where target.Attribute("name").Value.Equals(node.name) select target;
                if (xmleles == null || xmleles.Count()==0)
                {
                    CreateNode(node, rootele);
                }
                else
                    updateNode(node, rootele, xmleles.FirstOrDefault());
            }
            catch (Exception ex)
            { 
                
            }            

        }

        public void CreateNode(entity.Node node, XElement rootele)
        {
            XElement xele = new XElement("Control", new XAttribute("name", node.name), new XAttribute("state", node.state), new XAttribute("controlType", node.controlType));
            rootele.Add(xele);
            rootele.Save(filePath);
        }
        public void updateNode(entity.Node node, XElement rootele,XElement ele)
        {
            if (!ele.Attribute("name").Value.Equals(node.state) || !ele.Attribute("controlType").Value.Equals(node.controlType))
            {
                ele.SetAttributeValue("state", node.state);
                ele.SetAttributeValue("controlType", node.controlType);
            }
            rootele.Save(filePath);
        }

        public List<entity.Node> getMainXml()
        {
            var xele = XElement.Load(filePath).Descendants("Control");
            if(xele.Count()>0)
              return (from target in xele select new entity.Node() { name = target.Attribute("name").Value, state = target.Attribute("state").Value, controlType = target.Attribute("controlType").Value }).ToList();
            return null;
        }
    }
}
