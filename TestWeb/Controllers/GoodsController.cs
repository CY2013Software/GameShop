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
    public class GoodsController : Controller
    {
        /// <summary>
        /// 前台商品查询页面
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
            else
            { //分类不为空，或者搜索关键字不为空

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
        /// 商品详细信息页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GoodsInfo(string goods_id) {
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
                    if (!string.IsNullOrEmpty(item["goods_price_original"].ToString()))
                    {
                        _goodsInfo.GoodsPriceOriginal = (decimal)item["goods_price_original"];
                    }
                    else
                    {
                        _goodsInfo.GoodsPriceOriginal = new decimal(0);
                    }
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

        [HttpPost]
        public ActionResult GoodsInfo(string id,UserCart _userCart) {
            if (!string.IsNullOrEmpty(id) && id.Equals("0")) {
                int rows = new UserCartDAL().InsertCart(_userCart);
                if (rows > 0)
                {
                    return RedirectToAction("Cart", "OrderForm");
                }
                else if(rows == -1){
                    return RedirectToAction("Cart", "OrderForm", new { id = (-1).ToString() });         //商品数量过多
                }else{
                    return RedirectToAction("Cart", "OrderForm", new { id = 0.ToString()});           //购物车已满
                }
            }
            else if (!string.IsNullOrEmpty(id) && id.Equals("1"))
            {
                int rows = new OrderFormDAL().addOrderForm(_userCart);                               //存入订单表，并返回订单ID
                if (rows > 0)
                {
                    return RedirectToAction("PayMent", "OrderForm", new { orderId = rows.ToString() });    //进入支付页面
                }
                else {
                    return RedirectToAction("GoodsInfo", "Goods", new { goods_id = _userCart.GoodsId.ToString() });   //直接购买失败，刷新页面
                }
            }
            else { 
                return RedirectToAction("Login", "User");
            }
        }
    }
}