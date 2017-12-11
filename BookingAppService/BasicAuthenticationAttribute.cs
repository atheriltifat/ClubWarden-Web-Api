using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

// author: Ather Iltifat
namespace BookingAppService
{
    public class BasicAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /**
         *@ brief: this method grants the authorization access to Web API
         *@ Params:  HttpActionContext actionContext
         **/
        public override void OnActionExecuting(HttpActionContext actionContext) {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }

                else
                {
                    string authToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                    string username = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                    string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);
                    if (username != "ather" || password != "abc")
                    {
                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch(Exception ex)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}