using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shop.Models;
using System.Data;

namespace TestWeb.Controllers
{
    public class OrderFormController : Controller
    {
        /// <summary>
        /// 购物车页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Cart(string id) {
            string statusId = "";
            if (!string.IsNullOrEmpty(id)) {
                statusId = id;
            }
            ViewData["StatusId"] = statusId;
        return View();
    }

        [HttpPost]
        public ActionResult Cart(int cartId) {
            if (cartId > 0)
            {
                int rows = new UserCartDAL().DeleteCart(cartId);
                if (rows > 0) {
                    return RedirectToAction("Cart", "OrderForm");
                }
            }
            return View();
        }

        /// <summary>
        /// 个人订单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderFormList(string userId) {
            List<UserOrderForm> orderFormList = new List<UserOrderForm>();
            if (!string.IsNullOrEmpty(userId)) {
                DataTable orderFormTable = new OrderFormDAL().orderFormQuery(int.Parse(userId));
                if (orderFormTable.Rows.Count > 0) { 
                    foreach (DataRow item in orderFormTable.Rows) {
                        UserOrderForm _userOrderForm = new UserOrderForm();
                        _userOrderForm.UserOrderFormId = (int)item["user_orderForm_id"];
                        _userOrderForm.UserId = (int)item["user_id"];
                        _userOrderForm.GoodsId = (int)item["goods_id"];
                        _userOrderForm.GoodsPurchaseQuantity = (int)item["goods_purchase_quantity"];
                        _userOrderForm.UserOrderFormStatusId = (int)item["user_orderForm_status_id"];
                        orderFormList.Add(_userOrderForm);
                }
                }
                
            }
            ViewData["OrderFormList"] = orderFormList;
            return View();
        }

        [HttpPost]
        public ActionResult OrderFormList(string id,UserOrderForm _orderForm) { 
             if(!string.IsNullOrEmpty(id)){
                 int rows = new OrderFormDAL().ChangeStatus(id, _orderForm);
             }
             return RedirectToAction("OrderFormList", "OrderForm", new { userId = _orderForm.UserId.ToString() });
        }

        /// <summary>
        /// 收货信息页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PostInfo(string cartId,string res) {
            string id = "";
            string postInfoRes = "";
            if (!string.IsNullOrEmpty(cartId)) {
                id = cartId.ToString();
            }
            if (!string.IsNullOrEmpty(res)) {
                postInfoRes = res;
            }
            ViewData["CartId"] = id;
            ViewData["PostInfoRes"] = postInfoRes;
            return View();
        }

        [HttpPost]
        public ActionResult PostInfo(string cartId,UserCart _userCart,UserPostInfo _userPostInfo)
        {
            if (_userCart.GoodsPurchaseQuantity > 0)
            {
                int orderFormId = new OrderFormDAL().addOrderForm(_userCart);
                if (orderFormId > 0)
                {
                    int rows = new UserCartDAL().DeleteCart(int.Parse(cartId));
                    return RedirectToAction("PayMent", "OrderForm", new { orderId = orderFormId.ToString() });
                }
                else
                {
                    return RedirectToAction("PostInfo", "OrderForm", new { cartId = cartId });
                }
            }
            else {
                int rows = new UserPostInfoDAL().InsertPostInfo(_userPostInfo);
                return RedirectToAction("PostInfo", "OrderForm", new {cartId = cartId, res = 1.ToString() });
            }
            
        }

        /// <summary>
        /// 支付页面
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PayMent(string orderId){
            string orderFormId = "";
            if (!string.IsNullOrEmpty(orderId)) {
                orderFormId = orderId;
            }
            ViewData["OrderFormId"] = orderFormId;
            return View();
        }

        [HttpPost]
        public ActionResult PayMent(string order_id,string payWay) {
            int rows = new OrderFormDAL().insertPaymentInfo(int.Parse(order_id));
            if (rows > 0)
            {
                return RedirectToAction("Cart", "OrderForm");
            }
            else {
                return RedirectToAction("PayMent", "OrderForm", new { orderId = order_id });
            }
            
        }

        /// <summary>
        /// 评论页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Remark(string id,GoodsRemark _goodsRemark) {
            string resId = "";
            GoodsRemark _remark = new GoodsRemark();
            if (!string.IsNullOrEmpty(id)) {
                resId = id;
            }
            if(_goodsRemark.GoodsId > 0){
                _remark = _goodsRemark;
            }
            ViewData["Id"] = resId;
            ViewData["Remark"] = _remark;
            return View();
        }

        [HttpPost]
        public ActionResult Remark(GoodsRemark _goodsRemark) {
            if (_goodsRemark.GoodsId > 0) {
                int rows = new GoodsRemarkDAL().InsertRemark(_goodsRemark);
                if (rows > 0) {
                    return RedirectToAction("Remark", "OrderForm", new { id = 1.ToString() });
                }
            }
            return RedirectToAction("Remark", "OrderForm");
        }
    }
}