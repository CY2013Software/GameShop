using DAL;
using shop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string id)
        {
            string errorId = "";
            if (!string.IsNullOrEmpty(id))
            {
                errorId = id;
            }
            ViewData["ErrorId"] = errorId;
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAcc _userAcc) {
            //判断登录是否成功
            int loginRes = new UserAccDAL().LoginRes(_userAcc);

            if (loginRes != 0)
            {

                if (loginRes != 1)
                {

                    HttpCookie aCookie = new HttpCookie("UserEmail");
                    aCookie.Value = _userAcc.UserEmail.ToString();
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(aCookie);

                    if (loginRes == 3)
                    {
                        return RedirectToAction("GoodsList", "Goods");               //在页面上获取cookie中eamil
                    }
                    else if (loginRes == 2) {
                        return RedirectToAction("UserInfos", "User", new { email = _userAcc.UserEmail});               //在页面上获取cookie中email
                    }
                }
                else
                {
                    return RedirectToAction("Login", "User", new { id = 1.ToString() });     //返回状态为用户名或密码错误
                }
            }
            else
            {
                return RedirectToAction("Login", "User", new { id = 0.ToString() });      //返回状态为账户锁定
            }

            return RedirectToAction("Login", "User");
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register(string id) {
            string failureId = "";
            if (!string.IsNullOrEmpty(id))
            {
                if (id.Equals("0") || id.Equals("1"))
                {
                    failureId = id;
                }
            }
            ViewData["FailureId"] = failureId;
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAcc _userAcc) {
            if (!string.IsNullOrEmpty(_userAcc.UserName))
            {
                int rows = new UserAccDAL().RegRes(_userAcc);
                if (rows == 0)
                {
                    return RedirectToAction("Register", "User", new { id = 0.ToString() });
                }
                else if (rows == 1)
                {
                    return RedirectToAction("Register", "User", new { id = 1.ToString() });
                }
                else if (rows == 2)
                {
                    //Cookie cookie = new Cookie("UserEmail", _userAcc.UserEmail);
                    //Cookie["UserEmail"]
                    return RedirectToAction("UserInfos", "User", new { email = _userAcc.UserEmail});                    //在页面上从cookie中获取email
                }
                else
                {
                    throw new Exception("用户注册失败");
                }
            }
            return RedirectToAction("Register", "User");
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PwdChange(string operationId,string status,string userEmail)
        {
            string id = "";
            string email = "";
            string pwdHint = "";
            string statusId = "";
            if (!string.IsNullOrEmpty(operationId))
            {
                id = operationId;
                if (operationId.Equals("1"))
                {
                    email = userEmail;
                    pwdHint = new UserAccDAL().HintQuery(userEmail);
                }
            }
            if (!string.IsNullOrEmpty(status)) {
                statusId = status;
            }
            
            ViewData["Id"] = id;
            ViewData["Email"] = email;
            ViewData["PwdHint"] = pwdHint;
            ViewData["StatusId"] = statusId;
            return View();
        }

        [HttpPost]
        public ActionResult PwdChange(string id,PwdChange _pwdChange)
        {
            if (!string.IsNullOrEmpty(id))
            {                 //验证邮箱
                if (id.Equals("0"))
                {
                    int rows = new UserAccDAL().ForgotPwd(0, _pwdChange);
                    if (rows == -1)                             //邮箱不存在
                    {
                        return RedirectToAction("PwdChange", "User", new { operationId = 0.ToString(), status = (-1).ToString() });
                    }
                    else
                    {                                             //跳转到填写密保的页面
                        return RedirectToAction("PwdChange", "User", new { operationId = 1.ToString(), userEmail = _pwdChange.Email });
                    }
                }
                else if (id.Equals("1"))
                {             //验证密保答案
                    int rows = new UserAccDAL().ForgotPwd(1, _pwdChange);
                    if (rows == -2)                        //密保答案错误
                    {
                        return RedirectToAction("PwdChange", "User", new { operationId = 1.ToString(), status = (-2).ToString(), userEmail = _pwdChange.Email });
                    }
                    else
                    {                                        //重置密码
                        int Res = new UserAccDAL().ChangePwd(0, _pwdChange);
                        if (Res > 0)
                        {
                            HttpCookie aCookie = new HttpCookie("UserEmail");
                            aCookie.Value = _pwdChange.Email.ToString();
                            aCookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(aCookie);

                            return RedirectToAction("GoodsList", "Goods");
                        }
                    }
                }
            }
            else {
                if (!string.IsNullOrEmpty(_pwdChange.Email)) {               //更改密码
                    int rows = new UserAccDAL().ChangePwd(1, _pwdChange);
                    if (rows > 0)
                    {
                        return RedirectToAction("GoodsList", "Goods");
                    }
                    else {
                        return RedirectToAction("PwdChange", "User", new { status = (-3).ToString() });
                    }
                }
            }

            return RedirectToAction("PwdChange", "User", new { operationId = 0.ToString() });
        }

        /// <summary>
        /// 更改个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserInfos(string email)
        {
            string existsId = "";
            List<UserInfo> _infoList = new List<UserInfo>();
            if (!string.IsNullOrEmpty(email))
            {
                int userId = new UserInfoDAL().UserId(email);
                if (userId > 0)
                {
                    DataTable _infoTable = new UserInfoDAL().UserInfoQuery(userId);
                    int rows = _infoTable.Rows.Count;
                    if (rows > 0)
                    {   
                        foreach (DataRow item in _infoTable.Rows)
                        {
                            UserInfo _info = new UserInfo();
                            _info.UserRealname = item["user_realname"].ToString();
                            _info.UserGender = (bool)item["user_gender"];
                            _info.UserPhone = item["user_phone"].ToString();
                            _info.UserPwdHint = item["user_pwd_hint"].ToString();
                            _infoList.Add(_info);
                        }
                    }
                }
            }
            else {
                existsId = "1";
            }
            ViewData["ExistsId"] = existsId;
            ViewData["Info"] = _infoList;
            return View();
        }

        [HttpPost]
        public ActionResult UserInfos(string id, string email, UserInfo _userInfo) {
            if (!string.IsNullOrEmpty(id) && id.Equals("0"))
            {                                    //新增个人信息
                int rows = new UserInfoDAL().InsertOrUpdateInfo(0, email, _userInfo);
            }
            else {                               //更改个人信息
                    int rows = new UserInfoDAL().InsertOrUpdateInfo(1, email, _userInfo);
            }
            return RedirectToAction("UserInfos", "User", new { email = email });
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadImage()
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            if (hfc.Count > 0)
            {
                if (!string.IsNullOrEmpty(hfc[0].FileName))
                {
                    imgPath = "/UserImg/" + hfc[0].FileName;     //根目录下的Upload文件夹下
                    string PhysicalPath = Server.MapPath(imgPath);//把图片的虚拟路径改为物理路径

                    hfc[0].SaveAs(PhysicalPath);//上传
                }
                else
                {
                    return Content("未选择图片");
                }
            }
            return Content(imgPath);    //返回访问路径，让前台可以显示
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="id"></param>
        public ActionResult Logout()
        {
            HttpCookie cookies = new HttpCookie("UserEmail");
            cookies.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookies);
            return RedirectToAction("GoodsList", "Goods");
        }
    }
}