﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace javascripttest
{
    public class GetScript
    {
        public string jsstring { set; get; }   
        public GetScript()
        {
            string path = "E:\\vs开发文件\\javascripttest\\javascripttest\\jsscript\\kl_login.js";
            jsstring = File.ReadAllText(path);  
        }
         
    }
}
 