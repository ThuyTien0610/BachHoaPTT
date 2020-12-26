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
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SoLuongSPTheoLoai()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                int countLoaiSP = db.LoaiSanPhams.Count();
                int sumSanPham = db.SanPhams.Count();
                int[] listCount = new int[countLoaiSP];
                int[] listPercent = new int[countLoaiSP];
                List<LoaiSanPham> listLoaiSP = new List<LoaiSanPham>();
                List<SanPham> listSP = db.SanPhams.ToList();
                listLoaiSP = db.LoaiSanPhams.ToList();
                for (int i = 0; i < countLoaiSP; i++)
                {
                    listCount[i] = 0;
                    foreach (var sp in listSP)
                    {
                        if (sp.MaLoai.Equals(listLoaiSP[i].MaLoai))
                            listCount[i]++;
                    }
                    listPercent[i] = Convert.ToInt32(((float)listCount[i] / (float)sumSanPham) * 100);
                    dataPoints.Add(new DataPoint(listLoaiSP[i].TenLoai, listPercent[i]));
                }
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }

        public ActionResult TopFiveCustomerMuaNhieu()
        {
            List<DataPointCol> dataPoints = new List<DataPointCol>();

            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                List<KhachHangThanhToan> listThanhToan = new List<KhachHangThanhToan>();
                int sumKH = db.TaiKhoans.Count();
                foreach (var kh in db.TaiKhoans)
                {
                    KhachHangThanhToan khtt = new KhachHangThanhToan();
                    khtt.Id = kh.Id;
                    khtt.TongSoTien = 0;
                    listThanhToan.Add(khtt);
                }
                List<GiaoDich> listGiaoDich = new List<GiaoDich>();
                listGiaoDich = db.GiaoDiches.ToList();

                foreach (var gd in db.GiaoDiches)
                {
                    int index = listThanhToan.FindIndex(x => x.Id == gd.MaKhachHang);
                    listThanhToan[index].TongSoTien += (double)gd.TongTien;
                }
                listThanhToan.Sort(delegate (KhachHangThanhToan x, KhachHangThanhToan y)
                {
                    return x.TongSoTien.CompareTo(y.TongSoTien) * (-1);
                });

                for (int i = 0; i < 5; i++)
                {
                    dataPoints.Add(new DataPointCol(db.TaiKhoans.Find(listThanhToan[i].Id).HoVaTen, listThanhToan[i].TongSoTien));
                }
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        public ActionResult TopLoaiMot()
        {
            List<DataPointLoaiMot> dataPoints = new List<DataPointLoaiMot>();

            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                string maLoai = "RC";
                List<SanPhamMuaNhieu> listSP = new List<SanPhamMuaNhieu>();
                foreach (var s in db.SanPhams)
                {
                    if (s.MaLoai == maLoai)
                    {
                        SanPhamMuaNhieu sp = new SanPhamMuaNhieu();
                        sp.MaSP = s.Id;
                        sp.TenSP = s.TenSanPham;
                        sp.SoLuong = 0;
                        listSP.Add(sp);
                    }
                }
                foreach (var s in db.DonHangs)
                {
                    foreach (SanPhamMuaNhieu sp in listSP)
                    {
                        if (sp.MaSP == s.MaSanPham)
                            sp.SoLuong += s.SoLuong.GetValueOrDefault();
                    }
                }
                listSP.Sort(delegate (SanPhamMuaNhieu x, SanPhamMuaNhieu y)
                {
                    return x.SoLuong.CompareTo(y.SoLuong) * (-1);
                });
                for (int i = 0; i < 3; i++)
                {
                    dataPoints.Add(new DataPointLoaiMot(listSP[i].TenSP, listSP[i].SoLuong));
                }
            }
            ViewBag.DataPointLoaiMots = JsonConvert.SerializeObject(dataPoints);
            return View();
        }

        public ActionResult TopLoaiHai()
        {
            List<DataPointLoaiMot> dataPoints = new List<DataPointLoaiMot>();

            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                string maLoai = "TC";
                List<SanPhamMuaNhieu> listSP = new List<SanPhamMuaNhieu>();
                foreach (var s in db.SanPhams)
                {
                    if (s.MaLoai == maLoai)
                    {
                        SanPhamMuaNhieu sp = new SanPhamMuaNhieu();
                        sp.MaSP = s.Id;
                        sp.TenSP = s.TenSanPham;
                        sp.SoLuong = 0;
                        listSP.Add(sp);
                    }
                }
                foreach (var s in db.DonHangs)
                {
                    foreach (SanPhamMuaNhieu sp in listSP)
                    {
                        if (sp.MaSP == s.MaSanPham)
                            sp.SoLuong += s.SoLuong.GetValueOrDefault();
                    }
                }
                listSP.Sort(delegate (SanPhamMuaNhieu x, SanPhamMuaNhieu y)
                {
                    return x.SoLuong.CompareTo(y.SoLuong) * (-1);
                });
                for (int i = 0; i < 3; i++)
                {
                    dataPoints.Add(new DataPointLoaiMot(listSP[i].TenSP, listSP[i].SoLuong));
                }
            }
            ViewBag.DataPointLoaiHais = JsonConvert.SerializeObject(dataPoints);
            return View();
        }

        public ActionResult TopLoaiBa()
        {
            List<DataPointLoaiMot> dataPoints = new List<DataPointLoaiMot>();

            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                string maLoai = "TU";
                List<SanPhamMuaNhieu> listSP = new List<SanPhamMuaNhieu>();
                foreach (var s in db.SanPhams)
                {
                    if (s.MaLoai == maLoai)
                    {
                        SanPhamMuaNhieu sp = new SanPhamMuaNhieu();
                        sp.MaSP = s.Id;
                        sp.TenSP = s.TenSanPham;
                        sp.SoLuong = 0;
                        listSP.Add(sp);
                    }
                }
                foreach (var s in db.DonHangs)
                {
                    foreach (SanPhamMuaNhieu sp in listSP)
                    {
                        if (sp.MaSP == s.MaSanPham)
                            sp.SoLuong += s.SoLuong.GetValueOrDefault();
                    }
                }
                listSP.Sort(delegate (SanPhamMuaNhieu x, SanPhamMuaNhieu y)
                {
                    return x.SoLuong.CompareTo(y.SoLuong) * (-1);
                });
                for (int i = 0; i < 3; i++)
                {
                    dataPoints.Add(new DataPointLoaiMot(listSP[i].TenSP, listSP[i].SoLuong));
                }
            }
            ViewBag.DataPointLoaiBas = JsonConvert.SerializeObject(dataPoints);
            return View();
        }

        public ActionResult TopLoaiBon()
        {
            List<DataPointLoaiMot> dataPoints = new List<DataPointLoaiMot>();

            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                string maLoai = "TPK";
                List<SanPhamMuaNhieu> listSP = new List<SanPhamMuaNhieu>();
                foreach (var s in db.SanPhams)
                {
                    if (s.MaLoai == maLoai)
                    {
                        SanPhamMuaNhieu sp = new SanPhamMuaNhieu();
                        sp.MaSP = s.Id;
                        sp.TenSP = s.TenSanPham;
                        sp.SoLuong = 0;
                        listSP.Add(sp);
                    }
                }
                foreach (var s in db.DonHangs)
                {
                    foreach (SanPhamMuaNhieu sp in listSP)
                    {
                        if (sp.MaSP == s.MaSanPham)
                            sp.SoLuong += s.SoLuong.GetValueOrDefault();
                    }
                }
                listSP.Sort(delegate (SanPhamMuaNhieu x, SanPhamMuaNhieu y)
                {
                    return x.SoLuong.CompareTo(y.SoLuong) * (-1);
                });
                for (int i = 0; i < 3; i++)
                {
                    dataPoints.Add(new DataPointLoaiMot(listSP[i].TenSP, listSP[i].SoLuong));
                }
            }
            ViewBag.DataPointLoaiBons = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
    }
}