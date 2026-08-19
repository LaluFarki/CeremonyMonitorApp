namespace CeremonyMonitorApp.Models
{
    public enum RoleType
    {
        
        CeremonyCommander,
        Speech,
        Pancasila,
        SevenPrinciples,
        Mc,
        Prayer,
        FlagBearer

    }
    public class RoleAssignment
    {
        public int Id { get; set; }

        public int CeremonyId { get; set; }
        public Ceremony? Ceremony { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public RoleType RoleType { get; set; }
        public bool Submitted { get; set; } = false;
    }
}
