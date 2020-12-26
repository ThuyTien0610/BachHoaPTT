using BachHoaPTT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BachHoaPTT.Areas.Customer.Models
{
    public class DetailDonHang
    {
        public string MaDon { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public List<GioHang> ListGioHang { get; set; }
        public decimal TongTien { get; set; }
        public DateTime ThoiGian { get; set; }
    }

    public static class ReposDetail
    {
        public static string MaDon { get; set; }
        public static string MaKhachHang { get; set; }
        public static string TenKhachHang { get; set; }
        public static string DiaChi { get; set; }
        public static List<GioHang> ListGioHang { get; set; }
        public static decimal TongTien { get; set; }
        public static DateTime ThoiGian { get; set; }

        public static void Get(DetailDonHang dt)
        {
            ListGioHang = new List<GioHang>();
            MaDon = dt.MaDon;
            MaKhachHang = dt.MaKhachHang;
            TenKhachHang = dt.TenKhachHang;
            DiaChi = dt.DiaChi;
            TongTien = dt.TongTien;
            ThoiGian = dt.ThoiGian;
            foreach (GioHang gh in dt.ListGioHang)
                ListGioHang.Add(gh);
        }
    }
}