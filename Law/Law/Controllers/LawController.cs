using Law.BLL;
using Law.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Law.Controllers
{
    public class LawController : Controller
    {
        private RuleBLL _instanceRule => new RuleBLL();
        private DetailBLL _instanceDetail => new DetailBLL();


        public ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult ShowGrid(string sidx, string sord, int page, int rows,
                                          bool _search, string searchField, string searchString,
                                          string searchOper, string filters)
        {
            try
            {
                int pageIndex = page - 1;
                int pageSize = rows;
                var todoListsResults = _instanceRule.GetListQuery();
                int totalRecords = todoListsResults.Count();
                var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);
                //Search Toolbar
                //todoListsResults = new JqGridSearch().ApplyFilter(todoListsResults, _search, searchField, searchString,
                //    searchOper, filters, this.Request.Form);

                // Setting Sorting  
                if (sord.ToUpper() != "DESC")
                    todoListsResults = todoListsResults.OrderBy(s => s.ApprovalDate);

                if (_search)
                    todoListsResults = _instanceRule.SearchInGrid(todoListsResults, searchField, searchString);

                //todoListsResults = todoListsResults.OrderBy(s => s.ApprovalDate);
                //todoListsResults = todoListsResults.Skip(pageIndex * pageSize).Take(pageSize);

                var jsonRows2 = todoListsResults.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                //var jsonRows3 = jsonRows2.AsEnumerable();

                //System.Threading.Thread.Sleep(1000);
                var jsonRows = new List<RuleViewModel>();//jsonRows3.Select(Mapper.Mapper.Map).ToList();

                while (jsonRows.Count == 0)
                {
                    try
                    {
                        jsonRows = jsonRows2.Select(Mapper.Mapper.Map).ToList();
                    }
                    catch
                    {

                    }
                }

                var FinalRows = jsonRows.Select(row => new
                {
                    row.RuleId,
                    row.Title,
                    row.ApprovalDate,
                    row.AnnouncementDate

                }).ToList();

                // Sending Json Object to View.
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = FinalRows
                };

                var jsonObject = Json(jsonData, JsonRequestBehavior.AllowGet);
                jsonObject.MaxJsonLength = int.MaxValue;
                return jsonObject;
            }
            catch (Exception)
            {
                return null;
            }
        }


        // GET: Detail
        //[HttpPost]
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                var detail = _instanceDetail.Get(id);
                return View(detail);
            }
            return Redirect("/Law/Index");
        }

    }
}