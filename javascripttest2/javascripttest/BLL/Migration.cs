using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using javascripttest.entity;
using System.Threading.Tasks;

namespace javascripttest.BLL
{
    public class Migration:Extra
    {
        public Migration() {
            this.commonUrl = new commonurl();
            this.httpHelper = new UrlCommand();
        }
        public Queue<string> getMigrationList(AccountModel account)
        {
            Queue<string> migrationlist = new Queue<string>();
            string url = this.commonUrl.url_head + "act=vmanage.putvshow&userid="+account.user_id;
            string migrationstr = httpHelper.Html_get(url, this.commonUrl.url, account);
            string migrators = new Regex(@"\]\]></html(.|\n|\r|\s)*?><!\[CDATA\[", RegexOptions.None).Replace(migrationstr, "");
            MatchCollection migrateCollection = new Regex("vmanage.putvillage&putvid=(?<putvid>.*?)&", RegexOptions.None).Matches(migrators);
            foreach (Match match in migrateCollection)
            {
                migrationlist.Enqueue(match.Groups["putvid"].ToString());
            }
            return migrationlist;
        }
        public bool CanMigration(AccountModel account, string x, string y)
        {
            bool migrators = false;
            string url = this.commonUrl.url_head + "act=map.detail&uitx="+x+"&uity="+y;
            string migrationstr = httpHelper.Html_get(url, this.commonUrl.url, account);
            migrators = migrationstr.Contains("放置城镇");
            return migrators;
        }

        private commonurl commonUrl { get; set; }
        private UrlCommand httpHelper { get; set; }
        public bool Migrate(AccountModel account, string x, string y, string putvid)
        {
            bool result = false;
            string url = this.commonUrl.url_head + "act=vmanage.putvillage&putvid=" + putvid + "&tox="+x+"&toy="+y+"&userid="+account.user_id;
            string migrationstr = httpHelper.Html_get(url, this.commonUrl.url, account);
            migrationstr = this.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", migrationstr);
            if (migrationstr == "放置城镇操作成功")
                result = true;
            return result;
        }

        public Queue<string> get_XY_List(int x,int y,int distance,out int nextDistance)
        {
            Queue<string> XYMix = new Queue<string>();
            int m;
            int n;
            m=x-distance;
            for(int k=y-distance;k<=y+distance;k++)
            {
                XYMix.Enqueue(m+","+k);
            }
            m=x+distance;
            for(int k=y-distance;k<=y+distance;k++)
            {
               XYMix.Enqueue(m+","+k); 
            }
            n=y-distance;
            for (int k = x - distance + 1; k <= x + distance - 1; k++)
            {
                XYMix.Enqueue(k + "," + n); 
            }
            n = y + distance;
            for (int k = x - distance + 1; k <= x + distance - 1; k++)
            {
                XYMix.Enqueue(k + "," + n);
            }
            nextDistance = distance + 1;
            return XYMix;
        }

        
        
    }
}
