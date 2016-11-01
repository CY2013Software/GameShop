using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shop.Models;
using System.Data;

namespace DAL
{
    public class GoodsTypeDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //打开GoodsList页面时运行的方法
        public DataTable GoodsTypeQuery()
        {
            string goodsTypeQuery = "Select goods_type_id,goods_type_name from Goods_Type";
            DataTable goodsTypeTable = _sqlHelper.ExecuteDataTable(goodsTypeQuery);
            return goodsTypeTable;
        }

        //修改大类别名称
        public int GoodsTypeUpdate(int _goodsTypeId, string _goodsTypeName)
        {
            string goodsTypeUpdate = "Update Goods_Type set goods_type_name ='" + _goodsTypeName + "' where goods_type_id ='" + _goodsTypeId + "'";
            int rows = _sqlHelper.ExecuteNonQuery(goodsTypeUpdate);
            return rows;
        }

        //删除大类别
        public int GoodsTypeDelete(int _goodsTypeId)
        {
            string goodsTypeDelete = "Delete from Goods_Type where goods_type_id ='"  + _goodsTypeId + "'";
            int rows = _sqlHelper.ExecuteNonQuery(goodsTypeDelete);
            return rows;
        }

        //增加商品大类别
        public int GoodsTypeInsert(string _goodsTypeName) {
            string goodsTypeInsert = "Insert into Goods_Type(goods_type_name) values('" + _goodsTypeName + "')";
            int rows = _sqlHelper.ExecuteNonQuery(goodsTypeInsert);
            return rows;
        }
    }
}
