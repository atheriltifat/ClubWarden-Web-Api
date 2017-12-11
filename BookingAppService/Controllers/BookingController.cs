using BookingAppService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


// author: Ather Iltifat
namespace BookingAppService.Controllers
{
    [BasicAuthenticationAttribute]
    public class BookingController : ApiController
    {
        //private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BookingTable obj = new BookingTable();

        [HttpPost]
        public HttpResponseMessage bookCourt(int userID, int clubID, int courtTypeID, string bookingDate, int courtNumber, string timeSlot,
                          string dateTimeOfBooking)
        {
            //log.Error("This is my error message");
            return obj.bookCourtInDB(userID, clubID, courtTypeID, bookingDate, courtNumber, timeSlot, dateTimeOfBooking);
        }


        /**
         *@ brief:  the format of requested date should be equal to requestedDate = 2016/01/15 00:00:00 
         **/
        [HttpGet] 
        public HttpResponseMessage getBookingTblDataByDateFrmApi(string requestedDate, int clubID)
        {
            //log.Error("This is my error message");
            return obj.getBookingTblDataByDate(requestedDate, clubID);
        }

        [HttpDelete]
        public HttpResponseMessage unbookCourtByCoach(int coachID, int coachClubID, int bookingTblID, string requestedDate)
        {
            return obj.unbookCourtByCoachFrmDB(coachID, coachClubID, bookingTblID, requestedDate);
        }

        [HttpDelete]
        public HttpResponseMessage unbookCourtByUser(int userID, int clubID, int bookingTblID, string requestedDate)
        {
            return obj.unbookCourtByUserFrmDB(userID, clubID, bookingTblID, requestedDate);
        }

    }
}