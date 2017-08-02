using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Data;

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
            string AppPath = System.Windows.Forms.Application.StartupPath;
            this.rootname = rootname;
            CreateDir(AppPath, name);
            filePath = Path.Combine(AppPath, name + ".xml");
            if (!File.Exists(filePath))
            {
                CreateBasicXml(rootList);
            }
        }
        public void CreateDir(string AppPath,string rootname)
        { 
            string[] dirArray=rootname.Split('\\');
            if (dirArray.Length > 0)
            {
                
                for (var i = 0; i < dirArray.Length-1; i++)
                {
                    AppPath = Path.Combine(AppPath, dirArray[i]);
                    if (!Directory.Exists(AppPath))
                    {
                        Directory.CreateDirectory(AppPath);
                    }
                }
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

        public void setAttribute(object sender,  string parentNode, string sonName, string VillageId)
        {
            entity.Node node = new entity.Node();
            node.VillageId = VillageId;
          
            if (sender is CheckBox)
            {
                CheckBox control = (CheckBox)sender;
                node.name = control.Name;
                node.NodeName = "Control";
                node.state = control.Checked ? "true" : "false";
                node.text = control.Text;
                node.controlType = "CheckBox";
            }
            else if (sender is TextBox)
            {
                TextBox control = (TextBox)sender;
                node.name = control.Name;
                node.NodeName = "Control";
                node.state = control.Text;
                node.text = control.Text;
                node.controlType = "TextBox";
            }
            else if (sender is ComboBox)
            {
                ComboBox control = (ComboBox)sender;
                node.NodeName = "Control";
                node.name = control.Name;
                node.text = control.Text;
                node.state = control.SelectedIndex.ToString();
                if (node.state == "0")
                    return;
                node.controlType = "ComboBox";
            }
            else if (sender is RadioButton)
            {
                RadioButton control = (RadioButton)sender;
                node.name = control.Name;
                node.NodeName = "Control";
                node.text = control.Text;
                node.state = control.Checked ? "true" : "false";
                node.controlType = "RadioButton";
            }
            xmlSetNode(node, parentNode);
        }

        public void setAttribute(DataSet ds,string parentNode,string sonName)
        {            
          
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    entity.NodeAttack attack = new entity.NodeAttack();
                    PropertyInfo[] properties = attack.GetType().GetProperties();
                    foreach (var item in properties)
                    {
                        item.SetValue(attack, dr[item.Name]);
                    }
                  
                    xmlSetNode(attack, parentNode);
                }
            }
            
        }

        public void setAttribute(entity.NodeAttack node, string parentNode)
        {
            xmlSetNode(node, parentNode);
        }
        public void xmlSetNode<T>(T node, string parentNode)
        {
            XElement rootele = XElement.Load(filePath);
            var nodename = (node as entity.AbstractNode).NodeName;
            var PNode = rootele.Descendants(parentNode).FirstOrDefault();
            try
            {             
                IEnumerable<XElement> xmleles=null;
                if (nodename == "Control")
                {
                    if((node as entity.AbstractNode).name=="GeneralList")
                    {
                       xmleles= from target in rootele.Descendants(nodename) where target.Attribute("name").Value.Equals((node as entity.AbstractNode).name) && target.Attribute("VillageId").Value.Equals((node as entity.AbstractNode).VillageId)&&target.Attribute("text").Value.Equals((node as entity.Node).text) select target;
                    }
                    else
                        xmleles = from target in rootele.Descendants(nodename) where target.Attribute("name").Value.Equals((node as entity.AbstractNode).name) && target.Attribute("VillageId").Value.Equals((node as entity.AbstractNode).VillageId) select target;
                }
                    
                else
                    xmleles = from target in rootele.Descendants(nodename) where target.Attribute("x").Value.Equals((node as entity.NodeAttack).x) && target.Attribute("y").Value.Equals((node as entity.NodeAttack).y) && target.Attribute("VillageId").Value.Equals((node as entity.AbstractNode).VillageId) select target;
                if (xmleles!= null&&xmleles.Count()>0)
                {
                    updateNode(node, rootele, xmleles.FirstOrDefault());
                }
                else
                    CreateNode(node, rootele, PNode);
            }
            catch (Exception ex)
            {
                CreateNode(node, rootele, PNode);
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
        public void CreateNode<T>(T node, XElement rootele, XElement PNode)
        {
            var nodename = (node as entity.AbstractNode).NodeName;
            XElement xele = new XElement(nodename);
            PropertyInfo[] properties = new PropertyInfo[] { };
            if (nodename.Equals("Control"))
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
            if ((node as entity.AbstractNode).NodeName.Equals("Control"))
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
        public void removeNode(string x, string y, string VillageId)
        {
            var rootxele = XElement.Load(filePath);
            var xele = from target in rootxele.Descendants("Attack") where target.Attribute("x").Value.Equals(x) && target.Attribute("y").Value.Equals(y) && target.Attribute("VillageId").Value.Equals(VillageId) select target;
            if(xele!=null)
            xele.Remove();
            rootxele.Save(filePath);            
        }

        public void updateTime(string x, string y, string VillageId)
        {
            updateProp(x, y, VillageId, "Time", System.DateTime.Now.ToString());

        }
        public void updateProp(string x, string y, string VillageId, string pro, string proValue)
        {
            var rootxele = XElement.Load(filePath);
            XElement xele = (from target in rootxele.Descendants("Attack") where target.Attribute("x").Value.Equals(x) && target.Attribute("y").Value.Equals(y) && target.Attribute("VillageId").Value.Equals(VillageId) select target).FirstOrDefault();
            if (xele != null)
            {
                xele.SetAttributeValue(pro, proValue);
            }
        }
        public List<entity.Node> getControlXml(string VillageId)
        {
            var xele = XElement.Load(filePath).Descendants("Control");
            foreach (var item in xele)
            {
                string vv = item.Attribute("VillageId").Value.ToString();
            }
            if (xele.Count() > 0)
                return (from target in xele where target.Attribute("VillageId").Value.ToString().Equals(VillageId) select new entity.Node() { name = target.Attribute("name").Value, state = target.Attribute("state").Value, controlType = target.Attribute("controlType").Value, VillageId = target.Attribute("VillageId").Value,text=target.Attribute("text").Value }).ToList();
            return null;
        }
        public List<entity.NodeAttack> getAttackXml(string VillageId)
        {
            var xele = XElement.Load(filePath).Descendants("Attack");

            if (xele.Count() > 0)
                return (from target in xele
                        where target.Attribute("VillageId").Value.ToString().Equals(VillageId)
                        select new entity.NodeAttack()
                        {
                            name = target.Attribute("name").Value,
                            VillageId=target.Attribute("VillageId").Value,
                            x=target.Attribute("x").Value,
                            y=target.Attribute("y").Value,
                            chief=target.Attribute("chief").Value,
                            hand=target.Attribute("hand").Value,
                            city=target.Attribute("city").Value,
                            Time=target.Attribute("Time").Value
                        }).OrderByDescending(item=>item.Time).ToList();
            return null;
        }
    }
   
}
