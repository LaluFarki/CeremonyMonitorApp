namespace CeremonyMonitorApp.Models
{
    public enum ApprovalStage
    {
        Submitted,
        HradminReview,
        HrManagerFinal,
        Approved,
        Rejected
    }
    public class Awardee
    {
        public int Id { get; set; }
        public int CeremonyId { get; set; }
        public Ceremony? Ceremony { get; set; }

        public int NominatingDepartmentId { get; set; }
        public Department? NominatingDepartment { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;

        public int SubmittedById { get; set; }
        public AppUser? SubmittedBy { get; set; }

        public ApprovalStage Stage { get; set; } = ApprovalStage.Submitted;

        public string? HrAdminNotes { get; set; }
        public int? ReviewedByHrAdminId { get; set; }
        public AppUser? ReviewedByHrAdmin { get; set; }

        public string? HrManagerNotes { get; set; }
        public int? ReviewedByHrManagerId { get; set; }
        public AppUser? ReviewedByHrManager { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}
