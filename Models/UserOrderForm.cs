using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class UserOrderForm
    {
        /// <summary>
        ///订单ID
        /// </summary>
        public int UserOrderFormId { set; get; }

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
        ///下单时间
        /// </summary>
        public DateTime GoodsPurchaseTime { set; get; }

        /// <summary>
        ///付款时间
        /// </summary>
        public DateTime GoodsPaymentTime { set; get; }

        /// <summary>
        ///确认收货倒计时的开始时间
        /// </summary>
        public DateTime GoodsVerifyStartTime { set; get; }

        /// <summary>
        ///订单状态号
        /// </summary>
        public int UserOrderFormStatusId { set; get; }
    }
}
