﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BachHoaPTT.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BachHoaPTTdb_CNPMEntities2 : DbContext
    {
        public BachHoaPTTdb_CNPMEntities2()
            : base("name=BachHoaPTTdb_CNPMEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<EmailSubcribed> EmailSubcribeds { get; set; }
        public virtual DbSet<GiaoDich> GiaoDiches { get; set; }
        public virtual DbSet<GioHang> GioHangs { get; set; }
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<MaXacNhan> MaXacNhans { get; set; }
        public virtual DbSet<PhanHoi> PhanHois { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
    }
}
