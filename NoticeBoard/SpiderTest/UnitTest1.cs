using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using NoticeBoard;
using System.Text.RegularExpressions;

namespace SpiderTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string result = RequestHelper.GetHttpWebRequest("https://kucoin.udesk.cn/hc");

            string p = @"<li class="""">\s*<a aria-selected=""false"" href=""(.*?)""(?:.*?)>(.*?)</a>";

            Regex reg = new Regex(p, RegexOptions.IgnoreCase| RegexOptions.Singleline);

            MatchCollection mc = reg.Matches(result);

            int count = mc.Count;
        }
    }
}
