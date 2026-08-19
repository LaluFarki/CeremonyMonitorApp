using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CeremonyMonitorApp.Models
{
    public class AppDbContext : DbContext
    {
        //constructor untuk menerima parameter options dari luar, yang berisi info koneksi database
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //connecting to the Department table in the database
        public DbSet<Department> Departments { get; set; }
        //connecting to the Employee table in the database
        public DbSet<Employee> Employees { get; set; }

        //connecting to the Ceremony table in the database
        public DbSet<Ceremony> Ceremonies { get; set; }

        //connecting to the RoleAssignment table in the database
        public DbSet<RoleAssignment> RoleAssignments { get; set; }

        //connecting to the AppUser table in the database
        public DbSet<AppUser> AppUsers { get; set; }

        //connecting to the Awardee table in the database
        public DbSet<Awardee> Awardees { get; set; }

        //connecting to the Speech table in the database
        public DbSet<Speech> Speeches { get; set; }

        //connecting to the PrayerText table in the database
        public DbSet<PrayerText> PrayerTexts { get; set; }

        //connecting to the MCChecklistItem table in the database
        public DbSet<MCChecklistItem> MCChecklistItems { get; set; }

        //connecting to the TrainingSession table in the database
        public DbSet<TrainingSession> TrainingSessions { get; set; }

        // connecting to the History table in the database
        public DbSet <History> Histories { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //Ini method bawaan DbContext yang otomatis dipanggil EF Core setiap kali
        //dia "menyusun" struktur database dari Model kamu.Kita override (timpa) 
        //supaya bisa nyisipin instruksi tambahan — dalam hal ini, data seed.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ini data awal untuk tabel Departments, kita kasih beberapa contoh departemen
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Air Conditioner" },
                new Department { Id = 2, Name = "Refrigeration" },
                new Department { Id = 3, Name = "Home Appliances" },
                new Department { Id = 4, Name = "TV Assembly" },
                new Department { Id = 5, Name = "Battery Pack" }
            );

            //ini data awal untuk tabel Employees, kita kasih beberapa contoh pegawai di tiap departemen
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FullName = "Takeshi Sato", Position = "Production Manager", DepartmentId = 1 },
                new Employee { Id = 2, FullName = "Maya Nakajima", Position = "Executive Assistant", DepartmentId = 1 },
                new Employee { Id = 3, FullName = "Ryu Watanabe", Position = "Internal Comms Specialist", DepartmentId = 1 },
                new Employee { Id = 4, FullName = "Dewi Anjani", Position = "HR Coordinator", DepartmentId = 2 },
                new Employee { Id = 5, FullName = "Hiroshi Kato", Position = "Line Lead", DepartmentId = 2 },
                new Employee { Id = 6, FullName = "Airi Kobayashi", Position = "Quality Inspector", DepartmentId = 3 },
                new Employee { Id = 7, FullName = "Budi Santoso", Position = "Shift Supervisor", DepartmentId = 3 }
            );

            //modelBuilder.Entity<RoleAssignment>() ini adalah cara EF Core tahu: "RoleAssignment itu entitas yang mau kita atur lebih lanjut"
            // dan model.Builder ini sendiri merupakaan
            modelBuilder.Entity<RoleAssignment>()
            .HasIndex(r => new { r.CeremonyId, r.EmployeeId })
            .IsUnique();

            //berfungsi untuk menolak jika data penuagasan mau di hapus, tapi pegawai yang bersangkutan
            //masih punya data penugasan. Jadi mencegah data penugasan jadi orphan (tanpa pemilik).

            // => ini di baca yg memiliki
            modelBuilder.Entity<RoleAssignment>()
            .HasOne(r => r.Employee)
            .WithMany()
            .HasForeignKey(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

            //.HasOne(r => r.Employee) — "satu RoleAssignment punya satu Employee"

            //.WithMany() — "satu Employee bisa punya banyak RoleAssignment", tapi kita nggak
            //butuh navigation property baliknya di Employee.cs(makanya kurung kosong)

            //.HasForeignKey(r => r.EmployeeId) — Menjelaskan ke EF Core bahwa kolom yang dipakai
            //sebagai kunci penghubung di tabel RoleAssignment adalah EmployeeId.

            //.OnDelete(DeleteBehavior.Restrict) — ini intinya: Aturan Proteksi Hapus: Jika ada data Employee mau dihapus,
            //SQL Server akan MENOLAK KERA-KERAS (Error) jika pegawai tersebut masih punya data penugasan (RoleAssignment).
            //Mencegah data riwayat tugas jadi orphan (penugasan tanpa pemilik).

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Employee)
                .WithMany()
                .HasForeignKey(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Department)
                .WithMany()
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //            modelBuilder.Entity<AppUser>()
            //"Hei EF Core, saya mau mengatur tabel/entitas AppUser nih..."

            //C#
            //.HasOne(akun => akun.Employee)
            //akun = Mewakili satu objek AppUser.

            //=> = "Tolong tunjuk..."

            //akun.Employee = Properti class Employee yang ada di dalam AppUser.

            //Artinya: "Tabel AppUser ini punya satu (HasOne) relasi ke akun.Employee."

            //C#
            //.WithMany()
            //"Dan sebaliknya, satu data Employee bisa punya banyak (WithMany) akun AppUser."

            //C#
            //.HasForeignKey(akun => akun.DepartmentId)
            //akun = Mewakili satu objek AppUser.

            //=> = "Tolong tunjuk..."


            //akun.DepartmentId = Kolom angka DepartmentId yang ada di dalam AppUser.


            //Artinya: "Kolom Kunci Penghubung (Foreign Key)-nya di tabel AppUser adalah properti akun.DepartmentId." 

            //C#
            //.OnDelete(DeleteBehavior.Restrict)

            //"Kalau tabel induknya mau dihapus, pasang rem darurat (Restrict)! Dilarang hapus kalau akun ini masih ada!"

            //>>>>>>>>>>>>>>>>>>>>>Awardee<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            modelBuilder.Entity<Awardee>()
                .HasOne(a => a.NominatingDepartment)
                .WithMany()
                .HasForeignKey(a => a.NominatingDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Awardee>()
                .HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Awardee>()
                .HasOne(a => a.SubmittedBy)
                .WithMany()
                .HasForeignKey(a => a.SubmittedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Awardee>()
                .HasOne(a => a.ReviewedByHrAdmin)
                .WithMany()
                .HasForeignKey(a => a.ReviewedByHrAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Awardee>()
                .HasOne(a => a.ReviewedByHrManager)
                .WithMany()
                .HasForeignKey(a => a.ReviewedByHrManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            //>>>>>>>>>>>>>>>>>>>>>Speech<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            modelBuilder.Entity<Speech>()
                .HasOne(s => s.AttributedTo)
                .WithMany()
                .HasForeignKey(s => s.AttributedToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Speech>()
                .HasOne(s => s.InputBy)
                .WithMany()
                .HasForeignKey(s => s.InputById)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
//Penjelasan singkat(ini pengulangan dari latihan kamu, tapi saya tegasin lagi biar nempel):

//: DbContext — class ini mewarisi kemampuan bawaan EF Core buat komunikasi ke database
//Constructor-nya (AppDbContext(DbContextOptions<AppDbContext> options) : base(options)) wajib persis kayak gitu — itu cara EF Core terima info koneksi database dari luar
//DbSet<Department> Departments dan DbSet<Employee> Employees — dua baris ini yang bikin EF Core tahu: "generate tabel Departments dari class Department, dan tabel Employees dari class Employee"

//Kalau nanti kita nambah Model baru (misal Ceremony), kita cuma tambah satu baris DbSet<Ceremony> Ceremonies di sini — itu polanya, akan sering kepake ke depan.