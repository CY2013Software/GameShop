using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shop.Models
{
    public class GoodsType
    {
        /// <summary>
        /// 大类别ID和大类别名称
        /// </summary>
        public int GoodsTypeId { set; get; }

        public string GoodsTypeName { set; get; }
    }
}