using BachHoaPTT.Areas.Customer.Models;
using BachHoaPTT.Custom;
using BachHoaPTT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace BachHoaPTT.Areas.Customer.Controllers
{
    public class CustomerHomeController : Controller
    {
        private BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2();
        // GET: Customer/CustomerHome
        public ActionResult CustomerIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemSanPhamMua(string id)
        {
            string userid = Session["UserId"].ToString();
            string kq = "";
            if (id == "")
            {
                kq = "Lỗi rồi !!!";
                return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (db.GioHangs.Any(x => x.MaSanPham == id && x.UserId == userid))
                {
                    GioHang addsp = db.GioHangs.Where(x => x.MaSanPham == id && x.UserId == userid).FirstOrDefault();
                    addsp.SoLuong++;
                    addsp.ThanhTien = addsp.SoLuong * addsp.Gia;
                    db.SaveChanges();
                    kq = "Thêm sản phẩm vào giỏ hàng thành công!";
                }
                else
                //if (listSP.FindIndex(x => x.TenSanPham == spm.TenSanPham) == 0)
                {
                    SanPham sp = db.SanPhams.Find(id);
                    GioHang spm = new GioHang();
                    spm.MaSanPham = id;
                    spm.TenSanPham = sp.TenSanPham;
                    spm.SoLuong = 1;
                    spm.Gia = sp.Gia;
                    spm.ThanhTien = sp.Gia;
                    spm.UserId = Session["UserId"].ToString();
                    db.GioHangs.Add(spm);
                    db.SaveChanges();
                    kq = "Thêm sản phẩm vào giỏ hàng thành công!";
                }
            }
            catch (Exception e)
            {

                kq = "Lỗi rồi !!!";
            }
            //else
            //{
            //    SanPhamMua check = listSP.First(x => x.TenSanPham == spm.TenSanPham);
            //    int count = check.SoLuong;
            //    listSP.Add(new SanPhamMua(check.Id, check.TenSanPham, check.DonGia, count, check.DonGia * count));
            //    listSP.Remove(check);
            //}
            return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CuaHang()
        {
            return View(db.SanPhams.Where(x => x.Status == true).ToList());
        }

        public ActionResult GioHang()
        {
            string userid = Session["UserId"].ToString();
            List<GioHang> list = db.GioHangs.Where(x => x.UserId == userid).ToList();
            return View(list);
        }

        [HttpPost]
        public ActionResult XoaSanPhamMua(string id)
        {
            string userid = Session["UserId"].ToString();
            //GioHang spm = new GioHang();
            //spm.Id = id;
            //spm.TenSanPham = tensp;
            //spm.DonGia = dg;
            //spm.SoLuong = sl;
            //spm.ThanhTien = tt;
            //GioHang del = db.GioHangs.Find(id);
            //if (listSP.FindIndex(x => x.TenSanPham == spm.TenSanPham) == 0)
            GioHang gh = db.GioHangs.Where(x => x.MaSanPham == id && x.UserId == userid).FirstOrDefault();
            string tensp = gh.TenSanPham;
            db.GioHangs.Remove(gh);
            db.SaveChanges();
            //else
            //{
            //    SanPhamMua check = listSP.First(x => x.TenSanPham == spm.TenSanPham);
            //    int count = check.SoLuong;
            //    listSP.Add(new SanPhamMua(check.Id, check.TenSanPham, check.DonGia, count, check.DonGia * count));
            //    listSP.Remove(check);
            //}
            return Json(new { tenSP = tensp }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaiKhoan()
        {
            TaiKhoan tk = new TaiKhoan();
            string id = Session["UserId"].ToString();
            tk = db.TaiKhoans.Where(x => x.Id == id).FirstOrDefault();
            tk.Sdt = "*******" + tk.Sdt.Remove(0, 7);
            return View(tk);
        }

        [HttpGet]
        public ActionResult DoiMatKhau()
        {
            TaiKhoan tk = new TaiKhoan();
            string id = Session["UserId"].ToString();
            tk = db.TaiKhoans.Where(x => x.Id == id).FirstOrDefault();
            tk.Sdt = "*******" + tk.Sdt.Remove(0, 7);
            return View(tk);
        }

        [HttpPost]
        public ActionResult DoiThongTin(string hoten, string email, string diachi)
        {
            string kq = "";
            try
            {
                string id = Session["UserId"].ToString();
                TaiKhoan tk = db.TaiKhoans.Find(id);
                tk.HoVaTen = hoten;
                tk.Email = email;
                tk.DiaChi = diachi;
                db.SaveChanges();
                string[] name = hoten.Split(' ');
                Session["UserName"] = name[name.Length - 1];
                kq = "Đổi thông tin thành công!";
            }
            catch (Exception)
            {
                kq = "Đổi thông tin không thành công! Vui lòng thử lại!";
            }
            return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoiMatKhau(string matkhau, string nhaplai, string matkhaucu)
        {
            string id = Session["UserId"].ToString();
            //edit
            TaiKhoan tk = db.TaiKhoans.Find(id);
            string kq = "";
            if (Encryptor.MD5Hash(matkhaucu) == tk.MatKhau)
            {
                if (matkhau == "" || nhaplai == "")
                    kq = "Vui lòng nhập !!!";
                else if (matkhau == nhaplai)
                {
                    try
                    {
                        tk.MatKhau = Encryptor.MD5Hash(matkhau);
                        db.SaveChanges();
                        kq = "Đổi mật khẩu thành công!";
                    }
                    catch (Exception)
                    {
                        kq = "Đổi mật khẩu không thành công! Vui lòng thử lại!";
                    }
                }
                else
                    kq = "Mật khẩu mới và xác nhận không trùng nhau !!!";
                return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                kq = "Mật khẩu cũ không đúng";
                return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult ChiTietSP(string id)
        {
            ViewBag.IdSP = id;
            SanPham sp = db.SanPhams.Find(id);
            return View(sp);
        }

        [HttpPost]
        public ActionResult ThemSanPham(string id)
        {
            string userid = Session["UserId"].ToString();
            string kq = "";
            if (id == "")
            {
                kq = "Lỗi rồi !!!";
                return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (db.GioHangs.Any(x => x.MaSanPham == id && x.UserId == userid))
                {
                    GioHang addsp = db.GioHangs.Where(x => x.MaSanPham == id && x.UserId == userid).FirstOrDefault();
                    addsp.SoLuong++;
                    addsp.ThanhTien = addsp.SoLuong * addsp.Gia;
                    db.SaveChanges();
                    kq = "Thêm sản phẩm vào giỏ hàng thành công!";
                }
                else
                //if (listSP.FindIndex(x => x.TenSanPham == spm.TenSanPham) == 0)
                {
                    SanPham sp = db.SanPhams.Find(id);
                    GioHang spm = new GioHang();
                    spm.MaSanPham = id;
                    spm.TenSanPham = sp.TenSanPham;
                    spm.SoLuong = 1;
                    spm.Gia = sp.Gia;
                    spm.ThanhTien = sp.Gia;
                    spm.UserId = Session["UserId"].ToString();
                    db.GioHangs.Add(spm);
                    db.SaveChanges();
                    kq = "Thêm sản phẩm vào giỏ hàng thành công!";
                }
            }
            catch (Exception)
            {
                kq = "Lỗi rồi !!!";
            }
            //else
            //{
            //    SanPhamMua check = listSP.First(x => x.TenSanPham == spm.TenSanPham);
            //    int count = check.SoLuong;
            //    listSP.Add(new SanPhamMua(check.Id, check.TenSanPham, check.DonGia, count, check.DonGia * count));
            //    listSP.Remove(check);
            //}
            return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RauCu()
        {
            List<SanPham> list = new List<SanPham>();
            foreach (var s in db.SanPhams.Where(x => x.Status == true))
            {
                if (s.MaLoai == "RC")
                    list.Add(s);
            }
            return View(list);
        }

        public ActionResult TraiCay()
        {
            List<SanPham> list = new List<SanPham>();
            foreach (var s in db.SanPhams.Where(x => x.Status == true))
            {
                if (s.MaLoai == "TC")
                    list.Add(s);
            }
            return View(list);
        }

        public ActionResult ThucUong()
        {
            List<SanPham> list = new List<SanPham>();
            foreach (var s in db.SanPhams.Where(x => x.Status == true))
            {
                if (s.MaLoai == "TU")
                    list.Add(s);
            }
            return View(list);
        }

        public ActionResult ThucPhamKhac()
        {
            List<SanPham> list = new List<SanPham>();
            foreach (var s in db.SanPhams.Where(x => x.Status == true))
            {
                if (s.MaLoai == "TPK")
                    list.Add(s);
            }
            return View(list);
        }

        public ActionResult DatHang()
        {
            DetailDonHang dt = new DetailDonHang();
            dt.MaKhachHang = Session["UserId"].ToString();
            using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
            {
                dt.MaDon = DateTime.Now.Year.ToString() + "DH" + db.GiaoDiches.Count().ToString();
                dt.TenKhachHang = db.TaiKhoans.Where(x => x.Id == dt.MaKhachHang).FirstOrDefault().HoVaTen;
                dt.DiaChi = db.TaiKhoans.Where(x => x.Id == dt.MaKhachHang).FirstOrDefault().DiaChi;
            }
            dt.ListGioHang = new List<GioHang>();
            dt.TongTien = 0;
            foreach (var gh in db.GioHangs.Where(x => x.UserId == dt.MaKhachHang).ToList())
            {
                dt.ListGioHang.Add(gh);
                dt.TongTien += gh.ThanhTien;
            }

            ReposDetail.Get(dt);

            return View(dt);
        }

        public ActionResult HoanThanh(string id)
        {
            string kq = ReposDetail.ListGioHang[0].TenSanPham;

            //try
            //{
            ReposDetail.DiaChi = id;
            foreach (GioHang gh in ReposDetail.ListGioHang)
            {
                GiaoDich gd = new GiaoDich();
                gd.Id =(db.GiaoDiches.Count() + 1).ToString();
                
                gd.MaKhachHang = ReposDetail.MaKhachHang;
                gd.TongTien = ReposDetail.TongTien;
                gd.ThoiGian = DateTime.Now;
                gd.TrangThai = "CHƯA GIAO HÀNG";
                db.GiaoDiches.Add(gd);
               //db.SaveChanges();
                DonHang dh = new DonHang();
                dh.Id = gd.Id;
                dh.MaSanPham = gh.MaSanPham;
                dh.SoLuong = gh.SoLuong;
                dh.ThanhTien = gh.ThanhTien;
                db.DonHangs.Add(dh);
                db.SaveChanges();
            }

            

            //db.GioHangs.RemoveRange(ReposDetail.ListGioHang);
            //db.SaveChanges();


            kq = "Đặt hàng thành công !!! Kiểm tra email của bạn!";
            //}
            //catch (Exception)
            //{
            //    kq = "Lỗi rồi !!!";
            //}
            SendMail.SendNote(db.TaiKhoans.Where(x => x.Id == ReposDetail.MaKhachHang).FirstOrDefault().Email);


            return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult TimKiem(string id)
        //{
        //    if(id=="")
        //        return Json(new { valu = "false" }, JsonRequestBehavior.AllowGet);
        //    if (!db.SanPhams.Any(x=>x.TenSanPham==id))
        //        return Json(new { valu = "false" }, JsonRequestBehavior.AllowGet);
        //    string maSp = db.SanPhams.Where(x => x.TenSanPham == id).FirstOrDefault().Id;
        //    return Json(new { valu = maSp }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Search(string search)
        {
            string TuKhoa = search;
            List<SanPham> list = new List<SanPham>();
            if (ModelState.IsValid)
            {
                var product_id = db.SanPhams.Where(s => s.TenSanPham.Contains(TuKhoa)).ToList();
                list = product_id;

            }
            return View(list);
        }
    }

}