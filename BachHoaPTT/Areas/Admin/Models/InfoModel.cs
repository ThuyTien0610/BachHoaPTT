using BachHoaPTT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BachHoaPTT.Areas.Admin.Models
{
    public class DetailDonHang
    {
        public string MaDon { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public List<GioHang> ListGioHang { get; set; }
        public decimal TongTien { get; set; }
        public DateTime ThoiGian { get; set; }
        public string TinhTrang { get; set; }

        public DetailDonHang()
        {
            ListGioHang = new List<GioHang>();
        }

        public DetailDonHang(string id)
        {
            using(BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                int i = 0;
                ListGioHang = new List<GioHang>();
                GiaoDich giaoDich = db.GiaoDiches.Find(id);
                MaDon = giaoDich.Id;
                MaKhachHang = giaoDich.MaKhachHang;
                TongTien = giaoDich.TongTien.GetValueOrDefault();
                ThoiGian = giaoDich.ThoiGian.GetValueOrDefault();
                TenKhachHang = db.TaiKhoans.Find(MaKhachHang).HoVaTen;
                TinhTrang = giaoDich.TrangThai;
                List<DonHang> listDon = new List<DonHang>();
                foreach(DonHang donHang in db.DonHangs)
                {
                    if(donHang.Id == giaoDich.Id)
                    {
                        listDon.Add(donHang);
                    }
                }
                foreach(DonHang donHang in listDon)
                {
                    GioHang gioHang = new GioHang();
                    gioHang.Id = i++;
                    gioHang.MaSanPham = donHang.MaSanPham;
                    gioHang.TenSanPham = db.SanPhams.Find(gioHang.MaSanPham).TenSanPham;
                    gioHang.Gia = db.SanPhams.Find(gioHang.MaSanPham).Gia;
                    gioHang.SoLuong = donHang.SoLuong.GetValueOrDefault();
                    gioHang.ThanhTien = donHang.ThanhTien.GetValueOrDefault();
                    ListGioHang.Add(gioHang);
                }
            }
        }
    }

    public class InfoBasic
    {
        public int SumKH { get; set; }
        public int DonHangChuaGiao { get; set; }
        public int EmailSubcribe { get; set; }
        public float TongDoanhThu { get; set; }

        public InfoBasic()
        {
            SumKH = DonHangChuaGiao = EmailSubcribe = 0;
            TongDoanhThu = 0;
        }
    }
}