using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

// author: Ather Iltifat
namespace BookingAppService.Quartz
{
    public class MainQuartz
    {
        /**
         **@ brief:  this method starts the Quartz which is a thread which automatically starts on the given time 
         * first it will check the password if password is correct then it will shutdown the Quartz first and then 
         * starts the Day Schedular and KeepAliveJob Quartz
         *@ Params:  string password
         *@ return:  bool
         **/
        public bool startMainQuartz(string password)
        {
            bool isSuccess = false;
            try
            {               
                if (password == "abc")
                {
                    //////////////////////////////  shutdown jobs /////////////////////////
                    var schedulerFactory = new StdSchedulerFactory();
                    var scheduler = schedulerFactory.GetScheduler();

                    var keepAliveJob = JobBuilder.Create<KeepAliveJob>().Build();
                    var keepAliveTrigger = TriggerBuilder.Create()
                                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())
                                    .Build();
                    scheduler.ScheduleJob(keepAliveJob, keepAliveTrigger);
                    scheduler.Shutdown();

                    var schedulerFact = new StdSchedulerFactory();
                    var sched = schedulerFact.GetScheduler();
                    var DaySchedular = JobBuilder.Create<DaySchedular>().Build();
                    var DaySchedularTrigger = TriggerBuilder.Create()
                                    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(24).OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(4, 0)))
                                    .Build();
                    sched.ScheduleJob(DaySchedular, DaySchedularTrigger);
                    sched.Shutdown();


                    ////////////////////////////////////// start jobs //////////////////////////////////////

                    var _schedulerFactory = new StdSchedulerFactory();
                    var _scheduler = _schedulerFactory.GetScheduler();
                    var _keepAliveJob = JobBuilder.Create<KeepAliveJob>().Build();
                    var _keepAliveTrigger = TriggerBuilder.Create()
                                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())
                                    .Build();
                    _scheduler.ScheduleJob(_keepAliveJob, _keepAliveTrigger);
                    _scheduler.Start();

                    var _schedulerFact = new StdSchedulerFactory();
                    var _sched = _schedulerFact.GetScheduler();
                    var _DaySchedular = JobBuilder.Create<DaySchedular>().Build();
                    var _DaySchedularTrigger = TriggerBuilder.Create()
                                    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(24).OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(4, 0)))
                                    .Build();
                    _sched.ScheduleJob(_DaySchedular, _DaySchedularTrigger);
                    _sched.Start();
                    isSuccess = true;

                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }


        /**
         **@ brief:  this method shutdown the Quartz which is a thread which automatically starts on the given time 
         * first it will check the password if password is correct then it will shutdown the Day Schedular and KeepAliveJob Quartz
         *@ Params:  string password
         *@ return:  bool
         **/
        public bool shutdownMainQuartz(string password)
        {
            bool isSuccess = false;
            try
            {
                if (password == "abc")
                {
                    //////////////////////////////  keep job alive /////////////////////////
                    var schedulerFactory = new StdSchedulerFactory();
                    var scheduler = schedulerFactory.GetScheduler();

                    var keepAliveJob = JobBuilder.Create<KeepAliveJob>().Build();
                    var keepAliveTrigger = TriggerBuilder.Create()
                                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())
                                    .Build();
                    scheduler.ScheduleJob(keepAliveJob, keepAliveTrigger);
                    scheduler.Shutdown();

                    ///////////////////////////////// Day schedular /////////////////////////
                    var schedulerFact = new StdSchedulerFactory();
                    var sched = schedulerFact.GetScheduler();
                    var DaySchedular = JobBuilder.Create<DaySchedular>().Build();
                    var DaySchedularTrigger = TriggerBuilder.Create()
                                    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(24).OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(4, 0)))
                                    .Build();
                    sched.ScheduleJob(DaySchedular, DaySchedularTrigger);
                    sched.Shutdown();
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        } 

    }
}