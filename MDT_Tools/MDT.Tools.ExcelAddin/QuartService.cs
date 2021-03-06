﻿using System;
using System.Collections.Generic;
using System.Text;
using Quartz;
using Quartz.Impl;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using MDT.Tools.ExcelAddin.Utils;
namespace MDT.Tools.ExcelAddin
{
    public class QuartService
    {

        public static void StartJob(string interval)
        {
            try
            {

                Random r = new Random();
                string a = r.Next().ToString();
                string b = r.Next().ToString();

                ISchedulerFactory sf = new StdSchedulerFactory();
                IScheduler sched = sf.GetScheduler();
                IJobDetail job = JobBuilder.Create<processExcelJob>()
                    .WithIdentity(a, b)
                    .Build();


                ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                        .WithIdentity(a, b)
                                                        .WithCronSchedule(interval)
                                                        .Build();

                sched.ScheduleJob(job, trigger);
                sched.Start();
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);
            }
        }
    }

    public class processExcelJob : IJob
    {
        public static Excel.Application application;
        public void Execute(IJobExecutionContext context)
        {
            if (application != null)
            {

                ProcessExcelHelper.processExcel(application);
                LogHelper.Info("定时器保存：" + context);
                //LogHelper.Info("Tullett Start at " + DateTime.Now);
                //LogFileName.Write("Tullett Start at " + DateTime.Now);
            }
        }

    }
}
