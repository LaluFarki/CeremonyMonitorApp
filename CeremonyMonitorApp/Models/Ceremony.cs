using CeremonyMonitorApp.Models;

namespace CeremonyMonitorApp.Models
{
    //ini adalah enum untuk status ceremony. Enum ini dipakai di property Status di class Ceremony.
    public enum CeremonyStatus
    {
        Draft,
        Scheduled,
        Locked,
        Completed
    }
    public class Ceremony
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public DateTime ScheduledDate { get; set; }
        public CeremonyStatus Status { get; set; } = CeremonyStatus.Draft;
        public DateTime? LockedAt { get; set; }
    }
}

//Bedah bagian barunya:

//public enum CeremonyStatus { Draft, Scheduled, Locked, Completed }
//Ini definisi "daftar pilihan yang valid". Saya taruh di file yang sama dengan Ceremony karena cuma dipakai di situ — kalau nanti enum ini dipakai di banyak tempat, biasanya dipisah ke file sendiri, tapi untuk sekarang begini cukup.

//public CeremonyStatus Status { get; set; } = CeremonyStatus.Draft;
//Perhatikan ada = CeremonyStatus.Draft di akhir — ini default value. Artinya, setiap kali kamu bikin Ceremony baru di kode tanpa nyebut status-nya secara eksplisit, otomatis dianggap Draft. Ini match sama business rule kita: ceremony baru selalu mulai dari draft, HR nggak perlu set manual tiap kali.

//Satu hal teknis yang perlu kamu tahu dari awal, biar nggak kejebak nanti:
//Secara default, EF Core simpan enum di database sebagai angka, bukan teks — Draft = 0, Scheduled = 1, Locked = 2, Completed = 3(urutan sesuai penulisan di kode).Ini efisien buat storage, tapi ada jebakannya: kalau suatu saat kamu nyisipin status baru di TENGAH daftar (misal nambah Cancelled di antara Scheduled dan Locked), semua angka setelahnya bakal geser, dan data lama di database jadi "salah baca" (data yang tadinya Locked=2 tiba-tiba kebaca sebagai status lain). Aturan aman: kalau nanti nambah status baru, selalu taruh di paling akhir daftar, jangan nyisip di tengah.

//public DateTime? LockedAt { get; set; }
//Nullable, karena field ini cuma keisi setelah HR klik "Mulai Ceremony". Sebelum itu terjadi, nilainya null — dan ini sekaligus jadi cara kita ngecek "ceremony ini udah pernah di-lock apa belum" dari data historisnya nanti.