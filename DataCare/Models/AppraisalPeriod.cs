namespace DataCare.Models
{
    public class AppraisalPeriod
    {
        public int AppraisalPeriodId { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
