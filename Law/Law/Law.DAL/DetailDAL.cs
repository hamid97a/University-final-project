using Law.Context;
using Law.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Law.DAL
{
    public class DetailDAL
    {
        private static DetailDAL _instance;
        public static DetailDAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DetailDAL();
                }
                return _instance;
            }
        }

        public Detail Get(int? id)
        {
            try
            {
                LawContext DB = new LawContext();

                var result = DB.Details.SingleOrDefault(t => t.RuleId == id);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}