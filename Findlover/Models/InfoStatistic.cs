using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
   /// <summary>
   /// 资料统计表，统计资料的完整度
   /// </summary>
    public class InfoStatistic
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        /// <summary>
        /// 基本信息表基数
        /// </summary>
        [DefaultValue(9)]
        public int BaseInfoBase { get; set; }

        [DefaultValue(0)]
        public int BaseInfoReal { get; set; }

        [DefaultValue(9)]
        public int DetialsInfoBase { get; set; }

        [DefaultValue(0)]
        public int DetialsInfoReal { get; set; }

        [DefaultValue(10)]
        public int LoveViewsBase { get; set; }

        [DefaultValue(0)]
        public int LoveViewsReal { get; set; }

        public int ImgCount { get; set; }

       /// <summary>
        /// 百分比
        /// </summary>
        public float Percent { get; set; }
    }
}