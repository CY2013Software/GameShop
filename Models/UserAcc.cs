using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class UserAcc
    {
        /// <summary>
        ///会员ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        ///会员用户名
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        ///会员密码
        /// </summary>
        public string UserPwd { set; get; }

        /// <summary>
        ///会员邮箱
        /// </summary>
        public string UserEmail { set; get; }

        /// <summary>
        ///是否被封号
        /// </summary>
        public bool UserIsForbided { set; get; }

        /// <summary>
        ///是否已激活
        /// </summary>
        public bool UserStatus { set; get; }
    }
}
