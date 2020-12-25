using Law.Context;
using Law.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Law.DAL
{
    public class RuleDAL
    {
        private static RuleDAL _instance;
        public static RuleDAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RuleDAL();
                }
                return _instance;
            }
        }
        public IQueryable<Rule> GetListQuery()
        {
            try
            {
                LawContext DB = new LawContext();

                return DB.Rules;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Rule Get(int id)
        {
            try
            {
                var result = GetListQuery().FirstOrDefault(t => t.RuleId == id);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}