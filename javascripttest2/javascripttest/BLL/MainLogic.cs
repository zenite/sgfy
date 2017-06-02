using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using javascripttest.entity;
using javascripttest.BLL;
using System.Collections;

namespace javascripttest
{    
    public class MainLogic:Extra
    {
        private UrlCommand httpHelper;
        
        private commonurl commonUrl;
        private CommonDelegate.recordLog recorder;
        private CommonDelegate.refreshForm refreshgrid;
        private string logger;
        public MainLogic(commonurl commonUrl, UrlCommand urlCommand, CommonDelegate.recordLog recorder, CommonDelegate.refreshForm refreshgrid)
        {
            httpHelper = urlCommand;            
            this.record = Convert.ToInt32(Math.Ceiling(new decimal(new Random().Next(0xf4240))));
            this.commonUrl = commonUrl;
            this.recorder = recorder;
            this.refreshgrid = refreshgrid;
        }        

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string string_0, string string_1, string string_2);

         private long record;
        public long Rand()
        {
            long num = 0L;
            try
            {
                record++;
                num = record;
            }
            catch (Exception ex)
            {
 
            }            
            return num;
        }

        public bool checkAccountLoginOut(AccountModel account)
        {
            string html = httpHelper.Html_get(this.commonUrl.url_head + "act=build.status&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), this.commonUrl.url, account);
            if (html.Contains("login"))
                return true;
            return false;
        }

        public AccountModel InitialAccountInfo(ref AccountModel account)
        {
            getGlodProp(ref  account);
            List<village> villages = new List<village>();
            string html = httpHelper.Html_get(this.commonUrl.url_head + "act=build.status&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), this.commonUrl.url, account);
            MatchCollection matchs = new Regex("<village id=\"(?<_villageId>\\d+)\" name=\"(?<_villageName>\\S+)\" x=\"(?<_x>\\d+)\" y=\"(?<_y>\\d+)\" state=\"(?<_state>\\S+)\" ismain=\"(?<_ismain>\\d+)\" isstatevillage=\"(?<_isstatevillage>\\d+)\" assistant=\"(?<_assistant>\\d+)\" />", RegexOptions.None).Matches(html);
            foreach (Match item in matchs)
            {                
                village village = new village
                {
                    VillageID = item.Groups["_villageId"].Value,
                    VillageName = item.Groups["_villageName"].Value,
                    X = item.Groups["_x"].Value,
                    Y = item.Groups["_y"].Value,
                    State = item.Groups["_state"].Value,
                    Ismain = item.Groups["_ismain"].Value,
                    IsStateVillage = item.Groups["_isstatevillage"].Value,
                    IsAssistant = item.Groups["_assistant"].Value
                };               

                string buildinghtml = httpHelper.Html_get(this.commonUrl.url_head + "act=build.status&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.commonUrl.url, account);
                Match corps_increase = new Regex("crop.*?increase=\"(?<increase>(\\d|(-?(\\d+)?(.\\d+))))\"", RegexOptions.None).Match(buildinghtml);

                string crops_per = corps_increase.Groups["increase"].Value;
                string crop = string.Empty;
                crop = new Func<string,string>((para) => {
                    if (para.IndexOf(".") != -1) return para.Substring(0, para.IndexOf(".") );
                    else return para;
                }).Invoke(crops_per);
                if (!string.IsNullOrEmpty(crops_per))
                if (Math.Abs(Convert.ToInt32(crop)) > 20000 ||( crop.IndexOf("-")!=-1&&Math.Abs(Convert.ToInt32(crop))>10000))
                    refreshgrid(account.chief, village.VillageName, village.X, village.Y, crop);
                string buildings = new Regex(@"\]\]></html(.|\n|\r|\s)*?><!\[CDATA\[", RegexOptions.None).Replace(buildinghtml, "");
                MatchCollection buildingsmatchs = new Regex("<area.*?sgtitle=\"\\s(?<_bname>.*?)\\s(?<_blevel>.*?)\"\\shref=.*?bid=(?<_bid>\\d+?)'", RegexOptions.None).Matches(buildings);
                int position=0;
                List<Building> buildingList = new List<Building>();
                foreach (Match building in buildingsmatchs)
                {                    
                    string level = building.Groups["_blevel"].Value;
                    int buidinglevel = 0;
                    if (level.Contains("Lv"))
                    {
                        int.TryParse(level.Replace("Lv.", ""), out buidinglevel);
                    }
                    Building build = new Building
                    {
                        buildingId = building.Groups["_bid"].Value,
                        buildingName = building.Groups["_bname"].Value,
                        buidinglevel = buidinglevel,
                        buidingPosition = position
                    };
                    buildingList.Add(build);
                    position++;
                    if (build.buildingName.Contains("中军"))
                        village.bid = build.buildingId;
                }
                village.buildings = buildingList;

                
                string resourceHtml = httpHelper.Html_get(this.commonUrl.url_head + "act=resources.status&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
                string resource = new Regex(@"\]\]></html(.|\n|\r|\s)*?><!\[CDATA\[", RegexOptions.None).Replace(resourceHtml, "");
                MatchCollection resourcematchs = new Regex("<area.*?sgtitle=\"(?<_bname>.*?)\\s(?<_blevel>.*?)\"\\sshape=.*?rid=(?<_rid>\\d+?)'", RegexOptions.None).Matches(resource);
                List<resourceInfo> resourselist = new List<resourceInfo>();
                foreach (Match resourcematch in resourcematchs)
                {
                    string level = resourcematch.Groups["_blevel"].Value;
                    int resourcelevel = 0;
                    if (level.Contains("Lv"))
                    {
                        int.TryParse(level.Replace("Lv.", ""), out resourcelevel);
                    }
                    resourceInfo resourse = new resourceInfo
                    {
                        resourceId=resourcematch.Groups["_rid"].Value,
                        resourceNum = resourcelevel.ToString(),
                        resourceName=resourcematch.Groups["_bname"].Value
                    };
                    resourselist.Add(resourse);                   
                }
                village.resources = resourselist;

                string soldierhtml = httpHelper.Html_get(this.commonUrl.url_head + "act=build.worksite&bid=" + village.bid + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url,account);
                string soldiers = new Regex(@"\]\]></html(.|\n|\r|\s)*?><!\[CDATA\[", RegexOptions.None).Replace(soldierhtml, "");
                soldiers = new Regex(@"\n|\r|\s\s", RegexOptions.None).Replace(soldiers, "");
                MatchCollection soldierlist = new Regex("自己的军队.*?<th colspan=\"3\">(?<_mainGeneral>.*?)</th>.*?武将.*?soldier.*?\">(?<_soldier0>.*?)</td>.*?soldier.*?\">(?<_soldier1>.*?)</td>.*?soldier.*?\">(?<_soldier2>.*?)</td>.*?soldier.*?\">(?<_soldier3>.*?)</td>.*?soldier.*?\">(?<_soldier4>.*?)</td>.*?soldier.*?\">(?<_soldier5>.*?)</td>.*?soldier.*?\">(?<_soldier6>.*?)</td>.*?soldier.*?\">(?<_soldier7>.*?)</td>.*?soldier.*?\">(?<_soldier8>.*?)</td>.*?soldier.*?\">(?<_soldier9>.*?)</td>.*?soldier.*?\">(?<_soldier10>.*?)</td>.*?soldier.*?\">(?<_soldier11>.*?)</td>.*?soldier.*?\">(?<_soldier12>.*?)</td>.*?soldier.*?\">(?<_soldier13>.*?)</td>.*?soldier.*?\">(?<_soldier14>.*?)</td>.*?soldier.*?\">(?<_soldier15>.*?)</td>.*?generalcol\">(?<_soldier16>.*?)</td>", RegexOptions.None).Matches(soldiers);
                List<MyVillageSoldiers> SoldierAndGeneral = new List<MyVillageSoldiers>();
                Match soldier = soldierlist.Cast<Match>().FirstOrDefault();
                if (soldier != null)//军队赋值
                {
                    MyVillageSoldiers villageSoldier = new MyVillageSoldiers
                    {
                        MainGeneral = soldier.Groups["_mainGeneral"].Value,
                        soldier0 = soldier.Groups["_soldier0"].Value,
                        soldier1 = soldier.Groups["_soldier1"].Value,
                        soldier2 = soldier.Groups["_soldier2"].Value,
                        soldier3 = soldier.Groups["_soldier3"].Value,
                        soldier4 = soldier.Groups["_soldier4"].Value,
                        soldier5 = soldier.Groups["_soldier5"].Value,
                        soldier6 = soldier.Groups["_soldier6"].Value,
                        soldier7 = soldier.Groups["_soldier7"].Value,
                        soldier8 = soldier.Groups["_soldier8"].Value,
                        soldier9 = soldier.Groups["_soldier9"].Value,
                        soldier10 = soldier.Groups["_soldier10"].Value,
                        soldier11 = soldier.Groups["_soldier11"].Value,
                        soldier12 = soldier.Groups["_soldier12"].Value,
                        soldier13 = soldier.Groups["_soldier13"].Value,
                        soldier14 = soldier.Groups["_soldier14"].Value,
                        soldier15 = soldier.Groups["_soldier15"].Value,
                        soldier16 = soldier.Groups["_soldier16"].Value
                    };
                    village.soldiers = villageSoldier;
                    village.soldierss = new string[soldier.Groups.Count];
                    for (int i = 0; i < soldier.Groups.Count; i++)
                    {
                        string amount = soldier.Groups["_soldier" + i].Value;

                        village.soldierss[i] = soldier.Groups["_soldier" + i].Value;
                    }
                }
                else
                {
                    MyVillageSoldiers villageSoldier = new MyVillageSoldiers
                    {
                        MainGeneral = "0",
                        soldier0 = "0",
                        soldier1 = "0",
                        soldier2 = "0",
                        soldier3 = "0",
                        soldier4 = "0",
                        soldier5 = "0",
                        soldier6 = "0",
                        soldier7 = "0",
                        soldier8 = "0",
                        soldier9 = "0",
                        soldier10 = "0",
                        soldier11 = "0",
                        soldier12 = "0",
                        soldier13 = "0",
                        soldier14 = "0",
                        soldier15 = "0",
                        soldier16 = "0"
                    };
                    village.soldiers = villageSoldier;
                    village.soldierss = new string[18];
                    for (int i = 0; i < 18; i++)
                    {
                        village.soldierss[i] = "0";
                    }
                }
               
                villages.Add(village);
            }
            account.village = villages;
            
            return account;
        }

        public void getGlodProp(ref AccountModel account)
        { 
              string html = httpHelper.Html_get(this.commonUrl.url_head + "act=store.main&first=1&villageid=" + account.villageid + "&userid="+account.user_id+"&rand=" + this.Rand().ToString(), this.commonUrl.url, account);
              string GlodProphtml = new Regex(@"\]\]></html(.|\n|\r|\s)*?><!\[CDATA\[", RegexOptions.None).Replace(html, "");
              MatchCollection GoldPropList = new Regex("innerHTML.*'(?<_GoldProp>.*?)'", RegexOptions.None).Matches(GlodProphtml);
              if (GoldPropList != null)
              {
                  foreach (Match item in GoldPropList)
                  {
                      account.goldProp = Convert.ToInt32(item.Groups["_GoldProp"].Value);
                  }
              }
              else
              {
                  account.goldProp = 0;
              }            
        }

        
        
        public List<village> getVillages(AccountModel account)
        {
            List<village> villages = new List<village>();
            string html = httpHelper.Html_get(this.commonUrl.url_head + "act=build.status&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), this.commonUrl.url, account);
            MatchCollection matchs = new Regex("<village id=\"(?<_villageId>\\d+)\" name=\"(?<_villageName>\\S+)\" x=\"(?<_x>\\d+)\" y=\"(?<_y>\\d+)\" state=\"(?<_state>\\S+)\" ismain=\"(?<_ismain>\\d+)\" isstatevillage=\"(?<_isstatevillage>\\d+)\" assistant=\"(?<_assistant>\\d+)\" />", RegexOptions.None).Matches(html);
            foreach (Match item in matchs)
            {
                village village = new village
                {
                    VillageID = item.Groups["_villageId"].Value,
                    VillageName = item.Groups["_villageName"].Value,
                    X = item.Groups["_x"].Value,
                    Y = item.Groups["_y"].Value,
                    State = item.Groups["_state"].Value,
                    Ismain = item.Groups["_ismain"].Value,
                    IsStateVillage = item.Groups["_isstatevillage"].Value,
                    IsAssistant = item.Groups["_assistant"].Value
                };
                villages.Add(village);
            }
            return villages;
        }


        public string[] getSoldiers(AccountModel account,string villageId)
        {
            string bid = account.village.Find(item => item.VillageID == villageId).bid;
            string soldierhtml = httpHelper.Html_get(this.commonUrl.url_head + "act=build.worksite&bid=" + bid + "&villageid=" + villageId + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            string soldiers = new Regex(@"\]\]></html(.|\n|\r|\s)*?><!\[CDATA\[", RegexOptions.None).Replace(soldierhtml, "");
            soldiers = new Regex(@"\n|\r|\s\s", RegexOptions.None).Replace(soldiers, "");
            MatchCollection soldierlist = new Regex("自己的军队.*?<th colspan=\"3\">(?<_mainGeneral>.*?)</th>.*?武将.*?soldier.*?\">(?<_soldier0>.*?)</td>.*?soldier.*?\">(?<_soldier1>.*?)</td>.*?soldier.*?\">(?<_soldier2>.*?)</td>.*?soldier.*?\">(?<_soldier3>.*?)</td>.*?soldier.*?\">(?<_soldier4>.*?)</td>.*?soldier.*?\">(?<_soldier5>.*?)</td>.*?soldier.*?\">(?<_soldier6>.*?)</td>.*?soldier.*?\">(?<_soldier7>.*?)</td>.*?soldier.*?\">(?<_soldier8>.*?)</td>.*?soldier.*?\">(?<_soldier9>.*?)</td>.*?soldier.*?\">(?<_soldier10>.*?)</td>.*?soldier.*?\">(?<_soldier11>.*?)</td>.*?soldier.*?\">(?<_soldier12>.*?)</td>.*?soldier.*?\">(?<_soldier13>.*?)</td>.*?soldier.*?\">(?<_soldier14>.*?)</td>.*?soldier.*?\">(?<_soldier15>.*?)</td>.*?generalcol\">(?<_soldier16>.*?)</td>", RegexOptions.None).Matches(soldiers);
            Match soldier = soldierlist.Cast<Match>().FirstOrDefault();
            string[] soldierArray;
            if (soldier != null)//军队赋值
            {
               
                 soldierArray= new string[soldier.Groups.Count];
                for (int i = 0; i < soldier.Groups.Count; i++)
                {
                    string amount = soldier.Groups["_soldier" + i].Value;

                    soldierArray[i] = soldier.Groups["_soldier" + i].Value;
                }
            }
            else
            {


                soldierArray = new string[18];
                for (int i = 0; i < 18; i++)
                {
                    soldierArray[i] = "0";
                }
            }
            return soldierArray;
        }
        public List<General> GetVillageGenerals(AccountModel account, string villageId)
        {
            string html = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=general_list&btid=30&villageid=" + villageId + "&rand=" + this.Rand().ToString(), this.commonUrl.url, account);
            List<General> list = new List<General>();
            MatchCollection matchs = new Regex(@"general.detail&gid=(?<_gid>.*?)&itemcate=equip.*?>(?<_gname>.*?)</strong>(.|\r|\n|\s)+?<td>(?<_officePosition>.*?)</td>(.|\n|\r|\s)+?<td>(?<_physicalPower>.*?)</td>(.|\r|\n|\s)+?<td>(?<_glevel>.*?)</td>(.|\n|\r|\s)+?<td>(?<_wu>.*?)</td>(.|\r|\n|\s)+?<td>(?<_tong>.*?)</td>(.|\r|\n|\s)+?<td>(?<_zhi>.*?)</td>(.|\r|\n|\s)+?<td>(?<_zheng>.*?)</td>(.|\r|\n|\s)+?<td>(?<_type>.*?)</td>(.|\r|\n|\s)+?<td>(?<_status>.*?)</td>", RegexOptions.None).Matches(html);
            IEnumerator enumerator = matchs.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Match current = (Match)enumerator.Current;
                    General item = new General
                    {
                        Gid = current.Groups["_gid"].Value,
                        Gname = current.Groups["_gname"].Value,
                        OfficePosition = current.Groups["_officePosition"].Value,
                        Identity = current.Groups["_identity"].Value,
                        PhysicalPower = current.Groups["_physicalPower"].Value,
                        Glevel = current.Groups["_glevel"].Value,
                        Wu = current.Groups["_wu"].Value,
                        Tong = current.Groups["_tong"].Value,
                        Zhi = current.Groups["_zhi"].Value,
                        Zheng = current.Groups["_zheng"].Value,
                        Type = current.Groups["_type"].Value,
                        Status = current.Groups["_status"].Value
                    };
                    list.Add(item);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            return list;
        }
        public List<General> GetVillageCurrentGenerals(AccountModel account, string villageId)
        {
            string html = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=war_list&btid=9&villageid=" + villageId + "&rand=" + this.Rand().ToString(), this.commonUrl.url, account);
            List<General> list = new List<General>();
            MatchCollection Originalmatchs = new Regex(@"<select.*?general1.*?</select>", RegexOptions.Singleline).Matches(html);
            MatchCollection matchs = new Regex(@"general.detail&gid=(?<_gid>.*?)&itemcate=equip.*?>(?<_gname>.*?)</strong>(.|\r|\n|\s)+?<td>(?<_officePosition>.*?)</td>(.|\n|\r|\s)+?<td>(?<_physicalPower>.*?)</td>(.|\r|\n|\s)+?<td>(?<_glevel>.*?)</td>(.|\n|\r|\s)+?<td>(?<_wu>.*?)</td>(.|\r|\n|\s)+?<td>(?<_tong>.*?)</td>(.|\r|\n|\s)+?<td>(?<_zhi>.*?)</td>(.|\r|\n|\s)+?<td>(?<_zheng>.*?)</td>(.|\r|\n|\s)+?<td>(?<_type>.*?)</td>(.|\r|\n|\s)+?<td>(?<_status>.*?)</td>", RegexOptions.None).Matches(Originalmatchs[0].Value);
            IEnumerator enumerator = matchs.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Match current = (Match)enumerator.Current;
                    General item = new General
                    {
                        Gid = current.Groups["_gid"].Value,
                        Gname = current.Groups["_gname"].Value,
                        OfficePosition = current.Groups["_officePosition"].Value,
                        Identity = current.Groups["_identity"].Value,
                        PhysicalPower = current.Groups["_physicalPower"].Value,
                        Glevel = current.Groups["_glevel"].Value,
                        Wu = current.Groups["_wu"].Value,
                        Tong = current.Groups["_tong"].Value,
                        Zhi = current.Groups["_zhi"].Value,
                        Zheng = current.Groups["_zheng"].Value,
                        Type = current.Groups["_type"].Value,
                        Status = current.Groups["_status"].Value
                    };
                    list.Add(item);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            return list;
        }
        

        public bool checkLoginout(ref AccountModel account)
        {
            string html = httpHelper.Html_get(this.commonUrl.url_head + "act=build.status&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), this.commonUrl.url, account);
            if (html.ToLower().Contains("login.logout"))
                return true;
            return false;
        }


       
        public object GetAttacker(village village, AccountModel account,int logIndex)
        {
            string input = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=main&btid=9&type=2&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            input = new Regex(@"\]\]></html(.|\n|\r|\s)*?><!\[CDATA\[", RegexOptions.None).Replace(input, "");
            input = new Regex(@"\n|\r|\s\s", RegexOptions.None).Replace(input, "");
            MatchCollection matchs = new Regex("来自.*?tips_green\">(?<_name>.*?)（(?<_x>\\d+),(?<_y>\\d+).*?</a>(?<_type>.*?)本城的部队.*?需时.*?sgtitle=\"(?<_time>.*?)\">", RegexOptions.None).Matches(input);
            List<Attacker> list = new List<Attacker>();            
            try
            {
                foreach(Match current in matchs)
                {
                    Attacker item = new Attacker
                    {
                        name = current.Groups["_name"].Value,
                        type = current.Groups["_type"].Value,
                         x= current.Groups["_x"].Value,
                         y= current.Groups["_y"].Value,
                        ArrivalTime = Convert.ToDateTime(current.Groups["_time"].Value)
                    };                    
                    list.Add(item);
                }                    
                    
                
            }
            finally
            {
               
            }            
            return list;
        }
        //募兵
        public string RecruitSolider(int ncountry,village village, AccountModel account, string type, string num, int logIndex, string btid = "5")
        {
            string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=raise&btid=" + btid + "&type= " + type + "&num=" + num + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str);
            string invTypeName = string.Empty;
            //invTypeName = this.GetInvTypeName(bid);
            
            if (str == "")
            {
                logger =account.chief+" 城镇"+ village.VillageName + "：招募到    " + num + Constant.m_strSoldierNames[ncountry, Convert.ToInt32(type)];
                recorder(logger, logIndex);
            }
            else
            {
                
            }
            return str;
        }


        public string RecruitSoliderByBug(int ncountry,village village, AccountModel account, string type,int logIndex)
        {
                string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=max_raise&btid=5&consume_type=-1&userid="+account.user_id+"&bid="+village.bid+"&type= " + type +"&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str);
            string invTypeName = string.Empty;
         
                logger = account.chief + " 城镇" + village.VillageName + "：bug招募"+ Constant.m_strSoldierNames[ncountry, Convert.ToInt32(type)];
                recorder(logger, logIndex);
         
            return str;
           // act=build.act&do=max_raise&bid=20&btid=5&type=2&consume_type=0&userid=385&villageid=7117&wc2au=2ad2541&rand=598646
        }

        //伤兵
        public void RepairSodier(village village, string bid, AccountModel account, int logIndex)
        {
            int num = 0;
        Label_0002: ;
        string str7 = this.commonUrl.url_head + "act=build.worksite&bid=" + bid + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString();
            string str = httpHelper.Html_get(str7,  commonUrl.url, account);
            if (string.IsNullOrEmpty(str))
            {
                //this.method_4(Convert.ToString(6), "连接", "连接中断", listView_0, null);
            }
            else
            {
                int num2 = 0x9c4;
                string str2 = this.getParaValue("(?<=(伤兵营<span>Lv.))(.+?)(?=</)", str);
                switch (str2)
                {
                    case "1":
                        num2 = 50;
                        break;

                    case "2":
                        num2 = 120;
                        break;

                    case "3":
                        num2 = 140;
                        break;

                    case "5":
                        num2 = 170;
                        break;

                    case "6":
                        num2 = 200;
                        break;

                    case "7":
                        num2 = 230;
                        break;

                    case "8":
                        num2 = 280;
                        break;

                    case "9":
                        num2 = 330;
                        break;

                    case "10":
                        num2 = 470;
                        break;

                    case "11":
                        num2 = 560;
                        break;

                    case "12":
                        num2 = 670;
                        break;

                    case "13":
                        num2 = 780;
                        break;

                    case "14":
                        num2 = 940;
                        break;

                    case "15":
                        num2 = 0x44c;
                        break;

                    case "16":
                        num2 = 0x514;
                        break;

                    case "17":
                        num2 = 0x5dc;
                        break;

                    case "18":
                        num2 = 0x708;
                        break;

                    case "19":
                        num2 = 0x834;
                        break;

                    case "20":
                        num2 = 0x9c4;
                        break;
                }
                //this.method_4(Convert.ToString(6), "伤兵营", "级别:" + str2 + " 每条队列:" + num2.ToString(), listView_0, null);
                str2 = this.getParaValue("(?<=(现有伤兵<.*>（))(.+?)(?=）<)", str);
                if (string.IsNullOrEmpty(str2))
                {
                    //this.method_4(Convert.ToString(6), "伤兵营", "无伤兵可治疗", listView_0, null);
                }
                else
                {
                    int num4;
                    int num5;
                    try
                    {
                        num4 = Convert.ToInt32(str2);
                    }
                    catch
                    {
                        num4 = 0;
                    }
                    string str5 = this.getParaValue("(?<=(可治疗.*>（))(.+?)(?=）<)", str);
                    try
                    {
                        num5 = Convert.ToInt32(str5);
                    }
                    catch
                    {
                        num5 = 0;
                    }
                    string str6 = this.getParaValue("(?<=(btid=39', ))(.+?)(?=,)", str);
                    //this.method_4(Convert.ToString(6), "伤兵营", "现有伤兵:" + this.GetInvTypeName(str6) + ":" + str2 + " 可治疗:" + str5, listView_0, null);
                    if (num5 <= 0)
                    {
                        //this.method_4(Convert.ToString(6), "伤兵营", "可治疗伤兵0，无法治疗", listView_0, null);
                    }
                    else
                    {
                        int num6;
                        if (num4 > num2)
                        {
                            num6 = num2;
                        }
                        else
                        {
                            num6 = num4;
                        }
                        if (num6 > num5)
                        {
                            num6 = num5;
                        }
                        num4 -= num6;
                        if (str6 == "")
                        {
                            str6 = "1";
                        }
                        str7 = this.commonUrl.url_head + "act=build.act&do=revive&bid=" + bid + "&btid=39&type=" + str6 + "&num=" + Convert.ToString(num6) + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString();
                        str2 = httpHelper.Html_get(str7,  commonUrl.url, account);
                        str2 = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str2);
                        if (str2 == "")
                        {
                            //this.method_4(Convert.ToString(6), "伤兵营", "治疗伤兵:" + this.GetInvTypeName(str6) + ":" + num6.ToString(), listView_0, null);
                        }
                        else
                        {
                            //this.method_4(Convert.ToString(6), "伤兵营", str2, listView_0, null);
                        }
                        if (num4 >= 0)
                        {
                            num++;
                            if (num <= 2)
                            {
                                goto Label_0002;
                            }
                        }
                    }
                }
            }
        }

        //遣返军队
        public object RepatriateSoldiers(village village, string bid, AccountModel account, int logIndex)
        {
            string input = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=main&btid=" + bid + "&type=4&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(),  commonUrl.url, account);
            input = new Regex(@"\]\]></html(.|\n|\r|\s)*?><!\[CDATA\[", RegexOptions.None).Replace(input, "");
            input = new Regex(@"\n|\r|\s\s", RegexOptions.None).Replace(input, "");
            MatchCollection matchs = new Regex("遣返.*?Load\\('(?<_url>.*?)'\\);\" ><strong class=\"green\">全部遣返", RegexOptions.None).Matches(input);
            new List<MyVillageSoldiers>();
            int num2 = 0;
            IEnumerator enumerator = matchs.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Match current = (Match)enumerator.Current;
                    num2++;
                    string str2 = current.Groups["_url"].Value;
                    input = httpHelper.Html_get(this.commonUrl.url_head + "act=" + str2 + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(),  commonUrl.url, account);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            //this.method_4(Convert.ToString(7), "遣返", Convert.ToString(num2) + "支部队", listView_0, null);
            return input;
        }

        //任务
        //public string RewardInfluenceTask(village village, AccountModel account)
        //{
        //    string str = httpHelper.Html_get(this.commonUrl.url_head + "act=task.showInfluenceTask&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //    for (int i = 0; i <= 10; i++)
        //    {
        //        if (str.Contains("task.showInfluenceTask&influence_id=" + i.ToString() + "&keep=all"))
        //        {
        //            string input = httpHelper.Html_get(this.commonUrl.url_head + "act=task.showInfluenceTask&influence_id=" + i.ToString() + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //            //??????
        //            if (input.Contains("task.influenceTaskDetail&uit_id"))
        //            {
        //                IEnumerator enumerator = new Regex("task.influenceTaskDetail&uit_id=(?<_uit_id>.*?)&keep=all", RegexOptions.None).Matches(input).GetEnumerator();
        //                try
        //                {
        //                    int num3 = 0;
        //                    int num4 = 0x45;
        //                    while (enumerator.MoveNext())
        //                    {
        //                        string str7;
        //                        string str8;
        //                        string str12;
        //                        string str14;
        //                        int num8;
        //                        //List<General>.Enumerator enumerator2;
        //                        num3++;
        //                        Match current = (Match)enumerator.Current;
        //                        string str3 = current.Groups["_uit_id"].Value;
        //                        string str4 = httpHelper.Html_get(this.commonUrl.url_head + "act=task.influenceTaskDetail&uit_id=" + str3 + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                        //??????
        //                        string str5 = this.getParaValue("(?<=(任务名称：.*?class=\"tips_red\">)).*?(?=</strong>)", str4);
        //                        string str6 = this.getParaValue("(?<=(完成奖励：.*?任务)).*?(?=\n\t\t\t</td>)", str4);
        //                        if (str5.Contains("捐献"))
        //                        {
        //                            if (str4.Contains("任务已完成"))
        //                            {
        //                                str6 = "任务尚已完成";
        //                            }
        //                            else
        //                            {
        //                                str6 = "任务尚未开始";
        //                            }
        //                        }
        //                        //this.method_4(Convert.ToString(6), "势力任务", str5 + ":" + str6, listView_0, null);
        //                        if (str6.Contains("进行中") || str4.Contains("任务已完成"))
        //                        {
        //                            continue;
        //                        }
        //                        if (str5.Contains("捐献"))
        //                        {
        //                            str7 = this.getParaValue("(?<=task.influencetaskcontribute&uit_id=).*?(?=&keep=all'\\);\")", str4);
        //                            str8 = httpHelper.Html_get(this.commonUrl.url_head + "act=task.influencetaskcontribute&uit_id=" + str7 + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                            str8 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str8);
        //                            //??????
        //                            //this.method_4(Convert.ToString(6), "势力任务", str5 + ":" + str8, listView_0, null);
        //                            continue;
        //                        }
        //                        string str9 = this.getParaValue("(?<=(onclick=\"MM_xmlLoad\\('battalion.main&city_id=.*?class=\"tips_green\">)).*?(?=</strong>)", str4);
        //                        string str10 = this.getParaValue("(?<=(任务地点：.*?battalion.main&city_id=)).*?(?='\\);\"><strong)", str4);
        //                        str8 = httpHelper.Html_get(this.commonUrl.url_head + "act=battalion.main&city_id=" + str10 + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                        //??????
        //                        str7 = this.getParaValue("(?<=onclick=\"startITask\\().*?(?=," + str10 + ",'" + str9 + "'\\);\" >【)", str8);
        //                        if (str7 == "")
        //                        {
        //                            //this.method_4(Convert.ToString(6), "势力任务", str5 + "可能正在进行中", listView_0, null);
        //                            continue;
        //                        }
        //                        string str11 = str7.Split(new char[] { ',' }).GetValue(1).ToString();
        //                        str7 = str7.Split(new char[] { ',' }).GetValue(0).ToString();
        //                        bool flag = false;
        //                        int num5 = 0;
        //                        if (str11 == "1")
        //                        {
        //                            goto Label_0C3B;
        //                        }
        //                        if ((((str11 == "2") | (str11 == "3")) | (str11 == "4")) | (str11 == "6"))
        //                        {
        //                            goto Label_0521;
        //                        }
        //                        if (!(str11 == "5"))
        //                        {
        //                            switch (num5)
        //                            {
        //                                case 1:
        //                                    goto Label_0C3B;

        //                                case 2:
        //                                    goto Label_0521;

        //                                case 3:
        //                                    goto Label_04FD;
        //                            }
        //                            //this.method_4(Convert.ToString(6), "势力任务", "不支持的势力任务" + str11, listView_0, null);
        //                            continue;
        //                        }
        //                    Label_04FD:
        //                        //this.method_4(Convert.ToString(6), "势力任务", str5 + " 运输资源容易导致封号，不做运输", listView_0, null);
        //                        continue;
        //                    Label_0521:
        //                        str12 = str11;
        //                        if (str12 != null)
        //                        {
        //                            if (str12 == "2")
        //                            {
        //                                goto Label_058B;
        //                            }
        //                            if (str12 == "3")
        //                            {
        //                                goto Label_0582;
        //                            }
        //                            if (str12 == "4")
        //                            {
        //                                goto Label_0579;
        //                            }
        //                            if (str12 == "6")
        //                            {
        //                                goto Label_0570;
        //                            }
        //                        }
        //                        string str13 = "1";
        //                        goto Label_0592;
        //                    Label_0570:
        //                        str13 = "1";
        //                        goto Label_0592;
        //                    Label_0579:
        //                        str13 = "2";
        //                        goto Label_0592;
        //                    Label_0582:
        //                        str13 = "3";
        //                        goto Label_0592;
        //                    Label_058B:
        //                        str13 = "0";
        //                    Label_0592:
        //                        str14 = this.getParaValue("(?<=(请选择</option>.*?<option value=\")).*?(?=\" selected=\"selected)", str8);
        //                        if (str13 == "2")
        //                        {
        //                            str14 = "0";
        //                        }
        //                        if ((str14 == "") || (str14.Length > 10))
        //                        {
        //                            //this.method_4(Convert.ToString(6), "势力任务", "没有可用武将", listView_0, null);
        //                            goto Label_0C2C;
        //                        }
        //                        string gname = this.getParaValue("(?<=(请选择</option>.*?selected=\"selected\">)).*?(?=</option>)", str8);
        //                        List<MyVillageSoldiers> list = new List<MyVillageSoldiers>();
        //                        list = (List<MyVillageSoldiers>)this.GetInfluenceTaskSoldiers(village.VillageID, str10, str9, str7, str14);
        //                        if (list == null)
        //                        {
        //                            goto Label_0C0D;
        //                        }
        //                        string str16 = "";
        //                        string str17 = "0";
        //                        if (list.Count > 0)
        //                        {
        //                            string str18;
        //                            if (str13 == "2")
        //                            {
        //                                str18 = "0;0;0;0;0;0;0;0;0;" + list[0].soldier9 + ";0;0;0;0;0;0;";
        //                            }
        //                            else
        //                            {
        //                                str18 = this.ConvertSoldiers(list[0]);
        //                            }
        //                            int num7 = str18.Split(new char[] { ';' }).Length - 1;
        //                            num8 = 0;
        //                            while (num8 < num7)
        //                            {
        //                                string str19 = str18.Split(new char[] { ';' }).GetValue(num8).ToString();
        //                                if ((Convert.ToInt32(str19) <= 0) || ((str13 == "2") & (num8 != 9)))
        //                                {
        //                                    goto Label_0988;
        //                                }
        //                                string str20 = "";
        //                                int num9 = 0;
        //                                while ((str17 == "") | (str17 == "0"))
        //                                {
        //                                    if (str20 == "")
        //                                    {
        //                                        str8 = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=21&btid=9&target_general_level=0&general1=" + str14 + "&type=" + str13 + "&battlearray=0&uit_condition_general=1&battle_type=task&soldier[" + num8.ToString() + "]=10&city_id=" + str10 + "&uit_id=" + str7 + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                                    }
        //                                    else
        //                                    {
        //                                        str8 = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=21&btid=9&checkcode=" + str20 + "&target_general_level=0&general1=" + str14 + "&type=" + str13 + "&battlearray=0&uit_condition_general=1&battle_type=task&soldier[" + num8.ToString() + "]=10&city_id=" + str10 + "&uit_id=" + str7 + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                                    }
        //                                    //??????
        //                                    if (!str8.Contains("输入验证码"))
        //                                    {
        //                                        goto Label_0920;
        //                                    }
        //                                    num9++;
        //                                    if (num9 > 20)
        //                                    {
        //                                        goto Label_08FF;
        //                                    }
        //                                    str20 = Convert.ToString(this.GetPKCheckCode(listView_0));
        //                                    //this.method_4(Convert.ToString(8), "势力验证", str20, listView_0, null);
        //                                    Thread.Sleep((int)(MainModule.longsleep * 2));
        //                                }
        //                                goto Label_0958;
        //                            Label_08FF:
        //                                //this.method_4(Convert.ToString(8), "势力任务", str5 + ":验证码识别错误超限", listView_0, null);
        //                                goto Label_0988;
        //                            Label_0920:
        //                                if (str8.Contains("搬迁24小时内不允许出兵"))
        //                                {
        //                                    goto Label_099E;
        //                                }
        //                                str17 = this.getParaValue("(?<=至少要派).*?(?=个兵力)", str8);
        //                                if (str17 == "")
        //                                {
        //                                    str17 = "0";
        //                                }
        //                            Label_0958:
        //                                if (((str17 != "") && (Convert.ToInt32(str17) > 0)) && (Convert.ToInt32(str19) >= Convert.ToInt32(str17)))
        //                                {
        //                                    goto Label_09C6;
        //                                }
        //                            Label_0988:
        //                                num8++;
        //                            }
        //                        }
        //                        goto Label_0BEC;
        //                    Label_099E:
        //                        //this.method_4(Convert.ToString(6), "势力任务", str5 + ":搬迁24小时内不允许出兵", listView_0, null);
        //                        str16 = "无法出兵";
        //                        goto Label_09E0;
        //                    Label_09C6:
        //                        str16 = "soldier[" + num8.ToString() + "]=" + str17;
        //                    Label_09E0:
        //                        if (!(str16 != ""))
        //                        {
        //                            //this.method_4(Convert.ToString(6), "势力任务", str5 + ":兵力不足,需要兵力:" + str17, listView_0, null);
        //                        }
        //                        else if (str16 != "无法出兵")
        //                        {
        //                            str8 = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=21&btid=9&target_general_level=0&general1=" + str14 + "&type=" + str13 + "&battlearray=0&uit_condition_general=1&battle_type=task&" + str16 + "&city_id=" + str10 + "&uit_id=" + str7 + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                            str8 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str8);
        //                            //??????
        //                            str8 = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=21&btid=9&start=1&target_general_level=0&general1=" + str14 + "&type=" + str13 + "&battlearray=0&uit_condition_general=1&battle_type=task&" + str16 + "&city_id=" + str10 + "&uit_id=" + str7 + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                            str8 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str8);
        //                            //this.method_4(Convert.ToString(6), "势力任务", str5 + ":" + gname + "在" + str9 + ":" + str10 + "进行 " + str8, listView_0, null);
        //                        }
        //                        goto Label_0C2C;
        //                    Label_0BEC:
        //                        //this.method_4(Convert.ToString(6), "势力任务", str5 + ":无法获取兵力情况", listView_0, null);
        //                        goto Label_0C2C;
        //                    Label_0C0D:
        //                        //this.method_4(Convert.ToString(6), "势力任务", str5 + ":无法获取兵力情况", listView_0, null);
        //                    Label_0C2C:
        //                        //??????
        //                        continue;
        //                    Label_0C3B:
        //                        str14 = this.getParaValue("(?<=(请选择</option>.*?<option value=\")).*?(?=\" selected=\"selected)", str8);
        //                        if ((str14 == "") || (str14.Length > 10))
        //                        {
        //                            flag = true;
        //                        }
        //                        else
        //                        {
        //                            gname = this.getParaValue("(?<=(请选择</option>.*?selected=\"selected\">)).*?(?=</option>)", str8);
        //                            str8 = httpHelper.Html_get(this.commonUrl.url_head + "act=task.startITask&uit_id=" + str7 + "&gid=" + str14 + "&city_id=" + str10 + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                            str8 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str8);
        //                            //??????
        //                            //this.method_4(Convert.ToString(6), "势力任务", gname + "在" + str9 + ":" + str10 + "进行" + str8, listView_0, null);
        //                            if (str8.Trim() != "")
        //                            {
        //                                if (str8.Contains("级的武将"))
        //                                {
        //                                    str8 = this.getParaValue("(?<=(请派至少)).*?(?=(级的武将))", str8);
        //                                    try
        //                                    {
        //                                        num4 = Convert.ToInt32(str8);
        //                                    }
        //                                    catch (Exception)
        //                                    {
        //                                    }
        //                                }
        //                                flag = true;
        //                            }
        //                        }
        //                        if (!flag)
        //                        {
        //                            continue;
        //                        }
        //                        List<General> list2 = new List<General>();
        //                        list2 = (List<General>)this.GetVillageGenerals(village.VillageID, listView_0, listView_1, listView_2);
        //                        if (list2 == null)
        //                        {
        //                            continue;
        //                        }
        //                        goto Label_0F94;
        //                    Label_0DE3:
        //                        try
        //                        {
        //                            General general;
        //                            while (enumerator2.MoveNext())
        //                            {
        //                                general = enumerator2.Current;
        //                                if (((Conversions.ToDouble(general.Glevel) > num4) & (Conversions.ToDouble(general.PhysicalPower) > 19.0)) & ((general.Status == "待命") | (general.Status == " 太守")))
        //                                {
        //                                    goto Label_0E4F;
        //                                }
        //                            }
        //                            goto Label_0F94;
        //                        Label_0E4F:
        //                            str14 = general.Gid;
        //                            gname = general.Gname;
        //                            str8 = httpHelper.Html_get(this.commonUrl.url_head + "act=task.startITask&uit_id=" + str7 + "&gid=" + str14 + "&city_id=" + str10 + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                            str8 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str8);
        //                            //??????
        //                            //this.method_4(Convert.ToString(6), "势力任务", gname + "在" + str9 + ":" + str10 + "进行" + str8, listView_0, null);
        //                            if (str8.Contains("级的武将"))
        //                            {
        //                                str8 = this.getParaValue("(?<=(请派至少)).*?(?=(级的武将))", str8);
        //                                try
        //                                {
        //                                    num4 = Convert.ToInt32(str8);
        //                                }
        //                                catch (Exception)
        //                                {
        //                                }
        //                            }
        //                            else
        //                            {
        //                                general.Status = "任务";
        //                                continue;
        //                            }
        //                        }
        //                        finally
        //                        {
        //                            enumerator2.Dispose();
        //                        }
        //                    Label_0F94:
        //                        enumerator2 = list2.GetEnumerator();
        //                        goto Label_0DE3;
        //                    }
        //                }
        //                finally
        //                {
        //                    if (enumerator is IDisposable)
        //                    {
        //                        (enumerator as IDisposable).Dispose();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return string.Empty;
        //}
        //主线任务
        //public object RewardTaskMain(village village, AccountModel account)
        //{
        //    int num = 1;
        //Label_0002: ;
        //    string input = httpHelper.Html_get(this.commonUrl.url_head + "act=task.main&type_id=10&subtype_id=10" + num.ToString("00") + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //    if (!input.Contains("button3 redbutton_s"))
        //    {
        //        if (input.Contains("class=\"over"))
        //        {
        //            IEnumerator enumerator = new Regex("tasklist_(?<_ut_id>.*?)\" class=\"over\"", RegexOptions.None).Matches(input).GetEnumerator();
        //            try
        //            {
        //                while (enumerator.MoveNext())
        //                {
        //                    Match current = (Match)enumerator.Current;
        //                    string str4 = current.Groups["_ut_id"].Value;
        //                    string str5 = httpHelper.Html_get(this.commonUrl.url_head + "act=task.rewardTaskMain&ut_id=" + str4 + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                    //str5 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str5);
        //                    //this.method_4(Convert.ToString(6), "主线任务", str5, listView_0, null);
        //                    //??????
        //                }
        //                goto Label_034E;
        //            }
        //            finally
        //            {
        //                if (enumerator is IDisposable)
        //                {
        //                    (enumerator as IDisposable).Dispose();
        //                }
        //            }
        //        }
        //        if (input.Contains("newtask"))
        //        {
        //            IEnumerator enumerator2 = new Regex("tasklist_(?<_ut_id>.*?)\" class=\"newtask\"", RegexOptions.None).Matches(input).GetEnumerator();
        //            try
        //            {
        //                while (enumerator2.MoveNext())
        //                {
        //                    Match match2 = (Match)enumerator2.Current;
        //                    string str6 = match2.Groups["_ut_id"].Value;
        //                    string str7 = httpHelper.Html_get(this.commonUrl.url_head + "act=task.rewardTaskMain&ut_id=" + str6 + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //                    str7 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str7);
        //                    //this.method_4(Convert.ToString(6), "主线任务", str7, listView_0, null);
        //                    //??????
        //                }
        //            }
        //            finally
        //            {
        //                if (enumerator2 is IDisposable)
        //                {
        //                    (enumerator2 as IDisposable).Dispose();
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string str2 = this.getParaValue(@"(?<=(task\.rewardTaskMain&amp;ut_id=)).*?(?=('))", input);
        //        string str3 = httpHelper.Html_get(this.commonUrl.url_head + "act=task.rewardTaskMain&ut_id=" + str2 + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //        str3 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str3);
        //        //this.method_4(Convert.ToString(6), "主线任务", str3, listView_0, null);
        //    }
        //Label_034E:
        //    num++;
        //    if (num <= 10)
        //    {
        //        goto Label_0002;
        //    }
        //    return string.Empty;
        //}
        //其他任务
        //public object RewardTaskOther(village village, AccountModel account)
        //{
        //    string str = httpHelper.Html_get(this.commonUrl.url_head + "act=task.main&type_id=30&subtype_id=3003&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //    str = this.getParaValue("(?<=(ut_id=)).*?(?=('))", str);
        //    str = httpHelper.Html_get(this.commonUrl.url_head + "act=task.rewardTaskOther&ut_id=" + str + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //    str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
        //    //this.method_4(Convert.ToString(6), "领取俸禄", str, listView_0, null);
        //    return str;
        //}
        //
        //public string RewardTaskOtherPK(string village.VillageID, ListView listView_0)
        //{
        //    string str = httpHelper.Html_get(this.commonUrl.url_head + "act=task.main&type_id=30&subtype_id=3004&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //    str = this.getParaValue("(?<=(ut_id=)).*?(?=('))", str);
        //    if (str != "")
        //    {
        //        str = httpHelper.Html_get(this.commonUrl.url_head + "act=task.rewardTaskOther&ut_id=" + str + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //        str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
        //        //this.method_4(Convert.ToString(6), "闻鸡起舞", str, listView_0, null);
        //    }
        //    return str;
        //}
        
        //承上
        //public string RunStudyID(string village.VillageID)
        //{
        //    return this.getParaValue("(?<=build.act&do=upgrade&btid=11Rtypeid=).+?(?=')", village.VillageID);
        //}
        //承上+
        //public bool RunStudyNeed(string village.VillageID, string bid)
        //{
        //    return village.VillageID.Contains("build.act&do=upgrade&btid=11&typeid=" + bid + "'");
        //}
        //承上++
        //public void RunStudyOne(village village, AccountModel account)
        //{
        //    string str = this.commonUrl.url_head + "act=build.act&do=upgrade&btid=11&typeid=" + bid + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString();
        //    string str2 = httpHelper.Html_get(str,  commonUrl.url, account);
        //    str2 = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str2);
        //    if (str2 == "")
        //    {
        //        str2 = "研发中...";
        //    }
        //    //this.method_4(Convert.ToString(6), "虎贲营", this.GetInvTypeName(bid) + str2, listView_0, null);
        //}

        //自动出征
        //public object SendAutoArmy(string village.VillageID, string bid, string string_16, string string_17, string string_18, string string_19, string string_20, string string_21, ListView listView_0)
        //{
        //    int num = 0;
        //    while (true)
        //    {
        //        string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=4&btid=9&target_general_level=0&" + bid + "type=" + string_17 + "&battlearray=0&" + string_16 + "&autoatk_time=" + string_18 + "&autoatk_exception=1&x=" + string_19 + "&y=" + string_20 + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //        if (!this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str).Contains("传令兵"))
        //        {
        //            object obj2 = "";
        //            if (!string.IsNullOrEmpty(str = httpHelper.Html_get(this.ServerUrl + "index.php?act=build.act&do=start_war&btid=9&start=1&" + bid + "type=" + string_17 + "&battlearray=0&autoatk_time=" + string_18 + "&autoatk_exception=1&" + string_16 + "&x=" + string_19 + "&y=" + string_20 + "&keep=right&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl)))
        //            {
        //                string str2 = this.getParaValue("(?<=(autoatk_time=\")).+?(?=\" autoatk_exception=)", str);
        //                //this.method_4(Convert.ToString(1), "自动出征", "前往 X:" + string_19 + " Y:" + string_20 + " 出发时间 " + str2 + " 于 " + string_21 + " 到达", listView_0, null);
        //                return obj2;
        //            }
        //            num++;
        //            if (num > 4)
        //            {
        //                return null;
        //            }
        //            this.Strssid = "";
        //            this.LoginServer(listView_0, null, null, true, false);
        //        }
        //        if (!Operators.ConditionalCompareObjectEqual(this.ExtBatman(village.VillageID, listView_0), "扩充传令兵成功！", false))
        //        {
        //            return "传令失败";
        //        }
        //    }
        //}
      

        
        
        //城镇搬迁
        public object MoveCity(AccountModel account)
        {
            string str = httpHelper.Html_get(commonUrl.url_head + "?act=task.main&type_id=30&subtype_id=3003&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url, account);
            str = this.getParaValue("(?<=(ut_id=)).*?(?=('))", str);
            str = httpHelper.Html_get(commonUrl.url_head + "?act=task.rewardTaskOther&ut_id=" + str + "&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url, account);
            str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);            
            return str;
        }
        //
        public object MoveCityState(AccountModel account, string string_16)
        {
            string input = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.itemdepot&page=1&iname=州符&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url,account);
            input = input.Substring(input.IndexOf("搜索道具"));
            MatchCollection matchs = new Regex(@"useitem&id=(?<_id>.*?)&subid=(?<_subid>.*?)&isbind=(?<_isbind>.*?)&istrade=(?<_istrade>.*?)&keep\=all'", RegexOptions.None).Matches(input);
            try
            {
                foreach(Match match in matchs)
                {                    
                    input = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.useitem&id=" + match.Groups["_id"].Value + "&subid=" + match.Groups["_subid"].Value + "&isbind=" + match.Groups["_isbind"].Value + "&istrade=" + match.Groups["_istrade"].Value + "&keep=all&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url,account);
                    input = httpHelper.Html_get(Constant.Server_Url + "index.php?act=vmove.state&isbind=" + match.Groups["_isbind"].Value + "&istrade=" + match.Groups["_istrade"].Value + "&gid=" + match.Groups["_id"].Value + "&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url,account);
                    input = httpHelper.Html_get(Constant.Server_Url + "index.php?act=vmove.statemove&state=" + string_16 + "&gid=" + match.Groups["_id"].Value + "&isbind=" + match.Groups["_isbind"].Value + "&istrade=" + match.Groups["_istrade"].Value + "&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url,account);
                }
            }
            finally
            {
             
            }
            input = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", input);
            ////this.method_4(Convert.ToString(6), "城镇搬迁", this.Username + input, null);
            return input;
        }

        //搜索道具
        public MyItem SearchItem(string PropName,AccountModel account)
        {
            MyItem item = new MyItem();
            string input = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.itemdepot&page=1&iname=" + PropName + "&page=1&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url, account);
            input = input.Substring(input.IndexOf("搜索道具"));
            MatchCollection matchs = new Regex("iteminfo&id=(?<_id>.*?)&subid=(?<_subid>.*?)&isbind=(?<_isbind>.*?)&istrade=(?<_istrade>.*?)'", RegexOptions.None).Matches(input);
            try
            {
                if (matchs.Count > 0)
                {
                    Match match = matchs.Cast<Match>().FirstOrDefault();
                    item = new MyItem()
                    {
                        Id = match.Groups["_id"].ToString(),
                        IsBind = match.Groups["_isbind"].ToString(),
                        IsTrade = match.Groups["_istrade"].ToString(),
                        Num = "1",
                        SubId = match.Groups["_subid"].ToString(),
                        Name = PropName
                    };
                }
            }
            finally
            {
                
            }
            return item;
        }

        public string reserveRuneToRune(AccountModel account)
        {
            string str = httpHelper.Html_get(Constant.Server_Url + "act=rune.reserveRuneToRune&rand=" + this.Rand().ToString(), Constant.Server_Url, account);
            str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);           
            if (str == "")
            {
                return "帐号安全";
            }           
            return str;
        }



        public void getProp(string PropName, AccountModel account)
        {
            string str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.itemdepot&page=1&iname=" + PropName + "&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url, account);
            str = str.Substring(str.IndexOf("搜索道具"));

        }
        //开包
        public object OpenBag(AccountModel account)
        {
            string str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.iteminfo&id=118&subid=7&isbind=1&istrade=0&villageid=" + account.villageid + "&rand=" + this.Rand().ToString() + "&keep=all", Constant.Server_Url,account);
            str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.useitem&id=118&subid=7&isbind=1&istrade=0&keep=all&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url,account);
            string str2 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
            
            str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.iteminfo&id=118&subid=8&isbind=1&istrade=0&villageid=" + account.villageid + "&rand=" + this.Rand().ToString() + "&keep=all", Constant.Server_Url,account);
            str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.useitem&id=118&subid=8&isbind=1&istrade=0&keep=all&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url,account);
            str2 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
            
            str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.iteminfo&id=118&subid=9&isbind=1&istrade=0&villageid=" + account.villageid + "&rand=" + this.Rand().ToString() + "&keep=all", Constant.Server_Url,account);
            str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=store.useitem&id=118&subid=9&isbind=1&istrade=0&keep=all&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url,account);
            str2 = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
            
