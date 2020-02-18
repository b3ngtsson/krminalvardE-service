using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
         
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Form(FormClass form)
        {
            ServiceReference1.BlankettService1Client asd = new ServiceReference1.BlankettService1Client();

            //form id status
            form.FormId = 0;
            form.Status = "";

            ServiceReference1.Blankett asd1 = new ServiceReference1.Blankett();

            asd1.FormId = form.FormId;
            asd1.VisitorFName = form.VisitorFName;
            asd1.VisitorLName = form.VisitorLName;
            asd1.PersonOfInterestFName = form.PersonOfInterestFName;
            asd1.PersonOfInterestLName = form.PersonOfInterestLName;
            asd1.InmateSSN = form.InmateSSN;
            asd1.PhoneNumber = form.PhoneNumber;
            asd1.EMail = form.EMail;
            asd1.Status = form.Status;

            form.Status = asd.SaveForm(asd1);

            return View(form);
        }
    }
}