using BachHoaPTT.Areas.Admin.Models;
using BachHoaPTT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BachHoaPTT.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: Admin/AdminHome
        public ActionResult AdminIndex()
        {
            InfoBasic info = new InfoBasic();
            List<DataPointLoaiMot> dataPointMots = new List<DataPointLoaiMot>();
            List<DataPointLoaiHai> dataPointHais = new List<DataPointLoaiHai>();
            List<DataPointLoaiBa> dataPointBas = new List<DataPointLoaiBa>();
            List<DataPointLoaiBon> dataPointBons = new List<DataPointLoaiBon>();

            double[] doanhThu = { 0, 0, 0, 0 };

            List<DataPointDoanhThu> dataPointDoanhThu = new List<DataPointDoanhThu>();

            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                foreach (var gd in db.GiaoDiches)
                {
                    switch (gd.ThoiGian.Value.Year)
                    {
                        case 2016: doanhThu[0] += (double)gd.TongTien.GetValueOrDefault(); break;
                        case 2017: doanhThu[1] += (double)gd.TongTien.GetValueOrDefault(); break;
                        case 2018: doanhThu[2] += (double)gd.TongTien.GetValueOrDefault(); break;
                        default: doanhThu[3] += (double)gd.TongTien.GetValueOrDefault(); break;
                    }
                }


                info.SumKH = db.TaiKhoans.Count();
                info.EmailSubcribe = db.EmailSubcribeds.Count();
                foreach (var dh in db.GiaoDiches)
                {
                    if (dh.TrangThai == "CHƯA GIAO HÀNG")
                        info.DonHangChuaGiao++;
                    else
                        info.TongDoanhThu += (float)dh.TongTien.GetValueOrDefault();
                }
                List<SanPhamMuaNhieu> listSPLoaiMot = new List<SanPhamMuaNhieu>();
                List<SanPhamMuaNhieu> listSPLoaiHai = new List<SanPhamMuaNhieu>();
                List<SanPhamMuaNhieu> listSPLoaiBa = new List<SanPhamMuaNhieu>();
                List<SanPhamMuaNhieu> listSPLoaiBon = new List<SanPhamMuaNhieu>();
                foreach (var s in db.SanPhams)
                {
                    SanPhamMuaNhieu sp = new SanPhamMuaNhieu();
                    sp.MaSP = s.Id;
                    sp.TenSP = s.TenSanPham;
                    sp.SoLuong = 0;
                    switch (s.MaLoai)
                    {
                        case "RC": listSPLoaiMot.Add(sp); break;
                        case "TC": listSPLoaiHai.Add(sp); break;
                        case "TU": listSPLoaiBa.Add(sp); break;
                        default: listSPLoaiBon.Add(sp); break;
                    }
                }
                foreach (var s in db.DonHangs)
                {
                    foreach (SanPhamMuaNhieu sp in listSPLoaiMot)
                    {
                        if (sp.MaSP == s.MaSanPham)
                            sp.SoLuong += s.SoLuong.GetValueOrDefault();
                    }
                    foreach (SanPhamMuaNhieu sp in listSPLoaiHai)
                    {
                        if (sp.MaSP == s.MaSanPham)
                            sp.SoLuong += s.SoLuong.GetValueOrDefault();
                    }
                    foreach (SanPhamMuaNhieu sp in listSPLoaiBa)
                    {
                        if (sp.MaSP == s.MaSanPham)
                            sp.SoLuong += s.SoLuong.GetValueOrDefault();
                    }
                    foreach (SanPhamMuaNhieu sp in listSPLoaiBon)
                    {
                        if (sp.MaSP == s.MaSanPham)
                            sp.SoLuong += s.SoLuong.GetValueOrDefault();
                    }
                }
                listSPLoaiMot.Sort(delegate (SanPhamMuaNhieu x, SanPhamMuaNhieu y)
                {
                    return x.SoLuong.CompareTo(y.SoLuong) * (-1);
                });
                listSPLoaiHai.Sort(delegate (SanPhamMuaNhieu x, SanPhamMuaNhieu y)
                {
                    return x.SoLuong.CompareTo(y.SoLuong) * (-1);
                });
                listSPLoaiBa.Sort(delegate (SanPhamMuaNhieu x, SanPhamMuaNhieu y)
                {
                    return x.SoLuong.CompareTo(y.SoLuong) * (-1);
                });
                listSPLoaiBon.Sort(delegate (SanPhamMuaNhieu x, SanPhamMuaNhieu y)
                {
                    return x.SoLuong.CompareTo(y.SoLuong) * (-1);
                });

                int t=listSPLoaiMot.Count();
                for (int i = 0; i < 3; i++)
                {
                    dataPointMots.Add(new DataPointLoaiMot(listSPLoaiMot[i].TenSP, listSPLoaiMot[i].SoLuong));
                    dataPointHais.Add(new DataPointLoaiHai(listSPLoaiHai[i].TenSP, listSPLoaiHai[i].SoLuong));
                    dataPointBas.Add(new DataPointLoaiBa(listSPLoaiBa[i].TenSP, listSPLoaiBa[i].SoLuong));
                    dataPointBons.Add(new DataPointLoaiBon(listSPLoaiBon[i].TenSP, listSPLoaiBon[i].SoLuong));
                }
            }

            dataPointDoanhThu.Add(new DataPointDoanhThu("2016", doanhThu[0] / 1000000));
            dataPointDoanhThu.Add(new DataPointDoanhThu("2017", doanhThu[1] / 1000000));
            dataPointDoanhThu.Add(new DataPointDoanhThu("2018", doanhThu[2] / 1000000));
            dataPointDoanhThu.Add(new DataPointDoanhThu("2019", doanhThu[3] / 1000000));

            ViewBag.DataPointDoanhThu = JsonConvert.SerializeObject(dataPointDoanhThu);

            info.TongDoanhThu /= 1000000;


            ViewBag.DataPointLoaiMots = JsonConvert.SerializeObject(dataPointMots);
            ViewBag.DataPointLoaiHais = JsonConvert.SerializeObject(dataPointHais);
            ViewBag.DataPointLoaiBas = JsonConvert.SerializeObject(dataPointBas);
            ViewBag.DataPointLoaiBons = JsonConvert.SerializeObject(dataPointBons);
            return View(info);
        }

        public ActionResult GopY()
        {
            List<PhanHoi> list = new List<PhanHoi>();
            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                list = db.PhanHois.ToList();
            }
            return View(list);
        }

        public ActionResult DetailDonHang(string idsp)
        {
            idsp = "05DH01";
            DetailDonHang dt = new DetailDonHang();
            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                dt.MaDon = idsp;
                dt.MaKhachHang = db.GiaoDiches.Where(x => x.Id == idsp).FirstOrDefault().MaKhachHang;
                dt.TenKhachHang = db.TaiKhoans.Where(x => x.Id == dt.MaKhachHang).FirstOrDefault().HoVaTen;
                dt.ThoiGian = db.GiaoDiches.Where(x => x.Id == idsp).FirstOrDefault().ThoiGian.GetValueOrDefault();
                dt.TongTien = db.GiaoDiches.Where(x => x.Id == idsp).FirstOrDefault().TongTien.GetValueOrDefault();
                dt.ListGioHang = new List<GioHang>();
                foreach (var s in db.DonHangs)
                {
                    if (s.Id == dt.MaDon)
                    {
                        GioHang sp = new GioHang();
                        sp.MaSanPham = s.MaSanPham;
                        sp.TenSanPham = db.SanPhams.Where(x => x.Id == sp.MaSanPham).FirstOrDefault().TenSanPham;
                        sp.SoLuong = s.SoLuong.GetValueOrDefault();
                        sp.ThanhTien = s.ThanhTien.GetValueOrDefault();
                        sp.UserId = dt.MaKhachHang;
                        dt.ListGioHang.Add(sp);
                    }
                }
            }
            return View(dt);
        }
    }
}