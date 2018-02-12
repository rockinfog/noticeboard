using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NoticeBoard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //注册AppId
            string appID = ConfigurationManager.AppSettings["AppId"];
            string secret = ConfigurationManager.AppSettings["AppSecret"];
            AccessTokenContainer.Register(appID, secret);


        }
    }
}
