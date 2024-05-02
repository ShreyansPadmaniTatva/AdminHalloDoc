namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class Viewcase
    {
        public int? Requestid { get; set; }
        public int? RequesClientid { get; set; }
        public string? Notes { get; set; }
        public string? FirstName { get; set; }
        public short? Status { get; set; }

        public int? RequestTypeID { get; set; }
        public int? RegionID { get; set; }

        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? RoomSite { get; set; }




        // not mandatory
        public int? date { get; set; }
        public int? year { get; set; }
        public string? month { get; set; }
    }
}
