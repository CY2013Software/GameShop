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
    }
}