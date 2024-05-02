namespace AdminHalloDoc.Entities.ViewModel
{
    public class Constant
    {
        public enum EmailAction
        {
            Sendorder = 1,
            Request,
            SendLink,
            SendAgreement,
            ResetPassLink,
            NewRegistration,
            ProviderContactEmail,
            ProviderContactBoth,
            ProviderContactSMS,
            CreateShift,
            ProviderRequestToAdmin,
            ProviderTimeSheetApprove

        }
        public string Name { get; set; }
        public enum RequestType
        {
            Business = 1,
            Patient,
            Family,
            Concierge
        }
        public enum AdminDashStatus
        {
            New = 1,
            Pending,
            Active,
            Conclude,
            ToClose,
            UnPaid
        }
        public enum Status
        {
            Unassigne = 1,
            Accepted,
            Cancelled,
            MDEnRoute,
            MDONSite,
            Conclude,
            CancelledByPatients,
            Closed,
            Unpaid,
            Clear,
            Block

        }
        public enum AdminStatus
        {
            Pending = 1,
            Active,
            NotActive


        }

        public enum AccountType
        {
            All = 1,
            Admin,
            Physician,
            Patient
        }

        public static int FindStatus(int status)
        {
            if (status == 1)
            {
                return 1;
            }
            else if (status == 2)
            {
                return 2;
            }
            else if (status == 3 || status == 7 || status == 8)
            {
                return 5;
            }
            else if (status == 4 || status == 5)
            {
                return 3;
            }
            else if (status == 6)
            {
                return 4;
            }
            else
            {
                return 6;
            }

        }



        public static Dictionary<int, List<string>> statusTdHtmlMap = new Dictionary<int, List<string>>
        {
            { 1, new List<string> { "Name", "Date of Birth", "Requestor", "Requested Date", "Phone" ,"Address", "Notes" ,  "Actions" } },
            { 2, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name",  "Date Of Service", "Phone" ,"Address", "Notes" , "Actions" } },
            { 3, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name", "Date Of Service", "Phone", "Address", "Notes" , "Actions" } },
            { 4, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service", "Phone", "Address" , "Actions" } },
            { 5, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service" ,"Address", "Notes" , "Actions" } },
            { 6, new List<string> { "Name", "Physician Name", "Date Of Service", "Phone" ,"Address" , "Actions" } }

        };

        public static Dictionary<int, List<string>> PstatusTdHtmlMap = new Dictionary<int, List<string>>
        {
            { 1, new List<string> { "Name",  "Phone" ,"Address", "Actions" } },
            { 2, new List<string> { "Name",  "Phone" ,"Address", "Actions" } },
            { 3, new List<string> { "Name",  "Phone" ,"Address", "Status", "Actions" } },
            { 4, new List<string>  { "Name",  "Phone" ,"Address", "Actions" } }

        };
    }
}
