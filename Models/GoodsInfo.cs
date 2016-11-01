using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class GoodsInfo
    {
        /// <summary>
        ///商品ID
        /// </summary>
        public int GoodsId { set; get; }

        /// <summary>
        ///商品小类别ID
        /// </summary>
        public int GoodsTypeSubclassId { set; get; }

        /// <summary>
        ///商品名称
        /// </summary>
        public string GoodsName { set; get; }

        /// <summary>
        ///商品图片路径
        /// </summary>
        public string GoodsImage { set; get; }

        /// <summary>
        ///商品简介
        /// </summary>
        public string GoodsIntro { set; get; }

        /// <summary>
        ///商品原价，原价要判断是否为空
        /// </summary>
        public decimal GoodsPriceOriginal { set; get; }

        /// <summary>
        ///商品单价
        /// </summary>
        public decimal GoodsPriceNow { set; get; }

        /// <summary>
        ///库存数量
        /// </summary>
        public int GoodsStorage { set; get; }

        /// <summary>
        ///上下架状态
        /// </summary>
        public bool GoodsStatus { set; get; }
    }
}
