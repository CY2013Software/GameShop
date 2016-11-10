using shop.Models;
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

        //向数据库中添加评论信息
        public int InsertRemark( GoodsRemark _goodsRemark) {
            int rows = 0;
            DateTime now = DateTime.Now;
            string remarkCheck = "Select goods_remark_id from Goods_Remark where goods_id = '" + _goodsRemark.GoodsId + "' and user_id = '"+ _goodsRemark.UserId +"'";
            DataTable remarkCheckTable = _sqlHelper.ExecuteDataTable(remarkCheck);
            if (remarkCheckTable.Rows.Count > 0)
            {
                string addRemark = "Update Goods_Remark set goods_remark_add = '" + _goodsRemark.GoodsRemarkFirst + "',goods_remark_addTime = '" + now + "' where goods_id = '" + _goodsRemark.GoodsId + "' and user_id = '" + _goodsRemark.UserId + "'";
                rows = _sqlHelper.ExecuteNonQuery(addRemark);
            }
            else {
                string insertRemark = "Insert into Goods_Remark(goods_id,user_id,goods_remark_first ,goods_remark_firstTime) values('" + _goodsRemark.GoodsId + "','"+ _goodsRemark.UserId +"','"+ _goodsRemark.GoodsRemarkFirst +"','"+ now +"')";
                rows = _sqlHelper.ExecuteNonQuery(insertRemark);
            }
            return rows;
        }
    }
}
