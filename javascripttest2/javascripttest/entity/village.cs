using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace javascripttest.entity
{
    public class village
    {
        public string VillageID { set; get; }
        public string VillageName { set; get; }
        public string X { set; get; }
        public string Y { set; get; }
        public string State { set; get; }
        public string Ismain { set; get; }
        public string IsStateVillage { set; get; }
        public string IsAssistant { set; get; }
        public List<Building> buildings { set; get; }
        public string bid;
        //public List<Soldier> soldiers { set; get; }
        public MyVillageSoldiers soldiers { set; get; }
        public string[] soldierss { set; get; }
        public List<resourceInfo> resources { set; get; }
        public string villageType { set; get; }
        public string attackNum { set; get; }
        public string crop_increase { set; get; }       
    }

    public class Building
    {
        public string buildingId { set; get; }
        public string buildingName { set; get; }
        public int buidinglevel { set; get; }
        public int buidingPosition { set; get; }
    }   
    public class Soldier
    {
        public string soldierId { set; get; }
        public string soldierName { set; get; }
        public int soldierNum { set; get; }        
    }

    public class resourceInfo
    {
        public SGEnum.resourceType resourceType { set; get; }
        public string resourceId { set; get; }
        public string resourceNum { set; get; }
        public string resourceName { set; get; }
    } 
}
