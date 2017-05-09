using System;
using System.Configuration;
using log4net;
using Quartz;
using Quartz.Impl;

namespace BladderChange.Service
{
    class BladderChangeDataService
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BladderChangeDataService).Name);

        public void Start()
        {
            _logger.Info("Starting service...");
            var cronExp = ConfigurationManager.AppSettings["JobScheduleCronExp"];

            try
            {
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Clear();
                scheduler.Start();

                var job = JobBuilder
                    .Create<BladderChangeDataJob>()
                    .WithIdentity("BladderChangeDataJob")
                    .Build();

                var trigger = TriggerBuilder
                    .Create()
                    .WithCronSchedule(cronExp)
                    .StartNow()
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                _logger.Error("Error when scheduling job.", ex);
            }
            _logger.Info("Service started.");
        }

        public void Stop()
        {
            _logger.Info("Stopping service...");
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            if (scheduler.IsStarted)
                scheduler.Shutdown();
            _logger.Info("Service stopped.");
        }
    }
}
