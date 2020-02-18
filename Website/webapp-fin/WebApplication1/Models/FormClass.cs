using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FormClass
    {
        int formId;
        string visitorFName;
        string visitorLName;
        string personOfInterestFName;
        string personOfInterestLName;
        int inmateSSN;
        int phonenumber;
        string email;
        string status;


        public int FormId
        {
            get { return formId; }
            set { formId = value; }
        }

        public string VisitorFName
        {
            get { return visitorFName; }
            set { visitorFName = value; }
        }


        public string VisitorLName
        {
            get { return visitorLName; }
            set { visitorLName = value; }
        }

        public string PersonOfInterestFName
        {
            get { return personOfInterestFName; }
            set { personOfInterestFName = value; }
        }

        public string PersonOfInterestLName
        {
            get { return personOfInterestLName; }
            set { personOfInterestLName = value; }
        }

        public int InmateSSN
        {
            get { return inmateSSN; }
            set { inmateSSN = value; }
        }

        public int PhoneNumber
        {
            get { return phonenumber; }
            set { phonenumber = value; }
        }

        public string EMail
        {
            get { return email; }
            set { email = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

    }
}