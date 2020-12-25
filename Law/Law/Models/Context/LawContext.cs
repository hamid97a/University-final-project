
using Law.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Law.Context
{
    public class LawContext:DbContext
    {
        public LawContext()
        {

        }

        public DbSet<Rule> Rules { get; set; }
        public DbSet<Approved> Approveds { get; set; }
        public DbSet<Detail> Details { get; set; }
    }
}