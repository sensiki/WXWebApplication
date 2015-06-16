using System;
using System.Collections.Generic;
using System.Text;

namespace cn.com.farsight.WX.WXModel
{
    public class User
    {
        private int? _id;
        /// <summary>
        /// 任务唯一标识
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private String _name;
        /// <summary>
        /// 用户名
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private String _pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public String Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }
        private int? user_permisson;
        /// <summary>
        /// 用户权限
        /// </summary>
        public int? User_permisson
        {
            get { return user_permisson; }
            set { user_permisson = value; }
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
