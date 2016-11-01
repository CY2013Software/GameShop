using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class UserInfo
    {
        /// <summary>
        ///会员ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        ///会员性别
        /// </summary>
        public bool UserGender { set; get; }

        /// <summary>
        ///会员真实姓名
        /// </summary>
        public string UserRealname { set; get; }

        /// <summary>
        ///会员图片
        /// </summary>
        public string UserImage { set; get; }

        /// <summary>
        ///会员电话
        /// </summary>
        public string UserPhone { set; get; }

    }
}
