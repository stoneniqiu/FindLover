using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Findlover.Models;

namespace Findlover.ViewModel
{
    /// <summary>
    /// 有回复的评论
    /// </summary>
    public class UninComments
    {
        public Comment Comment { get; set; }
        public Comment ReComment { get; set; }
        public UninUser User { get; set; }
        public string Sex { get; set; }

    }
}