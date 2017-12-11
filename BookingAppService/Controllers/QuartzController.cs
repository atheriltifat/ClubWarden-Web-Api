using BookingAppService.Quartz;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

// author: Ather Iltifat
namespace BookingAppService.Controllers
{
    public class QuartzController : ApiController
    {
        private HttpResponseMessage response;
        private MainQuartz quartzObj = new MainQuartz();

        /**
         **@ brief:  this method starts the Quartz which is a thread which automatically starts on the given time 
         * first it will check the password if password is correct then it will start the Quartz and return the 
         * http response Ok 
         **/
        [HttpGet]
        public HttpResponseMessage startQuartz(string password)
        {
            if (quartzObj.startMainQuartz(password))
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else 
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
            }

            return response;
        }


        /**
         **@ brief  this method starts the Quartz which is a thread which automatically starts on the given time 
         * first it will check the password if password is correct then it will shutdown the Quartz and return the 
         * http response Ok 
         **/
        [HttpGet]
        public HttpResponseMessage shutdownQuartz(string password)
        {

            if (quartzObj.shutdownMainQuartz(password))
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
            }

            return response;
        }
    }
}