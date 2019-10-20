using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javascripttest
{
    public class SGEnum
    {
        [Flags]
        public enum RegexType
        {
            User_id=1,
        }

        public enum resourceType
        {
            crop = 1,//粮食
            lumber=2,//木材
            clay=3,//石料 
            iron=4//铁
        }

        public enum wei
        {

           ch_piaodao=0, //朴刀 0
            ch_zhongbu=1,//重步 1
            ch_jinweibing=2,//近卫 2
            ch_qingqi=3,//轻骑 3
            ch_qingzhou=4,//青州 4
            ch_chongche=5,//冲车 5
            ch_pili=6,//霹雳 6
            ch_shuike=7,//水客 7
            ch_kenhuang=8,//垦荒 8
            ch_chihou=9,//斥候 9
            ch_changgong=10,//长弓 10
            ch_gongqi=11,//弓骑 11
            ch_tengjia=12,//藤甲 12
            ch_xiangbing=13,//象兵 13
            ch_wuwan=14,//乌丸 14 
            ch_baier=15//白耳 15
        }
        public enum wu
        {
            ch_piaodao = 0, //朴刀 0
            ch_zhongbu = 1,//重步 1
            ch_jinweibing = 2,//近卫 2
            ch_qingqi = 3,//轻骑 3
            ch_qingzhou = 4,//青州 4
            ch_chongche = 5,//冲车 5
            ch_pili = 6,//霹雳 6
            ch_shuike = 7,//水客 7
            ch_kenhuang = 8,//垦荒 8
            ch_chihou = 9,//斥候 9
            ch_changgong = 10,//长弓 10
            ch_gongqi = 11,//弓骑 11
            ch_tengjia = 12,//藤甲 12
            ch_xiangbing = 13,//象兵 13
            ch_wuwan = 14,//乌丸 14 
            ch_baier = 15//白耳 15
        }
        public enum shu
        {
            ch_piaodao = 0, //朴刀 0
            ch_zhongbu = 1,//重步 1
            ch_jinweibing = 2,//近卫 2
            ch_qingqi = 3,//轻骑 3
            ch_qingzhou = 4,//青州 4
            ch_chongche = 5,//冲车 5
            ch_pili = 6,//霹雳 6
            ch_shuike = 7,//水客 7
            ch_kenhuang = 8,//垦荒 8
            ch_chihou = 9,//斥候 9
            ch_changgong = 10,//长弓 10
            ch_gongqi = 11,//弓骑 11
            ch_tengjia = 12,//藤甲 12
            ch_xiangbing = 13,//象兵 13
            ch_wuwan = 14,//乌丸 14 
            ch_baier = 15//白耳 15
        }

        public enum btid
        {
            //仓库
            //粮仓
            //暗仓
            //藏兵洞
            //集市
            //冶铁监
            //兵器司
            //学馆
            //虎贲营
            //盟旗
            //歌舞坊
            //督造司
            //马场
            //斥候营
            //兵舍
            //工匠坊
            //别院
            //中军帐
            //校场
            //城墙
            //箭塔
            //
            //
            //http://h92.sg.kunlun.com/index.php?act=build.act&do=main&btid=9&k0a695s=629c4af20bb&userid=385&villageid=598&w4f7u=f7b9c6b&rand=267359
            //
        }
    }
}





