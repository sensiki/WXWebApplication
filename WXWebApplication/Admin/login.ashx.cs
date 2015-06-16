using System;
using System.Web;
using System.Web.SessionState;
using cn.com.farsight.WX.WXModel;
using cn.com.farsight.WX.WXDbVisit;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace cn.com.farsight.WX.WXWebApplication.Admin
{
    /// <summary>
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler, IRequiresSessionState
    {
        DataManager dm = new DataManager();
        CommandManager cm = new CommandManager();
        StatusManager sm = new StatusManager();
        TemHumManager thm = new TemHumManager();
        UserManager um = new UserManager();
        CommandLogManager clm = new CommandLogManager();

        private string CommandTableName = null;
        private string StatusTableName = null;

        public void ProcessRequest(HttpContext context)
        {
            StatusTableName = "status_qrscene_1111";
            CommandTableName = "command_qrscene_1111";
            SendCommand("41", "02", 0);

            bool isLogin = !string.IsNullOrEmpty(context.Request["login"]);
            if (isLogin)
            {
                context.Response.ContentType = "text/plain";
                Simple_Result lresult = new Simple_Result();
                string name = context.Request["name"];
                string pwd = context.Request["pwd"];
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
                {
                    lresult.result = "false";
                    lresult.data = "用户名或密码不能为空(>_<)";
                    context.Response.Write(JsonConvert.SerializeObject(lresult));
                    return;
                }
                User d = new User() { Name = name, Pwd = pwd };
                if (um.getModel(d))
                {
                    if (d.Pwd.CompareTo(pwd) != 0)
                    {
                        lresult.result = "false";
                        lresult.data = "用户名不存在或密码错误!(>_<)";
                    }
                    else
                    {
                        lresult.result = "true";
                        context.Session["LoginUserPermisson"] = d.User_permisson;
                        context.Session["LoginUserName"] = d.Name;
                        d.Time = DateTime.Now;
                        um.updateModel(d);
                        if (d.Name == "aumin")
                            lresult.data = "../Admin/admin.ashx";
                        else
                            lresult.data = "../Admin/admin.ashx";
                    }
                }
                else
                {
                    lresult.result = "false";
                    lresult.data = "用户名不存在或密码错误!(>_<)";
                }
                context.Response.Write(JsonConvert.SerializeObject(lresult));
            }
            else
            {
                context.Response.ContentType = "text/html";
                StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "html/login.html");
                string html = reader.ReadToEnd();
                reader.Close();
                context.Response.Write(html);
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private int SendCommand(string type, string modle, int On_Off)
        {
            NodeStatus s = new NodeStatus();
            s.Type = type;
            s.Modle = modle;
            s = sm.GetListByPage(StatusTableName, s);
            if (s != null && s.Time.CompareTo(DateTime.Now.AddSeconds(-10)) >= 0 && s.Time.CompareTo(DateTime.Now.AddSeconds(10)) <= 0)
            {
                byte[] array = new byte[1];
                NodeCommand n = new NodeCommand();
                n.Identify = "2343";//#C
                n.Type = s.Type;
                n.Modle =s.Modle;
                n.User = CommandTableName;
                n.Addr = s.Addr.Substring(2, 2) + s.Addr.Substring(0, 2); ;
                if (s.Data == "000001")//现在状态为开
                {
                    if (On_Off == 1)
                        return 11;//
                    n.Data = "30";
                    n.Time = DateTime.Now;
                    cm.addModel(StatusTableName, n);
                    return 0;
                }
                else
                {
                    if (On_Off == 0)
                        return 10;
                    n.Data = "31";
                    n.Time = DateTime.Now;
                    cm.addModel(CommandTableName, n);
                    return 1;
                }

            }
            else
            {
                return 2;
            }
        }
    }
}