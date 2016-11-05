using shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserCartDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //向购物车表中插入信息
        public int InsertCart(UserCart _userCart) {
            string insertCartSql = "Insert into User_Cart(user_id,goods_id,goods_purchase_quantity) values('" + _userCart.UserId + "','" + _userCart.GoodsId + "','" + _userCart.GoodsPurchaseQuantity + "')";
            int rows = _sqlHelper.ExecuteNonQuery(insertCartSql);
            return rows;
        }
    }
}
