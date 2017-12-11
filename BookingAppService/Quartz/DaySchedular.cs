using BookingAppService.Models;
using BookingAppService.StaticClass;
using Quartz;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

// author: Ather Iltifat
namespace BookingAppService.Quartz
{
    public class DaySchedular : IJob
    {
        /**
         **@ brief:  this method removes the data from bookibg table which becomes useless when the date passes
         **@ Params:  IJobExecutionContext context
         **/
        public void Execute(IJobExecutionContext context)
        {

            try
            {
                DAO dao = new DAO();
                DateTime currentDate = DateTime.Now;
                currentDate = currentDate.AddDays(-3);
                string dateStr = currentDate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateStr = DateFormatter.removeTime(dateStr);
                dateStr = DateFormatter.setDateFormat(dateStr);
                DateTime dateValue = DateTime.ParseExact(dateStr, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

                List<BookingTable> listData = (from obj in dao.BookingTable_DBset
                                               where obj.bookingDate <= dateValue
                                               select obj).ToList();
                if (listData.Count >= 1)
                {
                    foreach (BookingTable item in listData)
                    {
                        dao.BookingTable_DBset.Remove(item);
                        dao.SaveChanges();
                    }
                }

            }

            catch (Exception ex)
            {
            }

        }
    }
}