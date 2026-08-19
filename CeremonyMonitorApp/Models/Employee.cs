namespace CeremonyMonitorApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? ExternalID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Position { get; set; }
        public DateTime? LastSyncedAt { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

    }
}

/*
 public int DepartmentId { get; set; }
Ini disebut Foreign Key (FK) — kolom yang nyimpen "Employee ini punya Department dengan Id berapa". Perhatikan penamaannya: DepartmentId, persis nama class Department + kata Id. Ini bukan kebetulan — EF Core punya konvensi otomatis: kalau kamu bikin property bernama [NamaClassLain]Id, EF Core otomatis ngerti "oh, ini foreign key yang nunjuk ke tabel Department", tanpa kamu perlu nulis setting tambahan apa pun. Kalau kamu kasih nama lain (misal DeptId atau DepartmentRef), EF Core nggak otomatis ngerti, dan kamu harus setting manual — jadi ikutin konvensi ini persis.

public Department? Department { get; set; }
Ini disebut Navigation Property. Bedanya sama DepartmentId di atas: DepartmentId itu cuma angka (nomor ID doang) yang beneran disimpan sebagai kolom di database. Sedangkan Department (tanpa "Id") ini adalah objek Department yang utuh — nanti kalau kamu query Employee, kamu bisa langsung akses employee.Department.Name buat dapat nama departemennya, tanpa perlu query terpisah ke tabel Department. Ini nggak disimpan sebagai kolom sendiri di database — EF Core yang otomatis "mengisi" ini di belakang layar kalau kamu minta (nanti kita praktikkan langsung pas bikin Controller).

Kenapa dikasih tanda ? (nullable) di Department? Department: supaya C# nggak komplain pas kamu bikin Employee baru sebelum Department-nya di-set — ini standar aman, bukan berarti secara logika bisnis boleh Employee tanpa Department (itu tetap wajib, DepartmentId yang non-nullable di atas yang menjamin itu).

Analogi biar nempel: DepartmentId itu kayak nomor KTP orang tua di akta kelahiran anak — cuma nomornya doang yang disimpan. Department (navigation property) itu kayak kalau kamu bisa langsung "buka" data lengkap orang tuanya cuma dari lihat akta anaknya — praktis, tapi bukan data yang fisik disimpan dua kali.
 */