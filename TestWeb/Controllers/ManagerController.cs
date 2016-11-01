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
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult ManagerIndex()
        {
            return View();
        }
    }
}