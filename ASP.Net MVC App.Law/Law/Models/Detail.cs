using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Law.Models
{
    public class Detail
    {
        public Detail()
        {

        }

        [Display(Name = "متن")]
        public string Text { get; set; }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RuleId { get; set; }

        public int ApprovedId { get; set; }

        [Display(Name = "شماره ابلاغیه")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        public string AnnouncementNumber { get; set; }

        [Display(Name = "ماده")]
        [MaxLength(10, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        public string Article { get; set; }


        [Required]
        public virtual Rule Rule { get; set; }
        [Required]
        public virtual Approved Approved { get; set; }

    }
}
