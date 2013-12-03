using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Findlover.Models;

namespace Findlover.ViewModel
{
    public class RecommendRate
    {
        //用户热度
        public double UserHotRate { get; set; }
        //符合我的条件
        public double ForMeRate { get; set; }
        //符合别人的条件
        public double ForOtherRate { get; set; }
        //恋爱观匹配度
        public double LoveViwRate { get; set; }

        public double TotalRate { get; set;}
    }

    

    public class RecommendUser
    {
        public UninUser User { get; set; }

        public RecommendRate Rate { get; set; }

        public string InfoStr { get; set; }
        // 这个处理待定。
        public string LastSate { get; set; }

    }

}