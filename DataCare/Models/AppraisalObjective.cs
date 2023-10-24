namespace DataCare.Models
{
    public class AppraisalObjective
    {
        public int ObjectiveId { get; set; }
        public string? Name { get; set; }
        public DateTime? ActivityStartDate { get; set; }
        public DateTime? ActivityEndDate { get; set; }
        public Employee? Employee { get; set; }
    }
}
