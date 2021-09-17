using FIT5032_API.Models;
using FIT5032_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FIT5032_API.Controllers
{
    public class HomeController : Controller
    {
        private EmailSender _sendGridEmailService;
        public HomeController()
        {
            _sendGridEmailService = new EmailSender();
        }
       

        
         
        

        public ActionResult Index()
        {
            return View();
        }

     

        [HttpPost]
        public ActionResult Index(SendEmailViewModel emailContract)
        {
            if (!ModelState.IsValid)
            {
                return View(emailContract);
            }

            try
            {
                var response = _sendGridEmailService.Send(emailContract);
                ViewBag.Success = true;
                return View();
            }
            catch (Exception)
            {
                ViewBag.Success = false;
                return View();
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}