using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Law.Models
{
    public class Rule
    {
        public Rule()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? RuleId { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "تاریخ تصویب")]
        public DateTime ApprovalDate { get; set; }

        [Display(Name = "تاریخ ابلاغیه")]
        public DateTime AnnouncementDate { get; set; }


        public virtual Detail Detail { get; set; }
    }
}
