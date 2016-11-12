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
            int rows = 0,storage = 0;
            string storageSql = "Select goods_storage from Goods_Info where goods_id = '"+ _userCart.GoodsId +"'";
            if (_sqlHelper.ExecuteDataTable(storageSql).Rows.Count > 0) {
                storage = (int)_sqlHelper.ExecuteDataTable(storageSql).Rows[0]["goods_storage"];
            }
            if (storage > 0)
            {
                if (storage >= _userCart.GoodsPurchaseQuantity)
                {
                    DateTime now = DateTime.Now;
                    string insertOrderSql = "Insert into User_OrderForm(user_id,goods_id,goods_purchase_quantity,goods_purchase_time,user_orderForm_status_id) values('" + _userCart.UserId + "','" + _userCart.GoodsId + "','" + _userCart.GoodsPurchaseQuantity + "','" + now + "','1')";
                    if (_sqlHelper.ExecuteNonQuery(insertOrderSql) > 0)
                    {
                        string orderIdQuery = "Select user_orderForm_id from User_OrderForm where user_id = '" + _userCart.UserId + "' and goods_id = '" + _userCart.GoodsId + "' and user_orderForm_status_id = '1'";
                        rows = (int)_sqlHelper.ExecuteDataTable(orderIdQuery).Rows[0]["user_orderForm_id"];

                        int subRes = storage - _userCart.GoodsPurchaseQuantity;
                        string subtractStorage = "Update Goods_Info set goods_storage = '" + subRes + "' where goods_id = '" + _userCart.GoodsId + "'";
                        int res = _sqlHelper.ExecuteNonQuery(subtractStorage);                                     //更改库存
                    }
                }
            }
            else {                                   //更改上下架状态
                string statusChange = "Update Goods_Info set goods_status = 'False' where goods_id = '" + _userCart.GoodsId + "'";
                int changeRes = _sqlHelper.ExecuteNonQuery(statusChange);
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
        public int ChangeStatus(string id, UserOrderForm _orderForm) { 
            int rows = 0;
            if (!string.IsNullOrEmpty(id)) {
                if (id.Equals("0"))                           //取消订单
                {
                    string cancelSql = "Update User_OrderForm set user_orderForm_status_id = '3' where user_orderForm_id = '" + _orderForm.UserOrderFormId + "'";
                    rows = _sqlHelper.ExecuteNonQuery(cancelSql);

                    string goodsIdSql = "Select goods_id from User_OrderForm where user_orderForm_id = '"+ _orderForm.UserOrderFormId +"'";
                    int goodsId = (int)_sqlHelper.ExecuteDataTable(goodsIdSql).Rows[0]["goods_id"];

                    string storageSql = "Select goods_storage from Goods_Info where goods_id = '"+ goodsId +"'";
                    int storage = (int)_sqlHelper.ExecuteDataTable(storageSql).Rows[0]["goods_storage"];
                    int newStorage = storage + _orderForm.GoodsPurchaseQuantity;

                    string addStorageSql = "Update Goods_Info set goods_storage = '" + newStorage + "' where goods_id = '" + goodsId + "'";
                    int res = _sqlHelper.ExecuteNonQuery(addStorageSql);                     //更改库存数量
                }
                else                                                //确认收货
                {
                    string sureSql = "Update User_OrderForm set user_orderForm_status_id = '11' where user_orderForm_id = '" + _orderForm.UserOrderFormId + "'";
                    rows = _sqlHelper.ExecuteNonQuery(sureSql);
                }
            }
            return rows;
        }
    }
}
