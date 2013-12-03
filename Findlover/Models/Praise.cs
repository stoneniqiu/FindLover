using System;

namespace Findlover.Models
{
    /// <summary>
    /// 赞 是对文字传情，状态的赞
    /// </summary>
    public class Praise
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ToUserId { get; set; }
        public StateType StateType { get; set; }
        /// <summary>
        /// 别人的状态id
        /// </summary>
        public int StateId { get; set; } // 和状态和图片(imagebox) 关联起来。
        public DateTime ActionTime { get; set; }
    }
}