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

        //[HttpPost]
        //public ActionResult PostInfo() { 
        //}
    }
}