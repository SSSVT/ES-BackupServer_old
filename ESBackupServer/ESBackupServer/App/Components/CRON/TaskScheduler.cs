using ESBackupServer.App.Interfaces.CRON;
using ESBackupServer.App.Objects.CRON;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.App.Components.CRON
{
    public class TaskScheduler : ITaskScheduler
    {
        public TaskScheduler()
        {
            this.Run();
        }
        private IScheduler _scheduler;
        public void Run()
        {
            this._scheduler = StdSchedulerFactory.GetDefaultScheduler();

            this._scheduler.Start();

            #region Email job(s)

            IJobDetail EmailJob = 
                JobBuilder.Create<EmailSendTask>()
                .WithIdentity("EmailSendJob", "Email")
                .Build();

            //TODO: CRON schedule
            ITrigger EmailTrigger =
                TriggerBuilder.Create()
                .WithIdentity("EmailTrigger", "Email")
                .StartNow()
                .WithCronSchedule("0 0/1 * 1/1 * ? *")
                .Build();
            
            this._scheduler.ScheduleJob(EmailJob, EmailTrigger);

            #endregion

        }
        public void Stop()
        {
            this._scheduler.Shutdown();
        }
    }
}