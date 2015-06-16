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
    public class WSNStatusManager
    {
        DBHelper dbHelper = DBHelper.getHelper();
        public bool getModel(string TableName,WSNStatus model)
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
                if (al.Tables[0].Rows[0]["identify"] != null && al.Tables[0].Rows[0]["identify"].ToString() != "")
                {
                    model.Type = al.Tables[0].Rows[0]["type"].ToString();
                }
                if (al.Tables[0].Rows[0]["type"] != null && al.Tables[0].Rows[0]["type"].ToString() != "")
                {
                    model.Type = al.Tables[0].Rows[0]["type"].ToString();
                }
                if (al.Tables[0].Rows[0]["data"] != null && al.Tables[0].Rows[0]["data"].ToString() != "")
                {
                    model.Data = al.Tables[0].Rows[0]["data"].ToString().ToString();
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
        private DbParameter[] CreateWhere(WSNStatus model, out string where)
        {
            StringBuilder sqlwhere = new StringBuilder();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            SqlParameter parameter;
            if (!string.IsNullOrEmpty(model.Identify))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" identify like @identify");
                parameter = new SqlParameter("identify", SqlDbType.NText);
                parameter.Value = model.Identify;
                parameterList.Add(parameter);
            }
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
        public bool addModel(string TableName, WSNStatus model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ");
            strSql.Append(TableName);
            strSql.Append("(identify,type,data,checksum,time) values (@identify,@type,@data,@checksum,@time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@identify", SqlDbType.NText),
                    new SqlParameter("@type", SqlDbType.NText),
                    new SqlParameter("@data", SqlDbType.NText),
                    new SqlParameter("@checksum", SqlDbType.NText),
                    new SqlParameter("@time", SqlDbType.DateTime)
            };
            parameters[0].Value = model.Identify;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.Data;
            parameters[3].Value = model.Checksum;
            parameters[4].Value = model.Time;
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
        public bool deleteModel(string TableName, WSNStatus model)
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
        public List<WSNStatus> getModelList(string TableName, WSNStatus model)
        {
            List<WSNStatus> ModelList = new List<WSNStatus>();
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
        public WSNStatus CreateModel(DataRow dr)
        {
            WSNStatus model = new WSNStatus();
            if (dr["identify"] != null && dr["identify"].ToString() != "")
            {
                model.Type = dr["identify"].ToString();
            }
            if (dr["type"] != null && dr["type"].ToString() != "")
            {
                model.Type = dr["type"].ToString();
            }
            if (dr["data"] != null && dr["data"].ToString() != "")
            {
                model.Data = dr["data"].ToString();
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
        public List<WSNStatus> getModelListAll(string TableName, WSNStatus model)
        {
            List<WSNStatus> ModelList = new List<WSNStatus>();
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
        public int GetRecordCount(string TableName,WSNStatus model)
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
        public WSNStatus GetListByPage(string TableName, WSNStatus model)
        {
            StringBuilder sql = new StringBuilder();
            List<WSNStatus> ModelList = new List<WSNStatus>();
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

        public bool updateModel(string TableName, WSNStatus model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ");
            strSql.Append(TableName+" set ");
            strSql.Append("identify=@identify,");
            strSql.Append("type=@type,");
            strSql.Append("data=@data,");
            strSql.Append("checksum=@checksum,");
            strSql.Append("time=@time");
            strSql.Append(" where type like @type");
            SqlParameter[] parameters = {
                    new SqlParameter("@identify", SqlDbType.NText),
                    new SqlParameter("@type", SqlDbType.NText),
                    new SqlParameter("@data", SqlDbType.NText),
                    new SqlParameter("@checksum", SqlDbType.NText),
                    new SqlParameter("@time", SqlDbType.DateTime)};
            parameters[0].Value = model.Identify;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.Data;
            parameters[3].Value = model.Checksum;
            parameters[4].Value = model.Time;

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
            sql.Append("identify NText,");
            sql.Append("type NText,");
            sql.Append("data NText,");
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
