using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class UserPostInfo
    {
        /// <summary>
        ///邮递信息ID
        /// </summary>
        public int UserPostInfoId { set; get; }

        /// <summary>
        ///会员ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        ///通信地址
        /// </summary>
        public string UserAddress { set; get; }

        /// <summary>
        ///手机号码
        /// </summary>
        public string UserPhone { set; get; }

        /// <summary>
        ///邮编
        /// </summary>
        public string UserPostcode { set; get; }
    }
}
