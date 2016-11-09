using shop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserInfoDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //根据id查询会员个人信息
        public DataTable UserInfoQuery(int _userId) {
            string userInfoQuery = "Select user_gender,user_realname,user_image,user_phone,user_pwd_hint from User_Info where user_id = '" + _userId + "'";
            DataTable userInfoTable = _sqlHelper.ExecuteDataTable(userInfoQuery);
            return userInfoTable;
        }

        //根据邮箱查找ID
        public int UserId(string _email)
        {
            string userIdQuery = "Select user_id from User_Acc where user_email = '" + _email + "'";
            DataTable userIdTable = _sqlHelper.ExecuteDataTable(userIdQuery);
            DataRow item = userIdTable.Rows[0];
            int userId = (int)item["user_id"];
            return userId;
        }

        //新增或更改个人信息
        public int InsertOrUpdateInfo(int id, string _email,UserInfo _userInfo)
        {
            int rows = 0;
            int userId = new UserInfoDAL().UserId(_email);
            if(id == 0){                     //新增个人信息
                string insertInfo = "";
                if (string.IsNullOrEmpty(_userInfo.UserImage))
                {
                    insertInfo = "Insert into User_Info(user_id,user_gender,user_realname,user_phone,user_pwd_hint,user_pwd_answer) values('" + userId + "','" + _userInfo.UserGender + "','" + _userInfo.UserRealname + "','" + _userInfo.UserPhone + "','" + _userInfo.UserPwdHint + "','" + _userInfo.UserPwdAnswer + "')";
                }
                else {
                    insertInfo = "Insert into User_Info(user_id,user_gender,user_realname,user_image,user_phone,user_pwd_hint,user_pwd_answer) values('" + userId + "','" + _userInfo.UserGender + "','" + _userInfo.UserRealname + "','"+ _userInfo.UserImage +"','" + _userInfo.UserPhone + "','" + _userInfo.UserPwdHint + "','" + _userInfo.UserPwdAnswer + "')";
                }
                if (!string.IsNullOrEmpty(insertInfo)) {
                    rows = _sqlHelper.ExecuteNonQuery(insertInfo);
                }
            }else if(id == 1){              //修改个人信息
                string updateInfo = "";
                if (string.IsNullOrEmpty(_userInfo.UserImage))
                {
                    updateInfo = "Update User_Info set user_phone = '" + _userInfo.UserPhone + "' where user_id = '" + userId + "'";
                }
                else {
                    updateInfo = "Update User_Info set user_image = '" + _userInfo.UserImage + "',user_phone = '" + _userInfo.UserPhone + "' where user_id = '" + userId + "'";
                }
                if (!string.IsNullOrEmpty(updateInfo))
                {
                    rows = _sqlHelper.ExecuteNonQuery(updateInfo);
                }
            }

            return rows;
        }
    }
}
