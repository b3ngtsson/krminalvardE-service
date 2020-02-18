using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CalenderController : Controller
    {
        // GET: Calender
        public ActionResult Index()
        {
            if (Session["usr"] == null)
            {
               return RedirectToAction("index", "login");
            }
            else
            {
                ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();

                List<string> tempList = new List<string>();
                List<Bookings> list = new List<Bookings>();
                string[] tempString = new string[3];
                tempList = client.GetAvailableTime().ToList();

                foreach (var i in tempList)
                {
                    tempString = i.Split(' ');
                    string temp = tempString[0];
                    int id = int.Parse(temp);
                    list.Add(new Bookings { BookingId = id, Date = tempString[1], Time = tempString[2] });

                    //23:00-00:00
                }
                return View(list);
            }

        }
        public ActionResult BokaTid(int id)
        {
            ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();
            Userpage f1 = new Userpage();

            List<string> tempList = new List<string>();
            f1.User = client.GetUserIdWithUsername(Session["usr"].ToString());
            string poi = client.GetInmateSSNWIthUsername(Session["usr"].ToString());
            f1.PersonOfInterest = int.Parse(poi);

            string[] tempString = new string[3];
            tempList = client.GetBooktimeWithId(id).ToList();
  
            foreach (var i in tempList)
            {
                tempString = i.Split(' ');

                f1.Date = tempString[0];
                f1.Time = tempString[1];
            }

            return View(f1);

        }
        [HttpPost]
        public ActionResult BokaTid(Userpage f1)
        {
            ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();

            bool res = client.CheckStatusWithUsername(Session["usr"].ToString());
            if (res)
            {
                ViewBag.booktimeres = client.BookTime(f1.Date, f1.Time, Session["usr"].ToString(), f1.PersonOfInterest);

                return RedirectToAction("Index", "Calender");
            }
            else
            {
                f1.Status = "DU MÅSTE FÅ DIN BLANKETT GODKÄND FÖR ATT BOKA TID, DU KAN SE BLANKETT STATUS PÅ 'MINA SIDOR'.";
                return View(f1);
            }
        }
        public ActionResult AdminCalander()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminCalander(Bookings b1)
        {
            ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();

            bool res = client.SaveAvailableTimeToDB(b1.Date, b1.Time);
            if (res)
            {
                ViewBag.bookingRes = "Tiden bokades.";
                return RedirectToAction("AdminCalander", "Calender");
            }
            else
            {
                ViewBag.bookingRes = "Tiden kunde inte bokas.";
                return RedirectToAction("AdminCalander", "Calender");
            }
            

        }
    }
}