using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using cn.com.farsight.WX.ToolsHelper;

namespace cn.com.farsight.WX.DB.SqlDB
{
    /// <summary>
    /// 对Microsoft Sql Server 数据操作基础类
    /// </summary>
    public class DBHelper
    {
        #region 单例模式，DBHelper对象只会被创建一次

        private static DBHelper db;
        public static DBHelper getHelper()
        {
            if (db == null)
                db = new DBHelper();

            return db;
        }
        #endregion

        private void init_paramter(DbParameter[] paramters)
        {
            foreach (var item in paramters)
            {
                if (item.Value == null)
                    item.Value = DBNull.Value;
            }
        }
        //连接字符串
        string connectionString = IniHelper.ReadIni("DBHelper", "connectionString", AppDomain.CurrentDomain.BaseDirectory + "DBConfig.ini");
        /// <summary>
        /// 查询一组数据
        /// </summary>
        /// <param name="sql">要查询的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public DataSet Query(string sql, params DbParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter oda = new SqlDataAdapter(sql, connection))
                {
                    oda.SelectCommand.CommandTimeout = 5;
                    if (parameters != null && parameters.Length > 0)
                    {
                        init_paramter(parameters);
                        oda.SelectCommand.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        oda.Fill(ds, "ds");
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return ds;
        }
        /// <summary>
        /// 执行一条语句，返回受影响的行数
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public int ExecuteSql(string SQLString, params DbParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandTimeout = 5;
                        if (parameters != null && parameters.Length > 0)
                        {
                            init_paramter(parameters);
                            cmd.Parameters.AddRange(parameters);
                        }

                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否存在一条数据
        /// </summary>
        /// <param name="SQLString">查询字符串</param>
        /// <returns></returns>
        public bool Exists(string SQLString, params DbParameter[] parameters)
        {

            object obj = this.GetSinge(SQLString, parameters);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSinge(string SQLString, params DbParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandTimeout = 5;
                        if (parameters != null && parameters.Length > 0)
                        {
                            init_paramter(parameters);
                            cmd.Parameters.AddRange(parameters);
                        }
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (SqlException ex)
                    {

                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

    }
}
