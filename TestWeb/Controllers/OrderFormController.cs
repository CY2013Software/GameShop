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
        /// 直接购买时进入的页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderForm() {
            return View();
        }

        [HttpPost]
        public ActionResult OrderForm(UserOrderForm _userOrderForm)
        {
            return View();
        }

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
    }
}