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
            string userInfoQuery = "Select user_gender,user_realname,user_image,user_phone from User_Info where user_id = '" + _userId + "'";
            DataTable userInfoTable = _sqlHelper.ExecuteDataTable(userInfoQuery);
            return userInfoTable;
        }
    }
}
