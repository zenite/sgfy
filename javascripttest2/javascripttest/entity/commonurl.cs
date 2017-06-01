using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javascripttest.entity
{
    public class commonurl
    {
        public commonurl()
        {
            url = "http://" + Constant.Server_Url + "/";
            url_head = "http://" + Constant.Server_Url + "/index.php?";
        }
        public string url_head{get;set;}
        public string url;
        public  string geturl(string type,string para1,string para2="",string para3="",string para4="",long rand=0)
        {            
            string url=string.Empty;
            switch(type)
            {
                case "vmanage.status":
                    url = url_head + "act=vmanage.status&villageid={0}&rand=" + rand.ToString();
                    break;
                case "resources.status":
                    url = url_head + "act=resources.status&villageid={0}&rand=" + rand.ToString();
                    break;
                case "build.status":
                    url = url_head + "act=build.status&villageid={0}&rand=" + rand.ToString();
                    break;
                case "build.construction"://新建建筑
                    url = url_head + "act=build.construction&bid={0}&btid={1}&villageid={2}&rand=" + rand.ToString();
                    break;
                case "build.upgrade"://建筑升级
                    url = url_head + "act=build.upgrade&bid={0}&villageid={1}&rand=" + rand.ToString();
                    break;
                //case "vmanage.status":
                //    url = url_head + "act=vmanage.status&villageid={0}&rand=" + rand.ToString();
                //    break;
                //case "resources.status":
                //    url = url_head + "act=resources.status&villageid={0}&rand=" + rand.ToString();
                //    break;
                //case "build.status":
                //    url = url_head + "act=build.status&villageid={0}&rand=" + rand.ToString();
                //    break;

            }
            return url;
        }

       
    }
}
