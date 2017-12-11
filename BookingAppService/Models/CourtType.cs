using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

// author: Ather Iltifat
namespace BookingAppService.Models
{
    [Table("courttype")]
    public class CourtType
    {
        [Key]
        public int courtTypeID { get; set; }
        public string courtType { get; set; }

        public virtual ICollection<BookingTable> bookingTable { get; set; }
    }
}