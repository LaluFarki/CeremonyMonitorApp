namespace CeremonyMonitorApp.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }

        public int CeremonyId { get; set; }
        public Ceremony? Ceremony { get; set; }

        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}