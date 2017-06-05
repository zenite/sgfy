using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javascripttest.entity
{
    public  class Battle
    {
        public string x{get;set;}
        public string y{get;set;}
        public string battlearray{get;set;}
        public string bid{get;set;}
        public string btid{get;set;}
        public string general1{get;set;}
        public string general2{get;set;}
        public string general3{get;set;}
        public string general4{get;set;}
        public string general5{get;set;}
        public string[] soldier{get;set;}//兵力种类
        public string target_general_level{get;set;}
        public int  type{get;set;}//战斗类型
        public string userid{get;set;}
        public string villageid{get;set;}

        //出兵模式
        public int Pattern{get;set;}
        public string DefensePattern { get; set; }
        public string AttackPattern { get; set; }
        public int HowPercent { get; set; }
        public string BattleNum{get;set;}
        public int CitySelected{get;set;}
        public string directType{get;set;}
        public DateTime ArriveTime{get;set;}

        public List<target> targets { get; set; }
        public string attackBuilding { get; set; }

        public string attackRepeatTime { get; set; }
        public int defenseUpperAmount { get; set; }
        public int defenseLowerAmount { get; set; }


        //攻击连接车段字符串
        public string ArrayChe { get; set; }
    
        //http://h92.sg.kunlun.com/index.php?act=build.act&do=start_war&btid=9&k0a695s=629c4af20bb&start=1&target_general_level=0&type=0&battlearray=0&general1=20825&general2=33829&general3=49563&general4=27552&general5=60342&soldier[0]=1&soldier[2]=2&soldier[4]=3&soldier[5]=4&soldier[6]=5&soldier[9]=6&soldier[11]=7&target[0]=109&x=624&y=923&keep=all&userid=385&villageid=598&w4f7u=f7b9c6b&rand=267279
        //http://h92.sg.kunlun.com/index.php?act=build.act&btid=18&do=trade18_0&k0a695s=629c4af20bb&userid=385&villageid=7351&w4f7u=f7b9c6b&rand=267304
    }



    public class target
    {
        public string x { set; get; }
        public string y { set; get; }
    }
    //运输类
    public class cargo
    {
        public int Trader_Resource_Clay;
        public int Trader_Resource_Crop;
        public int Trader_Resource_Iron;
        public int Trader_Resource_Lumber;
        public string Trader_Target_X;
        public string Trader_Target_Y;
        public string restrictTime;
    }

    public class MyVillageSoldiers
    {
        public string soldier0 { get; set; }
        public string soldier1 { get; set; }
        public string soldier2 { get; set; }
        public string soldier3 { get; set; }
        public string soldier4 { get; set; }
        public string soldier5 { get; set; }
        public string soldier6 { get; set; }
        public string soldier7 { get; set; }
        public string soldier8 { get; set; }
        public string soldier9 { get; set; }
        public string soldier10 { get; set; }
        public string soldier11 { get; set; }
        public string soldier12 { get; set; }
        public string soldier13 { get; set; }
        public string soldier14 { get; set; }
        public string soldier15 { get; set; }
        public string soldier16 { get; set; }
        public string MainGeneral { get; set; }
       
    }

    public class Attacker
    {
        public string name { get; set; }
        public string type { get; set; }
        public System.DateTime ArrivalTime { get; set; }
        public string x { get; set; }
        public string y { get; set; }
    }
}
