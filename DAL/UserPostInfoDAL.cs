using shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserPostInfoDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //添加邮递信息
        public int InsertPostInfo(UserPostInfo  _userPostInfo) {
            string insertSql = "Insert into User_PostInfo(user_id,user_address,user_phone,user_postcode) values('" + _userPostInfo.UserId + "','" + _userPostInfo.UserAddress + "','" + _userPostInfo.UserPhone + "','" + _userPostInfo.UserPostcode + "')";
            int rows = _sqlHelper.ExecuteNonQuery(insertSql);
            return rows;
        }

        //更改邮递信息
        public int UpdatePostInfo(UserPostInfo _userPostInfo) {
            string updateSql = "Update User_PostInfo set user_id = '" + _userPostInfo.UserId + "', user_address = '" + _userPostInfo.UserAddress + "', user_phone = '" + _userPostInfo.UserPhone + "', user_postcode = '" + _userPostInfo.UserPostcode + "' where user_postInfo_id = '" + _userPostInfo.UserPostInfoId + "'";
            int rows = _sqlHelper.ExecuteNonQuery(updateSql);
            return rows;
        }
    }
}
