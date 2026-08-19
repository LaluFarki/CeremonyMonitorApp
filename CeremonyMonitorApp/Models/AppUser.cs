namespace CeremonyMonitorApp.Models
{
    public enum AppRole
    {
        HrAdmin,
        HrManager,
        Pf,
        Secreatary,
        User,
        SuperAdmin
    }
    public class AppUser
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public AppRole Role { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
