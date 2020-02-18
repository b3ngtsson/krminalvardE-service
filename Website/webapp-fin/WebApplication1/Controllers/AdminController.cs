using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();

            List<ServiceReference1.Blankett> lista = new List<ServiceReference1.Blankett>();
            

            lista = client.GetForms().ToList();

            List<FormClass> frm = new List<FormClass>();
            
            foreach(var i in lista)
            {
                frm.Add(new FormClass
                {
                    FormId = i.FormId,
                    VisitorFName = i.VisitorFName,
                    VisitorLName = i.VisitorLName,
                    InmateSSN = i.InmateSSN,
                    PersonOfInterestFName = i.PersonOfInterestFName,
                    PersonOfInterestLName = i.PersonOfInterestLName,
                    PhoneNumber = i.PhoneNumber,
                    EMail = i.EMail,
                    Status = i.Status
                });
            }

            return View(frm);
        }
        public ActionResult Edit(FormClass f1, int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index","Admin");
            }

            ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();
            ServiceReference1.Blankett b1 = client.GetFormWithId(id);

            f1.FormId = b1.FormId;
            f1.InmateSSN = b1.InmateSSN;
            f1.VisitorFName = b1.VisitorFName;
            f1.VisitorLName = b1.VisitorLName;
            f1.PersonOfInterestFName = b1.PersonOfInterestFName;
            f1.PersonOfInterestLName = b1.PersonOfInterestLName;
            f1.PhoneNumber = b1.PhoneNumber;
            f1.EMail = b1.EMail;
            f1.Status = b1.Status;

            return View(f1);
        }
        [HttpPost]
        public ActionResult Edit(FormClass f1, string status)
        {

            ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();
            //STATUS = ACCEPTED, PENDING OR DECLINED

            ServiceReference1.Blankett b1 = new ServiceReference1.Blankett();

            b1.FormId = f1.FormId;
            b1.InmateSSN = f1.InmateSSN;
            b1.VisitorFName = f1.VisitorFName;
            b1.VisitorLName = f1.VisitorLName;
            b1.PersonOfInterestFName = f1.PersonOfInterestFName;
            b1.PersonOfInterestLName = f1.PersonOfInterestLName;
            b1.PhoneNumber = f1.PhoneNumber;
            b1.EMail = f1.EMail;
            b1.Status = status;

            client.ChangeStatusForm(b1, status);

            return RedirectToAction( "Index", "Admin");

        }
        public ActionResult Inmates()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Inmates(InmateClass inmate)
        {
            ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();
            bool res = client.CreateInamate(inmate.InmateFName, inmate.InmateLName);

            if (res)
            {
                inmate.Result = "Ny intagen har skapats";
                return View(inmate);
            }
            else
            {
                inmate.Result = "Kunde inte skapa en ny intagen";
                return View(inmate);
            }

        }
        

        
    }
}