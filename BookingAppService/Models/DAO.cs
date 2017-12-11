using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

// author: Ather Iltifat
namespace BookingAppService.Models
{
    public class DAO : DbContext
    {
        public DbSet<User> User_DBset { get; set; }
        public DbSet<BookingTable> BookingTable_DBset { get; set; }
        public DbSet<UserType> UserType_DBset { get; set; }
        public DbSet<PasswordTbl> PasswordTbl_DBset { get; set; }
        public DbSet<Club> Club_DBset { get; set; }
        public DbSet<CourtType> CourtType_DBset { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}