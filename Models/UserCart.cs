using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class UserCart
    {
        /// <summary>
        /// 购物车ID
        /// </summary>
        public int CartId { set; get; }

        /// <summary>
        ///会员ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        ///商品ID
        /// </summary>
        public int GoodsId { set; get; }

        /// <summary>
        ///商品数量
        /// </summary>
        public int GoodsPurchaseQuantity { set; get; }

        /// <summary>
        /// 总价格
        /// </summary>
        public decimal TradeMoney { set; get; }
    }
}
