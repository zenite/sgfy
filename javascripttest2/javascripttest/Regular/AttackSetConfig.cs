using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using System.Threading.Tasks;

namespace javascripttest.Regular
{
    class AttackSetConfig
    {
        private string rootname;
        private string filePath;
        public AttackSetConfig(string name,string rootname,string[] rootList)
        {
            string dir = System.Windows.Forms.Application.StartupPath;
            this.rootname = rootname;
            if (!Directory.Exists(Path.Combine(dir, this.rootname)))
                Directory.CreateDirectory(Path.Combine(dir, this.rootname));
            filePath = Path.Combine(dir, name + ".xml");
            if (!File.Exists(filePath))
            {
                CreateBasicXml(rootList);
            }
        }
        public void CreateBasicXml(string[] rootList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode declear = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(declear);
            XmlNode rootNode = xmlDoc.CreateElement(rootname);
            xmlDoc.AppendChild(rootNode);
            if (rootList.Count() > 0)
            {
                foreach (var item in rootList)
                {
                    XmlNode node = xmlDoc.CreateElement(item);
                    rootNode.AppendChild(node);
                }
            }
            xmlDoc.Save(filePath);
        }
        public void setAttribute(object sender, EventArgs e,string parentNode,string sonName)
        {
            entity.Node node = new entity.Node();
            
            if (sender is CheckBox)
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
            xmlSetNode(node, parentNode, sonName);
        }
        public void xmlSetNode<T>(T node, string parentNode, string sonName)
        {
            try
            {
                XElement rootele = XElement.Load(filePath);
                var PNode = rootele.Descendants(parentNode).FirstOrDefault();
                IEnumerable<XElement> xmleles = from target in rootele.Descendants(sonName) where target.Attribute("name").Value.Equals((node as entity.AbstractNode).name) select target;
                if (xmleles == null || xmleles.Count() == 0)
                {
                    CreateNode(node, rootele, PNode, sonName);
                }
                else
                    updateNode(node, rootele, xmleles.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 插入节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="rootele"></param>
        /// <param name="PNode"></param>
        /// <param name="sonName"></param>
        public void CreateNode<T>(T node, XElement rootele, XElement PNode,string sonName)
        {
            XElement xele = new XElement(sonName);
            PropertyInfo[] properties = new PropertyInfo[] { };
            if ((node as entity.AbstractNode).name.Equals("Control"))
            {
                entity.Node control = node  as entity.Node;
                properties = control.GetType().GetProperties();
            }
            else
            {
                entity.NodeAttack attact = node as entity.NodeAttack;
                properties = attact.GetType().GetProperties();
            }
            foreach (var item in properties) {
                xele.SetAttributeValue(item.Name, item.GetValue(node));
            }
            PNode.Add(xele);
            rootele.Save(filePath);
        }
        /// <summary>
        /// 更新节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="rootele"></param>
        /// <param name="ele"></param>
        public void updateNode<T>(T node, XElement rootele, XElement ele)
        {
            var check = true;
            PropertyInfo[] properties = new PropertyInfo[] { };
            if ((node as entity.AbstractNode).name.Equals("Control"))
            {
                entity.Node control = node as entity.Node;
                properties = control.GetType().GetProperties();
            }
            else
            {
                entity.NodeAttack attact = node as entity.NodeAttack;
                properties = attact.GetType().GetProperties();
            }
            foreach (var item in properties)
            {
                check = check&&item.GetValue(node).ToString().Equals(ele.Attribute(item.Name).ToString());
            }
           if(!check)
               foreach (var item in properties)
               {
                   ele.SetAttributeValue(item.Name, item.GetValue(node));
               }
            rootele.Save(filePath);
        }
        public void removeNode(string x, string y)
        {
            var rootxele = XElement.Load(filePath);
            var xele= from target in   rootxele.Descendants("attack") where target.Attribute("x").Equals(x)&&target.Attribute("y").Equals(y) select target;
            if(xele!=null)
            xele.Remove();
            rootxele.Save(filePath);            
        }

        public List<entity.Node> getMainXml()
        {
            var xele = XElement.Load(filePath).Descendants("Control");
            if (xele.Count() > 0)
                return (from target in xele select new entity.Node() { name = target.Attribute("name").Value, state = target.Attribute("state").Value, controlType = target.Attribute("controlType").Value }).ToList();
            return null;
        }
    }
   
}
