namespace CeremonyMonitorApp.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? LastSyncedAt { get; set; }
    }
}
/*
 public int Id { get; set; }
Primary key. EF Core otomatis kenali property bernama Id sebagai primary key tanpa perlu setting tambahan — sama seperti di latihan Department kamu sebelumnya.

public string? ExternalId { get; set; }
Ingat pembahasan kita soal integrasi API HRD? Ini kolom penghubungnya. Tanda ? setelah string itu artinya nullable — boleh kosong. Kenapa harus nullable: karena sekarang kita masih pakai data dummy (departemen kita input manual, belum dari API), jadi ExternalId memang belum ada isinya. Nanti pas API HRD beneran nyambung, field ini yang dipakai buat "mencocokkan" data lokal kita dengan data asli dari HRD.

public string Name { get; set; } = string.Empty;
Nama departemen (contoh: "Air Conditioner", "Refrigeration"). Bagian = string.Empty; di akhir itu default value — cara C# bilang "kalau nggak diisi, defaultnya string kosong, bukan null". Ini penting karena Name sifatnya wajib ada (beda dari ExternalId yang boleh kosong), jadi kita nggak kasih tanda ? di sini.

public DateTime? LastSyncedAt { get; set; }
Kapan terakhir kali data ini disinkronkan dari API HRD. Nullable juga (?), karena data dummy belum pernah di-sync sama sekali — nanti kalau sync job jalan, field ini yang diupdate.
 
 */