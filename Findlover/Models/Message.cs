using System;
using System.ComponentModel.DataAnnotations;

namespace Findlover.Models
{
    /// <summary>
    /// 私信
    /// </summary>
    public class Message
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        [MaxLength(10)]
        public string FromUserName { get; set; }

        //[MaxLength(200)]
        //public string ImgUrl { get; set; }
        public int ToUserId { get; set; }
        /// <summary>
        /// 是否已阅读 属于ToUserId
        /// </summary>
        public bool IsReaded { get; set; }

        [MaxLength(450)]
        public string Content { get; set; }

        public MegType MegType { get; set; }//只有是私信的时候 再看statetype和stateId

        /// <summary>
        /// 记录 评论的是状态 还是照片
        /// </summary>
        public StateType StateType { get; set; }
        /// <summary>
        /// StateId不太准确,就是相关Id。 
        /// </summary>
        public int StateId { get; set; } // 和状态和图片(imagebox) 关联起来。


        public DateTime ActionTime { get; set; }
    }
}