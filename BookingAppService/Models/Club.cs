using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

// author: Ather Iltifat
namespace BookingAppService.Models
{
    [Table("club")]
    public class Club
    {
        [Key]
        public int clubID { get; set; }
        public string clubName { get; set; }
        public string clubCity { get; set; }
        public string clubCountry { get; set; }

        public virtual ICollection<BookingTable> bookingTable { get; set; }
        public virtual ICollection<PasswordTbl> passwordTbl { get; set; }
        public virtual ICollection<User> user { get; set; }
    }
}