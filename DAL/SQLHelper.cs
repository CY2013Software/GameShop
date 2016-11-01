using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL {

    /// <summary>
    /// SQL帮助
    /// </summary>
    public class SqlHelper {

        #region 初始化参数 public SqlConnection myConn
        public SqlConnection myConn = null;
        #endregion

        #region 构造函数
        public SqlHelper() {
            myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataString"].ConnectionString);
        }
        #endregion

        #region 重载连接字符串
        /// <summary>
        /// 重载连接字符串
        /// </summary>
        /// <param name="WebConfigConnectionStringsName"></param>
        public SqlHelper(string WebConfigConnectionStringsName) {
            if (WebConfigConnectionStringsName.ToLower().Contains("data source=")) {
                myConn = new SqlConnection(WebConfigConnectionStringsName);
            } else {
                myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[WebConfigConnectionStringsName].ConnectionString);
            }
        }
        #endregion

        #region 析构

        ~SqlHelper() {
            myConn.Close();
            myConn.Dispose();
        }
        #endregion

        #region 返回SqlDataAdapter
        /// <summary>
        /// 返回sda
        /// </summary>
        /// <param name="str">sql语句</param>
        /// <returns>SqlDataAdapter</returns>
        public SqlDataAdapter Getsda(string str) {
            return new SqlDataAdapter(str, myConn);
        }

        #endregion

        #region 执行查询，并返回查询所返回的结果集中第一行的第一列
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列
        /// </summary>
        /// <param name="sql">需要查询的SQL语句</param>
        /// <returns>返回Object类型的数据</returns>
        public object ExecuteScalar(string sql) {
            SqlCommand sqlCommand = new SqlCommand(sql, this.myConn);
            try {
                if (myConn.State == ConnectionState.Closed) myConn.Open();
                object reObj = sqlCommand.ExecuteScalar();
                myConn.Close();
                return reObj;
            } catch (Exception ex) {
                myConn.Close();
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }
        #endregion

        #region 执行查询，并返回查询所返回的结果集中第一行的第一列（SQL语句可带参数）
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列
        /// </summary>
        /// <param name="sql">需要查询的SQL语句</param>
        /// <param name="sqlParames">SQL语句中的参数</param>
        /// <returns>object类型的数据</returns>
        public object ExecuteScalar(string sql, params SqlParameter[] sqlParames) {
            SqlCommand sqlCommand = new SqlCommand(sql, this.myConn);

            try {
                if (myConn.State == ConnectionState.Closed) myConn.Open();
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.AddRange(sqlParames);
                object reObj = sqlCommand.ExecuteScalar();
                sqlCommand.Parameters.Clear();
                myConn.Close();
                return reObj;
            } catch (Exception ex) {
                myConn.Close();
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }
        #endregion

        #region 返回查询到的数据条数
        public int Count(string sql) {
            return Convert.ToInt32(ExecuteScalar(sql));
        }
        #endregion

        #region 返回查询到的数据条数（SQL语句可带参数）
        public int Count(string sql, params SqlParameter[] sqlParames) {
            return Convert.ToInt32(ExecuteScalar(sql, sqlParames));
        }
        #endregion

        #region 执行SQL语句。返回受影响行数
        public int ExecuteNonQuery(string sql) {
            SqlCommand sqlCommand = new SqlCommand(sql, this.myConn);
            try {
                if (myConn.State == ConnectionState.Closed) myConn.Open();
                int reInt = sqlCommand.ExecuteNonQuery();
                myConn.Close();
                return reInt;
            } catch (Exception ex) {
                myConn.Close();
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }
        #endregion

        #region 执行带参数的SQL语句。返回受影响行数

        public int ExecuteNonQuery(string sql, params SqlParameter[] sqlParames) {
            SqlCommand sqlCommand = new SqlCommand(sql, this.myConn);
            if (sqlParames.Length > 0) {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.AddRange(sqlParames);
            }
            try {
                if (myConn.State == ConnectionState.Closed) {
                    myConn.Open();
                }
                int reInt = sqlCommand.ExecuteNonQuery();
                myConn.Close();
                return reInt;
            } catch (Exception ex) {
                myConn.Close();
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public bool Exists(string strSql) {
            object obj = ExecuteScalar(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                cmdresult = 0;
            } else {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0) {
                return false;
            } else {
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public bool Exists(string strSql, params SqlParameter[] cmdParms) {
            object obj = ExecuteScalar(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                cmdresult = 0;
            } else {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0) {
                return false;
            } else {
                return true;
            }
        }
        #endregion

        #region 执行SQL语句。返回 DataSet
        public DataSet ExecuteDataSet(string sql) {
            SqlCommand sqlCommand = new SqlCommand(sql, this.myConn);
            DataSet retrunDataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            try {
                sqlDataAdapter.Fill(retrunDataSet);
                return retrunDataSet;
            } catch (Exception ex) {
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }
        #endregion

        #region 执行SQL语句。返回 DataTable
        public DataTable ExecuteDataTable(string sql) {
            SqlCommand sqlCommand = new SqlCommand(sql, this.myConn);
            DataTable retrunDataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            try {
                sqlDataAdapter.Fill(retrunDataTable);
                return retrunDataTable;
            } catch (Exception ex) {
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }
        #endregion

        #region 执行带参数的sql，返回一个DataSet
        /// <summary>
        /// 执行带参数的sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParames"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, params SqlParameter[] sqlParames) {
            SqlCommand cmd = new SqlCommand(sql, myConn);
            if (sqlParames.Length > 0) {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(sqlParames);
            }
            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            try {
                sda.Fill(ds);
                return ds;
            } catch (Exception ex) {
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }

        #endregion

        #region 执行带参数的sql，返回DataTable
        /// <summary>
        /// 执行带参数的sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParames"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, params SqlParameter[] sqlParames) {
            SqlCommand cmd = new SqlCommand(sql, myConn);
            if (sqlParames.Length > 0) {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(sqlParames);
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            try {
                sda.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            } catch (Exception ex) {
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }

        #endregion

        #region 执行一个带参数的存储过程，返回 DataSet
        /// <summary>
        /// 执行一个带参数的存储过程，返回 DataSet
        /// 格式 DataSet ds = ExecuteDataset("GetOrders", new SqlParameter("@prodid", 24));
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet RunProcedure(string commandText, SqlParameter[] commandParameters) {
            //创建一个命令
            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.CommandText = commandText;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = myConn;
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddRange(commandParameters);

            //创建 DataAdapter 和 DataSet
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataSet ds = new DataSet();
            try {
                da.Fill(ds);
                sqlCommand.Parameters.Clear();
                //返回结果集
                return ds;
            } catch (Exception ex) {
                string parasList = "";
                foreach (SqlParameter para in commandParameters) {
                    parasList += "<\r\n>@" + para.ParameterName + ":" + para.Value;
                }
                throw new Exception(ex + "<\r\n>" + commandText + parasList);
            }

        }
        #endregion

        #region 执行一个存储过程，不返回结果
        public void RunProcedureNoReturn(string commandText, SqlParameter[] commandParameters) {
            //创建一个命令

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = commandText;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = myConn;
            sqlCommand.Parameters.AddRange(commandParameters);
            try {
                if (myConn.State == ConnectionState.Closed) myConn.Open();
                sqlCommand.ExecuteNonQuery();
                myConn.Close();
            } catch (Exception ex) {
                myConn.Close();
                string parasList = "";
                foreach (SqlParameter para in commandParameters) {
                    parasList += "<\r\n>@" + para.ParameterName + ":" + para.Value;
                }
                throw new Exception(ex + "<\r\n>" + commandText + parasList);
            }

        }
        #endregion


        #region 以事务 执行带参数的SQL语句。返回受影响行数
        /// <summary>
        ///  以事务 执行带参数的SQL语句。返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParames"></param>
        /// <returns></returns>
        public int TransactionExecuteNonQuery(string sql, params SqlParameter[] sqlParames) {
            SqlCommand sqlCommand = new SqlCommand();
            SqlTransaction myTran;
            if (myConn.State == ConnectionState.Closed) {
                myConn.Open();
            }
            //创建一个事务
            myTran = this.myConn.BeginTransaction();
            try {
                //从此开始，基于该连接的数据操作都被认为是事务的一部分
                //下面绑定连接和事务对象
                sqlCommand.Connection = myConn;
                sqlCommand.Transaction = myTran;
                sqlCommand.CommandText = sql;
                if (sqlParames.Length > 0) {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddRange(sqlParames);
                }
                int reInt = sqlCommand.ExecuteNonQuery();
                //提交事务
                myTran.Commit();
                myConn.Close();
                return reInt;
            } catch (Exception ex) {
                myTran.Rollback();
                myConn.Close();
                throw new Exception(ex + "<\r\n>" + sql);
            }
        }
        #endregion
    }
}
