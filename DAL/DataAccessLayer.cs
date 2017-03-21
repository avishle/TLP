using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TLP.Models;

namespace TLP.DAL
{
    public class DataAccessLayer: DbContext 
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>().ToTable("tblUsers");
        }
        
        public DbSet<User> Users { get; set; }
    }
}