using cn.com.farsight.WX.WXDbVisit;
using cn.com.farsight.WX.WXModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;

namespace WXWebApplication
{
    /// <summary>
    /// WXWebApplication 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WXWebService : System.Web.Services.WebService
    {
        DataManager dm = new DataManager();
        CommandManager cm = new CommandManager();
        StatusManager sm = new StatusManager();
        TemHumManager thm = new TemHumManager();
        UserManager um = new UserManager();
        CommandLogManager clm = new CommandLogManager();
        [WebMethod]
        public string SerPut(string data)
        {
            if (data.Length == 466)
            {
                string id = data.Substring(462, 4);
                int num = int.Parse(id);
                id = num.ToString();
                string StatusTableName = "status_qrscene_" + id;
                string CommandTableName = "command_qrscene_" + id;
                if (!cm.isHasTable(CommandTableName))
                {
                    try
                    {
                        cm.CreateTable(CommandTableName);
                    }
                    catch
                    {
                        return CommandTableName;
                    }
                }
                if (!sm.isHasTable(StatusTableName))
                {
                    
                    try
                    {
                        sm.CreateTable(StatusTableName);
                    }
                    catch
                    {
                        return StatusTableName;
                    }
                    
                }
                
                string[] nodedata = new string[21];
                for (int i = 0; i < 21; i++)
                {
                    nodedata[i] = data.Substring(22 * i, 22);
                    if (nodedata[i].Substring(0, 2) != "00")
                    {
                        NodeStatus a = Data_Conversion(nodedata[i]);
                        int b = Convert.ToInt32(a.Modle, 16);
                        if (!sm.updateModel(StatusTableName, a))
                            sm.addModel(StatusTableName, a);
                    }
                }
                return "ok";


            }
            else
            {
                return "error3";
            }
        }

        [WebMethod]
        public string SerGet(string data)
        {
            int num = int.Parse(data);
            data = num.ToString();
            string CommandTableName = "command_qrscene_" + data;

            int count = 0;
            NodeCommand p = new NodeCommand();
            count = cm.GetRecordCount(CommandTableName,p);
            if (count > 0)
            {
                p = cm.GetListByPage(CommandTableName,p);
                while (p.Time.CompareTo(DateTime.Now.AddSeconds(-10)) < 0 || p.Time.CompareTo(DateTime.Now.AddMinutes(10)) > 0)
                {
                    cm.deleteModel(CommandTableName,p);
                    NodeCommand q = new NodeCommand();
                    if ((count = cm.GetRecordCount(CommandTableName,q)) > 0)
                    {
                        p = cm.GetListByPage(CommandTableName,q);
                    }
                    else
                    {
                        return null;
                    }
                }
                string a = p.Identify + p.Type + p.Modle + p.Addr + p.Data;
                a = hexStr2Str(a);
                cm.deleteModel(CommandTableName, p);
                return a;
            }
            return null;
        }
        [WebMethod]
        public string UserCheck(string name, string pwd)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                return "null";
            }
            User d = new User() { Name = name };

            if (um.getModel(d))//根据name查找数据库，把符合name的用户信息传递到变量d并返回true，否则返回false
            {
                if (d.Pwd.CompareTo(pwd) != 0)
                {
                    return "error2";
                }
                else
                {
                    um.updateModel(d);
                    return d.Id.ToString(); 
                }
            }
            else
            {
                return "error1"; ;
            }
        }
        private bool tem_hum_put(NodeStatus data)
        {
            NodeData p = new NodeData();
            while (thm.GetRecordCount(p) > 24)
            {
                thm.deleteModel(thm.GetListByPage(p, false));
            }
            p.Type = data.Type;
            p.Addr = data.Addr;
            p.Modle = data.Modle;
            p.Data = data.Data;
            p.IP = data.IP;
            p.Battery = data.Battery;
            p.Checksum = data.Checksum;
            p.Time = data.Time;
            if (thm.GetListByPage(p, false) == null)
            {
                return thm.addModel(p);
            }
            else
                return false;

        }
        private bool command_log_put(NodeCommand data)
        {
            NodeCommand p = new NodeCommand();
            while (clm.GetRecordCount(p) > 10)
            {
                clm.deleteModel(clm.GetListByPage(p));
            }
            return clm.addModel(data);
        }
        private NodeData Data_Conversion(byte[] data)
        {
            NodeData nd = new NodeData();
            nd.Type = String.Format("{0:X2}", data[0]);
            nd.Addr = String.Format("{0:X2}", data[1]) + String.Format("{0:X2}", data[2]);
            nd.Modle = String.Format("{0:X2}", data[3]);
            nd.Data = String.Format("{0:X2}", data[4]) + String.Format("{0:X2}", data[5]) + String.Format("{0:X2}", data[6]);
            nd.IP = String.Format("{0:X2}", data[7]) + String.Format("{0:X2}", data[8]);
            nd.Battery = String.Format("{0:X2}", data[9]);
            nd.Checksum = String.Format("{0:X2}", data[10]);
            nd.Time = DateTime.Now;
            return nd;
        }
        private NodeCommand Command_Conversion(byte[] data)
        {
            NodeCommand nc = new NodeCommand();
            nc.Identify = System.Text.ASCIIEncoding.Default.GetString(data, 0, 2);
            nc.Type = System.Text.ASCIIEncoding.Default.GetString(data, 2, 1);
            nc.Modle = System.Text.ASCIIEncoding.Default.GetString(data, 3, 1);
            nc.Addr = String.Format("{0:X2}", data[4]) + String.Format("{0:X2}", data[5]);
            nc.Data = System.Text.ASCIIEncoding.Default.GetString(data, 6, 1);
            nc.Time = DateTime.Now;
            return nc;
        }
        private NodeStatus Data_Conversion(string data)
        {
            NodeStatus nd = new NodeStatus();
            nd.Type = data.Substring(0, 2);
            nd.Addr = data.Substring(2, 4);
            nd.Modle = data.Substring(6, 2);
            nd.Data = data.Substring(8, 6);
            nd.IP = data.Substring(14, 4);
            nd.Battery = data.Substring(18, 2);
            nd.Checksum = data.Substring(20, 2);
            nd.Time = DateTime.Now;
            return nd;
        }
        private WSNStatus WSN_Data_Conversion(string data)
        {
            WSNStatus nd = new WSNStatus();
            nd.Identify = data.Substring(0, 2);
            nd.Type = data.Substring(2, 2);
            nd.Data = data.Substring(4, 4);
            nd.Checksum = data.Substring(8, 2);
            nd.Time = DateTime.Now;
            return nd;
        }
        private NodeCommand Command_Conversion(string data)
        {
            NodeCommand nc = new NodeCommand();
            nc.Identify = data.Substring(0, 4);
            nc.Type = data.Substring(4, 2);
            nc.Modle = data.Substring(6, 2);
            nc.Addr = data.Substring(8, 4);
            nc.Data = data.Substring(12, 2);
            nc.User = data.Substring(14, data.Length - 14);
            nc.Time = DateTime.Now;
            return nc;
        }
        private String hexStr2Str(String hexStr)
        {
            String str = "0123456789ABCDEF";
            char[] hexs = hexStr.ToCharArray();
            char[] chars = new char[hexStr.Length / 2];   
            string a=null;
            int n;

            for (int i = 0; i < chars.Length; i++)
            {
                n = str.IndexOf(hexs[2 * i]) * 16;
                n += str.IndexOf(hexs[2 * i + 1]);
                chars[i] = (char)n;  
            }
            return new String(chars);
        }     
        private void InitializeComponent()
        {

        }
    }
}
