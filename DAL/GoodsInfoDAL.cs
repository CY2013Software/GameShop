using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shop.Models;
using System.Data;

namespace DAL
{
    public class GoodsInfoDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //打开GoodsList页面时运行的方法
        public DataTable GoodsInfoQuery(){
            string goodsInfoQuery = "Select goods_id,goods_name,goods_image,goods_price_now from Goods_Info";
            DataTable goodsInfoTable = _sqlHelper.ExecuteDataTable(goodsInfoQuery);
            return goodsInfoTable;
        }

        public DataTable GoodsInfoForTypeQuery(int _goodsTypeSubclassId) {
            string goodsInfoForTypeQuery = "Select goods_id,goods_name,goods_image,goods_price_now from Goods_Info where goods_type_subclass_id = '" + _goodsTypeSubclassId + "'";
            DataTable goodsInfoForTypeTable = _sqlHelper.ExecuteDataTable(goodsInfoForTypeQuery);
            return goodsInfoForTypeTable;
        }

        public DataTable GoodsInfoForKeywordsQuery(string _keywords) {
            string goodsInfoForKeywordsQuery = "Select goods_id,goods_name,goods_image,goods_price_now from Goods_Info where goods_name like '%" + _keywords + "%'";
            DataTable goodsInfoForKeywordsTable = _sqlHelper.ExecuteDataTable(goodsInfoForKeywordsQuery);
            return goodsInfoForKeywordsTable;
        }
        //以下三个方法：对于某个商品的管理页面
        public DataTable GoodsInfoQuery(int _goodsId) {
            string goodsInfoQuery = "Select * from Goods_Info where goods_id = '" + _goodsId + "'";
            DataTable goodsInfoTable = _sqlHelper.ExecuteDataTable(goodsInfoQuery);
            return goodsInfoTable;
        }

        public DataTable GoodsRemarkQuery(int _goodsId) {
            string goodsRemarkQuery = "Select * from Goods_Remark where goods_id = '" + _goodsId + "'";
            DataTable goodsRemarkTable = _sqlHelper.ExecuteDataTable(goodsRemarkQuery);
            return goodsRemarkTable;
        }

        public DataTable GoodsRemarkUserQuery(int _goodsRemarkId)
        {
            string goodsRemarkUserQuery = "Select user_name,user_isForbided from User_Acc where user_id = (Select user_id from Goods_Remark where goods_remark_id ='" + _goodsRemarkId + "')";
            DataTable goodsRemarkUserTable = _sqlHelper.ExecuteDataTable(goodsRemarkUserQuery);
            return goodsRemarkUserTable;
        }

        //添加商品
        public int GoodsAdd(GoodsInfo _goodsInfo) {
            string goodsAdd = "insert into Goods_Info(goods_type_subclass_id,goods_name,goods_image,goods_intro,goods_price_now,goods_storage,goods_status) values('" + _goodsInfo.GoodsTypeSubclassId + "','" + _goodsInfo.GoodsName + "','" + _goodsInfo.GoodsImage + "','" + _goodsInfo.GoodsIntro + "','" + _goodsInfo.GoodsPriceNow + "','" + _goodsInfo.GoodsStorage + "',1)";
            int rows = _sqlHelper.ExecuteNonQuery(goodsAdd);
            return rows;
        }

        //查询某子类别下是否有商品
        public bool isNoGoods(int _goodsTypeSubclassId)
        {
            string goodsNum = "Select * from Goods_Info where goods_type_subclass_id ='" + _goodsTypeSubclassId + "'";
            DataTable rows = _sqlHelper.ExecuteDataTable(goodsNum);
            if (rows.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
