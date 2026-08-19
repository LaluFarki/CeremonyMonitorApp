namespace CeremonyMonitorApp.Models
{
    public class MCChecklistItem
    {
        public int Id { get; set; }

        public int CeremonyId { get; set; }
        public Ceremony? Ceremony { get; set; }

        public int OrderIndex { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ScripText { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
    }
}
