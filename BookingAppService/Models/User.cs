using BookingAppService.StaticClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

// author: Ather Iltifat
namespace BookingAppService.Models
{
    [Table("member")]
    public class User
    {
        [Key]
        [Column("member_ID")]
        public int userID { get; set; }
        public int clubID { get; set; }
        [Column("memberType_ID")]
        public int userTypeID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNo { get; set; }
        public bool isActive { get; set; }
        public bool isBanned { get; set; }
        public bool isApproved { get; set; }
        public string inActiveReason { get; set; }
        public DateTime joinDate { get; set; }

        public virtual ICollection<BookingTable> listBookingTable { get; set; }
        public virtual Club club { get; set; }
        public virtual UserType userType { get; set; }

        private DAO dao;
        private HttpRequestMessage Request;
        private HttpResponseMessage response;
        private HttpError err;

        /**
         *@ brief:  constructor initializes the DAO,  HttpRequestMessage and HttpError variables
         **/
        public User()
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
         *@ brief:  this method first validate the parameters then it will add the user in database and return the User object 
         *wrapped in response
         *@ Params:  int userTypeID, int clubID, string firstName, string lastName, string phoneNo,
            string joinDate
         *@ return:  HttpResponseMessage
         **/
        public HttpResponseMessage addUserInDB(int userTypeID, int clubID, string firstName, string lastName, string phoneNo,
            string joinDate)
        {
            try
            {
                string[] inputStrings = trimInputString(firstName, lastName, phoneNo, joinDate);
                firstName = inputStrings[0];
                lastName = inputStrings[1];
                phoneNo = inputStrings[2];
                joinDate = inputStrings[3];

                if (validatingUserCredentials(userTypeID, clubID, firstName, lastName, phoneNo, joinDate))
                {
                    joinDate = DateFormatter.setDateFormat(joinDate);
                    DateTime _joinDate = DateTime.ParseExact(joinDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                    this.userTypeID = userTypeID;
                    this.clubID = clubID;
                    this.firstName = firstName.ToLower();
                    this.lastName = lastName.ToLower();
                    this.phoneNo = phoneNo;
                    this.isActive = true;
                    this.isBanned = false;
                    this.isApproved = true;
                    this.joinDate = _joinDate;
                    dao.User_DBset.Add(this);
                    dao.SaveChanges();
                    dao.Configuration.ProxyCreationEnabled = false;
                    dao.Configuration.LazyLoadingEnabled = false;
                    var item = (from obj in dao.User_DBset where obj.userID == this.userID select obj).Include(c => c.club).FirstOrDefault(); // FirstOrDefault() returns null if it couldnt find the element.  // First() throws exception when it couldnt find the element.
                    response = Request.CreateResponse(HttpStatusCode.OK, item, GlobalConfiguration.Configuration);
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
         *@ brief: this method first checks that parameter is not equal to 0  then it will get the user by id from database
         *and send the User object wrapped in http Response if it does not find any user then it will send the 
         *bad request message in response 
         *@ Params:  int userId
         *@ return:  HttpResponseMessage
         **/
        public HttpResponseMessage getUserById(int userId)
        {
            try
            {
                if (userId != 0)
                {
                    dao.Configuration.ProxyCreationEnabled = false;
                    dao.Configuration.LazyLoadingEnabled = false;
                    var item = (from obj in dao.User_DBset where obj.userID == userId select obj).Include(c => c.club).FirstOrDefault(); // FirstOrDefault() returns null if it couldnt find the element.  // First() throws exception when it couldnt find the element.
                    if (item == null)
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, item, GlobalConfiguration.Configuration);
                    }
                }
                else
                {
                    
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }
            }
            catch (Exception ex)
            {
                //make log
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
            return response;
        }

        /**
         *@ brief:  this method first checks that parameters are not null or empty then it will check the parameter's character
         *length then it will get the user by name, phone and date. if it will get only one user then it will send that 
         *user object in response, if it get more than one user then it will send the message "multipleUsers" wrapped in 
         *http error response, if it does not finds any user then it will send the message "notFound" wrapped in 
         *http error response
         *@ Params:  string fname, string lname, string phone, string joinDate
         *@ return:  HttpResponseMessage
         **/
        public HttpResponseMessage getUserByNamePhoneAndDateFromDB(string fname, string lname, string phone, string joinDate)
        {
            try
            {
                string[] inputStrings = trimInputString(fname, lname, phone, joinDate);
                fname = inputStrings[0];
                lname = inputStrings[1];
                phone = inputStrings[2];
                joinDate = inputStrings[3];

                // checks string is null or empty
                if (!string.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(lname) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(joinDate))
                {
                    if (fname.Length <= 20 && lname.Length <= 20 && phone.Length <= 20 && joinDate.Length <= 50)
                    {
                        dao.Configuration.ProxyCreationEnabled = false;
                        dao.Configuration.LazyLoadingEnabled = false;
                        joinDate = DateFormatter.setDateFormat(joinDate);
                        DateTime _joinDate = DateTime.ParseExact(joinDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

                        var list = (from obj in dao.User_DBset
                                    where obj.firstName == fname && obj.lastName == lname &&
                                        obj.phoneNo == phone && obj.joinDate == _joinDate
                                    select obj).Include(c => c.club).ToList();

                        if (list.Count == 1)
                        {
                            var item = list.First();
                            response = Request.CreateResponse(HttpStatusCode.OK, item, GlobalConfiguration.Configuration);
                        }
                        if (list.Count > 1)
                        {
                            // make log that multiple user exist
                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "multipleUsers");
                        }
                        if (list.Count == 0)
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "notFound");
                        }
                    }
                    else 
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                    }
                    
                }
                else
                {
                    //make log
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
         *@ brief:  this method removes leading and trailing spaces from its parameters, .Trim() method will actually removes 
         *those spaces, .Trim() will throw error if string is null 
         *@ Params:  string firstName, string lastName, string phoneNo, string joinDate
         *@ return:  string[]
         **/
        private string[] trimInputString(string firstName, string lastName, string phoneNo, string joinDate)
        {
            if (firstName != null)
            {
                firstName = firstName.Trim();
            }
            else if (lastName != null)
            {
                lastName = lastName.Trim();
            }
            else if (phoneNo != null)
            {
                phoneNo = phoneNo.Trim();
            }
            else if (joinDate != null)
            {
                joinDate = joinDate.Trim();
            }
            string[] inputStrings = { firstName, lastName, phoneNo, joinDate };
            return inputStrings;
        }

        /**
         *@ brief:  this method first checks that parameters are not null or empty then it will check the character limit
         *of its parameter if both validations are true then it will return bool true, else it will return false
         *@ Params:  int userTypeID, int clubID, string firstName, string lastName, string phoneNo,
            string joinDate
         *@ return:  bool
         **/
        private bool validatingUserCredentials(int userTypeID, int clubID, string firstName, string lastName, string phoneNo,
            string joinDate)
        {
            bool isValid = chkEmptyOrNullStr(userTypeID, clubID, firstName, lastName, phoneNo, joinDate);
            if (isValid)
            {
                isValid = chkInputStrLength(firstName, lastName, phoneNo, joinDate);
            }
            return isValid;
        }

        /**
         *@ brief:  this method  checks that parameters are not null or empty and return true if all parameters are not null and
         *empty, else it will return false
         *@ Params:  int userTypeID, int clubID, string firstName, string lastName, string phoneNo,
            string joinDate
         *@ return:  bool
         **/
        private bool chkEmptyOrNullStr(int userTypeID, int clubID, string firstName, string lastName, string phoneNo,
            string joinDate)
        {
            bool isValid = true;
            if (userTypeID <= 0 || clubID <= 0 || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                   string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(joinDate))
            {
                isValid = false;
            }
            return isValid;
        }

        /**
         *@ brief:   this method will check the character limit of its parameter if character limit does not exceeds then it
         *will return true, else it will return false 
         *@ Params:  string firstName, string lastName, string phoneNo, string joinDate
         *@ return:  bool
         **/
        private bool chkInputStrLength(string firstName, string lastName, string phoneNo, string joinDate)
        {
            bool isValid = true;
            int charLimit = 20;
            // checks length of string should not be greater than 20 char 
            if (firstName.Length >= charLimit || lastName.Length >= charLimit || phoneNo.Length >= charLimit || joinDate.Length >= 50)
            {
                isValid = false;
            }
            return isValid;
        }

    }
}