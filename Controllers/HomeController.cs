using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using YMITSDeployedWebsite.Models;

namespace YMITSDeployedWebsite.Controllers
{
    public class HomeController : AsyncController
    {


        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

       
    }

}