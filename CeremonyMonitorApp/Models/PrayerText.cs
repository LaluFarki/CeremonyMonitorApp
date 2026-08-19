namespace CeremonyMonitorApp.Models
{
    public class PrayerText
    {
        public int Id { get; set; }
        
        public int CeremonyId { get; set; }
        public Ceremony? Ceremony { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
