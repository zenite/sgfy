using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using javascripttest.entity;
using System.Net;
using System.Web;


namespace javascripttest
{

    public class AccountModel : accountBase
    {     
        public List<village> village { set; get; }

        public List<otherInfo> others { set; get; }
        public CookieContainer cookies { set; get; }//cookies
        public int MaxTuntianNum { set; get; }
        public int CurTuntianNum { set; get; }
        public int goldProp { set; get; }
        public List<TuntianInfo> TuntianList { set; get; }
    }

    public class accountBase
    { 
        public string Server_url { get; set; }//账号区url
        public string username { get; set; }//用户名
        public string password { get; set; }//密码

        public string user_id { get; set; }//登陆账号用户id
        public string AccountName { get; set; }//账户名
        
        public string cookieStr { set; get; }//
        public string hasMulti { set; get; }//是否是多账号

        public string extreHtml { set; get; }//

        public int accountCount { set; get; }//账号数量
        public int CurrentScale { set; get; }//  当前账号标号 

        public string Initial_Status { set; get; }//是否初始化

        public string villageid { set; get; }//主城id
        public string chief { set; get; }//君主名
        public string typeOfCountry { set; get; }//国别
        public string rankOfNobility { set; get; }//爵位

       
        public string city_num{ set; get; }//账号城镇数量
    }
    public class urlService
    {
        public string IndexUrl { set; get; }
        public string ServerUlr { set; get; }
        public string Host { set; get; }
        public string originalUlr { set; get; }
    }

    public class otherInfo
    {
        public string propId { set; get; }
        public string propName { set; get; }
        public string propNum { set; get; }
    }


    public class MyItem
    {
        public MyItem()
        {
          
        }

        public string Id
        {
            get;
            set;
        }

        public string IsBind
        {
            get;
            set;
        }

        public string IsTrade
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Num
        {
            get;
            set;
        }

        public string SubId
        {
            get;
            set;
        }
    }


    public class AccountTask
    {
        public AccountModel account { set; get; }
        public string accountId { set; get; }
    }
    
}
