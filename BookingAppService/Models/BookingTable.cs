using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using BookingAppService.StaticClass;

// author: Ather Iltifat
namespace BookingAppService.Models
{
    [Table("booking_table")]
    public class BookingTable
    {
        [Key]
        public int bookingTableID { get; set; }
        [Column("member_ID")]
        public int userID { get; set; }
        public int clubID { get; set; }
        public int courtTypeID { get; set; }
        public DateTime bookingDate { get; set; }
        public DateTime dateTimeOfBooking { get; set; }
        public int courtNumber { get; set; }
        public string timeSlot { get; set; }

        public virtual User user { get; set; }
        public virtual Club club { get; set; }
        public virtual CourtType courtType { get; set; }


        private DAO dao;
        private HttpRequestMessage Request;
        private HttpResponseMessage response;
        private HttpError err;

        /**
         *@ brief:  constructor initializes the DAO,  HttpRequestMessage and HttpError variables
         **/
        public BookingTable()
        {
            try
            {
                dao = new DAO();
                Request = new HttpRequestMessage();
                err = new HttpError();
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server error");
            }
        }


        /**
         *@ brief:  this method first checks that parameters are null or not then it checks the user status then it will check the 
         *availability of court if everything thing is fine then it will book court
         *@ Params:  int userID, int clubID, int courtTypeID, string bookingDate, int courtNumber, string timeSlot,
                   string dateTimeOfBooking
         *@ return:  HttpResponseMessage
         **/
        public HttpResponseMessage bookCourtInDB(int userID, int clubID, int courtTypeID, string bookingDate, int courtNumber, string timeSlot,
                          string dateTimeOfBooking)
        {
            try
            {
                bookingDate = DateFormatter.removeTime(bookingDate);
                bookingDate = DateFormatter.setDateFormat(bookingDate);
                dateTimeOfBooking = DateFormatter.setDateFormat(dateTimeOfBooking);
                if (userID != 0 && clubID != 0 && courtTypeID != 0 && bookingDate != null && courtNumber != 0 && timeSlot != null && dateTimeOfBooking != null)
                {
                    if (chkUserStatus(userID))
                    {

                        if (chkCourtAvailability(bookingDate, courtNumber, timeSlot))
                        {
                            this.userID = userID;
                            this.clubID = clubID;
                            this.courtTypeID = courtTypeID;
                            DateTime _bookingDate = DateTime.ParseExact(bookingDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                            this.bookingDate = _bookingDate;
                            this.courtNumber = courtNumber;
                            this.timeSlot = timeSlot;
                            DateTime _dateTimeOfBooking = DateTime.ParseExact(dateTimeOfBooking, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                            this.dateTimeOfBooking = _dateTimeOfBooking;
                            dao.BookingTable_DBset.Add(this);
                            dao.SaveChanges();
                            response = response = Request.CreateResponse(HttpStatusCode.OK, this, GlobalConfiguration.Configuration);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "alreadyBooked");
                        }
                    }

                    else
                    {
                        // make log
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "unauthorized"); ;
                    }

                }
                else
                {
                    //make log
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ""); ;
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
            return response;
        }

        /**
         *@ brief:  this method first checks that parameters are not null then it will get the booking table data by date 
         *dateFormat should be "yyyy/MM/dd HH:mm:ss"
         *@ Params:  string requestedDate, int clubID
         *@ return:  HttpResponseMessage
         **/
        public HttpResponseMessage getBookingTblDataByDate(string requestedDate, int clubID)
        {
            try
            {
                if (requestedDate != null && clubID != 0)
                {
                    requestedDate = DateFormatter.removeTime(requestedDate);
                    requestedDate = DateFormatter.setDateFormat(requestedDate);
                    DateTime dateValue = DateTime.ParseExact(requestedDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                    dao.Configuration.ProxyCreationEnabled = false;
                    dao.Configuration.LazyLoadingEnabled = false;
                    var list = (from obj in dao.BookingTable_DBset
                                where obj.bookingDate == dateValue
                                select obj).Include(u => u.user).ToList();
                    foreach (var item in list)
                    {
                        item.user.listBookingTable = null;
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, list, GlobalConfiguration.Configuration);
                }
                else
                {
                    //log that date is null
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err);
            }
            return response;

        }

        /**
         *@ brief:  this method first checks that parameters are not null then it will check that user is coach thenit will 
         *unbook court from database  
         *@ Params:  int coachID, int coachClubID, int bookingTblID, string requestedDate
         *@ return:  HttpResponseMessage
         **/
        public HttpResponseMessage unbookCourtByCoachFrmDB(int coachID, int coachClubID, int bookingTblID, string requestedDate)
        {
            try
            {
                if (coachID != 0 && coachClubID != 0 && bookingTblID != 0 && requestedDate != null)
                {
                    var item = (from obj in dao.User_DBset
                                where obj.userID == coachID && obj.clubID == coachClubID
                                select obj).FirstOrDefault();

                    if (item != null && item.userTypeID == 1)
                    {
                        requestedDate = DateFormatter.removeTime(requestedDate);
                        requestedDate = DateFormatter.setDateFormat(requestedDate);
                        DateTime _requestedDate = DateTime.ParseExact(requestedDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                        BookingTable bookingTableObj = (from obj in dao.BookingTable_DBset
                                                        where obj.bookingTableID == bookingTblID
                                                        select obj).FirstOrDefault();
                        if (bookingTableObj != null)
                        {
                            dao.BookingTable_DBset.Remove(bookingTableObj);
                            dao.SaveChanges();
                        }
                        response = getBookingTblDataByDate(requestedDate, coachClubID);
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                    }
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
            return response;
        }

        /**
         *@ brief:  this method first checks that parameters are not null then it will check the user status if it is fine then 
         *it will unbook the court from database
         *@ Params:  int userID, int clubID, int bookingTblID, string requestedDate
         *@ return:  HttpResponseMessage
         **/
        public HttpResponseMessage unbookCourtByUserFrmDB(int userID, int clubID, int bookingTblID, string requestedDate)
        {
            try
            {
                if (userID != 0 && clubID != 0 && bookingTblID != 0 && requestedDate != null)
                {
                    if (chkUserStatus(userID))
                    {
                        requestedDate = DateFormatter.removeTime(requestedDate);
                        requestedDate = DateFormatter.setDateFormat(requestedDate);
                        DateTime _requestedDate = DateTime.ParseExact(requestedDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                        BookingTable bookingTableObj = (from obj in dao.BookingTable_DBset
                                                        where obj.bookingTableID == bookingTblID
                                                        select obj).FirstOrDefault();
                        if (bookingTableObj != null)
                        {
                            dao.BookingTable_DBset.Remove(bookingTableObj);
                            dao.SaveChanges();
                        }
                        response = getBookingTblDataByDate(requestedDate, clubID);
                    }
                    else
                    {
                        // make log
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "statusUnauthorized");
                    }
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
            return response;
        }

        /**
         *@ brief:  this method checks the user status the he is active and he is approved by coach and he is not banned
         * then it will return true, else it will return false 
         *@ Params:  int userID
         *@ return:  bool
         **/
        private bool chkUserStatus(int userID)
        {
            bool isSuccess = false;
            try
            {
                dao.Configuration.ProxyCreationEnabled = false;
                dao.Configuration.LazyLoadingEnabled = false;
                var item = (from obj in dao.User_DBset where obj.userID == userID select obj).FirstOrDefault(); // FirstOrDefault() returns null if it couldnt find the element.  // First() throws exception when it couldnt find the element.
                if (item == null)
                {
                    //make log
                    isSuccess = false;
                }
                else
                {
                    if (item.userTypeID == 2)
                    {
                        if (item.isActive && !item.isBanned && item.isApproved)
                        {
                            isSuccess = true;
                        }
                    }
                    if (item.userTypeID == 1)
                    {
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //make log
                isSuccess = false;
            }
            return isSuccess;
        }

        /**
         *@ brief:  this method checks that the court is not reserved on the requested booking date and on the requested time slot
         *if it is not reserved it will return true, else it will return false
         *@ Params:  string bookingDate, int courtNumber, string timeSlot
         *@ return:  bool
         **/
        private bool chkCourtAvailability(string bookingDate, int courtNumber, string timeSlot)
        {
            bool isSuccess = false;
            try
            {
                DateTime _bookingDate = DateTime.ParseExact(bookingDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                var item = (from obj in dao.BookingTable_DBset
                            where obj.bookingDate == _bookingDate && obj.courtNumber == courtNumber && obj.timeSlot == timeSlot
                            select obj).FirstOrDefault();
                if (item == null)
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }


    }


}