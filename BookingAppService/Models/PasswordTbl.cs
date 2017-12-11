using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Dynamic;

// author: Ather Iltifat
namespace BookingAppService.Models
{
    [Table("password_tbl")]
    public class PasswordTbl
    {
        [Key]
        public int passwordID { get; set; }
        public string password { get; set; }
        [Column("memberType_ID")]
        public int userTypeID { get; set; }
        public int clubID { get; set; }

        public virtual UserType userType { get; set; }
        public virtual Club club { get; set; }



        private DAO dao;
        private HttpRequestMessage Request;
        private HttpResponseMessage response;
        private HttpError err;

        /**
         *@ brief:  constructor initializes the DAO,  HttpRequestMessage and HttpError variables
         **/
        public PasswordTbl()
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
         *@ brief: this method first checks that parameters are not null or empty then it checks that password length should less or
         *equal to 20 then it will check that password is correct or wrong if it is correct then it will send the userTypeID and
         *clubID associated with that password
         *if the password is wrong then it will send the "incorrext message"
         *@ Params:  string password
         *@ return:  HttpResponseMessage
         **/
        public HttpResponseMessage verifyPassword(string password)
        {
            try
            {

                if (!string.IsNullOrEmpty(password))
                {
                    if (password.Length <= 20)
                    {
                        dao.Configuration.ProxyCreationEnabled = false;
                        dao.Configuration.LazyLoadingEnabled = false;
                        var item = (from obj in dao.PasswordTbl_DBset where obj.password == password select obj).FirstOrDefault();
                        if (item != null)
                        {
                            var data = new { UserTypeID = item.userTypeID, ClubID = item.clubID };
                            response = Request.CreateResponse(HttpStatusCode.OK, data, GlobalConfiguration.Configuration);
                        }
                        else
                        {
                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "incorrectPassword");
                        }
                    }
                    else 
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "incorrectPassword");
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
    }
}