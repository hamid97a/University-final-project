using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.ViewModel
{
    public class DetailViewModel
    {
        public int? RuleId { get; set; }

        [Display(Name = "متن")]
        public string Text { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "مرجع تصویب")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        public string ApprovedName { get; set; }

        [Display(Name = "شماره ابلاغیه")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        public string AnnouncementNumber { get; set; }

        [Display(Name = "ماده")]
        [MaxLength(10, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        public string Article { get; set; }

        [Display(Name = "تاریخ تصویب")]
        public string ApprovalDate { get; set; }

        [Display(Name = "تاریخ ابلاغیه")]
        public string AnnouncementDate { get; set; }
    }
}
