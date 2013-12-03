using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Findlover.Models
{
    /// <summary>
    /// 管理员角色表
    /// </summary>
    public class Role
    {
      //  private Authority _authority=new Authority();

        public int Id { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 系统管理员，管理员,（其它人默认一般用户）
        /// </summary>
        [MaxLength(10)]
        public string RoleType { get; set; }

        //public Authority Authority
        //{
        //    get { return _authority ?? (_authority = new Authority()); }
        //    set { _authority = value; }
        //}

        /// <summary>
        /// 成为管理员的时间
        /// </summary>
        public DateTime ActionTime { get; set; }

        public Role(string role,int userid)
        {
            //switch (role)
            //{
            //        case Models.RoleType.Admin://一般管理员
            //        _authority.CheckUser = true;
            //        _authority.AddQuestion = true;
            //        break;
            //        case Models.RoleType.Editor://小编
            //        _authority.PublishArticle = true;
            //        break;
            //        case Models.RoleType.Guest://嘉宾
            //        _authority.AnswerQuestion = true;
            //        break;
            //        case Models.RoleType.SysAdmin://系统管理员
            //        _authority.Admin = true;  
            //        _authority.PublishArticle = true;
            //        _authority.AnswerQuestion = true;
            //        _authority.CheckUser = true;
            //        _authority.AddQuestion = true;
            //        break;
            //}
            //_authority.RoleId = this.Id;//这个地方有漏洞，此时id还是为空的。但权限id是对的 
            ActionTime = DateTime.Now;
            RoleType = role;
            UserId = userid;

        }

        public Role()
        {

        }
    }

    public class RoleLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(100)]
        public string Detail { get; set; }//记录管理员升迁，变角色之类的。
        [MaxLength(20)]
        public string ActionUserName { get; set; }
        public int ActionUserId { get; set; }
        public DateTime ActionTime { get; set; }
    }
    /// <summary>
    /// 一般管理员的数据 添加管理员的时候要配套加上这张表
    /// </summary>
    public class AdminStatistic
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CheckImgCount { get; set; }// 审核的照片数目
        public int CheckInfoCount { get; set; }//审核的基本资料数目
        public DateTime LastLogin { get; set; }// 最近的登录时间 
        public int SignInDays { get; set;}
        public int Experience { get; set; }// 经验值。
        public int Gold { get; set; } //金币数
    }

    public class Authority// 管理员默认有checkuser权限  ，系统管理员多一个admin权限。
    {
        public int Id { get; set; }
        public int RoleId { get; set; }//管理员id
        [DefaultValue(false)]
        public bool CheckUser { get; set; }// 检查用户信息是否属实。包括img
        [DefaultValue(false)]
        public bool AddQuestion { get; set; }//添加问题的权限
       [DefaultValue(false)]
        public bool PublishArticle { get; set; }//发表文章
       [DefaultValue(false)]
        public bool AnswerQuestion { get; set; }//回答情感类文章。
       [DefaultValue(false)]
        public bool Admin { get; set; }//管理管理员
    }



    public enum RoleType
    {
        SysAdmin,//系统管理员
        Admin,//管理员
        Guest,//嘉宾
        Editor//小编
    }

   
}