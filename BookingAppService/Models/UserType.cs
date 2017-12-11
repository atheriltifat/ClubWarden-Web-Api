using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

// author: Ather Iltifat
namespace BookingAppService.Models
{
    [Table("member_type")]
    public class UserType
    {
        [Key]
        [Column("memberType_ID")]
        public int userTypeID { get; set; }
        [Column("memberType")]
        public string userType { get; set; }

        public virtual ICollection<PasswordTbl> listPasswordTbl { get; set; }
        public virtual ICollection<User> listUser { get; set; }
    }
}