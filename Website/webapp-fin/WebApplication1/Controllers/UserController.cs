using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            ServiceReference1.BlankettService1Client ser = new ServiceReference1.BlankettService1Client();
            int formid = ser.GetFormIdFromUsername(Session["usr"].ToString());
            Userpage f1 = new Userpage();
            ServiceReference1.Blankett b1 = ser.GetFormWithId(formid);

            List<string> tempList = new List<string>();

            int userId = ser.GetUserIdWithUsername(Session["usr"].ToString());

            string[] tempString = new string[3];
            tempList = ser.GetBookedTimeWithUserId(userId).ToList();

            foreach (var i in tempList)
            {
                tempString = i.Split(' ');

                f1.Date = tempString[0];
                f1.Time = tempString[1];
            }

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
    }
}