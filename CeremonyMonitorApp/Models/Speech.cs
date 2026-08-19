namespace CeremonyMonitorApp.Models
{
    public class Speech
    {
        public int Id { get; set; }

        public int CeremonyId { get; set; }
        public Ceremony? Ceremony { get; set; }

        public int AttributedToId { get; set; }
        public Employee? AttributedTo { get; set; }

        public int InputById { get; set; }
        public AppUser? InputBy { get; set; }

        //ini string.empty 
        public string TextJapanaese { get; set; } = string.Empty;
        public string TextIndonesia { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
