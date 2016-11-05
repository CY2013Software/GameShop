using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Models
{
    public class PwdChange
    {
        public string UserName { set; get; }
        public string OriginalPwd { set;get;  }
        public string NewPwd { set; get; }
    }
}
