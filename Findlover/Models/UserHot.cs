using System.ComponentModel.DataAnnotations;

namespace Findlover.Models
{
    /// <summary>
    /// 用户热度
    /// </summary>
    public class UserHot
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Display(Name = "被访问的次数")]
        public int VistitCount { get; set; }

        [Display(Name = "被赞的次数")]
        public int PraiseCount { get; set; }

        [Display(Name = "被收藏的次数")]
        public int CollectCount { get; set; }

        [Display(Name = "被邀请的次数")]
        public int InvitationCount { get; set; }

        [Display(Name = "被打招呼的次数")]
        public int HelloCount { get; set; }

        public int LogCount { get; set; }

        /// <summary>
        /// 热度值
        /// </summary>
        public int HotValue { get; set; }
    }
}