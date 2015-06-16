using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;

namespace cn.com.farsight.WX.WXWebApplication
{
    public class WebResult
    {
    }
    /*
     * 以下类只用于网页与后台的数据传输载体
     */
    /// <summary>
    /// 简单json结果
    /// </summary>
    public class Simple_Result
    {
        public string result { get; set; }
        public string data { get; set; }
    }
    public class Simple_Result2
    {
        public string result { get; set; }
        public int count { get; set; }
        public string data { get; set; }
    }
  
}