using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Findlover.Models;

namespace Findlover.ViewModel
{
    public class ReportDetailUser
    {
        public UninUser UninUser { get; set; }
        public List<SimpleReport> Reports { get; set; }
        public ReportSum Sum { get; set; }
    }

    public class SimpleReport
    {
        public Report ReportRaw { get; set; }
        public string Content { get; set; }
    }

    //数据统计
    public class ReportSum
    {
        //被举报的次数
        public int ReportedNum { get; set; }
       //未处理的条数
        public int UnDone { get; set; }
        public int Done { get; set; }

    }

}