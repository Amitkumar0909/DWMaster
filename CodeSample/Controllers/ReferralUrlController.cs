using CodeSample.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeSample.Controllers
{
    public class ReferralUrlController : Controller
    {
        // GET: ReferralUrl
        public ActionResult Index()
        {
            string referrerUrl = string.Empty;
            if (Request.Cookies.Get("URLREFERRER") == null && System.Web.HttpContext.Current.Request.UrlReferrer != null)
            {
                referrerUrl = System.Web.HttpContext.Current.Request.UrlReferrer.OriginalString;
            }
            else if(Request.Cookies.Get("URLREFERRER") != null) 
            {
                referrerUrl = Request.Cookies.Get("URLREFERRER").Value;
            }
            ViewBag.ReferrerUrl = referrerUrl;

            var httpCookie = new HttpCookie("URLREFERRER", referrerUrl);
            httpCookie.Expires = Helper.GetCurrentDateTime().AddDays(1);
            Response.SetCookie(httpCookie);


        
            return View();
        }
    }
}