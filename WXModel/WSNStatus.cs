using System;
using System.Collections.Generic;
using System.Text;

namespace cn.com.farsight.WX.WXModel
{
    public class WSNStatus
    {

        private String _identify;
        /// <summary>
        /// 数据头
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
        private String _data;
        /// <summary>
        /// 节点数据
        /// </summary>
        public String Data
        {
            get { return _data; }
            set { _data = value; }
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
        /// 节点时间
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }
}
