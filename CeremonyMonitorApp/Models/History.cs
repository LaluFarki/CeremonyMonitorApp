namespace CeremonyMonitorApp.Models
{
    public class History
    {
        public int Id { get; set; }

        public int CeremonyId { get; set; }
        public Ceremony? Ceremony { get; set; }

        public string SnapshotData { get; set; } = string.Empty;

        public DateTime ArchivedAt { get; set; } = DateTime.Now;
    }
}