using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shop.Models;
using System.Data;
using System.Net;

namespace TestWeb.Controllers
{
    public class ManagerController : Controller
    {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManagerLogin(string id)
        {
            string errorId = "";
            if (!string.IsNullOrEmpty(id)) {
                    errorId = id;
            }
            ViewData["ErrorId"] = errorId;
            return View();
        }

        [HttpPost]
        public ActionResult ManagerLogin(ManagerAcc _managerAcc)
        {
            //判断登录是否成功
            int loginRes = new ManagerAccDAL().LoginRes(_managerAcc);

            if (loginRes != 0)
            {

                if (loginRes != 1)
                {
                    Cookie cookie = new Cookie("ManagerAccount", _managerAcc.ManagerAccount);
                    //Cookie["ManagerAccount"]
                    if (loginRes == 2)
                    {
                        return RedirectToAction("ManagerPwdChange", "Manager");               //在页面上获取cookie中账户名
                    }
                    else if (loginRes == 3)
                    {
                        return RedirectToAction("GoodsList", "GoodsManage");                      //在页面上获取cookie中账户名
                    }
                    else if (loginRes == 4) {
                        return RedirectToAction("ManagerManage", "Manager");                  //在页面上获取cookie中账户名
                    }
                }
                else
                {
                    return RedirectToAction("ManagerLogin", "Manager", new { id = 1.ToString() });     //返回状态为用户名或密码错误
                }
            }
            else {
                return RedirectToAction("ManagerLogin", "Manager", new { id = 0.ToString() });      //返回状态为账户锁定
            }

            return RedirectToAction("ManagerLogin", "Manager");
        }


        /// <summary>
        /// 普通管理员更改密码页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManagerPwdChange() {
            return View();
        }

        [HttpPost]
        public ActionResult ManagerPwdChange(PwdChange _pwdChange)
        {
            if(!string.IsNullOrEmpty(_pwdChange.UserName)){
                int rows = new ManagerAccDAL().ManagerPwdChange(_pwdChange,0);
                if (rows > 0)
                {
                    return RedirectToAction("GoodsList", "GoodsManage");
                }
                else {
                    return RedirectToAction("ManagerPwdChange", "Manager");
                }
            }
            else { 
            return RedirectToAction("ManagerPwdChange", "Manager");
            }
        }

        /// <summary>
        /// 管理普通管理员页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManagerManage(string search_box)
        {
            List<ManagerAcc> _managerAccList = new List<ManagerAcc>();
            if (!string.IsNullOrEmpty(search_box))
            {
                //链接数据库，根据关键词搜索普通管理员
                DataTable _managerAccTable = new ManagerAccDAL().ManagerQuery(search_box);
                foreach (DataRow item in _managerAccTable.Rows)
                {
                    ManagerAcc _managerAcc = new ManagerAcc();
                    _managerAcc.ManagerAccount = item["manager_acc"].ToString();
                    _managerAcc.ManagerName = item["manager_name"].ToString();
                    _managerAcc.ManagerStatus = (bool)item["manager_status"];
                    _managerAccList.Add(_managerAcc);
                }
            }

            //将查询结果返回到页面
            ViewData["ManagerAcc"] = _managerAccList;//不能为Null
            return View();
        }

        [HttpPost]
        public ActionResult ManagerManage(string account, string id,PwdChange _pwdChange)
        {
            if (!string.IsNullOrEmpty(_pwdChange.UserName))
            {                                  //重置普通管理员密码
                int rows = new ManagerAccDAL().ManagerPwdChange(_pwdChange, 1);
                if (rows > 0)
                {
                    return RedirectToAction("ManagerManage", "Manager");
                }
                else {
                    throw new Exception("重置密码失败");
                }
            }
            else {
                int rows = new ManagerAccDAL().LockManager(account,int.Parse(id));
                if (rows > 0)
                {
                    return RedirectToAction("ManagerManage", "Manager");
                }
                else
                {
                    throw new Exception("锁定或解锁失败");
                }
            }
        }

        /// <summary>
        /// 添加普通管理员，从管理普通管理员页面进入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManagerAdd(string id) {
            string failureId = "";
            if (!string.IsNullOrEmpty(id)) {
            if (id.Equals("0")) {
                failureId = id;
            } 
            }
            ViewData["FailureId"] = failureId;
            return View();
        }

        [HttpPost]
        public ActionResult ManagerAdd(ManagerAcc _managerAcc)
        {
            if (!string.IsNullOrEmpty(_managerAcc.ManagerAccount)) {
                int rows = new ManagerAccDAL().ManagerAdd(_managerAcc);
                if (rows == 0) {
                    return RedirectToAction("ManagerAdd", "Manager", new { id = 0.ToString() });
                }
                else if (rows > 0)
                {
                    return RedirectToAction("ManagerAdd", "Manager");
                }
                else {
                    throw new Exception("添加管理员失败");
                }
            }
            return RedirectToAction("ManagerAdd", "Manager");
        }
    }
}