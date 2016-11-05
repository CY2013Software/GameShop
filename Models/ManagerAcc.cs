using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class ManagerAcc
    {
        /// <summary>
        ///账户
        /// </summary>
        public string ManagerAccount { set; get; }

        /// <summary>
        ///密码
        /// </summary>
        public string ManagerPwd { set; get; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string ManagerEmail { set; get; }

        /// <summary>
        ///姓名
        /// </summary>
        public string ManagerName { set; get; }

        /// <summary>
        ///性别
        /// </summary>
        public bool ManagerGender { set; get; }

        /// <summary>
        ///手机
        /// </summary>
        public string ManagerPhone { set; get; }

        /// <summary>
        ///类型
        /// </summary>
        public bool ManagerType { set; get; }

        /// <summary>
        ///状态
        /// </summary>
        public bool ManagerStatus { set; get; }
    }
}
