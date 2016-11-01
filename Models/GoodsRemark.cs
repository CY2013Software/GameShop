using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class GoodsRemark
    {
        /// <summary>
        ///评论ID
        /// </summary>
        public int GoodsRemarkId { set; get; }

        /// <summary>
        ///商品ID
        /// </summary>
        public int GoodsId { set; get; }

        /// <summary>
        ///会员ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        ///首次评论内容
        /// </summary>
        public string GoodsRemarkFirst { set; get; }

        /// <summary>
        ///追加评论内容
        /// </summary>
        public string GoodsRemarkAdd { set; get; }

        /// <summary>
        ///首次评论时间
        /// </summary>
        public DateTime GoodsRemarkFirstTime { set; get; }

        /// <summary>
        ///追加评论时间
        /// </summary>
        public DateTime GoodsRemarkAddTime { set; get; }
    }
}
