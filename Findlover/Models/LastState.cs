using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
    /// <summary>
    /// 记录用户最新消息
    /// </summary>
    public class LastState
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //记录最新消息对象的id 
       // public int StateId { get; set; }
        [MaxLength(50)]
        public string StateTypeName { get; set;}
        public DateTime StateTriggerTime { get; set; }
    }
}