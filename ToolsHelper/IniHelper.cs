using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace cn.com.farsight.WX.ToolsHelper
{
    /// <summary>
    /// 对Init文件进行读写操作
    /// </summary>
    public class IniHelper
    {
        #region "内部操作"

        /// <summary>        
        /// 写入INI文件        
        /// </summary>        
        /// <param name="section">节点名称[如[TypeName]]</param>        
        /// <param name="key">键</param>        
        /// <param name="val">值</param>        
        /// <param name="filepath">文件路径</param>        
        /// <returns></returns>        
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>        
        /// 读取INI文件        
        /// </summary>        
        /// <param name="section">节点名称</param>        
        /// <param name="key">键</param>        
        /// <param name="def">值</param>        
        /// <param name="retval">stringbulider对象</param>        
        /// <param name="size">字节大小</param>        
        /// <param name="filePath">文件路径</param>        
        /// <returns></returns>        
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        #endregion

        /// <summary>
        /// 自定义读取INI文件内容
        /// </summary>
        /// <param name="Section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="path">Ini文件路径</param>
        /// <returns></returns>
        public static string ReadIni(string Section, string key, string path)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, path);
            return temp.ToString();
        }
        /// <summary>
        /// 写入Ini文件
        /// </summary>
        /// <param name="Section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="path">文件路径</param>
        public static long WriteIni(string Section, string key, string value, string path)
        {
            long id = WritePrivateProfileString(Section, key, value, path);
            return id;
        }
    }
}
