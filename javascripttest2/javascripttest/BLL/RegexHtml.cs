using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace javascripttest
{
    public class RegexHtml
    {
        public RegexHtml()
        { }
        public static void getList(SGEnum.RegexType type,string html,out List<string> list)
        {
            List<string> outlist = new List<string>();
            if (type == SGEnum.RegexType.User_id)
            {
                Regex regex = new Regex("index.php\\?act=login.rolebind&user_id=(?<xxxx>.*?)\\\"", RegexOptions.IgnoreCase);
                MatchCollection matchs=regex.Matches(html);
                int length = matchs.OfType<Match>().Count();
                foreach (Match item in matchs)
                {
                    outlist.Add(item.Groups["xxxx"].Value);
                }
                list = outlist;
            }
            else
            {
                list = null;
            }          
        }
    }
}
