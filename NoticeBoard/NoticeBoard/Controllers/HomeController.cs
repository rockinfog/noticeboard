using NoticeBoard.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using DapperExtensions;
using Hangfire;
using NoticeBoard.Spiders;
using NoticeBoard.Filters;

namespace NoticeBoard.Controllers
{
    public class HomeController : Controller
    {
        [WechatAuthorizeFilter]
        public ActionResult Index(string source, int page = 1, int pageSize = 20)
        {
            var openId = Session["openid"].ToString();

            IList<Notice> list = new List<Notice>();
            int totalCnt = 0;
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            {
                IList<ISort> sortList = new List<ISort>();
                sortList.Add(new Sort() { PropertyName = "CreateDate", Ascending = false });
                if (!string.IsNullOrEmpty(source))
                {
                    var predicate = Predicates.Field<Notice>(f => f.Source, Operator.Eq, source);
                    list = conn.GetPage<Notice>(predicate, sortList, page - 1, pageSize).ToList();
                    totalCnt = conn.Count<Notice>(predicate);
                }
                else
                {
                    list = conn.GetPage<Notice>(null, sortList, page - 1, pageSize).ToList();
                    totalCnt = conn.Count<Notice>(null);
                }
            }
            var result = list.ToPagedList(page, pageSize, totalCnt);
            return View(result);
        }


        public ActionResult Sources()
        {
            IList<NoticeSource> list = new List<NoticeSource>();
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            {
                list = conn.GetList<NoticeSource>().ToList();
            }

            return View(list);
        }

        public ActionResult CreateSource()
        {
            NoticeSource source = new NoticeSource();
            return View("CreateOrUpdateSource", source);
        }


        public ActionResult EditSource(int id)
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            {
                var source = conn.Get<NoticeSource>(id);
                source.RegexRule = HttpUtility.HtmlDecode(source.RegexRule);
                return View("CreateOrUpdateSource", source);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveSource(NoticeSource source)
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            {
                if (source.Id == 0)
                {
                    source.RegexRule = HttpUtility.HtmlEncode(source.RegexRule);
                    var result = conn.Insert(source);
                }
                else
                {
                    conn.Update(source);
                }

            }
            RecurringJob.AddOrUpdate(source.Name, () => NoticeSpider.Run(source), Cron.Minutely);
            return RedirectToAction("Sources");
        }


        [HttpPost]
        public ActionResult DeleteSource(int id)
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
            {
                if (id != 0)
                {
                    var source = conn.Get<NoticeSource>(id);
                    if (source != null)
                    {
                        conn.Delete<NoticeSource>(source);
                    }
                }

            }
            return RedirectToAction("Sources");
        }





    }
}