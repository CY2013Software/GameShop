using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GoodsRemarkDAL
    {
        //数据库操作对象
        SqlHelper _sqlHelper = new SqlHelper();

        //按商品id查询某商品评论信息
        public DataTable GoodsRemarkQuery(int _goodsId)
        {
            string goodsRemarkQuery = "Select * from Goods_Remark where goods_id = '" + _goodsId + "'";
            DataTable goodsRemarkTable = _sqlHelper.ExecuteDataTable(goodsRemarkQuery);
            return goodsRemarkTable;
        }

        //按id删除某条评论（初评或追评）
        public int GoodsRemarkDelete(int id,int _goodsRemarkId) {
            string goodsRemarkDelete = "";
            int rows = 0;
            if (id == 0)                                                 //删除初评
            {
                goodsRemarkDelete = "Update Goods_Remark set goods_remark_first = NULL,goods_remark_firstTime = NULL where goods_remark_id = '" + _goodsRemarkId + "'";
                rows = _sqlHelper.ExecuteNonQuery(goodsRemarkDelete);
            }else if(id == 1)                                         //删除追评
            {
                goodsRemarkDelete = "Update Goods_Remark set goods_remark_add = NULL,goods_remark_addTime = NULL where goods_remark_id = '" + _goodsRemarkId + "'";
                rows = _sqlHelper.ExecuteNonQuery(goodsRemarkDelete);
            }
            return rows;
        }

        //根据评论id查找商品id
        public DataTable GoodsId(int _goodsRemarkId) {
            string goodsId = "Select goods_id from Goods_Remark where goods_remark_id = '" + _goodsRemarkId + "'";
            DataTable goodsIdTable = _sqlHelper.ExecuteDataTable(goodsId);
            return goodsIdTable;
        }
    }
}
