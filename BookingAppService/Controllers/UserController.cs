using BookingAppService.Models;
using System.Net;
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
    public class UserController : ApiController
    {
       // private static readonly log4net.ILog log = LogHelper.GetLogger();
        User obj = new User();

        
        [HttpGet]
        public HttpResponseMessage getUserByIdFromController(int userId)
        {
            //log.Error("This is my error message");            
            return obj.getUserById(userId);
        }
        

        [HttpGet]
        public HttpResponseMessage getUserByNamePhoneAndDate(string fname, string lname, string phone, string joinDate)
        {
            //log.Error("This is my error message");
            return obj.getUserByNamePhoneAndDateFromDB(fname, lname, phone, joinDate);
        }

    }
}