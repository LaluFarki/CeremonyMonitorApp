Revisi ceremony 
Perusahaan saya mengadakan upcara besar 1 bulan sekali , dan Setiap satu bulan 1 departemen maju sebagai petugas upcara , Setiap Upacara masih memakai kertas dan kadang tidak Memudahkan , jadi perusahaan memutuskan untuk memakai tablet saat upcara danuntuk Managenya nanti makai website saja , Hr yg akan memanage nya . ini beberapa masalah yg saya temukan 
•	Pemilihan orang nya ribet karna HR harus email PF dulu , dan kadang terlambat dan harus lempar email bolak balik , jadinya gitu kurang efisien 
•	Nama-nama peserta yg menjadi petugas tidak bisa dilihat oleh HR ,jadi nanti datang nya pas latihan tiba-tiba , ( ini harusnya bisa dilihat oleh semua orang siapa petugas dan namanya , nama departemen nya , karna nanti akan ada user biasa yg Cuma bisa read only) 
•	penyusunan text MC kesusahan saat ada awarding karena tanggal awarding nya (paling tidak butuh approval dengan ada notes dengan ibu mirah dan manager paling lambat h-3. ada case bisa di ubah sampai h-1 untuk tanggal nya)
•	concern PF ngga ada confirm submit, langsung datang saja
•	-ada case perubahan text hari h
•	-text apel mc masih print saat hari H
•	-jadwal Latihan di system
•	-bu santi input untuk text sacho dan tanggal untuk apel besar
•	-notif ke bu santi untuk input speech sacho
•	-setelah acara ter-arsip history

User yg terlibat : 
•	HR admin = Input Jadwal Ceremony schedule, approval awardee level 2, input ceremony text( Speech , doa )
•	Personal Factory =  assign petugas Upacara, input awardee
•	Secertary = input speech text & transalte 
•	HR Manager = approval awardee level 1
•	User = view
•	SuperAdmin = view and edit

