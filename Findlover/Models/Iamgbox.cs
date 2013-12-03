using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Findlover.Models
{
    /// <summary>
    /// 相册
    /// </summary>
    public class Iamgbox
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(20)]
        public string BoxName { get; set; }
        [MaxLength(100)]
        public string ImgUrl { get; set; }
        /// <summary>
        /// 图片备注
        /// </summary>
        public string Remark { get; set; }
        [DefaultValue(true)]
        public bool IsValid { get; set; }
        /// <summary>
        /// 浏览的次数
        /// </summary>
        public int VisitCount { get; set; }

        public int PraiseCount { get; set; }

        public DateTime ActionTime { get; set; }
    }
}