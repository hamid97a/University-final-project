using Law.Convertors;
using Law.DAL;
using Law.Models;
using Law.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Law.BLL
{
    public class RuleBLL
    {
        private RuleDAL RuleDAL => RuleDAL.Instance;
        public IQueryable<Rule> GetListQuery()
        {
            var returnedList = RuleDAL.GetListQuery();
            return returnedList.OrderByDescending(t => t.ApprovalDate);
        }

        public IQueryable<Rule> SearchInGrid(IQueryable<Rule> query, string searchField= "Title", string searchString="")
        {
            switch (searchField)
            {
                case "RuleId":
                    query = query.Where(t => t.RuleId == (int.Parse(searchString)));
                    break;
                case "Title":
                    query = query.Where(t => t.Title.Contains(searchString));
                    break;
                case "ApprovalDate":
                    query = query.Where(t => t.ApprovalDate.ToShamsi() == searchString);
                    break;
                case "AnnouncementDate":
                    query = query.Where(t => t.AnnouncementDate.ToShamsi() == searchString);
                    break;
            }
            return query;
        }

        public RuleViewModel Get(int Id)
        {
            return Mapper.Mapper.Map(RuleDAL.Get(Convert.ToInt32(Id)));
        }
    }
}