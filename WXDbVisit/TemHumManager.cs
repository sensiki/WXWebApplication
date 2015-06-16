using cn.com.farsight.WX.DB.SqlDB;
using cn.com.farsight.WX.WXModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace cn.com.farsight.WX.WXDbVisit
{
    public class TemHumManager
    {
        DBHelper dbHelper = DBHelper.getHelper();
        public bool getModel(NodeData model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from tem_hum_table ");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }
            DataSet al = dbHelper.Query(sql.ToString(), parameters);
            if (al.Tables[0].Rows.Count > 0)
            {

                if (al.Tables[0].Rows[0]["type"] != null && al.Tables[0].Rows[0]["type"].ToString() != "")
                {
                    model.Type = al.Tables[0].Rows[0]["type"].ToString();
                }
                if (al.Tables[0].Rows[0]["addr"] != null && al.Tables[0].Rows[0]["addr"].ToString() != "")
                {
                    model.Addr = al.Tables[0].Rows[0]["addr"].ToString().ToString();
                }
                if (al.Tables[0].Rows[0]["modle"] != null && al.Tables[0].Rows[0]["modle"].ToString() != "")
                {
                    model.Modle = al.Tables[0].Rows[0]["modle"].ToString().ToString();
                }
                if (al.Tables[0].Rows[0]["data"] != null && al.Tables[0].Rows[0]["data"].ToString() != "")
                {
                    model.Data = al.Tables[0].Rows[0]["data"].ToString().ToString();
                }
                if (al.Tables[0].Rows[0]["ip"] != null && al.Tables[0].Rows[0]["ip"].ToString() != "")
                {
                    model.IP = al.Tables[0].Rows[0]["ip"].ToString().ToString();
                }
                if (al.Tables[0].Rows[0]["battery"] != null && al.Tables[0].Rows[0]["battery"].ToString() != "")
                {
                    model.Battery = al.Tables[0].Rows[0]["battery"].ToString().ToString();
                }
                if (al.Tables[0].Rows[0]["checksum"] != null && al.Tables[0].Rows[0]["battery"].ToString() != "")
                {
                    model.Checksum = al.Tables[0].Rows[0]["checksum"].ToString().ToString();
                }
                if (al.Tables[0].Rows[0]["time"] != null && al.Tables[0].Rows[0]["time"].ToString() != "")
                {
                    model.Time = Convert.ToDateTime(al.Tables[0].Rows[0]["time"]);
                }
                return true;
            }
            return false;
        }
        private DbParameter[] CreateWhere(NodeData model, out string where)
        {
            StringBuilder sqlwhere = new StringBuilder();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            SqlParameter parameter;
            if (!string.IsNullOrEmpty(model.Type))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" type like @type");
                parameter = new SqlParameter("type", SqlDbType.NText);
                parameter.Value = model.Type;
                parameterList.Add(parameter);
            }
            if (!string.IsNullOrEmpty(model.Addr))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" addr like @addr");
                parameter = new SqlParameter("addr", SqlDbType.NText);
                parameter.Value = model.Addr;
                parameterList.Add(parameter);
            }
            if (!string.IsNullOrEmpty(model.Modle))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" modle like @modle");
                parameter = new SqlParameter("modle", SqlDbType.NText);
                parameter.Value = model.Modle;
                parameterList.Add(parameter);
            }
            //if (!string.IsNullOrEmpty(model.Data))
            //{
            //    if (sqlwhere.Length > 0)
            //    {
            //        sqlwhere.Append(" and ");
            //    }
            //    sqlwhere.Append(" data like @data");
            //    parameter = new SqlParameter("data", SqlDbType.NText);
            //    parameter.Value = model.Data;
            //    parameterList.Add(parameter);
            //}
            if (!string.IsNullOrEmpty(model.IP))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" ip like @ip");
                parameter = new SqlParameter("ip", SqlDbType.NText);
                parameter.Value = model.IP;
                parameterList.Add(parameter);
            }
            //if (!string.IsNullOrEmpty(model.Battery))
            //{
            //    if (sqlwhere.Length > 0)
            //    {
            //        sqlwhere.Append(" and ");
            //    }
            //    sqlwhere.Append(" battery like @battery");
            //    parameter = new SqlParameter("battery", SqlDbType.NText);
            //    parameter.Value = model.Battery;
            //    parameterList.Add(parameter);
            //}
            //if (!string.IsNullOrEmpty(model.Checksum))
            //{
            //    if (sqlwhere.Length > 0)
            //    {
            //        sqlwhere.Append(" and ");
            //    }
            //    sqlwhere.Append(" checksum like @checksum");
            //    parameter = new SqlParameter("checksum", SqlDbType.NText);
            //    parameter.Value = model.Checksum;
            //    parameterList.Add(parameter);
            //}
            if (model.Time.CompareTo(DateTime.Now.AddDays(-1)) >= 0 && model.Time.CompareTo(DateTime.Now.AddDays(1)) <= 0)
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" time like @time");
                parameter = new SqlParameter("time", SqlDbType.DateTime);
                parameter.Value = model.Time;
                parameterList.Add(parameter);
            }
            where = sqlwhere.ToString();
            return parameterList.ToArray();
        }
        public bool addModel(NodeData model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tem_hum_table(type,addr,modle,data,ip,battery,checksum,time) values (@type,@addr,@modle,@data,@ip,@battery,@checksum,@time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@type", SqlDbType.NText),
                    new SqlParameter("@addr", SqlDbType.NText),
                    new SqlParameter("@modle", SqlDbType.NText),
                    new SqlParameter("@data", SqlDbType.NText),
                    new SqlParameter("@ip", SqlDbType.NText),
                    new SqlParameter("@battery", SqlDbType.NText),
                    new SqlParameter("@checksum", SqlDbType.NText),
                    new SqlParameter("@time", SqlDbType.DateTime),
            };
            parameters[0].Value = model.Type;
            parameters[1].Value = model.Addr;
            parameters[2].Value = model.Modle;
            parameters[3].Value = model.Data;
            parameters[4].Value = model.IP;
            parameters[5].Value = model.Battery;
            parameters[6].Value = model.Checksum;
            parameters[7].Value = model.Time;
            int rows = dbHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deleteModel(NodeData model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from tem_hum_table ");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }
            int cout = dbHelper.ExecuteSql(sql.ToString(), parameters);
            if (cout > 0)
            {
                return true;
            }
            return false;
        }
        public List<NodeData> getModelList(NodeData model)
        {
            List<NodeData> ModelList = new List<NodeData>();
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from doctor_table ");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }
            DataSet dt = dbHelper.Query(sql.ToString(), parameters);
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                ModelList.Add(CreateModel(dt.Tables[0].Rows[i]));
            }
            return ModelList;
        }
        /// <summary>
        /// 根据DataRow创建对象
        /// </summary>
        /// <returns></returns>
        public NodeData CreateModel(DataRow dr)
        {
            NodeData model = new NodeData();
            if (dr["type"] != null && dr["type"].ToString() != "")
            {
                model.Type = dr["type"].ToString();
            }
            if (dr["addr"] != null && dr["addr"].ToString() != "")
            {
                model.Addr = dr["addr"].ToString();
            }
            if (dr["modle"] != null && dr["modle"].ToString() != "")
            {
                model.Modle = dr["modle"].ToString();
            }
            if (dr["data"] != null && dr["data"].ToString() != "")
            {
                model.Data = dr["data"].ToString();
            }
            if (dr["ip"] != null && dr["ip"].ToString() != "")
            {
                model.IP = dr["ip"].ToString();
            }
            if (dr["battery"] != null && dr["battery"].ToString() != "")
            {
                model.Battery = dr["battery"].ToString();
            }
            if (dr["checksum"] != null && dr["checksum"].ToString() != "")
            {
                model.Checksum = dr["checksum"].ToString();
            }
            //if (Convert.ToDateTime(dr["time"]).CompareTo(DateTime.Now.AddDays(-1)) >= 0 && Convert.ToDateTime(dr["time"]).CompareTo(DateTime.Now.AddDays(1)) <= 0)
            if (dr["time"] != null && dr["time"].ToString() != "")
            {
                model.Time = Convert.ToDateTime(dr["time"]);

            }
            return model;
        }
        /// <summary>
        /// 查询一定数量的数据
        /// </summary>
        /// <returns></returns>
        public List<NodeData> getModelListTop(NodeData model, int top)
        {
            List<NodeData> ModelList = new List<NodeData>();
            StringBuilder sql = new StringBuilder();
            sql.Append("select ");
            //if (top > 0)
            //    sql.Append(" top " + top.ToString());
            sql.Append("* from tem_hum_table ");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }
            sql.Append(" order by time");
            DataSet dt = dbHelper.Query(sql.ToString(), parameters);
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                ModelList.Add(CreateModel(dt.Tables[0].Rows[i]));
            }
            return ModelList;
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(NodeData model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*) FROM tem_hum_table ");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }
            object obj = dbHelper.GetSinge(sql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// NewOrOld:1为最新，0为最旧
        /// </summary>
        public NodeData GetListByPage(NodeData model, bool NewOrOld)
        {
            StringBuilder sql = new StringBuilder();
            List<NodeData> ModelList = new List<NodeData>();
            sql.Append("SELECT * FROM ( ");
            sql.Append(" SELECT ROW_NUMBER() OVER (");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);

            sql.Append("order by T.time desc");
            sql.Append(")AS Row, T.*  from tem_hum_table T ");
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }

            sql.Append(" ) TT");
            DataSet dt = dbHelper.Query(sql.ToString(), parameters);
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (NewOrOld)
                    return CreateModel(dt.Tables[0].Rows[0]);
                else
                    return CreateModel(dt.Tables[0].Rows[dt.Tables[0].Rows.Count - 1]);
            }
            return null;
        }
    }
}
