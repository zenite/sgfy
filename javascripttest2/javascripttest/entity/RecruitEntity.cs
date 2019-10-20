using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javascripttest.entity
{
    public class RecruitEntity
    {
        public AccountModel account { get; set; }
        public int nCountry { get; set; }
        public List<string> recruite { get; set; }
        public int soldierAmount { get; set; }
        public string cityFilter { get; set; }
    }
}
