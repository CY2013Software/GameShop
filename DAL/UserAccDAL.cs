using shop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserAccDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //按评论id查询某条评论的评论者的信息
        public DataTable GoodsRemarkUserQuery(int _goodsRemarkId)
        {
            string goodsRemarkUserQuery = "Select user_name,user_isForbided from User_Acc where user_id = (Select user_id from Goods_Remark where goods_remark_id ='" + _goodsRemarkId + "')";
            DataTable goodsRemarkUserTable = _sqlHelper.ExecuteDataTable(goodsRemarkUserQuery);
            return goodsRemarkUserTable;
        }

        //按关键字查询会员的账户信息
        public DataTable UserAccInfoQuery(string _keyWords) {
            string userAccInfoQuery = "Select user_id,user_name,user_email,user_isForbided from User_Acc where user_name like '%" + _keyWords + "%'";
            DataTable userAccInfoTable = _sqlHelper.ExecuteDataTable(userAccInfoQuery);
            return userAccInfoTable;
        }

        //锁定会员状态
        public int ForbidUserAcc(int _userId,int id) {
            int rows = 0;
            string forbidUserAcc = "Update User_Acc set user_isForbided = 'True' where user_id = '" + _userId + "'";
            string unForbidUserAcc = "Update User_Acc set user_isForbided = 'False' where user_id = '" + _userId + "'";
            if (id == 1) { 
                rows = _sqlHelper.ExecuteNonQuery(forbidUserAcc);
            }
            else if (id == 0) {
                rows = _sqlHelper.ExecuteNonQuery(unForbidUserAcc);
            }
            return rows;
        }

        //验证登录
        public int LoginRes(UserAcc _userAcc)
        {
            int rows = 0;
            string isForbided = "Select user_isForbided from User_Acc where user_email = '" + _userAcc.UserEmail + "'";
            string pwdSql = "Select count(1) from User_Acc where user_email = '" + _userAcc.UserEmail + "' and user_pwd= '" + _userAcc.UserPwd + "' ";

            if (_sqlHelper.Count(pwdSql) != 0)
            {
                DataTable isForbidedTable = _sqlHelper.ExecuteDataTable(isForbided);
                DataRow isForbidedItem = isForbidedTable.Rows[0];

                //锁定状态返回0
                if ((bool)isForbidedItem["user_isForbided"] == true)
                {
                    return rows;
                }
                else
                {
                    string infoQueryRes = " Select * from User_Info where user_id = (Select user_id from User_Acc where user_email = '" + _userAcc.UserEmail + "')";
                    if (_sqlHelper.ExecuteDataTable(infoQueryRes).Rows.Count == 0)    //未填写个人资料，返回2
                    {
                        rows = 2;
                    }
                    else {
                        //登录成功返回3
                    rows = 3;
                    }
                }
            }
            else
            {
                //登录失败则返回1（用户名或密码错误）
                rows = 1;
            }

            return rows;
        }

        //用户注册
        public int RegRes(UserAcc _userAcc) {
            int rows = 0;
            string nameCheck = "Select * from User_Acc where user_name = '" + _userAcc.UserName + "'";
            string emailCheck = "Select * from User_Acc where user_email = '" + _userAcc.UserEmail + "'";
            int nameCheckRes = _sqlHelper.Count(nameCheck);
            if (nameCheckRes > 0)
            {
                return rows;                        //昵称已存在则返回0
            }
            else if (_sqlHelper.ExecuteDataTable(emailCheck).Rows.Count > 0)               //邮箱存在则返回1
            {
                rows = 1;
            }
            else {                                                        //激活状态为已激活（所有操作将不再判断激活状态）
                String userReg = "Insert into User_Acc(user_name,user_pwd,user_email,user_isForbided,user_status) values('" + _userAcc.UserName + "','" + _userAcc.UserPwd + "','" + _userAcc.UserEmail + "','False','True')";
                rows = _sqlHelper.ExecuteNonQuery(userReg);
                rows++;                            //账户添加成功则返回2
            }

            return rows;
        }

        //忘记密码
        public int ForgotPwd(int id,PwdChange _pwdChange) {
            int rows = 0;
            if (!string.IsNullOrEmpty(_pwdChange.Email)) { 
            if (id == 0)                        //验证邮箱正确性
            {
                string emailCheck = "Select user_id from User_Acc where user_email = '" + _pwdChange.Email + "'";
                if(_sqlHelper.Count(emailCheck) > 0){
                    rows = 1;                              //邮箱存在
                }else{
                    rows = -1;                              //邮箱不存在
                }
            }else if(id == 1)                 //验证密保答案
            {
                string answerCheck = "Select user_realname from User_Info where user_pwd_hint = '" + _pwdChange.PwdHint + "',user_pwd_answer = '" + _pwdChange.PwdHintAnswer + "' and user_id = (Select user_id from User_Acc where user_email = '" + _pwdChange.Email + "')";
                if (_sqlHelper.ExecuteDataTable(answerCheck).Rows.Count > 0)
                {
                    rows = 2;                          //答案正确
                }
                else {
                    rows = -2;                        //答案错误
                }
            }
            }
            return rows;
        }

        //查询密保问题
        public string HintQuery(string _email){
            string pwdHint = "";
            string hintQuery = "Select user_pwd_hint from User_Info where user_id = (Select user_id from User_Acc where user_email = '" + _email + "')";
            DataTable hintQueryTable = _sqlHelper.ExecuteDataTable(hintQuery);
            if (hintQueryTable.Rows.Count > 0)
            {
                DataRow item = hintQueryTable.Rows[0];
                pwdHint = item["user_pwd_hint"].ToString();
            }
            return pwdHint;
        }

        //更改密码
        public int ChangePwd(int id,PwdChange _pwdChange) {
            int rows = 0;
            if (id == 0)                      //忘记密码时更改密码
            {
                string changePwd = "Update User_Acc set user_pwd ='" + _pwdChange.NewPwd + "' where user_email ='" + _pwdChange.Email + "'";
                rows = _sqlHelper.ExecuteNonQuery(changePwd);
            }else if(id == 1)               //登录后更改密码
            {
                UserAccDAL _userAccDAL = new UserAccDAL();
                if (_userAccDAL.pwdCheck(_pwdChange) > 0) {
                    string changePwd = "Update User_Acc set user_pwd ='" + _pwdChange.NewPwd + "' where manager_acc ='" + _pwdChange.Email + "'";
                    rows = _sqlHelper.ExecuteNonQuery(changePwd);
                }
            }
            return rows;
        }

        //验证密码正确性
        public int pwdCheck(PwdChange _pwdChange)
        {
            string pwdCheckSql = "Select user_id from User_Acc where user_email = '" + _pwdChange.Email + "' and user_pwd = '" + _pwdChange.OriginalPwd + "'";
            DataTable pwdCheckTable = _sqlHelper.ExecuteDataTable(pwdCheckSql);
            int rows = pwdCheckTable.Rows.Count;
            return rows;
        }
    }
}
