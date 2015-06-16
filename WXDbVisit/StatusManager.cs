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
    public class StatusManager
    {
        DBHelper dbHelper = DBHelper.getHelper();
        public bool getModel(string TableName,NodeStatus model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from ");
            sql.Append(TableName+" ");
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
        private DbParameter[] CreateWhere(NodeStatus model, out string where)
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
            if (!string.IsNullOrEmpty(model.Data))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" data like @data");
                parameter = new SqlParameter("data", SqlDbType.NText);
                parameter.Value = model.Data;
                parameterList.Add(parameter);
            }
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
            if (!string.IsNullOrEmpty(model.Battery))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" battery like @battery");
                parameter = new SqlParameter("battery", SqlDbType.NText);
                parameter.Value = model.Battery;
                parameterList.Add(parameter);
            }
            if (!string.IsNullOrEmpty(model.Checksum))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" checksum like @checksum");
                parameter = new SqlParameter("checksum", SqlDbType.NText);
                parameter.Value = model.Checksum;
                parameterList.Add(parameter);
            }
            if (model.Time.CompareTo(DateTime.Now.AddDays(-1)) >= 0 && model.Time.CompareTo(DateTime.Now.AddDays(1)) <= 0)
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" time = @time");
                parameter = new SqlParameter("time", SqlDbType.DateTime);
                parameter.Value = model.Time;
                parameterList.Add(parameter);
            }
            where = sqlwhere.ToString();
            return parameterList.ToArray();
        }
        public bool addModel(string TableName,NodeStatus model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ");
            strSql.Append(TableName);
            strSql.Append("(type,addr,modle,data,ip,battery,checksum,time) values (@type,@addr,@modle,@data,@ip,@battery,@checksum,@time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@type", SqlDbType.NText),
                    new SqlParameter("@addr", SqlDbType.NText),
                    new SqlParameter("@modle", SqlDbType.NText),
                    new SqlParameter("@data", SqlDbType.NText),
                    new SqlParameter("@ip", SqlDbType.NText),
                    new SqlParameter("@battery", SqlDbType.NText),
                    new SqlParameter("@checksum", SqlDbType.NText),
                    new SqlParameter("@time", SqlDbType.DateTime)
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
        public bool deleteModel(string TableName,NodeStatus model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from ");
            sql.Append(TableName+" ");
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
        public List<NodeStatus> getModelList(string TableName,NodeStatus model)
        {
            List<NodeStatus> ModelList = new List<NodeStatus>();
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from ");
            sql.Append(TableName+" ");
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
        public NodeStatus CreateModel(DataRow dr)
        {
            NodeStatus model = new NodeStatus();
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
        public List<NodeStatus> getModelListAll(string TableName,NodeStatus model)
        {
            List<NodeStatus> ModelList = new List<NodeStatus>();
            StringBuilder sql = new StringBuilder();
            sql.Append("select ");
            //if (top > 0)
            //    sql.Append(" top " + top.ToString());
            sql.Append("* from ");
            sql.Append(TableName+" ");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }
            //sql.Append(" order by time");
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
        public int GetRecordCount(string TableName,NodeStatus model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*) FROM ");
            sql.Append(TableName+" ");
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
        /// 获取数据列表
        /// </summary>
        public NodeStatus GetListByPage(string TableName,NodeStatus model)
        {
            StringBuilder sql = new StringBuilder();
            List<NodeStatus> ModelList = new List<NodeStatus>();
            sql.Append("SELECT * FROM ( ");
            sql.Append(" SELECT ROW_NUMBER() OVER (");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);

            sql.Append("order by T.time desc");
            sql.Append(")AS Row, T.*  from ");
            sql.Append(TableName+" T ");
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }

            sql.Append(" ) TT");
            DataSet dt = dbHelper.Query(sql.ToString(), parameters);
            if (dt.Tables[0].Rows.Count > 0)
            {
                    return CreateModel(dt.Tables[0].Rows[0]);
            }
            return null;
        }

        public bool updateModel(string TableName,NodeStatus model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ");
            strSql.Append(TableName+" set ");
            strSql.Append("type=@type,");
            strSql.Append("addr=@addr,");
            strSql.Append("modle=@modle,");
            strSql.Append("data=@data,");
            strSql.Append("ip=@ip,");
            strSql.Append("battery=@battery,");
            strSql.Append("checksum=@checksum,");
            strSql.Append("time=@time");
            strSql.Append(" where modle like @modle");
            SqlParameter[] parameters = {
                    new SqlParameter("@type", SqlDbType.NText),
                    new SqlParameter("@addr", SqlDbType.NText),
                    new SqlParameter("@modle", SqlDbType.NText),
                    new SqlParameter("@data", SqlDbType.NText),
                    new SqlParameter("@ip", SqlDbType.NText),
                    new SqlParameter("@battery", SqlDbType.NText),
                    new SqlParameter("@checksum", SqlDbType.NText),
                    new SqlParameter("@time", SqlDbType.DateTime)};
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
        public bool CreateTable(string TabeName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("CREATE TABLE ");
            sql.Append(TabeName);
            sql.Append("(");
            sql.Append("type NText,");
            sql.Append("addr NText,");
            sql.Append("modle NText,");
            sql.Append("data NText,");
            sql.Append("ip NText,");
            sql.Append("battery NText,");
            sql.Append("checksum NText,");
            sql.Append("time DateTime");
            sql.Append(")");
            int rows = dbHelper.ExecuteSql(sql.ToString(), null);
            if (rows == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isHasTable(string TabeName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(1) from sys.objects where name = '");
            sql.Append(TabeName);
            sql.Append("'");
            return  dbHelper.Exists(sql.ToString(), null);
        }
    }
}
