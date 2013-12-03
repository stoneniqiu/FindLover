using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Findlover.Models
{


    /// <summary>
    /// 基本资料
    /// </summary>
    public class BaseInfo
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        [Display(Name = "身高")]
        [MaxLength(10)]
        public string Height { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [MaxLength(20)]
        [Display(Name = "学历")]
        public string Education { get; set; }

        /// <summary>
        /// 月收入
        /// </summary>
        [MaxLength(15)]
        [Display(Name = "月收入")]
        public string MonthlyIncome { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        [MaxLength(20)]
        [Display(Name = "居住地")]
        public string ResidenceProvince { get; set; }


        [MaxLength(20)]
        public string ResidenceCity { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        [MaxLength(20)]
        [Display(Name = "毕业院校")]
        public string School { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        [MaxLength(20)]
        [Display(Name = "行业")]
        public string Profession { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "工作单位")]
        public string Company { get; set; }

        /// <summary>
        /// 目前职位
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "目前职位")]
        public string Position { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        [MaxLength(20)]
        [Display(Name = "当前状态")]
        public string State { get; set; }

        public bool IsChecked { get; set; }
    }

    /// <summary>
    /// 择偶需求
    /// </summary>
    public class RequirementClone
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "身高")]
        public string HightLl { get; set; }

        public string HightUl { get; set; }

        [Display(Name = "年龄")]
        public string AgeLl { get; set; }

        public string AgeUl { get; set; }

        [MaxLength(20)]
        [Display(Name = "学历")]
        public string Education { get; set; }

        [Display(Name = "月收入")]
        public string MonthlyIncomeLl { get; set; }

        public string MonthlyIncomeUl { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        [MaxLength(20)]
        [Display(Name = "居住地")]
        public string ResidenceProvince { get; set; }


        [MaxLength(10)]
        public string ResidenceCity { get; set; }
    }

    public class DetailInfo
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 住房情况
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "住房情况")]
        public string Housing { get; set; }

        [MaxLength(10)]
        [Display(Name = "购车情况")]
        public string Car { get; set; }

        [Display(Name = "体重")]
        public float Weight { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
       [MaxLength(10)]
        [Display(Name = "民族")]
        public string People { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        [MaxLength(20)]
        [Display(Name = "籍贯")]
         public string NativePlace { get; set; }

        [MaxLength(20)]
        [Display(Name = "籍贯")]
         public string NativeCity { get; set; }

        /// <summary>
        /// 星座
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "星座")]
        public string Constellation { get; set; }

        [MaxLength(10)]
        [Display(Name = "血型")]
        public string BloodType { get; set; }

        [MaxLength(200)]
        [Display(Name = "微博地址")]
        public string MicroBlog { get; set; }

        [MaxLength(200)]
        [Display(Name = "豆瓣地址")]
        public string DouBan { get; set; }
    }

    public class LoveView
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 工作时间安排
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "工作时间安排")]
        public string WorkTimePlan { get; set; }

        /// <summary>
        /// 是否吸烟
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "是否吸烟")]
        public string Smoking { get; set; }

        /// <summary>
        /// 是否喝酒
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "是否喝酒")]
        public string Drinking { get; set; }

        /// <summary>
        /// 恋爱多久结婚
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "恋爱多久结婚")]
        public string LoveDuration { get; set; }

        /// <summary>
        /// 婚后是否要孩子
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "婚后是否要孩子")]
        public string WantaBaby { get; set; }

        /// <summary>
        /// 婚后是否介意与父母同住
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "婚后是否介意与父母同住")]
        public string ParentLiveTogether { get; set; }

        /// <summary>
        /// 家庭中的家务应该由谁来承担
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "家庭中的家务应该由谁来承担")]
        public string Housework { get; set; }

        /// <summary>
        /// 婚后家庭理财方面，我更倾向于
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "婚后家庭理财方面，我更倾向于")]
        public string ManageMoney { get; set; }

        /// <summary>
        /// 厨艺如何？对对方的厨艺有无要求
        /// </summary>
        [MaxLength(30)]
        [Display(Name = "厨艺如何？对对方的厨艺有无要求")]
        public string Cooking { get; set; }

        [MaxLength(30)]
        [Display(Name = "是否接受异地恋")]
        public string Allopatry { get; set; }
    }

    public class Item  
    {
        public string Name  { get; set; }
        public object Value { get; set; }
      
    }
}