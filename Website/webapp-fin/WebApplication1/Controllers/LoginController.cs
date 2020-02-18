using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (Session["usr"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Index(LoginModel login)
        {
            ServiceReference1.BlankettService1Client client = new ServiceReference1.BlankettService1Client();


            if (login.Username != null || login.Password != null)
            {
                bool result = client.VerifyAccount(login.Username, login.Password);
                if (result)
                {
                    bool admin = client.VerifyAdminAccount(login.Username, login.Password);

                    if (admin)
                    {
                        Session["usr"] = login.Username;
                        Session["admin"] = "1"; //1 är för admin
                        login.loginerrormessage = "DU ÄR INLOGGAD SOM ADMIN";
                        return View(login);
                    }
                    else
                    {
                        Session["usr"] = login.Username;
                        Session["admin"] = "0"; //0 är för vanlig användare
                        login.loginerrormessage = "DU ÄR INLOGGAD SOM VANLIG ANVÄNDARE";
                        return View(login);
                    }

                }
                else
                {
                    login.loginerrormessage = "Felaktigt användarnamn eller lösenord";
                    return View();
                }
            }
            else
            {
                login.loginerrormessage = "Du måste mata in något";
                return RedirectToAction("index", "inlog");
            }
        }
        public ActionResult logout()
        {
            Session.Clear(); 
            return RedirectToAction("index", "login");
        }
    }
}