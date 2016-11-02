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
        public int ForbidUserAcc(int _userId) {
            string forbidUserAcc = "Update User_Acc set user_isForbided = True where user_id = '" + _userId + "'";
            int rows = _sqlHelper.ExecuteNonQuery(forbidUserAcc);
            return rows;
        }
    }
}
