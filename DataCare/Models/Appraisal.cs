namespace DataCare.Models
{
    public class Appraisal
    {
        public int ObjectiveId { get; set; }
        public int AppraisalPeriodId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int EmployeeId { get; set; }
        public string? WorkflowStatus { get; set; }
        public string? ApproverComment { get; set; }
    }
}
