using System.ComponentModel.DataAnnotations;

namespace Findlover.Models
{
    /// <summary>
    /// 择偶需求
    /// </summary>
    public class Requirement 
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "身高")]
        public int HightLl { get; set; }

        public int HightUl { get; set; }

        [Display(Name = "年龄")]
        public int AgeLl { get; set; }

        public int AgeUl { get; set; }
        [MaxLength(20)]
        [Display(Name = "学历")]
        public string Education { get; set; }
        [Display(Name = "月收入")]
        public int MonthlyIncomeLl { get; set; }

        public int MonthlyIncomeUl { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        [MaxLength(20)]
        [Display(Name = "居住地")]
        public string ResidenceProvince { get; set; }

       
        [MaxLength(10)]
        public string ResidenceCity { get; set; }

       
    }
}