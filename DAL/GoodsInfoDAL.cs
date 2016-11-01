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

        //按类别查询商品
        public DataTable GoodsInfoForTypeQuery(int _goodsTypeSubclassId) {
            string goodsInfoForTypeQuery = "Select goods_id,goods_name,goods_image,goods_price_now from Goods_Info where goods_type_subclass_id = '" + _goodsTypeSubclassId + "'";
            DataTable goodsInfoForTypeTable = _sqlHelper.ExecuteDataTable(goodsInfoForTypeQuery);
            return goodsInfoForTypeTable;
        }

        //按关键字查询商品
        public DataTable GoodsInfoForKeywordsQuery(string _keywords) {
            string goodsInfoForKeywordsQuery = "Select goods_id,goods_name,goods_image,goods_price_now from Goods_Info where goods_name like '%" + _keywords + "%'";
            DataTable goodsInfoForKeywordsTable = _sqlHelper.ExecuteDataTable(goodsInfoForKeywordsQuery);
            return goodsInfoForKeywordsTable;
        }

                  //按id查询某商品信息
        public DataTable GoodsInfoQuery(int _goodsId) {
            string goodsInfoQuery = "Select * from Goods_Info where goods_id = '" + _goodsId + "'";
            DataTable goodsInfoTable = _sqlHelper.ExecuteDataTable(goodsInfoQuery);
            return goodsInfoTable;
        }
               
             //按id更改某商品信息
        public int GoodsInfoUpdate(GoodsInfo _goodsInfo) {
            string goodsUpdate = "Update Goods_Info set goods_name = '" + _goodsInfo.GoodsName + "',goods_image = '" + _goodsInfo.GoodsImage + "',goods_intro = '" + _goodsInfo.GoodsIntro + "',goods_price_original = '" + _goodsInfo.GoodsPriceOriginal + "',goods_price_now = '" + _goodsInfo.GoodsPriceNow + "',goods_storage = '" + _goodsInfo.GoodsStorage + "',goods_status = '" + _goodsInfo.GoodsStatus + "' where goods_id = '" + _goodsInfo.GoodsId + "'";
            int rows = _sqlHelper.ExecuteNonQuery(goodsUpdate);
            return rows;
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
