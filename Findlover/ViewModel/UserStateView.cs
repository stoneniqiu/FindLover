using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Findlover.Models;

namespace Findlover.ViewModel
{
    /// <summary>
    /// 专门的用户状态视图
    /// </summary>
    public class UserStateView
    {
        public User SatateUser { get; set; } //id ,name,age,hight,education,imgcount 
        public int ImgCount { get; set; }
        public string Hight { get; set; }
        public string Education { get; set; }
        public double RecommentRate { get; set; }//推荐的比率
        // 状态 有独白，回答问题，上传头像和上传图片  如何区分？
        public State State { get; set; }
    
    }
}