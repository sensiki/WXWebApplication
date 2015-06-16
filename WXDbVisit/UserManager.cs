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
    public class UserManager
    {
        DBHelper dbHelper = DBHelper.getHelper();
        public bool getModel(User model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from user_table ");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }
            DataSet al = dbHelper.Query(sql.ToString(), parameters);
            if (al.Tables[0].Rows.Count > 0)
            {
                if (al.Tables[0].Rows[0]["_id"] != null && al.Tables[0].Rows[0]["_id"].ToString() != "")
                {
                    model.Id = int.Parse(al.Tables[0].Rows[0]["_id"].ToString());
                }
                if (al.Tables[0].Rows[0]["name"] != null && al.Tables[0].Rows[0]["name"].ToString() != "")
                {
                    model.Name = al.Tables[0].Rows[0]["name"].ToString();
                }
                if (al.Tables[0].Rows[0]["pwd"] != null && al.Tables[0].Rows[0]["pwd"].ToString() != "")
                {
                    model.Pwd = al.Tables[0].Rows[0]["pwd"].ToString().ToString();
                }
                if (al.Tables[0].Rows[0]["user_permisson"] != null && al.Tables[0].Rows[0]["user_permisson"].ToString() != "")
                {
                    model.User_permisson = int.Parse(al.Tables[0].Rows[0]["user_permisson"].ToString().ToString());
                }
                if (al.Tables[0].Rows[0]["time"] != null && al.Tables[0].Rows[0]["time"].ToString() != "")
                {
                    model.Time = Convert.ToDateTime(al.Tables[0].Rows[0]["time"]);
                }
                return true;
            }
            return false;
        }
        private DbParameter[] CreateWhere(User model, out string where)
        {
            StringBuilder sqlwhere = new StringBuilder();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            SqlParameter parameter;
            if (model.Id.HasValue)
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" _id = @_id");
                parameter = new SqlParameter("_id", SqlDbType.Int);
                parameter.Value = model.Id.Value;
                parameterList.Add(parameter);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" name like @name");
                parameter = new SqlParameter("name", SqlDbType.NText);
                parameter.Value = model.Name;
                parameterList.Add(parameter);
            }
            if (!string.IsNullOrEmpty(model.Pwd))
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" pwd like @pwd");
                parameter = new SqlParameter("pwd", SqlDbType.NText);
                parameter.Value = model.Pwd;
                parameterList.Add(parameter);
            }
            if (model.User_permisson.HasValue)
            {
                if (sqlwhere.Length > 0)
                {
                    sqlwhere.Append(" and ");
                }
                sqlwhere.Append(" user_permisson = @user_permisson");
                parameter = new SqlParameter("user_permisson", SqlDbType.Int);
                parameter.Value = model.User_permisson.Value;
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
        public bool addModel(User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_table(name,pwd,user_permisson,time) values (@name,@pwd,@user_permisson,@time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.NText),
                    new SqlParameter("@pwd", SqlDbType.NText),
                    new SqlParameter("@user_permisson", SqlDbType.Int),
                    new SqlParameter("@time", SqlDbType.DateTime)
            };

            parameters[0].Value = model.Name;
            parameters[1].Value = model.Pwd;
            parameters[2].Value = model.User_permisson;
            parameters[3].Value = model.Time;
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
        public bool deleteModel(User model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from user_table ");
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
        public List<User> getModelList(User model)
        {
            List<User> ModelList = new List<User>();
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from user_table ");
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
        public User CreateModel(DataRow dr)
        {
            User model = new User();
            if (dr["_id"] != null && dr["_id"].ToString() != "")
            {
                model.Id = int.Parse(dr["_id"].ToString());
            }
            if (dr["name"] != null && dr["name"].ToString() != "")
            {
                model.Name = dr["name"].ToString();
            }
            if (dr["pwd"] != null && dr["pwd"].ToString() != "")
            {
                model.Pwd = dr["pwd"].ToString();
            }
            if (dr["user_permisson"] != null && dr["user_permisson"].ToString() != "")
            {
                model.User_permisson = int.Parse(dr["user_permisson"].ToString());
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
        public List<User> getModelListTop(User model, int top)
        {
            List<User> ModelList = new List<User>();
            StringBuilder sql = new StringBuilder();
            sql.Append("select ");
            if (top > 0)
                sql.Append(" top " + top.ToString());
            sql.Append("* from user_table ");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }
            sql.Append(" order by _id");
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
        public int GetRecordCount(User model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(1) FROM user_table ");
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
        /// </summary>
        public List<User> GetListByPage(User model)
        {
            StringBuilder sql = new StringBuilder();
            List<User> ModelList = new List<User>();
            sql.Append("SELECT * FROM ( ");
            sql.Append(" SELECT ROW_NUMBER() OVER (");
            string where = string.Empty;
            DbParameter[] parameters = CreateWhere(model, out where);

            sql.Append("order by T._id");
            sql.Append(")AS Row, T.*  from user_table T ");
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" where " + where);
            }

            sql.Append(" ) TT");
            //sql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            DataSet dt = dbHelper.Query(sql.ToString(), parameters);
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                ModelList.Add(CreateModel(dt.Tables[0].Rows[i]));
            }
            return ModelList;
        }
        public bool updateModel(User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_table set ");
            strSql.Append("name=@name,");
            strSql.Append("pwd=@pwd,");
            strSql.Append("user_permisson=@user_permisson,");
            strSql.Append("time=@time");
            strSql.Append(" where name like @name");
            SqlParameter[] parameters = {
                    new SqlParameter("@_id", SqlDbType.Int),
                    new SqlParameter("@name", SqlDbType.NText),
                    new SqlParameter("@pwd", SqlDbType.NText),
                    new SqlParameter("@user_permisson", SqlDbType.BigInt),
                    new SqlParameter("@time", SqlDbType.DateTime)};
            parameters[0].Value = model.Id;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Pwd;
            parameters[3].Value = model.User_permisson;
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
    }
}
