using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// author: Ather Iltifat
namespace BookingAppService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            keepAlive();
            return View();
        }

        /**
         *@ brief:  this is used to ping the server so that th server will not freeze after 20 minutes
         *this method will call the  getUserByIdFromController() from "UserController" and 
         *getBookingTblDataByDateFrmApi from "BookingController" randomly
         **/
        private void keepAlive()
        {
            try
            {
                Random ran = new Random();
                int num = ran.Next(1, 3);
                if (num == 1)
                {
                    UserController userCtrlr = new UserController();
                    var dummyData = userCtrlr.getUserByIdFromController(100);
                }
                else
                {
                    BookingController bookingCtrlr = new BookingController();
                    var dummyData = bookingCtrlr.getBookingTblDataByDateFrmApi("2017/09/6 00:00:00", 1);
                }
            }
            catch
            {
                // make log
            }
        }
    }
}
