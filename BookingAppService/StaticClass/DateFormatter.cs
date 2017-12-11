using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// author: Ather Iltifat
namespace BookingAppService.StaticClass
{
    public class DateFormatter
    {
        /**
         *@ brief: this method sets the date format of a string
         *@ Params:  string dateTime
         *@ return:  string
         **/
        public static string setDateFormat(string dateTime)
        {
            DateTime date = DateTime.Parse(dateTime);
            string formattedDate = date.ToString("yyyy/MM/dd HH:mm:ss");
            return formattedDate;
        }


        /**
         *@ brief: this method removes the time in a string.
         *@ Params:  string dateTime
         *@ return:  string
         **/
        public static string removeTime(string dateTime)
        {
            string[] parts = null;
            if (dateTime.Contains(" "))
            {
                parts = dateTime.Split(' ');
                parts[1] = "00:00:00";
                dateTime = parts[0] + " " + parts[1];
            }
            if (dateTime.Contains("T"))
            {
                parts = dateTime.Split('T');
                parts[1] = "00:00:00";
                dateTime = parts[0] + "T" + parts[1];
            }
            return dateTime;
        }
    }


}