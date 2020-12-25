﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.ViewModel
{
    public class RuleViewModel
    {
        public int? RuleId { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "تاریخ تصویب")]
        public string ApprovalDate { get; set; }

        [Display(Name = "تاریخ ابلاغیه")]
        public string AnnouncementDate { get; set; }
    }
}
