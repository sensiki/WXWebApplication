using System;
using System.Collections.Generic;
using System.Text;

namespace cn.com.farsight.WX.WXModel
{
    public class NodeStatus
    {
        private String _type;
        /// <summary>
        /// 网络类型
        /// </summary>
        public String Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private String _addr;
        /// <summary>
        /// 节点地址
        /// </summary>
        public String Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }
        private String _modle;
        /// <summary>
        /// 节点类型
        /// </summary>
        public String Modle
        {
            get { return _modle; }
            set { _modle = value; }
        }
        private String _data;
        /// <summary>
        /// 节点数据
        /// </summary>
        public String Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private String _ip;
        /// <summary>
        /// 路由地址
        /// </summary>
        public String IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private String _battery;
        /// <summary>
        /// 节点电量
        /// </summary>
        public String Battery
        {
            get { return _battery; }
            set { _battery = value; }
        }
        private String _checksum;
        /// <summary>
        /// 节点校检
        /// </summary>
        public String Checksum
        {
            get { return _checksum; }
            set { _checksum = value; }
        }
        private DateTime _time;
        /// <summary>
        /// 节点校检
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }
}
