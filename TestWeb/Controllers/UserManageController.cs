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
    public class UserManageController : Controller
    {
        /// <summary>
        /// 查询和管理会员页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserManage(string search_box)
        {
            List<UserInfo> _userInfoList = new List<UserInfo>();
            List<UserAcc> _userAccList = new List<UserAcc>();

            //筛选
            if (!string.IsNullOrEmpty(search_box))
            { 
                    //链接数据库，根据关键词搜索用户
                    DataTable _userAccTable = new UserAccDAL().UserAccInfoQuery(search_box);
                    foreach (DataRow item in _userAccTable.Rows)
                    {
                        DataTable _userInfoTable = new UserInfoDAL().UserInfoQuery((int)item["user_id"]);
                        foreach(DataRow userInfoItem in _userInfoTable.Rows){
                            UserInfo _userInfo = new UserInfo();
                            _userInfo.UserImage = userInfoItem["user_image"].ToString();
                            _userInfo.UserGender = (bool)userInfoItem["user_gender"];
                            _userInfo.UserPhone = userInfoItem["user_phone"].ToString();
                            _userInfo.UserRealname = userInfoItem["user_realname"].ToString();
                            _userInfoList.Add(_userInfo);
                        }
                        UserAcc _userAcc = new UserAcc();
                        _userAcc.UserId = (int)item["user_id"];
                        _userAcc.UserName = item["user_name"].ToString();
                        _userAcc.UserEmail = item["user_email"].ToString();
                        _userAcc.UserIsForbided = (bool)item["user_isForbided"];
                        _userAccList.Add(_userAcc);
                    }
            }

            //将查询结果返回到页面
            ViewData["UserAcc"] = _userAccList;//不能为Null
            ViewData["UserInfo"] = _userInfoList;

            return View();
        }

       /// <summary>
        /// 查询和管理会员页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserManage(string user_id) {

            return RedirectToAction("UserManage", "UserManage");
        }
    }
}