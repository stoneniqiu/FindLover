using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Findlover.Models
{
  
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [MaxLength(10)]
        
        public string UserName { get; set; }

        [MaxLength(10)]
        public string Sex { get; set; }

        public DateTime BrithDate { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

       // public int LogCount { get; set; }

        public DateTime RegisterTime { get; set; }

        [MaxLength(100)]
        public string ImgUrl { get; set; }


        public int Age { get; set; }

        /// <summary>
        /// 用户是否禁用。
        /// </summary>
        public int Enable { get; set; }

        /// <summary>
        /// 资料是否公开
        /// </summary>
       [DefaultValue(true)]
        public bool IsOpen { get; set; }

        /// <summary>
        /// 身份是否合法
        /// </summary>
       [DefaultValue(false)]
        public bool IsVerified { get; set; }

        /// <summary>
        /// 是否已经检查过个人资料   
        /// </summary>
       [DefaultValue(false)]
       public bool IsChecked { get; set; }

        /// <summary>
        /// 图片是否合格
        /// </summary>
       [DefaultValue(false)]
        public bool IsVerifiedImg { get; set; }

        /// <summary>
        /// 是否检查过图片 修改图片后都要重置
        /// </summary>
       [DefaultValue(false)]
       public bool IsCheckedImg { get; set; }

        public User()
        {
            ImgUrl = "../../Content/Photos/ali1.png";
        }
        //  baseinfo的资料搬到这里


        // detailinfo的资料搬到这里



    }
    public class LogOnModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "用户名")]
        [Remote("CheckUserName", "User", ErrorMessage = "该用户名已存在!")]
        [MinLength(2,ErrorMessage = "名称不能太短")]
        [MaxLength(10,ErrorMessage ="名称不能太长" )]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "电子邮件地址")]
           // [RegularExpression(@"[A-Za-z0-9._%+-]+@[deltaww.com.cn]", ErrorMessage = "请输入正确的email")]
       [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "请输入正确的email")]
        [Remote("CheckMail", "User", ErrorMessage = "非公司邮箱或该邮箱已经注册过")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "男生/女生")]
        public string Sex { get; set; }

        [Required]
        [Display(Name = "出生日期")]
        [Remote("CheckBrithDate", "User", ErrorMessage = "日期不正确或不合理")]
        public DateTime BrithDate { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "密码和确认密码不匹配")]
        public string ConfirmPassword { get; set; }

         

    }

}