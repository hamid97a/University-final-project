using Law.DAL;
using Law.Models;
using Law.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Law.BLL
{
    public class DetailBLL
    {
        private DetailDAL DetailDAL => DetailDAL.Instance;

        public DetailViewModel Get(int? Id)
        {
            return Mapper.Mapper.Map(DetailDAL.Get(Id));
        }
    }
}