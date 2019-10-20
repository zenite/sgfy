using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javascripttest.entity
{
    public class NodeAttack : AbstractNode
    {

        public string x { get; set; }
        public string y { get; set; }
        public string chief { get; set; }
        public string hand { get; set; }
        public string city { get; set; }
        public string Time { get; set; }
        
    }
    
    public class City : NodeAttack
    {
        public string population { get; set; }
        public string rankOfNobility { get; set; }
        public string typeOfCountry { get; set; }
        
    }
}