Flow terbaru 
1.	HR input jadwal ceremony , bisa setahun jadwal ,jadi 12 departemen terdaftar (Disini tanggal nya biasanya tanggal 17 tapi ada kalanya berubah ke minggu depan nya jadi harusnya setelah HR input jadwal upcara dengan Departemen yg Maju masih bisa di edit Jadwal , tanggal dll ), (HR karna posisi nya sebagai admin , jadi dia harus nya bisa Dapat semua data dan bisa edit semua data kecuali data speech dan data Petugas upcara
2.	Setelah hr input , Personal Factory akan dapat email dan nanti departemen yg mendapatkan Jadwal di bulan itu Personal factory nya akan menginputkan Data-data petugas yg akan bekerja di Bulan itu ( nanti BU yg bertugas di bulan tersebut akan mendapatkan email , email nya masuk ke PF (Personal factory) , nanti dia akan input nama petugas dan tugas nya , dan dia juga Harus input awarde maximal h-3 sebelum upacara ( disini Hr akan approval kan , disana Hr bisa komen/daapat fitur komen di Awwarde approval nya ), disini juga peserta yg bertugas itu akan mendapatkan Email masing-masing , mereka memiliki email perusahaan , jadi nanti email pemberitahuan kalau mereka menjadi petugas akan masuk 
3.	Setelah itu Hr bisa input input ceremony text( MC ceklist , doa ), ini masi bisa editable ya , jadi mau h-1 di edit pun masih boleh , dan harus langsung berubah di tablet , BTW HR juga masih bisa lihat MC ceklist yg di pake di upacara saat itu , dan kalau mau di edit jadi enak 
4.	Sekretaris input speech dan translate , dia akan dapat notifikasi alert ke email nya nanti Max H-1 harus udah di isi , tapi masi bisa editable oleh sekretaris itu sendiri , 
5.	Apa lagi yaa . munkin bisa kamu kasi saya rekomendasi …..
























Alur End-to-End
1. Setup jadwal
HR Admin input jadwal ceremony bisa lebih dari 1  bulan ke depan, tiap bulan terikat satu departemen "maju". Tanggal bebas diedit HR Admin kapan pun — termasuk di hari-H itu sendiri — selama status belum locked.
2. Notifikasi ke PF
Sistem email PF departemen yang maju bulan itu.
3a. Petugas (khusus PF departemen maju)
Assign petugas (MC, doa, translator, dll) → submit , setelah itu masuk email ke petugas yg terpilih , berisi daftar-daftar nama dan tugasnya ,.
3b. Awardee (semua PF, independen dari 3a)
Tiap PF nominasikan awardee dari departemennya sendiri untuk ceremony bulan berjalan → submit.
4. Approval Awardee (bertingkat)
submitted → HR Admin review (hr_admin_review) → pass → HR Manager (hr_manager_final) → approved. Reject di tahap mana pun → balik ke PF nominator + notes. Deadline H-3 dari tanggal ceremony — lewat tanpa approve → email alert ke HR dan ke PF .
5. Awardee approved → masuk MC checklist
HR Admin manual masukkan ke checklist.
6. Notifikasi petugas
Petugas ke-assign dapat email otomatis.
7. MC Checklist & Doa
HR Admin input/edit — langsung tampil di tablet begitu diinput, tidak menunggu lock, karena dipakai untuk latihan.
8. Jadwal Latihan
HR Admin input, terikat ke ceremony & departemen yang sama, visible read-only ke semua karyawan.
9. Speech
Sekretaris input + translate — langsung tampil di tablet begitu diinput, sama seperti checklist. Deadline H-1 — lewat tanpa isi → email alert ke HR dan ke Sekretaris nya .
10. Latihan (opsional, kapan saja sebelum lock)
MC bisa pakai tablet buat latihan baca checklist — checklist yang dipakai adalah checklist ceremony asli (bukan salinan terpisah), progress centang tersimpan sementara.
11. Lock — HR Admin klik "Mulai Ceremony"
Efek serentak:
•	Freeze semua field (petugas, checklist, doa, speech, tanggal) — tidak bisa diedit siapa pun lagi
•	Reset semua progress centang MC checklist ke belum-selesai (isi/urutan tetap sama, cuma status centang dari latihan dihapus)
•	Snapshot ke History: departemen, petugas final, checklist final, doa, speech, status awardee final
12. Ceremony berjalan
MC centang checklist real-time — ini yang tercatat sebagai histori resmi pelaksanaan.
13. Selesai
Status → selesai. History dari langkah 11 sudah permanen.
________________________________________
Referensi cepat keputusan yang sudah dikunci
•	approval_stage pakai nama tahap (hr_admin_review → hr_manager_final), bukan angka level
•	Awardee: department_id nominator ≠ Ceremony.department_id (departemen maju); satu ceremony bisa punya banyak awardee dari banyak departemen
•	Dashboard PF kondisional: "Assign Petugas" cuma muncul kalau PF.department_id == Ceremony.department_id bulan itu; "Input Awardee" selalu muncul
•	Visibilitas tablet independen dari status lock — data live sync begitu diinput, dipakai untuk latihan
•	Lock = manual trigger HR, bukan otomatis berdasar tanggal — efeknya freeze + reset progress checklist + snapshot History

Role	Aksi
HR Admin (Ibu Mirah)	Jadwal ceremony (s/d 12 bulan), edit tanggal kapan saja tanpa batas waktu (termasuk hari-H), review awardee tahap 1, input/edit MC checklist + doa, input jadwal latihan, klik "Mulai Ceremony", akses semua data kecuali speech & data petugas
PF — departemen sedang maju	Assign petugas upacara + submit
PF — semua departemen	Input awardee dari departemen sendiri, scoped ke ceremony bulan berjalan
HR Manager	Review awardee tahap final
Sekretaris	Input & edit speech + translate
User (semua karyawan)	Read-only: jadwal ceremony, petugas, jadwal latihan
SuperAdmin	View + edit tanpa batas

