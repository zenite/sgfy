using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using javascripttest.Regular;
using System.Threading.Tasks;

namespace javascripttest.BLL
{
    public class HandlerAttack
    {
        private string RootDirPath;
        private string[] CreateXml ;
        private AttackSetConfig setConfig;
        public HandlerAttack(string dir,string rootname, string[] nodeList)
        {
            setConfig = new AttackSetConfig(dir, rootname, nodeList);
        }
        /// <summary>
        /// 设置控件节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="parentNode"></param>
        /// <param name="sonName"></param>
        public void ControlAttribute(object sender, string parentNode, string sonName, string VillageId)
        {
            setConfig.setAttribute(sender,parentNode, sonName, VillageId);
        }
        public void ControlAttribute1(object sender,  string parentNode, string sonName, string VillageId)
        {
            setConfig.setAttribute(sender, parentNode, sonName, VillageId);
        }
       /// <summary>
       /// 设置攻击图节点
       /// </summary>
       /// <param name="ds"></param>
       /// <param name="parentNode"></param>
       /// <param name="sonName"></param>
        public void AttackAttribute(DataSet ds, string parentNode, string sonName)
        {
            setConfig.setAttribute(ds, parentNode, sonName);
        }
        public List<entity.Node> getControlXml(string VillageId)
        {
            return setConfig.getControlXml(VillageId);
        }

        public List<entity.NodeAttack> getAttackXml(string VillageId)
        {
            return setConfig.getAttackXml(VillageId);
        }
        public void removeNode(string x, string y, string VillageId)
        {
            setConfig.removeNode(x, y, VillageId);
        }

        public void updateTime(string x, string y, string VillageId)
        {
            setConfig.updateTime(x, y, VillageId);
        }
        public void setAttribute(entity.NodeAttack node, string parentNode)
        {
            setConfig.setAttribute(node, parentNode);
        }
    }

}
