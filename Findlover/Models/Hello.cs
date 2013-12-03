using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Findlover.Models
{
    /// <summary>
    /// 打招呼
    /// </summary>
   //[Bind(Include = "Content,UserId")]
    public class Hello
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ToUserId { get; set; }
        public DateTime ActionTime { get; set; }

    }

    

}