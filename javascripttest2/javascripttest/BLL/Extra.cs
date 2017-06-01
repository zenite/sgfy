using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace javascripttest.BLL
{
    public class Extra
    {
        public Extra()
        { }
        /// <summary>
        /// 正则获取网页中对应的字段
        /// </summary>
        /// <param name="html"></param>
        /// <param name="regexPara"></param>
        /// <returns></returns>
        public string getParaValue(string regexPara, string html)
        {
            Regex regex = new Regex(regexPara, RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return regex.Match(html).ToString();
        }
    }
}
