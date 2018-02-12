using DapperExtensions;
using NoticeBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoticeBoard.Filters
{
    public class WechatAuthorizeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["openid"] == null)
            {
#if DEBUG
                filterContext.HttpContext.Session["openid"] = "12##o0XAsuEVAajjMPEnRByelvMX6Qus";
                filterContext.HttpContext.Session["nick"] = "鱼汤";
#else
                var request = filterContext.Controller.ControllerContext.RequestContext.HttpContext.Request;
                filterContext.Result = new RedirectResult("/client/Account/Authorize?appId=" + dealer.AppId + "&returnUrl=" + request.Url.PathAndQuery);
#endif

            }
            else
            {
                string openid = filterContext.HttpContext.Session["openid"].ToString();
                string nickname = filterContext.HttpContext.Session["nickname"].ToString();
               
            }
        }
    }
}