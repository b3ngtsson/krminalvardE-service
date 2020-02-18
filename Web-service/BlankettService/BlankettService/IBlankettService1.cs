using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BlankettService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IBlankettService1
    {

        [OperationContract]
        string SaveForm(Blankett b1);
        [OperationContract]
        string CreateAccount(Blankett b1);
        [OperationContract]
        void UpdateForm(Blankett b1);
        [OperationContract]
        List<Blankett> GetForms();
        [OperationContract]
        void ChangeStatusForm(Blankett b1, string newStatus);
        [OperationContract]
        bool VerifyAccount(string inputUsername, string inputPass);
        [OperationContract]
        bool CreateInamate(string inmateFirstname, string InmateSurname);
        [OperationContract]
        Blankett GetFormWithId(int formId);
        [OperationContract]
        bool CheckStatusWithUsername(string un);

        [OperationContract]
        bool SaveAvailableTimeToDB(string date, string time);
        [OperationContract]
        List<string> GetAvailableTime();
        [OperationContract]
        List<string> GetAllBookedTimes();
        [OperationContract]
        string BookTime(string date, string time, string user, int inmateSSN);
        [OperationContract]
        string UnbookTime(string date, string time, string user, int inmateSSN);
        [OperationContract]
        bool CheckIfFull(string date);
        [OperationContract]
        List<string> GetBooktimeWithId(int id);

        [OperationContract]
        bool VerifyAdminAccount(string inputUsername, string inputPass);
        [OperationContract]
        int GetFormIdFromUsername(string s);
        [OperationContract]
        int GetUserIdWithUsername(string s);
        [OperationContract]
        List<string> GetBookedTimeWithUserId(int id);
        [OperationContract]
        string GetInmateSSNWIthUsername(string un);
        


    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Blankett
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

        [DataMember]
        public int FormId
        {
            get { return formId; }
            set { formId = value; }
        }
        [DataMember]
        public string VisitorFName
        {
            get { return visitorFName; }
            set { visitorFName = value; }
        }

        [DataMember]
        public string VisitorLName
        {
            get { return visitorLName; }
            set { visitorLName = value; }
        }
        [DataMember]
        public string PersonOfInterestFName
        {
            get { return personOfInterestFName; }
            set { personOfInterestFName = value; }
        }
        [DataMember]
        public string PersonOfInterestLName
        {
            get { return personOfInterestLName; }
            set { personOfInterestLName = value; }
        }
        [DataMember]
        public int InmateSSN
        {
            get { return inmateSSN; }
            set { inmateSSN = value; }
        }
        [DataMember]
        public int PhoneNumber
        {
            get { return phonenumber; }
            set { phonenumber = value; }
        }
        [DataMember]
        public string EMail
        {
            get { return email; }
            set { email = value; }
        }
        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
