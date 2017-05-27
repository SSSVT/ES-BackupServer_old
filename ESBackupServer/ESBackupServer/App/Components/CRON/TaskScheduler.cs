﻿using ESBackupServer.App.Interfaces.CRON;
using ESBackupServer.App.Objects.CRON;
using Quartz;
using Quartz.Impl;

namespace ESBackupServer.App.Components.CRON
{
    public class TaskScheduler : ITaskScheduler
    {
        protected static TaskScheduler _Instance;
        public static TaskScheduler GetInstance()
        {
            return (TaskScheduler._Instance == null) ? new TaskScheduler() : TaskScheduler._Instance;
        }

        private TaskScheduler()
        {
            this.Run();
        }
        private IScheduler _scheduler;
        public void Run()
        {
            //TODO: FIX THIS SHIT ALREADY

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
            
            //this._scheduler.ScheduleJob(EmailJob, EmailTrigger);

            #endregion

        }
        public void Stop()
        {
            this._scheduler.Shutdown();
        }
    }
}