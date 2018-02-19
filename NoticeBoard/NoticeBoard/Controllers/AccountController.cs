using DapperExtensions;
using NoticeBoard.Models;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoticeBoard.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _domain;
        private readonly string _appId;
        private readonly string _appSecret;

        public AccountController()
        {
            _domain = ConfigurationManager.AppSettings["Domain"].ToString();
            _appId = ConfigurationManager.AppSettings["AppId"].ToString();
            _appSecret = ConfigurationManager.AppSettings["AppSecret"].ToString();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Authorize(string appId,string returnUrl)
        {
            string redirectUrl = Url.Encode(string.Format("{0}/Account/GetCode?appId={1}returnUrl={2}", _domain, appId,returnUrl));
            string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", appId, redirectUrl);
            return Redirect(url);
        }


        public ActionResult GetCode(string appId, string returnUrl)
        {
            string code = HttpContext.Request.QueryString["Code"];
            if (string.IsNullOrEmpty(code))
            {
                return Redirect("/Account/Authorize?returnUrl=" + returnUrl);
            }

            string appSecret = AccessTokenContainer.TryGetItem(appId).AppSecret;
            OAuthAccessTokenResult result = OAuthApi.GetAccessToken(appId, appSecret, code);
            if (result.errcode.ToString() == "请求成功")
            {
                string openId = result.openid;
                string access_token = result.access_token;
                var user = OAuthApi.GetUserInfo(access_token, openId);
                if (user!=null&&Session["openid"] == null)
                {
                    Session["openid"] = user.openid;
                    Session["nickname"] = user.nickname;
                    using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
                    {
                        var predicate = Predicates.Field<NBUser>(f => f.OpenId, Operator.Eq, openId);
                        var cnt = conn.Count<NBUser>(predicate);
                        if (cnt == 0)
                        {
                            NBUser nbUser = new NBUser()
                            {
                                OpenId = user.openid,
                                NickName = user.nickname,
                                CreateDate = DateTime.Now
                            };
                            conn.Insert<NBUser>(nbUser);
                        }
                    }
                }
            }
            else
            {
                Response.Write(result.errmsg);
            }
            return Redirect(returnUrl);
        }

    }
}