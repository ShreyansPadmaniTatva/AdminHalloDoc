using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel
{
    public class Constant
    {
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
       public static Dictionary<int, List<string>> statusTdHtmlMap = new Dictionary<int, List<string>>
    {
           // New
        { 1, new List<string> { "Name", "Date of Birth", "Requestor", "Requested Date", "Phone" ,"Address", "Notes" , "Chat With" , "Actions" } }, 
        // Panding
        { 2, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name",  "Date Of Service", "Phone" ,"Address", "Notes" , "Chat With" , "Actions" } }, 
        // Active
        { 4, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name", "Date Of Service", "Phone", "Address", "Notes" , "Chat With" , "Actions" } }, 
        { 5, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name", "Date Of Service", "Phone", "Address", "Notes" , "Chat With" , "Actions" } }, 
        
        //Conclude
        { 6, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service", "Phone", "Address" , "Chat With" , "Actions" } }, 

        // to close 
        { 3, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service" ,"Address", "Notes" , "Chat With" , "Actions" } }, 
        { 7, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service" ,"Address", "Notes" , "Chat With" , "Actions" } }, 
        { 8, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service" ,"Address", "Notes" , "Chat With" , "Actions" } }, 

        //un paid
        { 9, new List<string> { "Name", "Physician Name", "Date Of Service", "Phone" ,"Address" , "Chat With" , "Actions" } }, 
        
    };
    }
}
