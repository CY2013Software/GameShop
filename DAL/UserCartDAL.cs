using shop.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
            int rows = 0;
            string storage = "Select goods_storage from Goods_Info where goods_id = '"+ _userCart.GoodsId +"'";
            if (_userCart.GoodsPurchaseQuantity <= ((int)_sqlHelper.ExecuteDataTable(storage).Rows[0]["goods_storage"]))
            {
                string cartNum = "Select * from User_Cart where user_id = '" + _userCart.UserId + "'";
                if(_sqlHelper.ExecuteDataTable(cartNum).Rows.Count < 10){
                    string priceNow = "Select goods_price_now from Goods_Info where goods_id = '"+ _userCart.GoodsId +"'";
                    decimal tradeMondy = ((decimal)_sqlHelper.ExecuteDataTable(priceNow).Rows[0]["goods_price_now"]) * _userCart.GoodsPurchaseQuantity;
                    string insertCartSql = "Insert into User_Cart(user_id,goods_id,goods_purchase_quantity,trade_money) values('" + _userCart.UserId + "','" + _userCart.GoodsId + "','" + _userCart.GoodsPurchaseQuantity + "','" + tradeMondy + "')";
                rows = _sqlHelper.ExecuteNonQuery(insertCartSql);
                }
            }
            else {
                rows = -1;
            }
            return rows;
        }

        //从购物车表中查找信息
        public DataTable CartInfo(int user_id) {
            string cartInfoQuery = "Select * from User_Cart where user_id = '" + user_id + "'";
            DataTable cartInfoTable = _sqlHelper.ExecuteDataTable(cartInfoQuery);
            return cartInfoTable;
        }

        //按购物车ID删除购物车信息
        public int DeleteCart(int _cartId) {
            string deleteCartSql = "Delete from User_Cart where cart_id = '"+ _cartId +"'";
            int rows = _sqlHelper.ExecuteNonQuery(deleteCartSql);
            return rows;
        }
    }
}
