using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BlankettService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class BlankettService1 : IBlankettService1
    {
        public void ChangeStatusForm(Blankett b1, string newStatus)
        {
            b1.Status = newStatus;

            KrimmDBEntities2 db = new KrimmDBEntities2();

            int result = db.Form.Where(f => f.FormId == b1.FormId).Select(f => f.FormId).FirstOrDefault();

            if(result == b1.FormId)
            {
                Form updateStatus = db.Form.Single(s => s.FormId == b1.FormId);
                updateStatus.Status = b1.Status;
                db.SaveChanges();
            }

            //Save db;
            //0 = Declined
            //1 = Accepted
            //2 = Pending

            
        }

        public string CreateAccount(Blankett b1)
        {
            bool b = true;

            while (b)
            {
                Random rnd = new Random();
                KrimmDBEntities2 db = new KrimmDBEntities2();

                int username = rnd.Next(0000000, 99999999);
                string sUsername = username.ToString();
                string password = rnd.Next(0000000, 99999999).ToString();
                
                string result = db.Users.Where(u => u.Username == sUsername).Select(u => u.Username).FirstOrDefault();

                if(result != sUsername)
                {
                    var inmateSSN = db.Inmates.Where(i => i.InmateSSN == b1.InmateSSN).Select(i => i.InmateSSN);

                    int intInmateSSN = (int)inmateSSN.FirstOrDefault();
                    if (intInmateSSN == b1.InmateSSN)
                    {
                        int admin = 0;
                        Users create = new Users();
                        create.Username = sUsername;
                        create.Password = password;
                        create.PersonOfInterest = intInmateSSN;
                        create.Admin = admin;
                        db.Users.Add(create);
                        db.SaveChanges();
                        b = false;
                        return sUsername;
                    }
                    else
                    {
                        b = false;
                        return "Error";
                    }
                }
                else
                {
                    b = true; //FORTSÄTT LOOP TILLS KONTO SKAPAS
                }
            }
            return "Error";
            
        }


        public List<Blankett> GetForms()
        {
            List<Blankett> lista = new List<Blankett>();
            KrimmDBEntities2 db = new KrimmDBEntities2();

            var sql = db.Form.Select(s => s.FormId).ToArray();

            for(int i = 0; i < sql.Count(); i++)
            {

                var temp = sql[i];
                int formId = db.Form.Where(s => s.FormId == temp).Select(s => s.FormId).FirstOrDefault();
                string visitorFName = db.Form.Where(s => s.FormId == formId).Select(s => s.VisitorFName).FirstOrDefault();
                string visitorLName = db.Form.Where(s => s.FormId == formId).Select(s => s.VisitorLName).FirstOrDefault();

                int inmateId = db.Form.Where(s => s.FormId == formId).Select(s => s.PersonOfInterest).FirstOrDefault();
                string inmateFName = db.Inmates.Where(s => s.InmateSSN == inmateId).Select(s => s.Firstname).FirstOrDefault();
                string inmateLName = db.Inmates.Where(s => s.InmateSSN == inmateId).Select(s => s.Surname).FirstOrDefault();

                int phonenumber = db.Form.Where(s => s.FormId == formId).Select(s => s.Phonenumber).FirstOrDefault();
                string email = db.Form.Where(s => s.FormId == formId).Select(s => s.Email).FirstOrDefault();
                string status = db.Form.Where(s => s.FormId == formId).Select(s => s.Status).FirstOrDefault();

                

                lista.Add(new Blankett
                {
                    FormId = formId,
                    VisitorFName = visitorFName,
                    VisitorLName = visitorLName,
                    InmateSSN = inmateId
                ,
                    PersonOfInterestFName = inmateFName,
                    PersonOfInterestLName = inmateLName,
                    PhoneNumber = phonenumber,
                    EMail = email,
                    Status = status
                });
            }
            return lista;
        }

        public string SaveForm(Blankett b1)
        {

            KrimmDBEntities2 db = new KrimmDBEntities2();           
            string username = CreateAccount(b1);
            if (!username.Equals("Error")) {
                Form form = new Form();
                int id = db.Users.Where(s => s.Username == username).Select(s => s.UserId).FirstOrDefault();
                form.User = id;
                form.Email = b1.EMail;
                form.VisitorFName = b1.VisitorFName;
                form.VisitorLName = b1.VisitorLName;
                form.Status = "Pending";
                form.Phonenumber = b1.PhoneNumber;
                form.PersonOfInterest = b1.InmateSSN;
                db.Form.Add(form);
                db.SaveChanges();
                string pass = db.Users.Where(s => s.Username == username).Select(s => s.Password).FirstOrDefault();
                
                return "Blanketten är mottagen. Du har fått ett konto med" +
                    " användarnamn: " + username + " och lösenord: " + pass + " Skriv ner dina uppgifter" ;

            }
            else {
                return "Error";
            }
            

        }

        public void UpdateForm(Blankett b1)
        {
            //casesatts med int inparameter i framtiden
            throw new NotImplementedException();
        }

        public bool VerifyAccount(string inputUsername, string inputPass)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();
            int idcheck1 = db.Users.Where(u => u.Username == inputUsername).Select(u => u.UserId).FirstOrDefault();
            int idcheck2 = db.Users.Where(u => u.Password == inputPass).Select(u => u.UserId).FirstOrDefault();
            if (idcheck1 == idcheck2) {
                return true;
            }

            return false;
        }

        public Blankett GetDataUsingDataContract(Blankett b1)
        {

            return b1;
        }

        public bool SaveAvailableTimeToDB(string date, string time)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            if (date == null || time == null)
            {
                return false;
            }
            else
            {
                Bookings createBooking = new Bookings();
                createBooking.Date = date;
                createBooking.Time = time;
                db.Bookings.Add(createBooking);
                db.SaveChanges();
            }
            
            return true;

        }

        public List<string> GetAvailableTime()
        {
            List<string> bookList = new List<string>();
            KrimmDBEntities2 db = new KrimmDBEntities2();

            var sql = db.Bookings.Where(s => s.Iduser == null).Select(s => s.BookingId).ToArray();

            for(int i = 0; i<sql.Count(); i++)
            {
                var temp = sql[i];

                int id = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.BookingId).FirstOrDefault();
                string date = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Date).FirstOrDefault();
                string time = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Time).FirstOrDefault();


                bookList.Add(id.ToString() + " " + date + " " + time);
                //date 8, time 10
            }
            return bookList;

        }

        public List<string> GetAllBookedTimes()
        {
            List<string> bookList = new List<string>();
            KrimmDBEntities2 db = new KrimmDBEntities2();

            var sql = db.Bookings.Where(s => s.Iduser != null).Select(s => s.BookingId).ToArray();

            for (int i = 0; i < sql.Count(); i++)
            {
                var temp = sql[i];

                string date = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Date).FirstOrDefault();
                string time = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Time).FirstOrDefault();
                string user = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Iduser).FirstOrDefault().ToString();
                string poi = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.PersonOfInterest).FirstOrDefault().ToString();


                bookList.Add(date + " " + time + " " + user + " " + poi);
            }
            return bookList;
        }

        public string BookTime(string date, string time, string user, int inmateSSN)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            int booking = db.Bookings.Where(s => s.Date == date).Where(s => s.Time == time).Select(s => s.BookingId).FirstOrDefault();

            var tempUser = db.Bookings.Where(s => s.BookingId == booking).Select(s => s.Iduser).FirstOrDefault();

            if(tempUser == null)
            {
                int userId = db.Users.Where(s => s.Username == user).Select(s => s.UserId).FirstOrDefault();
                Bookings book = db.Bookings.Single(s => s.BookingId == booking);
                book.Iduser = userId;
                book.PersonOfInterest = inmateSSN;
                db.SaveChanges();
                return "Din bokning har genomförts den " + date + " klockan " + time;
            }
            else
            {
                return "Tiden var redan upptagen";
            }
        }

        public string UnbookTime(string date, string time, string user, int inmateSSN)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            int booking = db.Bookings.Where(s => s.Date == date).Where(s => s.Time == time).Select(s => s.BookingId).FirstOrDefault();

            Bookings book = db.Bookings.Single(s => s.BookingId == booking);
            book.Iduser = null;
            book.PersonOfInterest = null;

            return "Din tid är avbokad";
        }

        public bool CheckIfFull(string date)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();
            var temp = db.Bookings.Where(s => s.Date == date).Select(s => s.Time);

            if(temp.Count() > 5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CreateInamate(string inmateFirstname, string InmateSurname)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            Inmates inmate = new Inmates();
            inmate.Firstname = inmateFirstname;
            inmate.Surname = InmateSurname;
            db.Inmates.Add(inmate);
            db.SaveChanges();
            return true;
        }

        public bool VerifyAdminAccount(string inputUsername, string inputPass)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();
            int adminOrNah = db.Users.Where(u => u.Username == inputUsername).Where(u => u.Password == inputPass).Select(u => u.Admin).FirstOrDefault();
            

            if (adminOrNah==1){
                return true;
            } else {
                return false;
            }
            
        }

        public Blankett GetFormWithId(int formId)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            Blankett blank = new Blankett();

            blank.FormId = formId;
            blank.InmateSSN = db.Form.Where(s => s.FormId == formId).Select(s => s.PersonOfInterest).FirstOrDefault();
            blank.VisitorFName = db.Form.Where(s => s.FormId == formId).Select(s => s.VisitorFName).FirstOrDefault();
            blank.VisitorLName = db.Form.Where(s => s.FormId == formId).Select(s => s.VisitorLName).FirstOrDefault();

            int inmateID = db.Form.Where(s => s.FormId == formId).Select(s => s.PersonOfInterest).FirstOrDefault();
            blank.PersonOfInterestFName = db.Inmates.Where(s => s.InmateSSN == inmateID).Select(s => s.Firstname).FirstOrDefault();
            blank.PersonOfInterestLName = db.Inmates.Where(s => s.InmateSSN == inmateID).Select(s => s.Surname).FirstOrDefault();

            blank.PhoneNumber = db.Form.Where(s => s.FormId == formId).Select(s => s.Phonenumber).FirstOrDefault();
            blank.EMail = db.Form.Where(s => s.FormId == formId).Select(s => s.Email).FirstOrDefault();
            blank.Status = db.Form.Where(s => s.FormId == formId).Select(s => s.Status).FirstOrDefault();

            return blank;

        }

        public int GetFormIdFromUsername(string s)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            int userid = db.Users.Where(u => u.Username == s).Select(u => u.UserId).FirstOrDefault();

            return db.Form.Where(u => u.User == userid).Select(u => u.FormId).FirstOrDefault();
        }

        public List<string> GetBookedTimeWithUserId(int id)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            List<string> list = new List<string>();

            var sql = db.Bookings.Where(s => s.Iduser == id).Select(s => s.BookingId).ToArray();

            for (int i = 0; i < sql.Count(); i++)
            {
                var temp = sql[i];

                string date = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Date).FirstOrDefault();
                string time = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Time).FirstOrDefault();
                string poi = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.PersonOfInterest).FirstOrDefault().ToString();


                list.Add(date + " " + time + " " + poi);
            }
            return list;

        }

        public int GetUserIdWithUsername(string s)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            return db.Users.Where(p => p.Username == s).Select(p => p.UserId).FirstOrDefault();
        }

        public List<string> GetBooktimeWithId(int id)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            List<string> list = new List<string>();

            var sql = db.Bookings.Where(s => s.BookingId == id).Select(s => s.BookingId).ToArray();

            for (int i = 0; i < sql.Count(); i++)
            {
                var temp = sql[i];

                string date = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Date).FirstOrDefault();
                string time = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.Time).FirstOrDefault();
                int? poi = db.Bookings.Where(s => s.BookingId == temp).Select(s => s.PersonOfInterest).FirstOrDefault();


                list.Add(date + " " + time + " " + poi.ToString());
            }
            return list;
        }

        public string GetInmateSSNWIthUsername(string un)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            return db.Users.Where(s => s.Username == un).Select(s => s.PersonOfInterest).FirstOrDefault().ToString();
        }

        public bool CheckStatusWithUsername(string un)
        {
            KrimmDBEntities2 db = new KrimmDBEntities2();

            int id = db.Users.Where(s => s.Username == un).Select(s => s.UserId).FirstOrDefault();
            string status = db.Form.Where(s => s.User == id).Select(s => s.Status).FirstOrDefault();

            if (status.StartsWith("G"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
