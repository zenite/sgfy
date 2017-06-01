using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using System.Windows.Forms;
using System.Threading.Tasks;

namespace javascripttest.BLL
{
     public class ControlValueXmlConstruction
    {
         public void XmlConstruction(XmlTextWriter xmlWr,Control control, string excludeControlName)
         {
             if (!control.GetType().Name.Contains("DataGridView"))
             {
                 string controlname = control.Name;
                 Control.ControlCollection Controls = control.Controls;

                 if (Controls.Count > 0)
                 {
                     xmlWr.WriteStartElement(controlname);

                     foreach (Control sonControl in Controls)
                     {
                         XmlConstruction(xmlWr, control, excludeControlName);
                     }
                 }
                 else
                 {
                     xmlWr.WriteElementString("name", "janny");
                 }
                 xmlWr.WriteEndElement();
             }
             
         }

         public void xmlCreateXml(string accountName,string excludeControlName,Control control)
         {
             string dirFolder="Accountauto";
             string filepath=Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,dirFolder,accountName);
             if (File.Exists(filepath))
             {
                 File.Delete(filepath);
             }
             XmlTextWriter xmlWr = new XmlTextWriter(filepath, Encoding.ASCII);
             xmlWr.Formatting = Formatting.Indented;
             xmlWr.WriteStartDocument();             
             XmlConstruction(xmlWr, control, excludeControlName);
             xmlWr.WriteEndDocument();
             xmlWr.Flush();
             xmlWr.Close();
         }
    }
}
