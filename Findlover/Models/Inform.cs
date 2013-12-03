using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
    /// <summary>
    /// 举报表
    /// </summary>
    public class Inform
    {
        public int Id { get; set; }
        //举报者id
        public int UserId { get; set; }
        //被举报的id
        public int InformUserId { get; set; }

        // 待完善
    }
}