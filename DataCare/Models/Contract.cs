namespace DataCare.Models
{
    public class Contract
    {
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public long EmployeeId { get; set; }
    }
}
