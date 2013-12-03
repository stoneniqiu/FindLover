using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(40)]
        public string UserName { get; set; }
        public bool IsValid { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength]//注意返回项 
        public string Content { get; set; }
        public DateTime ActionTime { get; set; }
        public DateTime UpDataTime { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }//喜欢的次数。
        public bool IsRecomend { get; set; }//是否是管理员推荐的。
        public bool IsNew { get; set; }
    }

    // 周最热话题排行  
}