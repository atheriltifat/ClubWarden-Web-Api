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
    public class PasswordController : ApiController
    {
        private PasswordTbl obj = new PasswordTbl();


        /**
         *@ brief:  this method first verify the password if password is correct then it will send the user data in User class
         * in addUserInDB() method
         **/
        [HttpPost]
        public HttpResponseMessage verifyPassAndAddUser(string firstName, string lastName, string phoneNo, string joinDate,
            string password)
        {
            HttpResponseMessage response = obj.verifyPassword(password);
            int responseCode = (int)response.StatusCode;
            if (responseCode == 200)
            {
                var u = (dynamic)response.Content.ReadAsAsync(typeof(Object)).Result;
                int UserTypeID = u.UserTypeID;
                int ClubID = u.ClubID;
                User user = new User();
                response = user.addUserInDB(UserTypeID, ClubID, firstName, lastName, phoneNo, joinDate);
            }
            return response;
        }

    }
}