using shop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OrderFormDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //将订单信息存入订单信息表，并返回订单ID
        public int addOrderForm(UserCart _userCart) {
            int rows = 0;
            DateTime now = DateTime.Now;
            string insertOrderSql = "Insert into User_OrderForm(user_id,goods_id,goods_purchase_quantity,goods_purchase_time,user_orderForm_status_id) values('" + _userCart.UserId + "','" + _userCart.GoodsId + "','" + _userCart.GoodsPurchaseQuantity + "','" + now + "','1')";
            if (_sqlHelper.ExecuteNonQuery(insertOrderSql) > 0) {
                string orderIdQuery = "Select user_orderForm_id from User_OrderForm where user_id = '" + _userCart.UserId + "' and goods_id = '" + _userCart.GoodsId + "' and user_orderForm_status_id = '1'";
                rows = (int)_sqlHelper.ExecuteDataTable(orderIdQuery).Rows[0]["user_orderForm_id"];
            }
            return rows;
        }

        //存入付款信息
        public int insertPaymentInfo(int orderFormId){
            DateTime now = DateTime.Now;
            string paymentInfo = "Update User_OrderForm set goods_paymentTime = '" + now + "', user_orderForm_status_id = '2' where user_orderForm_id = '" + orderFormId + "'";
            int rows = _sqlHelper.ExecuteNonQuery(paymentInfo);
            return rows;
        }

        //按会员ID查找其所有订单
        public DataTable orderFormQuery(int userId) {
            string orderFormQuerySql = "Select * from User_OrderForm where user_id = '"+ userId +"'";
            DataTable queryTable = _sqlHelper.ExecuteDataTable(orderFormQuerySql);
            return queryTable;
        }

        //按订单ID更改订单状态
        public int ChangeStatus(string id, int orderFormId) { 
            int rows = 0;
            if (!string.IsNullOrEmpty(id)) {
                if (id.Equals("0"))                           //取消订单
                {
                    string cancelSql = "Update User_OrderForm set user_orderForm_status_id = '3' where user_orderForm_id = '" + orderFormId + "'";
                    rows = _sqlHelper.ExecuteNonQuery(cancelSql);
                }
                else                                                //确认收货
                {
                    string sureSql = "Update User_OrderForm set user_orderForm_status_id = '11' where user_orderForm_id = '" + orderFormId + "'";
                    rows = _sqlHelper.ExecuteNonQuery(sureSql);
                }
            }
            return rows;
        }
    }
}
