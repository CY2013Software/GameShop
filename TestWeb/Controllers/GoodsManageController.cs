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
    public class GoodsManageController : Controller
    {
        /// <summary>
        /// 商品查询页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GoodsList(string subclass_id, string search_box)
        {
            List<GoodsType> _goodsTypeList = new List<GoodsType>();
            List<GoodsInfo> _goodsInfoList = new List<GoodsInfo>();
            //获取商品分类
            DataTable _goodsTypeTable = new GoodsTypeDAL().GoodsTypeQuery();

            foreach (DataRow item in _goodsTypeTable.Rows)
            {
                GoodsType _goodsType = new GoodsType();
                _goodsType.GoodsTypeId = (int)item["goods_type_id"];
                _goodsType.GoodsTypeName = item["goods_type_name"].ToString();
                _goodsTypeList.Add(_goodsType);
            }
            //.........

            //筛选
            if (string.IsNullOrEmpty(search_box) && string.IsNullOrEmpty(subclass_id))
            { //分类为空，并且搜索关键字为空

                DataTable _goodsInfoTable = new GoodsInfoDAL().GoodsInfoQuery();//查询全部游戏商品
                foreach (DataRow item in _goodsInfoTable.Rows)
                {
                    GoodsInfo _goodsInfo = new GoodsInfo();
                    _goodsInfo.GoodsId = (int)item["goods_id"];
                    _goodsInfo.GoodsName = item["goods_name"].ToString();
                    _goodsInfo.GoodsImage = item["goods_image"].ToString();
                    _goodsInfo.GoodsPriceNow = (decimal)item["goods_price_now"];
                    _goodsInfoList.Add(_goodsInfo);
                }


            }
            else { //分类不为空，或者搜索关键字不为空

                if (search_box != null && search_box != "")
                {
                    //用户输入关键词搜索
                    //链接数据库，根据关键词搜索游戏
                    DataTable _goodsInfoTable = new GoodsInfoDAL().GoodsInfoForKeywordsQuery(search_box);
                    foreach (DataRow item in _goodsInfoTable.Rows)
                    {
                        GoodsInfo _goodsInfo = new GoodsInfo();
                        _goodsInfo.GoodsId = (int)item["goods_id"];
                        _goodsInfo.GoodsName = item["goods_name"].ToString();
                        _goodsInfo.GoodsImage = item["goods_image"].ToString();
                        _goodsInfo.GoodsPriceNow = (decimal)item["goods_price_now"];
                        _goodsInfoList.Add(_goodsInfo);
                    }

                }

                if (subclass_id != null)
                {
                    //用户选择小类别
                    //连接数据库，根据小类别ID查询商品
                    DataTable _goodsInfoTable = new GoodsInfoDAL().GoodsInfoForTypeQuery(int.Parse(subclass_id));
                    foreach (DataRow item in _goodsInfoTable.Rows)
                    {
                        GoodsInfo _goodsInfo = new GoodsInfo();
                        _goodsInfo.GoodsId = (int)item["goods_id"];
                        _goodsInfo.GoodsName = item["goods_name"].ToString();
                        _goodsInfo.GoodsImage = item["goods_image"].ToString();
                        _goodsInfo.GoodsPriceNow = (decimal)item["goods_price_now"];
                        _goodsInfoList.Add(_goodsInfo);
                    }

                }

            }


            //将查询结果返回到页面
            ViewData["GoodsInfo"] = _goodsInfoList;//不能为Null
            ViewData["GoodsType"] = _goodsTypeList;//不能为Null

            return View();
        }



        /// <summary>
        /// 商品修改页面，从商品查询页面进入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GoodsUpdate(string goods_id)
        {
            List<GoodsInfo> _goodsInfoList = new List<GoodsInfo>();
            List<GoodsRemark> _goodsRemarkList = new List<GoodsRemark>();
            List<UserAcc> _userAccList = new List<UserAcc>();

            if (!string.IsNullOrEmpty(goods_id))
            {
                DataTable _goodsInfoTable = new GoodsInfoDAL().GoodsInfoQuery(int.Parse(goods_id));
                DataTable _goodsRemarkTable = new GoodsRemarkDAL().GoodsRemarkQuery(int.Parse(goods_id));

                foreach (DataRow item in _goodsInfoTable.Rows)
                {
                    GoodsInfo _goodsInfo = new GoodsInfo();
                    _goodsInfo.GoodsId = (int)item["goods_id"];
                    _goodsInfo.GoodsName = item["goods_name"].ToString();
                    _goodsInfo.GoodsImage = item["goods_image"].ToString();
                    _goodsInfo.GoodsIntro = item["goods_intro"].ToString();
                    _goodsInfo.GoodsPriceOriginal = (decimal)item["goods_price_original"];
                    _goodsInfo.GoodsPriceNow = (decimal)item["goods_price_now"];
                    _goodsInfo.GoodsStorage = (int)item["goods_storage"];
                    _goodsInfo.GoodsStatus = (bool)item["goods_status"];
                    _goodsInfoList.Add(_goodsInfo);
                }

                foreach (DataRow item in _goodsRemarkTable.Rows)
                {
                    DataTable _userAccTable = new UserAccDAL().GoodsRemarkUserQuery((int)item["goods_remark_id"]);
                    foreach (DataRow userItem in _userAccTable.Rows)
                    {
                        UserAcc _userAcc = new UserAcc();
                        _userAcc.UserName = userItem["user_name"].ToString();
                        _userAcc.UserIsForbided = (bool)userItem["user_isForbided"];
                        _userAccList.Add(_userAcc);
                    }
                    GoodsRemark _goodsRemark = new GoodsRemark();
                    _goodsRemark.GoodsRemarkId = (int)item["goods_remark_id"];
                    _goodsRemark.GoodsRemarkFirst = item["goods_remark_first"].ToString();
                    _goodsRemark.GoodsRemarkAdd = item["goods_remark_add"].ToString();
                    _goodsRemarkList.Add(_goodsRemark);
                }
            }

            ViewData["GoodsInfo"] = _goodsInfoList;
            ViewData["GoodsRemark"] = _goodsRemarkList;
            ViewData["UserAcc"] = _userAccList;
            return View();
        }

        /// <summary>
        /// 商品修改页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GoodsUpdate(string id,string goodsRemarkId,GoodsInfo _goodsInfo)
        {
            if (!string.IsNullOrEmpty(id)) {
                int rows = new GoodsRemarkDAL().GoodsRemarkDelete(int.Parse(id), int.Parse(goodsRemarkId));
                if (rows > 0)
                {
                    return RedirectToAction("GoodsUpdate", "GoodsManage");
                }
                else {
                    throw new Exception("删除失败");
                }
            }else if(string.IsNullOrEmpty(_goodsInfo.GoodsName)){
                int rows = new GoodsInfoDAL().GoodsInfoUpdate(_goodsInfo);
                if (rows > 0)
                {
                    return RedirectToAction("GoodsUpdate", "GoodsManage");
                }
                else {
                    throw new Exception("更改失败");
                }
            }

            return RedirectToAction("GoodsUpdate", "GoodsManage");
        }

        /// <summary>
        /// 添加商品页面，从商品查询页面进入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GoodsAdd()
        {
            List<GoodsType> _goodsTypeList = new List<GoodsType>();
            //获取商品分类
            DataTable _goodsTypeTable = new GoodsTypeDAL().GoodsTypeQuery();

            foreach (DataRow item in _goodsTypeTable.Rows)
            {
                GoodsType _goodsType = new GoodsType();
                _goodsType.GoodsTypeId = (int)item["goods_type_id"];
                _goodsType.GoodsTypeName = item["goods_type_name"].ToString();
                _goodsTypeList.Add(_goodsType);
            }
           
            //将查询结果返回到页面
            ViewData["GoodsType"] = _goodsTypeList;//不能为Null

            return View();
        }

        /// <summary>
        /// 添加商品页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GoodsAdd(GoodsInfo _goodsInfo)
        {
            int rows = new GoodsInfoDAL().GoodsAdd(_goodsInfo);
            if (rows > 0)
            {
                //添加成功
                return RedirectToAction("GoodsAdd", "GoodsManage");
            }
            else
            {
                //添加失败
                throw new Exception("添加失败");
            }

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
                imgPath = "/GoodsImg/" + hfc[0].FileName;     //根目录下的Upload文件夹下
                string PhysicalPath = Server.MapPath(imgPath);//把图片的虚拟路径改为物理路径
                hfc[0].SaveAs(PhysicalPath);//上传
            }
            return Content(imgPath);    //返回访问路径，让前台可以显示
        }

        /// <summary>
        /// 商品类别查询页面，从商品查询页面进入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TypeList(string goods_typeId, string goods_typeSubclassId, string id)
        {
            
            List<GoodsType> _goodsTypeList = new List<GoodsType>();
            List<GoodsType> _TypeDeleteRes = new List<GoodsType>();
            List<GoodsType> _TypeSubclassDeleteRes = new List<GoodsType>();
            //获取商品分类
            DataTable _goodsTypeTable = new GoodsTypeDAL().GoodsTypeQuery();

            foreach (DataRow item in _goodsTypeTable.Rows)
            {
                GoodsType _goodsType = new GoodsType();
                _goodsType.GoodsTypeId = (int)item["goods_type_id"];
                _goodsType.GoodsTypeName = item["goods_type_name"].ToString();
                _goodsTypeList.Add(_goodsType);
            }

            if (!string.IsNullOrEmpty(goods_typeId))                                                                //删除大类别
            {
                bool isNoSubclass = new GoodsTypeSubclassDAL().isNoSubclass(int.Parse(goods_typeId));
                if (isNoSubclass)
                {
                    int rows = new GoodsTypeDAL().GoodsTypeDelete(int.Parse(goods_typeId));
                    if (rows > 0)
                    {//删除成功
                        return RedirectToAction("TypeList", "GoodsManage");
                    }
                    else
                    {//删除失败
                        throw new Exception("删除失败");
                    }
                }
                else
                {
                    return RedirectToAction("TypeList", "GoodsManage", new { id = "1" });   //返回提示不能删除大类别
                }
            }


            if (!string.IsNullOrEmpty(goods_typeSubclassId))                                                      //删除小类别
            {
                bool isNoGoods = new GoodsInfoDAL().isNoGoods(int.Parse(goods_typeSubclassId));
                if (isNoGoods)
                {
                    int rows = new GoodsTypeSubclassDAL().GoodsTypeSubclassDelete(int.Parse(goods_typeSubclassId));
                    if (rows > 0)
                    {//删除成功
                        return RedirectToAction("TypeList", "GoodsManage");
                    }
                    else
                    {//删除失败
                        throw new Exception("删除失败");
                    }
                }
                else
                {
                    return RedirectToAction("TypeList", "GoodsManage", new { id = "2" });         //返回提示不能删除小类别
                }
            }


            //判断是进行的大类别删除还是子类别删除
            if (!string.IsNullOrEmpty(id)) { 
            if (id.Equals("1")) {
                _TypeDeleteRes.AddRange(_goodsTypeList);
            }else if(id.Equals("2")){
                _TypeSubclassDeleteRes.AddRange(_goodsTypeList);
            }
            }

            //将查询结果返回到页面
            ViewData["TypeDelete"] = _TypeDeleteRes;                                     //行数大于0时，页面弹出遮罩层显示删除大类别失败
            ViewData["TypeSubclassDelete"] = _TypeSubclassDeleteRes;      //行数大于0时，页面弹出遮罩层显示删除小类别失败
            ViewData["GoodsType"] = _goodsTypeList;//不能为Null               

            return View();
        }



        /// <summary>
        /// 商品类别查询页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TypeList(GoodsType _goodsType, GoodsTypeSubclass _goodsTypeSubclass)
        {
            

            if (!string.IsNullOrEmpty(_goodsType.GoodsTypeName))                                                              //修改大类别名称
            {
                int rows = new GoodsTypeDAL().GoodsTypeUpdate(_goodsType.GoodsTypeId, _goodsType.GoodsTypeName);
                if (rows > 0)
                {//更改成功
                    return RedirectToAction("TypeList", "GoodsManage");
                }
                else
                {//更改失败
                    throw new Exception("更改大类别失败");
                }
            }


            if (!string.IsNullOrEmpty(_goodsTypeSubclass.GoodsTypeSubclassName))                                         //修改小类别名称
            {
                int rows = new GoodsTypeSubclassDAL().GoodsTypeSubclassUpdate(_goodsTypeSubclass.GoodsTypeSubclassId,_goodsTypeSubclass.GoodsTypeSubclassName);
                if (rows > 0)
                {//更改成功
                    return RedirectToAction("TypeList", "GoodsManage");
                }
                else
                {//更改失败
                    throw new Exception("更改小类别失败");
                }
            }

            return RedirectToAction("TypeList", "GoodsManage");
        }

        /// <summary>
        /// 添加商品类别页面，从商品类别查询页面进入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TypeAdd()
        {
            List<GoodsType> _goodsTypeList = new List<GoodsType>();
            //获取商品分类
            DataTable _goodsTypeTable = new GoodsTypeDAL().GoodsTypeQuery();

            foreach (DataRow item in _goodsTypeTable.Rows)
            {
                GoodsType _goodsType = new GoodsType();
                _goodsType.GoodsTypeId = (int)item["goods_type_id"];
                _goodsType.GoodsTypeName = item["goods_type_name"].ToString();
                _goodsTypeList.Add(_goodsType);
            }

            //将查询结果返回到页面
            ViewData["GoodsType"] = _goodsTypeList;//不能为Null               

            return View();
        }

        /// <summary>
        /// 添加商品类别页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TypeAdd(GoodsType _type,GoodsTypeSubclass _typeSubclass)
        {
            if (!string.IsNullOrEmpty(_type.GoodsTypeName)) {
                int rows = new GoodsTypeDAL().GoodsTypeInsert(_type.GoodsTypeName);
                if (rows > 0)
                {
                    //添加大类别成功
                    return RedirectToAction("TypeAdd", "GoodsManage");
                }
                else
                {
                    //添加大类别失败
                    throw new Exception("添加失败");
                }
            }
            else if (!string.IsNullOrEmpty(_typeSubclass.GoodsTypeSubclassName))
            {
                int rows = new GoodsTypeSubclassDAL().GoodsTypeSubInsert(_typeSubclass);
                if (rows > 0)
                {
                    //添加小类别成功
                    return RedirectToAction("TypeAdd", "GoodsManage");
                }
                else
                {
                    //添加小类别失败
                    throw new Exception("添加失败");
                }
            }

            return RedirectToAction("TypeAdd", "GoodsManage");
        }
    }
}