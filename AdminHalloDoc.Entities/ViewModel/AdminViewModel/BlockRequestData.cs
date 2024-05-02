using System.Collections;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class BlockRequestData
    {
        public int Blockrequestid { get; set; }

        public string? Phonenumber { get; set; }
        public string? PatientName { get; set; }

        public string? Email { get; set; }

        public BitArray? Isactive { get; set; }

        public string? Reason { get; set; }

        public int Requestid { get; set; }

        public string? Ip { get; set; }

        public DateTime Createddate { get; set; }

        public DateTime? Modifieddate { get; set; }
    }
}
