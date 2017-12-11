using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

// author: Ather Iltifat
namespace BookingAppService.Quartz
{
    public class KeepAliveJob : IJob
    {
        /**
         **@ brief:  this method will call the link  "http://bookingappservice.apphb.com/"
         **@ Params:  IJobExecutionContext context
         **/
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadString("http://bookingappservice.apphb.com/");
                }
            }
            catch { }
        }
    }
}