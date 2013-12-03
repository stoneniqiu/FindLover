
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
    /// <summary>
    /// 举报表
    /// </summary>
    public class Report
    {
        public int Id { get; set; }
        /// <summary>
        /// 举报人
        /// </summary>
        public int UserId { get; set; }
          [MaxLength(20)]
        public string UserName { get; set; }
        /// <summary>
        /// 被举报人id
        /// </summary>
        public int ReportedUserId { get; set; }

         [MaxLength(20)]
        public string ReportedUserName { get; set; }

        public ReportType ReportType { get; set; }

        public ReportMessageType MessageType { get; set; }

        /// <summary>
        /// 相关Id 可能是信息的，图片的.... 便于定位
        /// </summary>
        public int RelateId { get; set; }
     

        [MaxLength(100)]
        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime ActionTime { get; set; }
    }

    public class ReportLog
    {
        public int Id { get; set; }
        /// <summary>
        /// 举报人
        /// </summary>
        public int AdminId { get; set; }
        /// <summary>
        /// 举报信息的Id
        /// </summary>
        public int ReportId { get; set; }

        public DateTime ActionTime { get; set; }
    }

    /// <summary>
    /// 举报类型
    /// </summary>
    public enum ReportType
    {
        Personal,
        Message,
        Image,
        State,
        Topic,
        Comment,
    }

    public enum ReportMessageType
    {
        /// <summary>
        /// 没有类型，这里只是举报人。
        /// </summary>
        None,
        /// <summary>
        /// 色情污秽，语言粗暴
        /// </summary>
        Pornographic,
        /// <summary>
        /// 虚假资料冒用
        /// </summary>
        FakeMssage,
        /// <summary>
        /// 骚扰 
        /// </summary>
        Harass,
        /// <summary>
        /// 广告
        /// </summary>
        Advertisement,
        /// <summary>
        /// 诈骗信息
        /// </summary>
        Swindle,
        /// <summary>
        /// 其他理由
        /// </summary>
        Others,

        

    }
}