using NoticeBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using DapperExtensions;

namespace NoticeBoard.Spiders
{
    public class NoticeSpider
    {

        public static void Run(NoticeSource source)
        {
            try
            {
                if (string.IsNullOrEmpty(source.NoticePath)) return;
                string result = RequestHelper.GetHttpWebRequest(source.NoticePath);
                //string p = "<li class=\"\">\\s*<a aria-selected=\"false\" href=\"(.*?)\"(?:.*?)>(.*?)</a>";

                //string result2 = HttpUtility.HtmlEncode(p);//将HTML代码转码

                string pattern = HttpUtility.HtmlDecode(source.RegexRule).Replace(@"\\", @"\");
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                MatchCollection mc = reg.Matches(result);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
                {
                    var predicate = Predicates.Field<Notice>(f => f.Source, Operator.Eq, source.Name);
                    IEnumerable<Notice> existNoticeList = cn.GetList<Notice>(predicate);
                    IList<Notice> noticeList = new List<Notice>();
                    foreach (Match m in mc)
                    {
                        DateTime now = DateTime.Now;
                        var link = string.Format(source.Domain, m.Groups[1].Value);
                        Notice notice = new Notice()
                        {
                            Title = m.Groups[2].Value,
                            Link = link,
                            CreateDate = now,
                            Source = source.Name
                        };

                        noticeList.Add(notice);
                    }
                    var newNoticeList = noticeList.Except(existNoticeList, new NoticeComparer()).ToList();
                    foreach (var n in newNoticeList)
                    {
                        cn.Insert<Notice>(n);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            

        }
    }
}