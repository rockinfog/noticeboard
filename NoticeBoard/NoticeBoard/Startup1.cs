using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using NoticeBoard.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using DapperExtensions;
using System.Linq;
using NoticeBoard.Spiders;

[assembly: OwinStartup(typeof(NoticeBoard.Startup1))]

namespace NoticeBoard
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            GlobalConfiguration.Configuration.UseSqlServerStorage("Default");

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            Init();
            
        }

        private void Init()
        {
            IList<NoticeSource> list = new List<NoticeSource>();
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            {
                list = conn.GetList<NoticeSource>().ToList();
            }
            foreach(var source in list)
            {  
                RecurringJob.AddOrUpdate(source.Name,() => NoticeSpider.Run(source), Cron.Minutely);
            }
        }
    }
}
