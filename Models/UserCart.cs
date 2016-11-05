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
    }
}
