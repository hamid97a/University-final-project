using Law.Convertors;
using Law.Models;
using Law.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Law.Mapper
{
    public class Mapper
    {
        public static RuleViewModel Map(Rule entity)
        {
            if (entity == null)
                return null;

            var result = new RuleViewModel
            {
                RuleId = entity.RuleId,
                Title = entity.Title,
                ApprovalDate = (entity.ApprovalDate.Year !=1900) ? entity.ApprovalDate.ToShamsi() : "",
                AnnouncementDate = (entity.AnnouncementDate.Year != 1900) ? entity.AnnouncementDate.ToShamsi() : ""

            };
            return result;
        }

        public static DetailViewModel Map(Detail entity)
        {
            if (entity == null)
                return null;

            var result = new DetailViewModel
            {
                RuleId = entity.RuleId,
                Title = entity.Rule.Title,
                Text = entity.Text,
                ApprovalDate = (entity.Rule.ApprovalDate.Year != 1900) ? entity.Rule.ApprovalDate.ToShamsi() : "",
                AnnouncementDate = (entity.Rule.AnnouncementDate.Year != 1900) ? entity.Rule.AnnouncementDate.ToShamsi() : "",
                AnnouncementNumber = entity.AnnouncementNumber,
                ApprovedName = entity.Approved.ApprovedName,
                Article = entity.Article
            };
            return result;
        }
    }
}