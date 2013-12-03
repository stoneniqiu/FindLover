using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Findlover.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        [MaxLength(40)]
        public string UserName { get; set; }
        public bool IsValid { get; set; }
        [MaxLength(400)]
        public string Content { get; set; }
        [DefaultValue(0)]
        public int ReplyId { get; set; }//是否是回复  回复之后引用
        public DateTime ActionTime { get; set; }
    }
}