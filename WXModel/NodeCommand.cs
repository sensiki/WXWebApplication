using System;
using System.Collections.Generic;
using System.Text;

namespace cn.com.farsight.WX.WXModel
{
    public class NodeCommand
    {
        private String _identify;
        /// <summary>
        /// 标识
        /// </summary>
        public String Identify
        {
            get { return _identify; }
            set { _identify = value; }
        }
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
        /// 节点命令
        /// </summary>
        public String Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private String _user;
        /// <summary>
        /// 节点命令
        /// </summary>
        public String User
        {
            get { return _user; }
            set { _user = value; }
        }
        private DateTime _time;
        /// <summary>
        /// 节点命令
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }
}
