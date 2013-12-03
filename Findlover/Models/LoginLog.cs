using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
   /// <summary>
   /// 登陆日志
   /// </summary>
    public class LoginLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(100)]
        public string IpAddress { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }

    }
    /// <summary>
    /// 收藏表
    /// </summary>
    public class Collect
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CollectedUserId { get; set; }
        public DateTime ActionTime { get; set; }
    }
}