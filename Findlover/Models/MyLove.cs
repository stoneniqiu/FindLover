using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
    /// <summary>
    /// 喜欢的人
    /// </summary>
    public class MyLove
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int LoverId { get; set; }
        /// <summary>
        /// 当信息一样处理。是否已经查看
        /// </summary>
        public bool IsRead { get; set; }
        public DateTime ActionTime { get; set; }
    }

    /// <summary>
    /// 不喜欢的人
    /// </summary>
    public class DisLove
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int DisLoveId { get; set; }
         public DateTime ActionTime { get; set; }
    
    }
}