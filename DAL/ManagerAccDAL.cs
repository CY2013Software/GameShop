using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shop.Models;
using System.Net;
using System.Data;

namespace DAL
{
    public class ManagerAccDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //判断登录状态
        public bool IsLogin()
        {
            //Cookie["ManagerAccount"]

            return false;
        }


        //登录验证
        public int LoginRes(ManagerAcc _managerAcc)
        {
            int rows = 0;
            string managerStatus = "Select manager_status from Manager_Acc where manager_acc = '" + _managerAcc.ManagerAccount + "'";
            string pwdSql = "Select count(1) from Manager_Acc where manager_acc = '" + _managerAcc.ManagerAccount + "' and manager_pwd= '" + _managerAcc.ManagerPwd + "' ";
            string initialPwdSql = "Select initialPwd from OrdinaryManager_InitialPwd";
            //System.Data.SqlClient.SqlParameter[] _params = new System.Data.SqlClient.SqlParameter[]{
            //new System.Data.SqlClient.SqlParameter("@manager_acc",_managerAcc.ManagerAccount),
            //new System.Data.SqlClient.SqlParameter("@manager_pwd",_managerAcc.ManagerPwd)
            //};

            
            if (_sqlHelper.Count(pwdSql) != 0){
            DataTable statusTable = _sqlHelper.ExecuteDataTable(managerStatus);
            DataRow statusItem = statusTable.Rows[0];

            //锁定状态返回0
            if ((bool)statusItem["manager_status"] == true){
                return rows;
            }

                DataTable initialPwdTable = _sqlHelper.ExecuteDataTable(initialPwdSql);
                DataRow item = initialPwdTable.Rows[0];
                //登录成功但未更改默认密码，返回2
                if (_managerAcc.ManagerPwd == item["initialPwd"].ToString())
                {
                    rows = 2;
                }
                else 
                {
                    bool managerType = new ManagerAccDAL ().ManagerType(_managerAcc.ManagerAccount);
                    if(managerType == false){
                    //普通管理员登录成功，返回3
                    rows = 3;
                    }else{
                        //系统管理员登录成功，返回4
                        rows = 4;
                    }
                }
            }
            else {
                //登录失败则返回1（用户名或密码错误）
                rows = 1;
            }
            
            return rows;
        }

        //查找管理员类型
        public bool ManagerType(string _managerAccount) {
            string managerType = "Select manager_type from Manager_Acc where manager_acc = '" + _managerAccount + "'";
            DataTable managerTypeTable = _sqlHelper.ExecuteDataTable(managerType);
            DataRow item = managerTypeTable.Rows[0];
            return (bool)item["manager_type"];
        }

        //添加普通管理员
        public int ManagerAdd(ManagerAcc _managerAcc) {
            int rows = 0;                        //账户名已存在则返回0
            string accCheck = "Select * from Manager_Acc where manager_acc = '" + _managerAcc.ManagerAccount + "'";
            int accCheckRes = _sqlHelper.Count(accCheck);
            if(!(accCheckRes > 0)){
                string initialPwdSql = "Select initialPwd from OrdinaryManager_InitialPwd";
                DataTable initialPwdTable = _sqlHelper.ExecuteDataTable(initialPwdSql);
                DataRow item = initialPwdTable.Rows[0];
                string initialPwd = item["initialPwd"].ToString();
                String managerAdd = "Insert into Manager_Acc(manager_acc,manager_pwd,manager_email,manager_type,manager_status,manager_name,manager_gender,manager_phone) values('" + _managerAcc.ManagerAccount + "','" + initialPwd + "','" + _managerAcc.ManagerEmail + "','False','False','" + _managerAcc.ManagerName + "','" + _managerAcc.ManagerGender + "','" + _managerAcc.ManagerPhone + "')";
                rows = _sqlHelper.ExecuteNonQuery(managerAdd);               //账户添加成功则返回非0
            }

            return rows;
        }

        //更改或重置普通管理员密码
        public int ManagerPwdChange(PwdChange _pwdChange,int id) {
            int rows = 0;
            string pwdChangeSql = "Update Manager_Acc set manager_pwd ='" + _pwdChange.NewPwd + "' where manager_acc ='" + _pwdChange.UserName + "'";
            string initialPwdSql = "Select initialPwd from OrdinaryManager_InitialPwd";
            if (id == 0) {
                //验证原密码
                ManagerAccDAL _managerAccDAL = new ManagerAccDAL() ;
                if (_managerAccDAL.pwdCheck(_pwdChange) > 0)
                { 
                    //更改密码
                    rows = _sqlHelper.ExecuteNonQuery(pwdChangeSql);
                }
            }
            else if (id == 1) {                      //重置密码
                DataTable initialPwdTable = _sqlHelper.ExecuteDataTable(initialPwdSql);
                DataRow item = initialPwdTable.Rows[0];
                 string initialPwd = item["initialPwd"].ToString();
                 string initializePwdSql = "Update Manager_Acc set manager_pwd ='" + initialPwd + "' where manager_acc ='" + _pwdChange.UserName + "'";
                rows = _sqlHelper.ExecuteNonQuery(initializePwdSql);
            }
            return rows;
        }

        //验证密码
        public int pwdCheck(PwdChange _pwdChange) {
            string pwdCheckSql = "Select manager_name from Manager_Acc where manager_acc = '" + _pwdChange.UserName + "' and manager_pwd = '" + _pwdChange.OriginalPwd + "'";
            DataTable pwdCheckTable = _sqlHelper.ExecuteDataTable(pwdCheckSql);
            int rows = pwdCheckTable.Rows.Count;
            return rows;
        }

        //查找普通管理员
        public DataTable ManagerQuery(string _keyWords) {
            string managerQuerySql = "Select manager_acc,manager_name,manager_status from Manager_Acc where manager_acc like '%" + _keyWords + "%' and manager_type = 'False'";
            DataTable managerAccTable = _sqlHelper.ExecuteDataTable(managerQuerySql);
            return managerAccTable;
        }

        //锁定或解锁普通管理员
        public int LockManager(string _account, int id)
        {
            int rows = 0;
            string lockManager = "Update Manager_Acc set manager_status = 'True' where manager_acc = '" + _account + "'";
            string unlockManager = "Update Manager_Acc set manager_status = 'False' where manager_acc = '" + _account + "'";
            if (id == 1)                   //锁定普通管理员
            {
                rows = _sqlHelper.ExecuteNonQuery(lockManager);
            }
            else if (id == 0)               //解锁普通管理员
            {
                rows = _sqlHelper.ExecuteNonQuery(unlockManager);
            }
            return rows;
        }
    }
}
