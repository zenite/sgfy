using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javascripttest
{
    public class Constant
    {

        /// <summary>
        /// 当前登录的服务器url
        /// </summary>
        public static string Server_Url { set; get; }
        public const int resourceIndex = 0;
        public const int recruiteIndex=1;
        public const int defenseIndex=2;
        public const int cargoIndex = 3;
        public const int attackIndex = 4;
        public const int otherIndex = 5;
        public const int pwdIndex = 6;
        public static int[,] m_nBaseOff;
        public static int[,] m_nBaseRaiseSec;
        public static int[,] m_nBaseSpd;
        public const int m_nBattalionCount = 0x23;
        public const int m_nBldCount = 0x20;
        public const int m_nBldTypeCount = 0x30;
        public const int m_nCountryCount = 3;
        public static int[] m_nInfluencePartyID;
        public const int m_nMapMaxX = 800;
        public const int m_nMapMaxY = 800;
        public const int m_nMapTypeCount = 7;
        public const int m_nPartyCount = 10;
        public const int m_nResCount = 0x12;
        public static int[,] m_nresType;
        public static string[] m_nresTypeChNames;
        public const int m_nResTypeCount = 4;
        public static string[] m_nresTypeNames;
        public const int m_nSldTypeCount = 12;
        //public static CARGO[,] m_raisereq;
        //public static CARGO[,] m_reslvlupreq;
        //public static CARGO[] m_specialTradereq;
        public const string m_strAvoidBld = "空地";
        public static string[] m_strBldNames;
        public static string[] m_BldNames;
        public static string[] m_BldValues;
        public static string[] m_strCountryChNames;
        public static string[] m_strCountryNames;
        public static string[] m_strInfluenceCityName;
        public static string[] m_strPartyName;
        public static string[] m_strResNames;
        public static string[,] m_strSoldierNames;
        public static string[] m_strSoldierType;
        public static string[] m_strBidType;
        public static string[,] m_strSoldierRice;
        public static string[] m_strSpecialTradeNames;
        public static string[] m_strSoldierBtidName;
        //public static CARGO[, ,] m_techlvlupreq;
        public static string s_strAccName;
        public static string s_strAppPath;
        public static string s_strCorejsName;
        public static string s_strGamejsName;
        public static string s_strLogPath;
        public static string s_strMapDBName;
        public static string s_strReqName;
        public static string s_strSavePath;
        public static string s_strScriptPath;
         static Constant()
        {    
            m_nBaseOff = new int[,] { { 40, 30, 70, 120, 180, 60, 70, 50, 0, 0, 100, 150 }, { 40, 10, 60, 0x37, 150, 0x41, 50, 40, 10, 0, 80, 120 }, { 15, 0x41, 90, 0x2d, 140, 50, 70, 40, 0, 0, 80, 150 } };
            m_nBaseRaiseSec = new int[,] { { 0x7d0, 0x898, 0x960, 0xce4, 0x1130, 0x11f8, 0x2328, 0x1624c, 0x6914, 0x6a4, 0x9d8, 0xe10 }, { 900, 0x578, 0x5dc, 0xbb8, 0xe74, 0x1068, 0x2328, 0x11364, 0x7918, 0x578, 0x7f8, 0xbb8 }, { 0x514, 0x708, 0xc1c, 0xc80, 0x1194, 0x1388, 0x2328, 0x1624c, 0x58ac, 0x6a4, 0x8e8, 0xd98 } };
            m_nBaseSpd = new int[,] { { 6, 5, 7, 14, 10, 4, 3, 4, 5, 0x10, 6, 14 }, { 7, 7, 6, 10, 9, 4, 3, 4, 5, 9, 7, 7 }, { 7, 6, 0x13, 0x10, 13, 4, 3, 5, 5, 0x11, 7, 7 } };
            m_nInfluencePartyID = new int[] { 
                1, 0, 2, 0, 0, 3, 3, 0, 5, 8, 8, 7, 7, 4, 0, 0, 
                4, 9, 4, 7, 0, 7, 7, 5, 4, 5, 5, 5, 5, 7, 5, 7, 
                7, 7, 6
             };
            m_nresType = new int[,] { { 
                1, 3, 0, 2, 1, 2, 0, 3, 3, 3, 3, 0, 3, 0, 1, 2, 
                1, 2
             }, { 
                1, 3, 0, 2, 0, 2, 0, 3, 3, 3, 3, 0, 3, 0, 1, 2, 
                1, 2
             }, { 
                1, 3, 0, 1, 1, 2, 0, 3, 3, 3, 3, 0, 3, 0, 1, 2, 
                1, 2
             }, { 
                1, 3, 3, 2, 1, 2, 0, 3, 2, 3, 3, 0, 3, 0, 1, 2, 
                1, 2
             }, { 
                1, 3, 3, 2, 3, 3, 0, 3, 3, 3, 3, 0, 3, 0, 1, 2, 
                1, 2
             }, { 
                3, 3, 3, 3, 1, 2, 3, 3, 3, 3, 3, 3, 3, 0, 3, 3, 
                3, 3
             }, { 
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 
                3, 3
             } };
            m_nresTypeChNames = new string[] { "6田", "5木", "5石", "5铁", "9田", "15田", "18田" };
            m_nresTypeNames = new string[] { "4446", "5346", "4536", "3456", "3339", "11115", "00018" };
            m_strBldNames = new string[] { 
                "粮仓", "仓库", "内政厅", "校场", "兵器司", "兵舍", "冶铁监", "城墙", "招贤馆", "中军帐", "马场", "虎贲营", "", "工匠坊", "", "暗仓", 
                "聚义厅", "土木司", "集市", "官邸", "别院", "督造司", "歌舞坊", "学馆", "郡守府", "刺史府", "斥候营", "箭塔", "", "", "太守府", "盟旗", 
                "藏兵洞", "伐木场", "采石场", "采铁场", "磨坊", "皇宫", "", "伤兵营", "魏国城墙", "蜀国城墙", "吴国城墙", "", "", "", "", ""
             };
            m_BldNames=new string[]{								
                    "林场","石矿", "铁矿" ,"农田" ,"中军帐","兵舍","仓库","粮仓","招贤馆","校场","集市","官邸","别院","冶铁监","兵器司","马场","斥候营","土木司","学馆","歌舞坊","工匠坊","虎贲营","盟旗","太守府","皇宫","伐木场","采石场","采铁场","磨坊"							
            };

            m_BldValues=new string[] { 
                "0","1","2","3","109","105","101","100","108","103","118","119","120","106","104","110","126","117","123","122","113","111","131","130","137","133","134","135","136",
            };
            m_strCountryChNames = new string[] { "魏", "蜀", "吴" };
            m_strCountryNames = new string[] { "Wei", "Shu", "Wu" };
            m_strInfluenceCityName = new string[] { 
                "涿县", "安喜", "虎牢", "北海", "磐河", "仓亭", "乌巢", "九原", "襄阳", "新野", "隆中", "博望", "长板", "赤壁", "桂阳", "长沙", 
                "柴桑", "潼关", "渭口", "沙头", "巴郡", "葭萌", "陆口", "上庸", "濡须", "瓦口", "天荡", "定军", "樊城", "麦城", "洛阳", "猇亭", 
                "白帝", "成都", "沾益"
             };
            m_strPartyName = new string[] { "汉军", "黄巾", "董卓", "袁绍", "孙权", "曹操", "孟获", "刘备", "刘表", "马腾" };
            m_strResNames = new string[] { "林场", "石矿", "铁矿", "农田" };
            m_strSoldierNames = new string[,] { { "朴刀兵", "重步兵", "近卫兵", "轻骑兵", "青州骑兵", "冲车", "霹雳车", "说客", "垦荒者", "斥候骑兵", "长弓兵", "弓骑兵" }, { "民兵", "长枪兵", "大刀兵", "枪骑兵", "羽林卫", "冲车", "霹雳车", "说客", "垦荒者", "斥候", "弩兵", "连弩兵" }, { "戈兵", "剑兵", "女骑兵", "重装骑兵", "近卫骑兵", "冲车", "霹雳车", "说客", "垦荒者", "斥候骑兵", "弓兵", "神臂弓兵" } };
            m_strSoldierType = new string[] {  "朴刀兵, 重步兵, 近卫兵, 民兵, 长枪兵, 大刀兵, 戈兵, 剑兵" ,  "轻骑兵, 青州骑兵, 枪骑兵, 羽林卫, 女骑兵, 重装骑兵, 近卫骑兵" ,  "斥候骑兵, 长弓兵, 弓骑兵, 斥候, 弩兵, 连弩兵, 弓兵, 神臂弓兵" , "冲车,霹雳车" };
            m_strBidType = new string[] { "兵舍", "马场", "斥候营", "工匠坊" };
            m_strSoldierRice = new string[,] { { "1", "1", "1", "3", "4", "0", "0", "0", "0", "2", "2", "3" }, { "1", "1", "1", "2", "3", "0", "0", "0", "0", "1", "2", "2" }, { "1", "1", "2", "2", "3", "0", "0", "0", "0", "2", "2", "3" } };
            m_strSoldierBtidName = new string[]{"朴刀兵, 重步兵, 近卫兵,民兵, 长枪兵, 大刀兵,戈兵, 剑兵","轻骑兵, 青州骑兵,枪骑兵, 羽林卫,女骑兵, 重装骑兵, 近卫骑兵 ","斥候骑兵, 长弓兵, 弓骑兵,斥候, 弩兵, 连弩兵,, 弓兵, 神臂弓兵", "冲车, 霹雳车","说客, 垦荒者"};
            m_strSpecialTradeNames = new string[] { "运输骑兵", "木牛流马", "水运" };
            //s_strAppPath = Application.StartupPath;
            s_strAccName = s_strAppPath + @"\accounts.xml";
            s_strCorejsName = s_strAppPath + @"\core.js";
            s_strGamejsName = s_strAppPath + @"\game.js";
            //s_strLogPath = Application.StartupPath + @"\Log";
            s_strMapDBName = s_strAppPath + @"\SGFYAssMap.mdb";
            s_strReqName = s_strAppPath + @"\requirement.xml";
            //s_strSavePath = Application.StartupPath + @"\Save";
            //s_strScriptPath = Application.StartupPath + @"\Script";
        }        
        /// <summary>
        /// 获取国家id
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
         public static int getNCountry(string str)
         {
             for (int i = 0; i < 0x30; i++)
             {
                 if (str.CompareTo(m_strCountryChNames[i]) == 0)
                 {
                     return i;
                 }
             }
             return -1;
         }


        public static int getSoldierBtidByName(string SoldierName)
        {            
            for (int i = 0; i < 5; i++)
            {
                if (m_strSoldierBtidName[i].Contains(SoldierName))
                {
                    if (i == 0)
                        return 5;

                    else if (i == 1)
                        return 10;
                    else if (i == 2)
                        return 26;
                    else if (i == 3)
                        return 13;
                    else
                        return 20;
                }
            }
            return -1;
        }
        //获取兵种id
        public static int GetSldTypeByName(int nCountry, string strName)
        {
            for (int i = 0; i < 12; i++)
            {
                if (m_strSoldierNames[nCountry, i].CompareTo(strName) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        //获取兵营
        public static string GetSoldierHome( string strName)
        {
            for (int i = 0; i < 12; i++)
            {
                if (m_strSoldierType[i].Contains(strName))
                {
                    return m_strBidType[i];
                }
            }
            return "兵舍";
        }
        public static int getSRiceByName(int nCountry, string strName)
        {
            for (int i = 0; i < 3; i++)
            {
                if (m_strSoldierNames[nCountry, i].CompareTo(strName) == 0)
                {
                    return Convert.ToInt16(m_strSoldierRice[nCountry, i]);
                }
            }
            return 0;
        }
        public static int GetResouceid(string str)
        {
            for (int i = 0; i < 0x30; i++)
            {
                if (!(string.IsNullOrEmpty(m_strResNames[i]) || (str.CompareTo(m_strResNames[i]) != 0)))
                {
                    return i;
                }
            }
            return -1;
        }

        public static int GetBldTypeByName(string str)
        {
            for (int i = 0; i < 0x30; i++)
            {
                if (!(string.IsNullOrEmpty(m_strBldNames[i]) || (str.CompareTo(m_strBldNames[i]) != 0)))
                {
                    return i;
                }
            }
            return -1;
        }
        
        public static string GetSpecialTechNameByType(int type)
        {
            switch (type)
            {
                case 100:
                    return "商";

                case 0x65:
                    return "步";

                case 0x66:
                    return "骑";
            }
            return "";
        }

       

        public static int GetTargetLevel(int nGenLvl)
        {
            if (nGenLvl < 3)
            {
                return 5;
            }
            if (nGenLvl < 9)
            {
                return (nGenLvl + 2);
            }
            if (nGenLvl == 9)
            {
                return 10;
            }
            return nGenLvl;
        }

        public static int GetTechIndexByType(int type)
        {
            int num = type;
            switch (type)
            {
                case 100:
                    return 14;

                case 0x65:
                    return 12;

                case 0x66:
                    return 13;
            }
            return num;
        }
    }
}



