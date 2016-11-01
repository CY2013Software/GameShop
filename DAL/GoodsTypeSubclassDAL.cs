using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shop.Models;
using System.Data;

namespace DAL
{
    public class GoodsTypeSubclassDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //打开GoodsList页面时运行的方法
        public DataTable GoodsTypeSubclassQuery(int _goodsTypeId)
        {
            string goodsTypeSubclassQuery = "Select goods_type_subclass_id,goods_type_subclass_name from Goods_Type_Subclass where goods_type_id = '"+_goodsTypeId+"'";
            DataTable goodsTypeSubclassTable = _sqlHelper.ExecuteDataTable(goodsTypeSubclassQuery);
            return goodsTypeSubclassTable;
        }

        //修改小类别名称
        public int GoodsTypeSubclassUpdate(int _goodsTypeSubclassId, string _goodsTypeSubclassName)
        {
            string goodsTypeSubclassUpdate = "Update Goods_Type_Subclass set goods_type_subclass_name='" + _goodsTypeSubclassName + "' where goods_type_subclass_id ='" + _goodsTypeSubclassId + "'";
            int rows = _sqlHelper.ExecuteNonQuery(goodsTypeSubclassUpdate);
            return rows;
        }

        //查询某大类别下是否有子类别
        public bool isNoSubclass(int _goodsTypeId) {
            string subclassNum = "Select * from Goods_Type_Subclass where goods_type_id ='" + _goodsTypeId + "'";
            DataTable rows = _sqlHelper.ExecuteDataTable(subclassNum);
            if (rows.Rows.Count > 0)
            {
                return false;
            }
            else {
                return true; 
            }
        }

        //删除小类别
        public int GoodsTypeSubclassDelete(int _goodsTypeSubclassId)
        {
            string goodsTypeSubclassDelete = "Delete from Goods_Type_Subclass where goods_type_subclass_id ='" + _goodsTypeSubclassId + "'";
            int rows = _sqlHelper.ExecuteNonQuery(goodsTypeSubclassDelete);
            return rows;
        }

        //增加商品子类别
        public int GoodsTypeSubInsert(GoodsTypeSubclass _goodsTypeSub) {
            string goodsTypeSubInsert = "Insert into Goods_Type_Subclass(goods_type_id,goods_type_subclass_name) values('" + _goodsTypeSub.GoodsTypeId + "','" + _goodsTypeSub.GoodsTypeSubclassName + "')";
            int rows = _sqlHelper.ExecuteNonQuery(goodsTypeSubInsert);
            return rows;
        }
    }
}