            return "";
        }

        public string commonFunction(AccountModel account,string url,string regexStr)
        {
            string str = string.Empty;
            str = httpHelper.Html_get(url + this.Rand().ToString(),"http://"+ Constant.Server_Url, account);
            str = this.getParaValue(regexStr, str);
            return str;
        }

        public Match commonRegex(string original, string regexStr)
        {
            string str = string.Empty;
            Match match = new Regex(regexStr).Match(original);
            return match;
        }
        public MatchCollection commonFunction1(AccountModel account, string url, string regexStr,string filterRegex)
        {
            string str = string.Empty;
            str = httpHelper.Html_get(url + this.Rand().ToString(), "http://" + Constant.Server_Url, account);
            str = new Regex(filterRegex, RegexOptions.None).Replace(str, "");
            MatchCollection matchs = new Regex(regexStr, RegexOptions.None).Matches(str);
            return matchs;            
        }
        public MatchCollection commonFunction1(AccountModel account, string url, string regexStr)
        {
            string str = string.Empty;
            str = httpHelper.Html_get(url + this.Rand().ToString(), "http://" + Constant.Server_Url, account);           
            MatchCollection matchs = new Regex(regexStr, RegexOptions.Singleline).Matches(str);
            return matchs;            
        }

        public string PostForm(string url, string hostUrl, string refernceUrl, AccountModel account, string form_string)
        {
          return   new UrlCommand().PostForm(url + this.Rand(), Constant.Server_Url, "http://" + Constant.Server_Url, account, form_string);
        }
       
       
        //招募垦荒
        //public void RecruitKenhuang(string account.villageid, string bid)
        //{
        //    string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=recruit&bid=" + bid + "&btid=20&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(),  commonUrl.url, account);
        //    string str2 = "8";
        //    string str3 = this.getParaValue("(?<=(现有.*?>（)).+?(?=）</strong></p>)", str);
        //    int num2 = 0;
        //    if (str3 != "")
        //    {
        //        try
        //        {
        //            num2 = Convert.ToInt32(str3);
        //        }
        //        catch (Exception)
        //        {
        //            num2 = 0;
        //        }
        //    }
        //    if (num2 > 0)
        //    {          
                
        //    }
        //    str = this.getParaValue("(?<=(可招募.*?>（)).+?(?=）</a>)", str);
        //    if (str != "")
        //    {
        //        try
        //        {
        //            num2 = Convert.ToInt32(str);
        //        }
        //        catch (Exception)
        //        {
        //            num2 = 0;
        //        }
        //    }
        //    if (num2 > 0)
        //    {
        //        str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=raise&btid=20&type= " + str2 + "&num=" + num2.ToString() + "&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(),  commonUrl.url, account);
        //        str = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str);
        //        string str4 = string.Empty;
        //        if (str == "")
        //        {
                  
        //        }
        //        else
        //        {
                   
        //        }
        //    }
        //}
        /// <summary>
        /// 召回军队a
        /// </summary>
        /// <param name="string_14"></param>
        /// <param name="listView_0"></param>
        /// <returns></returns>
        public object CallbackTuntianArmy(AccountModel account, int logIndex)
        {
            foreach (TuntianInfo info in account.TuntianList)
            {
                string str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=hermithouse.backall&_uintgid=" + info.Gid + "&_uintvid=" + info.Vid + "&userid=" + account.user_id + "&villageid=" + account.villageid + "&rand=" + this.Rand(), Constant.Server_Url, account);
                string str2 = this.getParaValue("(?<=(<locat act.*?>)).+?(?=<)", str);
                recorder("召回屯田" + str2, logIndex);              
            }
            return "";
        }
        //刷新屯田
        public object RefreshTuntian( ref AccountModel account)
        {
            account.MaxTuntianNum = 0;
            account.CurTuntianNum = 0;
            List<TuntianInfo> TuntianList = new List<TuntianInfo>();
            string input = httpHelper.Html_get(Constant.Server_Url + "index.php?act=hermithouse.hoardShow&villageid=" + account.villageid + "&rand=" + this.Rand().ToString(), Constant.Server_Url, account);
            if (input != "")
            {
                MatchCollection matchs = new Regex(@">当前屯田所驻扎兵力人口上限：(?<_MaxNum>\d+)&nbsp;目前已驻扎兵力人口数：(?<_curnum>\d+)(?=<)", RegexOptions.None).Matches(input);
                try
                {
                    foreach (Match match in matchs)                    
                    {
                        account.MaxTuntianNum = Convert.ToInt32(match.Groups["_MaxNum"].Value);
                        account.CurTuntianNum = Convert.ToInt32(match.Groups["_curnum"].Value);
                    }
                }
                finally
                {
                  
                }
                MatchCollection matchs1 = new Regex(@"backall&_uintgid=(?<_uintgid>\d+)&_uintvid=(?<_uintvid>\d+)'").Matches(input);                
                try
                {
                    foreach (Match match in matchs1)                    
                    {
                        
                        TuntianInfo item = new TuntianInfo
                        {
                            Gid = match.Groups["_uintgid"].Value,
                            Vid = match.Groups["_uintvid"].Value
                        };
                        TuntianList.Add(item);
                    }
                    account.TuntianList = TuntianList;
                }
                finally
                {
                   
                }
            }
            return "";
        }
        //向屯田所派兵
        public object SendTuntianArmy(AccountModel account, int logIndex, string villageId,string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                recorder("屯田"+ " 无法派遣",  logIndex);
                return string.Empty;
            }
            string str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=hermithouse.hoard&villageid=" + villageId + "&_uintgid=&_uintvid=&" + data + "&rand=" + this.Rand(), Constant.Server_Url, account);
            string str2 = this.getParaValue("(?<=(<locat act.*?>)).+?(?=<)", str);            
            recorder("屯田" + str2, logIndex);
            return "";
        }
        public object CallbackLocSoldiers(village village, string bid,string x,string y, AccountModel account, int logIndex)
        {
            string str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=build.worksite&bid=" + bid + "&villageid=" + village.VillageID + "&rand=" + this.Rand(), Constant.Server_Url, account);
            if (str.Contains("x=" + x + "&y=" + y))
            {
                str = httpHelper.Html_get(Constant.Server_Url + "index.php?act=build.act&do=callback_support&btid=9&x=" + x + "&y=" + y + "&villageid=" + village.VillageID + "&rand=" + this.Rand(), Constant.Server_Url, account);
                recorder(village.VillageName+ " 军队召回成功", logIndex);
                return str;
            }
            recorder(village.VillageName +"无驻军", logIndex);
            return str;
        }
        //学馆研发
        //public void StudyStudy(string village.VillageID, string bid, ListView listView_0)
        //{
        //    string str = this.commonUrl.url_head + "act=build.act&btid=23&do=traderUpgrade&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString();
        //    string str2 = httpHelper.Html_get(str,  commonUrl.url, account);
        //    str2 = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str2);
        //    if (str2 == "")
        //    {
        //        //this.method_4(Convert.ToString(6), "学馆", "学馆研发中...", listView_0, null);
        //    }
        //    else
        //    {
        //        //this.method_4(Convert.ToString(6), "学馆", str2, listView_0, null);
        //    }
        //    string str3 = this.commonUrl.url_head + "act=build.act&btid=23&do=traderTechUpgrade&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString();
        //    str2 = httpHelper.Html_get(str3,  commonUrl.url, account);
        //    str2 = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str2);
        //    if (str2 == "")
        //    {
        //        //this.method_4(Convert.ToString(6), "学馆", "运输升级中...", listView_0, null);
        //    }
        //    else
        //    {
        //        //this.method_4(Convert.ToString(6), "学馆", str2, listView_0, null);
        //    }
        //}

       
        //地图
        //public string TestLoc(string village.VillageID, string bid, string string_16, ref List<MyMap> list_7, ListView listView_0, ListView listView_1, ListView listView_2)
        //{
        //    list_7.Clear();
        //    int num = 0;
        //Label_0040: ;
        //    string input = httpHelper.Html_get(this.ServerUrl + "index.php?act=map.status&uitx=" + bid + "&uity=" + string_16 + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), this.ServerUrl);
        //    MatchCollection matchs = new Regex("mapinfo=\"(?<_x>\\d+)#(?<_y>\\d+)#(?<_state>.*?)#(?<_user>.*?)#(?<_name>.*?)#(?<_unin>.*?)#(?<_people>.*?)#(?<_id>\\d+)\"", RegexOptions.None).Matches(input);
        //    if (matchs.Count == 0)
        //    {
        //        num++;
        //        if (num > MaxErrNum)
        //        {
        //            return string.Empty;
        //        }
        //        this.Strssid = "";
        //        this.LoginServer(listView_0, listView_1, listView_2, true, false);
        //        goto Label_0040;
        //    }
        //    IEnumerator enumerator = matchs.GetEnumerator();
        //    try
        //    {
        //        while (enumerator.MoveNext())
        //        {
        //            Match current = (Match)enumerator.Current;
        //            MyMap item = new MyMap
        //            {
        //                X = current.Groups["_x"].Value,
        //                Y = current.Groups["_y"].Value,
        //                State = current.Groups["_state"].Value,
        //                User = current.Groups["_user"].Value,
        //                Name = current.Groups["_name"].Value,
        //                Union = current.Groups["_unin"].Value,
        //                People = current.Groups["_people"].Value,
        //                Id = current.Groups["_id"].Value
        //            };
        //            list_7.Add(item);
        //        }
        //    }
        //    finally
        //    {
        //        if (enumerator is IDisposable)
        //        {
        //            (enumerator as IDisposable).Dispose();
        //        }
        //    }
        //    return "";
        //}

        //防御辅助
        public string defensebattle(string x, string y, village village, AccountModel account, string soldierlist, int logIndex, string type, DateTime dd = new DateTime())
        {
            string zhongjun = village.buildings.Find(item => item.buildingName.Contains("中军帐")).buildingId;
            if (string.IsNullOrEmpty(zhongjun))
            return "城镇内无中军帐";
            int count = 0;
            defense_go:
            string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=" + zhongjun + "&btid=9&target_general_level=0&type=" + type + " &battlearray=0&" + soldierlist + "x=" + x + "&y=" + y + "&userid=" + account.user_id + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str);            
            if (str.Contains("传令兵"))
            {
                string message=(string) ExtBatman(village.VillageID, account);
                if(!string.IsNullOrEmpty(message))
                    recorder(message, logIndex);
                if (count < 3)
                {
                    count++;
                    goto defense_go;
                }              
            }

            if((str.Contains("目标不正确") || str.Contains("不可以发兵攻打")) || str.Contains("搬迁24小时内不允许出兵")||str.Contains("不允许增援"))
            {
                 recorder(str, logIndex);
                return str;
            }
            string str3 = this.getParaValue("(?<=(需时.*?<strong>)).+?(?=<)", str);
            string str4 = this.getParaValue("(?<=(=\"需时\"> <strong>.*?'>)).+?(?=</strong>到达)", str);

            str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&btid=9&start=1&target_general_level=0&type="+type+" &battlearray=0&" + soldierlist + "x=" + x + "&y=" + y + "&userid="+account.user_id+"&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
                     
            if (str != "")
            {
                logger = account.chief+"    "+village.VillageName+"出兵：   前往 X:" + x + " Y:" + y + " 需时 " + str3 + " 于 " + str4 + " 到达";
                recorder(logger, logIndex);
            }
            return str;           
        }
        public string attack(string x, string y, village village, AccountModel account, string soldierlist,string type)
        {
            string str1 = string.Empty;
            string zhongjun = village.buildings.Find(item => item.buildingName.Contains("中军帐")).buildingId;
            if (string.IsNullOrEmpty(zhongjun))
                return "城镇内无中军帐";
            int count = 0;
        defense_go:
            string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=" + zhongjun + "&btid=9&target_general_level=0&type=" + type + " &battlearray=0&" + soldierlist + "x=" + x + "&y=" + y + "&userid=" + account.user_id + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str1 = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str);
            if (str.Contains("传令兵"))
            {
                string message = (string)ExtBatman(village.VillageID, account);
                //if (!string.IsNullOrEmpty(message))
                //    recorder(message, logIndex);
                if (count < 3)
                {
                    count++;
                    goto defense_go;
                }
            }

            if ((str.Contains("目标不正确") || str.Contains("不可以发兵攻打")) || str.Contains("搬迁24小时内不允许出兵") || str.Contains("不允许增援"))
            {
                //recorder(str, logIndex);
                return str;
            }
            string str3 = this.getParaValue("(?<=(需时.*?<strong>)).+?(?=<)", str);
            string str4 = this.getParaValue("(?<=(=\"需时\"> <strong>.*?'>)).+?(?=</strong>到达)", str);

            str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&btid=9&start=1&target_general_level=0&type=" + type + " &battlearray=0&" + soldierlist + "x=" + x + "&y=" + y + "&userid=" + account.user_id + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);

            if (str == "")
            {
                str = account.chief + "    " + village.VillageName + "出兵：   前往 X:" + x + " Y:" + y + " 需时 " + str3 + " 于 " + str4 + " 到达";
            }
            return str;
        }
        
        //压秒车城
        public string attackbattle(string x, string y, village village, AccountModel account, string soldierlist, int logIndex, DateTime dd = new DateTime())
        {
            string result=string.Empty;
        defense_go:
            string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=4&btid=9&target_general_level=0&type=0 &battlearray=0&" + soldierlist + "x=" + x + "&y=" + y + "&userid=" + account.user_id + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str);
            int count = 0;
            if (str.Contains("传令兵"))
            {
                string message = (string)ExtBatman(village.VillageID, account);
                if (!string.IsNullOrEmpty(message))
                    recorder(message, logIndex);
                if (count < 3)
                {
                    count++;
                    goto defense_go;
                }
            }
            if ((str.Contains("目标不正确") || str.Contains("不可以发兵攻打")) || str.Contains("搬迁24小时内不允许出兵") || str.Contains("不允许增援") || str.Contains("没有足够数量的士兵"))
            {
                recorder(str, logIndex);
                return str;
            }
            
            string str3 = this.getParaValue("(?<=(需时.*?<strong>)).+?(?=<)", str);
            string[] span=str3.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            DateTime attackLanch = dd.Subtract(new TimeSpan(Convert.ToInt32(span[0]), Convert.ToInt32(span[1]), Convert.ToInt32(span[2])));
            if (attackLanch < System.DateTime.Now)
                result = "无法设置，已经超出时间范围";
            else
            {
                str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&bid=4&btid=9&target_general_level=0&type=0 &battlearray=0&autoatk_exception=1&autoatk_time=" + HttpUtility.UrlEncode(attackLanch.ToString()) + "&" + soldierlist + "x=" + x + "&y=" + y + "&userid=" + account.user_id + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
                str = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str);
                if (str.Contains("没有足够数量的士兵"))
                    return str;
                str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=start_war&start=1&btid=9&target_general_level=0&type=0 &battlearray=0&autoatk_exception=1&autoatk_time=" + HttpUtility.UrlEncode(attackLanch.ToString()) + "&" + soldierlist + "x=" + x + "&y=" + y + "&userid=" + account.user_id + "&keep=all&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
                str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
                if (str != "")
                {
                    logger = account.chief + "    " + village.VillageName + "压秒  " + dd .ToString()+ "：   前往 X:" + x + " Y:" + y + " 需时 " + str3+"设置成功";
                    recorder(logger, logIndex);
                }
                result = "";
            }
            return result;
        }

        public object ExtBatman(string villageid, AccountModel account)
        {
            string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=rebuild_batman&btid=9&keep=all&villageid=" + villageid + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.act&do=ext_orderly&btid=9&villageid=" + villageid + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
            string str2 = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", str);
            if (str2 == "扩充传令兵成功！")
            {                
                return str2;
            }            
            return "增加失败";
        }
        //升级资源
        public string UpgradeResources(village village, string rid, AccountModel account, int logIndex)
        {
            string obj2;
            try
            {
                string str = httpHelper.Html_get(this.commonUrl.url_head + "act=resources.detailup&resourceid=" + rid + "&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(), commonUrl.url,account);
                str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
                if (str != "")
                {
                    
                }
                obj2 = str;
            }
            catch (Exception exception)
            {               
                obj2 = exception.ToString();                
            }
            return obj2;
        }


        public string BuildingCreate(village village, string bid, string btid, AccountModel account, int logIndex)
        {
            string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.construction&bid=" + bid + "&btid=" + btid + "&villageid="+village.VillageID+"&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
            if (str != "")
            { }
            return str;
        }
        public string BuildingUpgrade(village village, string bid, AccountModel account, int logIndex)
        {
            string str = httpHelper.Html_get(this.commonUrl.url_head + "act=build.upgrade&bid="+bid+"&villageid="+village.VillageID+"&rand=" + this.Rand().ToString(), commonUrl.url, account);
            str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
            if (str != "")
            { }
            return str;
        }
        public object DestroyBuilding(string bid, string villageid, AccountModel account)
        {
            return httpHelper.Html_get(this.commonUrl.url_head + "act=build.detailde&bid=" + bid + "&type=1&villageid=" + villageid + "&rand=" + this.Rand().ToString(), commonUrl.url, account);
        }

        public string doubleous(string total,string percentage)
        {
            double percent = Math.Round((float)Convert.ToInt32(percentage.Replace("%", "")) / 100, 2);
            string soldiernum = (Convert.ToInt32(total) * percent).ToString().Split(new char[] { '.' })[0];
            return soldiernum;
        }


        //public void postForm()
        //{
 
        //}

        public DateTime concact(DateTime d1, DateTime d2)
        {
            return new DateTime(d1.Year, d1.Month, d1.Day, d2.Hour, d2.Minute, d2.Second);
        }

        public string renameVillage(AccountModel account, village village, string name)
        {
            string str = httpHelper.Html_get(this.commonUrl.url_head + "act=village.rename&vname=test&userid=7858&villageid=189977&rand=" + this.Rand().ToString(), "http://" + Constant.Server_Url, account);
            str = this.getParaValue("(?<=(<locat act=.*?>)).*?(?=(</locat>))", str);
            if (str.Contains("城镇名称修改成功"))
                return str;
            return string.Empty;
        }

        
        
        //祭天
        //public int worshipheaven(string village.VillageID)
        //{
        //    int num = 0;
        //    string str = httpHelper.Html_get(this.commonUrl.url_head + "act=worship.heaven&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(),  commonUrl.url, account);
        //    str = this.getParaValue("(?<=(今日还有.*?=\\\"#FF0000\\\">)).*(?=</font></strong>次机会免费使用祭天功能)", str);
        //    if (str != "")
        //    {
        //        try
        //        {
        //            num = Convert.ToInt32(str);
        //        }
        //        catch (Exception)
        //        {
        //            num = 0;
        //        }
        //    }
        //    else
        //    {
        //        num = 0;
        //    }
        //    if (num > 6)
        //    {
        //        num = 6;
        //    }
        //    return num;
        //}

        //public void worshipheavendo(string village.VillageID, ListView listView_0, ListView listView_1, ListView listView_2)
        //{
        //    int num = this.worshipheaven(village.VillageID);
        //    //this.method_4(Convert.ToString(7), "祭天", "目前可免费祭天" + num.ToString() + "次", listView_0, null);
        //    while (num > 0)
        //    {
        //        string str = httpHelper.Html_get(this.commonUrl.url_head + "act=worship.heavendo&villageid=" + village.VillageID + "&rand=" + this.Rand().ToString(),  commonUrl.url, account);
        //        if (!str.Contains("祭天成功！"))
        //        {
        //            break;
        //        }
        //        //this.method_4(Convert.ToString(7), "祭天", "祭天" + num.ToString() + "成功", listView_0, null);
        //        num--;
        //        if ((str == "") && !this.LoginServer(listView_0, listView_1, listView_2, true, false).Contains("登录成功"))
        //        {
        //        }
        //        num = this.worshipheaven(village.VillageID);
        //    }
        //    //this.method_4(Convert.ToString(7), "祭天", "祭天完成", listView_0, null);
        //}

        public void  changeOrder(AccountModel account, int orderid, int position)
        {
            var url = "http://"+Constant.Server_Url+"/index.php?act=vmanage.setAssistant&orderid=" + orderid + "&cid=" + orderid + "&keep=all&pos=" + position;
            httpHelper.Html_get1(url, Constant.Server_Url, account);
        }
        public void repeatClick(AccountModel account, string villageId)
        {
            string url = string.Empty;
            for (var i = 0; i <= 17; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    url = "http://" + Constant.Server_Url + "/index.php?act=resources.uppertop&_uintrid=" + i + "&userid=" + account.user_id + "&villageid=" + villageId + "&w61bu=1bb0a5a&rand=753215";
                    httpHelper.Html_get1(url, Constant.Server_Url, account);
                    Thread.Sleep(20);
                }
            }
            Thread.Sleep(500);
        }
       
    }
        
}


