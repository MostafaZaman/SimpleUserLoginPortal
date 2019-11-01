using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNDAuth.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //[Authorize]
        public ActionResult Index()
        {

            var SessionVal = HttpContext.Session.GetString("EmailID");

            if(string.IsNullOrEmpty(SessionVal))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }
    }
}