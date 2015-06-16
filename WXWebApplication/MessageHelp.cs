using cn.com.farsight.WX.WXDbVisit;
using cn.com.farsight.WX.WXModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace cn.com.farsight.WX.WXWebApplication
{
    /// <summary>
    /// 接受/发送消息帮助类
    /// </summary>
    public class messageHelp
    {
        DataManager dm = new DataManager();
        CommandManager cm = new CommandManager();
        StatusManager sm = new StatusManager();
        TemHumManager thm = new TemHumManager();
        UserManager um = new UserManager();
        WSNStatusManager wsnsm = new WSNStatusManager();
        WSNCommandManager wsncm = new WSNCommandManager();
        private string CommandTableName = null;
        private string StatusTableName = null;
        //返回消息
        public string ReturnMessage(string postStr)
        {
            string responseContent = "";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("GB2312").GetBytes(postStr)));
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (MsgType != null)
            {
                XmlNode FromUserName = MsgType.SelectSingleNode("/xml/FromUserName");
                XmlNode ToUserName = MsgType.SelectSingleNode("/xml/ToUserName");
                XmlNode Event = MsgType.SelectSingleNode("/xml/Event");
                User u = new User();
                u.Name = FromUserName.InnerText;
                if (um.getModel(u))
                {
                    StatusTableName = "status_qrscene_" + u.Pwd;
                    CommandTableName = "command_qrscene_" + u.Pwd;
                }
                else if ((!Event.InnerText.Equals("subscribe")) & (!Event.InnerText.Equals("SCAN")))
                {
                    responseContent = string.Format(ReplyType.Message_Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "未绑定任何设备！请扫码进行绑定！！！");
                    return responseContent;
                }
                switch (MsgType.InnerText)
                {
                    case "event":
                        responseContent = EventHandle(xmldoc);//事件处理
                        break;
                    case "text":
                        responseContent = TextHandle(xmldoc);//接受文本消息处理
                        break;
                    case "voice":
                        responseContent = VoiceHandle(xmldoc);//接受语音消息处理
                        break;
                    case "image":
                        responseContent = ImageHandle(xmldoc);//接受图片消息处理
                        break;
                    default:
                        break;
                }

            }
            return responseContent;
        }
        //事件
        public string EventHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            XmlNode EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (Event != null)
            {
                //菜单单击事件
                if (Event.InnerText.Equals("CLICK"))
                {
                    string a = "";
                    int Send_Result;
                    switch (EventKey.InnerText)
                    {
                        case "Light"://灯
                            if ((Send_Result = SendCommand("41", "02", 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭灯1";
                                else if (Send_Result == 3)
                                    a = "正为您改变灯1状态";
                                else
                                    a = "正为您打开灯1";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Bluetooth_Buzzer"://蜂鸣器
                            if ((Send_Result = SendCommand("42", "62", 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭蜂鸣器";
                                else if (Send_Result == 3)
                                    a = "正为您改变蜂鸣器状态";
                                else
                                    a = "正为您打开蜂鸣器";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "IPv6_Relay"://继电器
                            if ((Send_Result = SendCommand("49", "72", 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭继电器";
                                else if (Send_Result == 3)
                                    a = "正为您改变继电器状态";
                                else
                                    a = "正为您打开继电器";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;
                        case "Zigbee_Fan"://风扇
                            if ((Send_Result = SendCommand("5A", "66", 2)) != 2)
                            {
                                if (Send_Result == 0)
                                    a = "正为您关闭风扇";
                                else if (Send_Result == 3)
                                    a = "正为您改变风扇状态";
                                else
                                    a = "正为您打开风扇";
                            }
                            else
                                a = "操作失败了/::'(，节点好像断开了/:break";
                            break;

                        case "ZigBee":
                            NodeStatus ns = new NodeStatus();
                            ns.Type = "5A";
                            List<NodeStatus> list = sm.getModelListAll(StatusTableName, ns);
                            a = create_node_list(list);
                            break;
                        case "WiFi":
                            NodeStatus ns2 = new NodeStatus();
                            ns2.Type = "57";
                            List<NodeStatus> list2 = sm.getModelListAll(StatusTableName, ns2);
                            a = create_node_list(list2);
                            break;
                        case "Bluetooth":
                            NodeStatus ns3 = new NodeStatus();
                            ns3.Type = "42";
                            List<NodeStatus> list3 = sm.getModelListAll(StatusTableName, ns3);
                            a = create_node_list(list3);
                            break;
                        case "IPv6":
                           NodeStatus ns4 = new NodeStatus();
                            ns4.Type = "49";
                            List<NodeStatus> list4 = sm.getModelListAll(StatusTableName, ns4);
                            a = create_node_list(list4);
                            break;
                        case "Android":
                            NodeStatus ns5 = new NodeStatus();
                            ns5.Type = "41";
                            List<NodeStatus> list5 = sm.getModelListAll(StatusTableName, ns5);
                            a = create_node_list(list5);
                            break;
                        default:
                            a = "未知错误！";
                            break;

                    }
                    responseContent = string.Format(ReplyType.Message_Text,
                        FromUserName.InnerText,
                        ToUserName.InnerText,
                        DateTime.Now.Ticks,
                        a);

                }
                else if (Event.InnerText.Equals("subscribe"))
                {
                    User u = new User();
                    u.Name = FromUserName.InnerText;
                    string a = EventKey.InnerText;
                    if (a != null)
                        u.Pwd = a.Substring(8, a.Length - 8);
                    u.User_permisson = 1;
                    u.Time = DateTime.Now;
                    if (!um.updateModel(u))
                        um.addModel(u);
                    responseContent = string.Format(ReplyType.Message_Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "您好！感谢您的关注！很高兴为您服务,您已绑定设备：" + u.Pwd + "，请输入序号了解相关信息！\n[1]查看联系方式\n[2]查看最新资讯\n[3]官方网站");
                }
                else if (Event.InnerText.Equals("SCAN"))
                {
                    User u = new User();
                    u.Name = FromUserName.InnerText;
                    u.Pwd = EventKey.InnerText;
                    u.User_permisson = 1;
                    u.Time = DateTime.Now;
                    if (!um.updateModel(u))
                        um.addModel(u);

                    responseContent = string.Format(ReplyType.Message_Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "您已绑定设备：" + EventKey.InnerText);
                }
                else if (Event.InnerText.Equals("unsubscribe"))
                {
                    User u = new User();
                    u.Name = FromUserName.InnerText;
                    u.User_permisson = 0;
                    u.Time = DateTime.Now;
                    um.updateModel(u);
                    responseContent = string.Format(ReplyType.Message_Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "欢迎您的再次关注！");
                }

            }
            return responseContent;
        }
        //接受文本消息
        public string TextHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            if (Content != null)
            {
                StringBuilder a = new StringBuilder();
                if (Content.InnerText.IndexOf("歌") != -1)
                {
                    responseContent = string.Format(ReplyType.Message_Music,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    "旅行的意义",
                    "陈绮贞",
                    "http://tsmusic24.tc.qq.com/466727.mp3",
                    "http://tsmusic24.tc.qq.com/466727.mp3"
                    );
                }
                else if (Content.InnerText == "帮助")
                {
                    a.Append("您好！很高兴为您服务。请输入序号了解相关信息！");
                    a.Append("\n[1]查看联系方式");
                    a.Append("\n[2]查看最新资讯");
                    a.Append("\n[3]官方网站");
                }
                else if (Content.InnerText.IndexOf("笑话") != -1)
                {
                    a.Append("有个中年男子骑着摩托车飞驰在马路上，车的后面还坐着一个小男孩，由于路面高低不平车又开得快所以摩托车后面的男孩被颠簸得左晃右晃，这一切都被路边的另外一个男人看到，他心想这个骑摩托车的大人也太不负责了，这要是把小孩摔下来可咋办，于是他便开着他的车追了上去，然后拦下那辆摩托车，并斥责那个开摩托车的男子说：“有你这样开车的哇！这要是把小孩摔下来咋办！”，这时骑摩托车的男子回头惊讶的对小孩说：“儿子，你妈呢？” ");
                    a.Append("\r\n<a href=\"http://m.qiushibaike.com/\">点击进入糗事百科</a>");
                }
                else if ((Content.InnerText.IndexOf("图片") != -1) || (Content.InnerText.IndexOf("美女") != -1))
                {
                    a.Append("IdY69mE7bQJEGBA82EVsy8-3Tp8XkXJR85g4pMxyZakRiVoIyJllNWPd4wae_fg4");
                }
                else if (Content.InnerText == "1")
                {
                    a.Append("地址：北京市海淀区西三旗悦秀路北京明园大学校内 华清远见教育集团");
                    a.Append("\n高校业务洽谈电话：18600463336");
                    a.Append("\n技术支持电话：010-82600386转855/851");
                    a.Append("\n技术支持邮箱：support@farsight.com.cn");
                    a.Append("\n产品咨询QQ：752605080");
                }
                else if (Content.InnerText == "2")
                {
                    responseContent = string.Format(ReplyType.Message_News_Main,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "1",
                             string.Format(ReplyType.Message_News_Item, "FarSight Watch开源智能手表", "",
                             "http://image.baidu.com/i?tn=download&word=download&ie=utf8&fr=detail&url=http%3A%2F%2Fdev.hqyj.com%2Fproducts%2Fimages%2Fcase40.jpg&thumburl=http%3A%2F%2Fimg2.imgtn.bdimg.com%2Fit%2Fu%3D306995081%2C1400109639%26fm%3D11%26gp%3D0.jpg",
                             "http://dev.hqyj.com/products/case40.htm"));
                    return responseContent;
                }
                else if (Content.InnerText == "3")
                {
                    responseContent = string.Format(ReplyType.Message_News_Main,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTime.Now.Ticks,
                            "1",
                             string.Format(ReplyType.Message_News_Item, "华清远见研发中心", "专业始于专注 卓识源于远见",
                             "http://image.baidu.com/i?tn=download&word=download&ie=utf8&fr=detail&url=http%3A%2F%2Ff.seals.qq.com%2Ffilestore%2F10024%2Fc5%2Fb3%2F2e%2F1000%2Fpic%2FAgencyNew%2F201406%2F1402905994_702112156.jpg&thumburl=http%3A%2F%2Fimg3.imgtn.bdimg.com%2Fit%2Fu%3D3188260314%2C3062773615%26fm%3D21%26gp%3D0.jpg",
                             "http://dev.hqyj.com/"));
                    return responseContent;
                }
                else if (Content.InnerText == "4")
                {
                    a.Append("啊哈哈哈哈哈哈哈哈！！！Surprise！！！！");
                }
                else
                {
                    a.Append(Command_Control(Content.InnerText, FromUserName.InnerText));
                }
                responseContent = string.Format(ReplyType.Message_Text,
                        FromUserName.InnerText,
                        ToUserName.InnerText,
                        DateTime.Now.Ticks,
                        a);
            }
            return responseContent;
        }
        //接收语音消息
        public string VoiceHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            XmlNode Recognition = xmldoc.SelectSingleNode("/xml/Recognition");
            if (Recognition.InnerText != null)
            {
                if (Recognition.InnerText.IndexOf("歌") != -1)
                {
                    responseContent = string.Format(ReplyType.Message_Music,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    "旅行的意义",
                    "陈绮贞",
                    "http://tsmusic24.tc.qq.com/466727.mp3",
                    "http://tsmusic24.tc.qq.com/466727.mp3"
                    );
                }

                else
                {
                    string a = Command_Control(Recognition.InnerText, FromUserName.InnerText);
                    responseContent = string.Format(ReplyType.Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    a);
                }
            }
            return responseContent;
        }
        //接收图片消息
        public string ImageHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            string a = "";
            a = "IdY69mE7bQJEGBA82EVsy8-3Tp8XkXJR85g4pMxyZakRiVoIyJllNWPd4wae_fg4";
            responseContent = string.Format(ReplyType.Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    a);


            return responseContent;
        }
        //写入日志
        public void WriteLog(string text)
        {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(".") + "\\log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
        }
        /// <summary>
        /// 发送命令
        /// On_Off：0关闭，1打开，2改变状态
        /// return：0已发送关闭指令，1已发送打开命令，2节点断开连接，10节点已经是关闭状态，11节点已经是打开状态
        /// </summary>
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
                n.Modle = s.Modle;
                n.User = CommandTableName;
                n.Addr = s.Addr.Substring(2, 2) + s.Addr.Substring(0, 2); ;
                if (s.Data == "000001")//现在状态为开
                {
                    if (On_Off == 1)
                        return 11;//
                    n.Data = "30";
                    n.Time = DateTime.Now;
                    cm.addModel(CommandTableName, n);
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
        private string Command_Control(string command, string FromUserName)
        {
            string result = "抱歉，现在还无法理解您的指令（" + command + "），不过我会努力的/:,@f";//"没有听明白这句指令：" + command;
            int Send_Result;
            if (command.IndexOf("灯") != -1)
            {
                //result = Light_Control(command, FromUserName);
            }
            else if (command.IndexOf("蜂鸣器") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("", "04", 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开蜂鸣器";
                        else if (Send_Result == 3)
                            result = "正为您改变蜂鸣器状态";
                        else
                            result = "蜂鸣器为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("", "04", 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭蜂鸣器";
                        else if (Send_Result == 3)
                            result = "正为您改变蜂鸣器状态";
                        else
                            result = "蜂鸣器已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            else if (command.IndexOf("继电器") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("", "05", 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开继电器";
                        else if (Send_Result == 3)
                            result = "正为您改变继电器状态";
                        else
                            result = "继电器为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("", "05", 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭继电器";
                        else if (Send_Result == 3)
                            result = "正为您改变继电器状态";
                        else
                            result = "继电器已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            else if (command.IndexOf("风扇") != -1)
            {
                if ((command.IndexOf("开") != -1) & (command.IndexOf("关") != -1))
                    return result;
                else if (command.IndexOf("开") != -1)
                {
                    if ((Send_Result = SendCommand("", "06", 1)) != 2)
                    {
                        if (Send_Result == 1)
                            result = "正为您打开风扇";
                        else if (Send_Result == 3)
                            result = "正为您改变风扇状态";
                        else
                            result = "风扇为打开状态";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
                else if (command.IndexOf("关") != -1)
                {
                    if ((Send_Result = SendCommand("", "06", 0)) != 2)
                    {
                        if (Send_Result == 0)
                            result = "正为您关闭风扇";
                        else if (Send_Result == 3)
                            result = "正为您改变风扇状态";
                        else
                            result = "风扇已经关闭了";
                    }
                    else
                        result = "操作失败了/::'(，节点好像断开了/:break";
                }
            }
            else if (command.IndexOf("温度") != -1)
            {
                WSNStatus s = new WSNStatus();
                s.Type = "08";
                s = wsnsm.GetListByPage(StatusTableName, s);
                result = "温度：" + int.Parse(s.Data.Substring(0, 2), NumberStyles.HexNumber) + "℃" + "\n时间：" + s.Time;
            }

            else if (command.IndexOf("光强") != -1)
            {
                WSNStatus s2 = new WSNStatus();
                s2.Type = "09";
                s2 = wsnsm.GetListByPage(StatusTableName, s2);
                result = "光强：" + (int.Parse(s2.Data.Substring(2, 2), NumberStyles.HexNumber) * 127 + int.Parse(s2.Data.Substring(0, 2), NumberStyles.HexNumber)) + "lux" + "\n时间：" + s2.Time;
            }
            else if (command.IndexOf("电量") != -1)
            {
                WSNStatus s = new WSNStatus();
                s.Type = "10";
                s = wsnsm.GetListByPage(StatusTableName, s);
                result = "电量：" + int.Parse(s.Data.Substring(0, 2), NumberStyles.HexNumber) + "%" + "\n时间：" + s.Time;
            }

            return result;
        }

        private string create_node_list(List<NodeStatus> list)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            for (i = 0; i < list.Count; i++)
            {
                if(i!=0)
                    sb.Append("\n");

                int b = Convert.ToInt32(list[i].Modle, 16);
                switch (b)
                {
                    case 0x66://风扇
                        sb.Append("风扇：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x50://光电
                        sb.Append("光电：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x47://烟雾
                        sb.Append("烟雾：");
                        sb.Append(Convert.ToInt32(list[i].Data.Substring(2, 4), 16));
                        break;
                    case 0x72://继电器
                        sb.Append("继电器：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x62://蜂鸣器
                        sb.Append("蜂鸣器：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x59://光感
                        sb.Append("光感：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x41://三轴
                        sb.Append("三轴：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x49://红外
                        sb.Append("红外：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x53://超声波
                        sb.Append("超声波：");
                        sb.Append(Convert.ToInt32(list[i].Data.Substring(2, 4), 16));
                        break;
                    case 0x46://火焰
                        sb.Append("火焰：");
                        if (list[i].Data == "000001")
                            sb.Append("有");
                        else
                            sb.Append("无");
                        break;
                    case 0x56://电位器
                        sb.Append("电位器：");
                        sb.Append(Convert.ToInt32(list[i].Data.Substring(2, 4), 16));
                        break;
                    case 0x54:
                        sb.Append("温度：");
                        sb.Append(int.Parse(list[i].Data.Substring(2, 2), NumberStyles.HexNumber) + "℃");
                        if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-30)) > 0)
                        {
                            sb.Append("  online");
                        }
                        else
                            sb.Append("   offline");

                        sb.Append("\n湿度：");
                        sb.Append(int.Parse(list[i].Data.Substring(4, 2), NumberStyles.HexNumber) + "%");
                        break;
                    case 0x01://继电器
                        sb.Append("继电器：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x02://灯1
                        sb.Append("灯1：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x03://灯2
                        sb.Append("灯2：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x04://灯3
                        sb.Append("灯3：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                    case 0x05://灯4
                        sb.Append("灯4：");
                        if (list[i].Data == "000001")
                            sb.Append("开");
                        else
                            sb.Append("关");
                        break;
                }
                if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-10)) > 0)
                {
                    sb.Append("   online");
                }
                else
                    sb.Append("   offline");

            }
            return sb.ToString();
        }
    }
        
    

    //回复类型
    public class ReplyType
    {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";
            }
        }
        /// <summary>
        /// 图片消息
        /// </summary>
        public static string Message_Image
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[image]]></MsgType>
                            <Image>
                            <MediaId><![CDATA[{3}]]></MediaId>
                            </Image>
                            </xml>";
            }
        }
        public static string Message_Music
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[music]]></MsgType>
                            <Music>
                            <Title><![CDATA[{3}]]></Title>
                            <Description><![CDATA[{4}]]></Description>
                            <MusicUrl><![CDATA[{5}]]></MusicUrl>
                            <HQMusicUrl><![CDATA[{6}]]></HQMusicUrl>
                            </Music>
                            </xml>";
            }
        }
        /// <summary>
        /// 图文消息主体
        /// </summary>
        public static string Message_News_Main
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[news]]></MsgType>
                            <ArticleCount>{3}</ArticleCount>
                            <Articles>
                            {4}
                            </Articles>
                            </xml> ";
            }
        }
        /// <summary>
        /// 图文消息项
        /// </summary>
        public static string Message_News_Item
        {
            get
            {
                return @"<item>
                            <Title><![CDATA[{0}]]></Title> 
                            <Description><![CDATA[{1}]]></Description>
                            <PicUrl><![CDATA[{2}]]></PicUrl>
                            <Url><![CDATA[{3}]]></Url>
                            </item>";
            }
        }
       
    }
}
