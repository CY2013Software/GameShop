using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shop.Models
{
    public class GoodsTypeSubclass
    {
        /// <summary>
        /// 小类别ID、所属大类别ID、小类别名称
        /// </summary>
        public int GoodsTypeSubclassId { set; get; }
        public int GoodsTypeId { set; get; }
        public string GoodsTypeSubclassName { set; get; }
    }
}