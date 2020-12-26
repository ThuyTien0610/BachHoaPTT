using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BachHoaPTT.Custom;
using BachHoaPTT.Models;

namespace BachHoaPTT.Controllers
{
    public class HomeController : Controller
    {
        static string idrecover = null;
        BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView acc)
        {
            if (ModelState.IsValid)
            {
                using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
                {
                    if (db.TaiKhoans.Any(x => x.Sdt == acc.Sdt))
                    {
                        string pass = (from s in db.TaiKhoans where s.Sdt == acc.Sdt select s.MatKhau).FirstOrDefault();
                        if (pass == Encryptor.MD5Hash(acc.MatKhau))
                        {
                            string id = (from s in db.TaiKhoans where s.Sdt == acc.Sdt select s.Id).FirstOrDefault();
                            Session["UserId"] = id;
                            Session["UserSdt"] = acc.Sdt;
                            string[] name = (from s in db.TaiKhoans where s.Sdt == acc.Sdt select s.HoVaTen).FirstOrDefault().Split(' ');
                            Session["UserName"] = name[name.Length - 1];
                            string role = (from s in db.Roles where s.Id == id select s.Role1).FirstOrDefault();
                            if (role == "Admin")
                            {
                                return Redirect("/Admin/AdminHome/AdminIndex");
                            }
                            else
                                return Redirect("/Customer/CustomerHome/CustomerIndex");
                        }
                        else
                            ModelState.AddModelError("", "Mật khẩu không đúng!");
                    }
                    else
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(TaiKhoan acc)
        {
            if (ModelState.IsValid)
            {
                Regex reg = new Regex(@"\d");
                Match result = reg.Match(acc.Sdt);
                int count = 0;
                do
                {
                    count++;
                    result = result.NextMatch(); // Chuyển qua kết quả trùng khớp kế tiếp
                }
                while (result != Match.Empty);
                if (count != 10)
                {
                    ModelState.AddModelError("", "Số điện thoại không hợp lệ");
                    return View();
                }

                if (acc.Sdt.Length != 10)
                {
                    ModelState.AddModelError("", "Số điện thoại không hợp lệ");
                    return View();
                }
                using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
                {
                    if (db.TaiKhoans.Any(x => x.Sdt == acc.Sdt))
                    {
                        ModelState.AddModelError("", "Số điện thoại này đã được đăng ký!");
                        return View();
                    }
                    if (db.TaiKhoans.Any(x => x.Email == acc.Email))
                    {
                        ModelState.AddModelError("", "Email này đã được đăng ký!");
                        return View();
                    }
                    else
                    {
                        acc.Id = DateTime.Today.Year.ToString() + (db.TaiKhoans.Count() + 1).ToString();
                        acc.MatKhau = Encryptor.MD5Hash(acc.MatKhau);
                        db.TaiKhoans.Add(acc);
                        db.SaveChanges();
                        string id = (from s in db.TaiKhoans where s.Sdt == acc.Sdt select s.Id).FirstOrDefault();
                        Role r = new Role();
                        r.Id = id;
                        r.Role1 = "Customer";
                        db.Roles.Add(r);
                        db.SaveChanges();
                        ModelState.AddModelError("", "Đăng ký thành công! Đăng nhập lại!");
                        return View("Login");
                    }
                }
            }
            return View("");
        }

        public JsonResult Subcribe(string email)
        {
            string kq = "";
            if (email == "")
                kq = "Vui lòng nhập email!";
            else
            {
                try
                {
                    using (var db = new BachHoaPTTdb_CNPMEntities2())
                    {
                        if (db.EmailSubcribeds.Any(x => x.Email == email))
                            kq = "Email đã đăng kí !!!";
                        else
                        {
                            EmailSubcribed em = new EmailSubcribed();
                            em.Email = email;
                            em.ThoiGian = DateTime.Now;
                            db.EmailSubcribeds.Add(em);
                            db.SaveChanges();
                            kq = "Đăng kí thành công !!!";
                            kq += Environment.NewLine + "Vui lòng kiểm tra email!";
                            SendMail.SendEmail(email, "17110377@student.hcmute.edu.vn", "Gtien610");
                        }
                    }
                }
                catch (Exception)
                {
                    kq = "Lỗi rồi !!!";
                }
            }
            return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GopY()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GopY([Bind(Include = "Id,HoTen,Email,MoTa")] PhanHoi phanHoi)
        {
            if (ModelState.IsValid)
            {
                using (BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2())
                {
                    db.PhanHois.Add(phanHoi);
                    db.SaveChanges();
                    ModelState.AddModelError("", "Gửi góp ý thành công! Cảm ơn bạn!");
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult QuenMatKhau()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QuenMatKhau(QuenMatKhau model)
        {
            if (db.TaiKhoans.Any(x => x.Email == model.Email))
            {
                TaiKhoan t = db.TaiKhoans.Where(x => x.Email == model.Email).FirstOrDefault();
                idrecover = t.Id;
                string code = TaoMaXacNhan.RandomString();
                if (db.MaXacNhans.Any(x => x.UserId == idrecover))
                {

                    MaXacNhan macu = db.MaXacNhans.Where(x=>x.UserId==idrecover).First();
                    db.MaXacNhans.Remove(macu);
                }
                MaXacNhan maxn = new MaXacNhan();
                maxn.Code = code;
                maxn.UserId = idrecover;
                db.MaXacNhans.Add(maxn);
                db.SaveChanges();


                QuenMatKhau mail = new QuenMatKhau();
                string bodymail = mail.BodyMail_LayLaiMatKhau(model.Email, code);
                string ThongBao = mail.Send("Lấy lại mật khẩu", bodymail, model.Email, true, true);
                ViewBag.ThongBao = ThongBao;
            }
            return RedirectToAction("LayMaXacNhan");
        }

        public ActionResult RauCu()
        {
            List<SanPham> list = new List<SanPham>();
            foreach (var s in db.SanPhams.Where(x=>x.Status==true))
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
        public ActionResult ChiTietSP(string id)
        {
            ViewBag.IdSP = id;
            SanPham sp = db.SanPhams.Find(id);
            return View(sp);
        }
        public ActionResult CuaHang()
        {
            return View(db.SanPhams.Where(x => x.Status == true).ToList());
        }
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
        [HttpGet]
        public ActionResult LayMaXacNhan()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LayMaXacNhan(LayMaXacNhan model)
        {
            MaXacNhan maxn = db.MaXacNhans.Find(model.Code);
            if (maxn != null)
            {
                if (maxn.UserId == idrecover)
                {
                    return RedirectToAction("MatKhauMoi");
                }
            }
            return View();
        }

        public ActionResult MatKhauMoi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MatKhauMoi(string matkhaumoi, string nhaplaimatkhaumoi)
        {
            TaiKhoan tk = db.TaiKhoans.Find(idrecover);
            if (matkhaumoi == nhaplaimatkhaumoi)
            {
                tk.MatKhau = Encryptor.MD5Hash(matkhaumoi);
                db.SaveChanges();
                idrecover = null;
                return RedirectToAction("MoiDangNhapLai");
            }
            else
            {
                ViewBag.Check = "False";
                return View();
            }
                
        }
        public ActionResult MoiDangNhapLai()
        {
            return View();
        }
        

        //    [HttpPost]
        //    public JsonResult LayMaXacNhan(string email)
        //    {
        //        Session["Email"] = email;
        //        string kq = "";
        //        if (string.IsNullOrWhiteSpace(email))
        //            kq = "Vui lòng nhập email !!!";
        //        else
        //        {
        //            using (BachHoaPTTdbEntities db = new BachHoaPTTdbEntities())
        //            {
        //                if (db.TaiKhoans.Any(x => x.Email == email))
        //                {
        //                    SendMail.SendPass(email, "thuytienpn106@gmail.com", "tktmdhcvd");
        //                    kq = "Mã xác nhận đã được gửi qua mail của bạn !!!";
        //                }
        //                else
        //                    kq = "Email không tồn tại !!!";
        //            }
        //        }
        //        return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
        //    }

        //    [HttpPost]
        //    public ActionResult LayLaiMatKhau(LayMK user)
        //    {
        //        string kq = "false";
        //        if (string.IsNullOrWhiteSpace(user.Email))
        //        {
        //            kq = "Vui lòng nhập email !!!";
        //            ModelState.AddModelError("", kq);
        //        }
        //        else if (string.IsNullOrWhiteSpace(user.Code))
        //        {
        //            kq = "Vui lòng nhập code !!!";
        //            ModelState.AddModelError("", kq);
        //        }
        //        else if (user.Email != Session["Email"].ToString())
        //        {
        //            kq = "Email không chính xác !!!";
        //            ModelState.AddModelError("", kq);
        //        }
        //        else if (user.Code != "abcxyz")
        //        {
        //            kq = "Code sai !!!";
        //            ModelState.AddModelError("", kq);
        //        }
        //        else if (user.Email == Session["Email"].ToString() && user.Code == "abcxyz")
        //        {
        //            using (BachHoaPTTdbEntities db = new BachHoaPTTdbEntities())
        //            {
        //                if (db.TaiKhoans.Any(x => x.Email == user.Email))
        //                {
        //                    Session["UserId"] = db.TaiKhoans.Where(x => x.Email == user.Email).FirstOrDefault().Id;
        //                    Session["UserSdt"] = db.TaiKhoans.Where(x => x.Email == user.Email).FirstOrDefault().Sdt;
        //                    string[] name = db.TaiKhoans.Where(x => x.Email == user.Email).FirstOrDefault().HoVaTen.Split(' ');
        //                    Session["UserName"] = name[name.Length - 1];
        //                    ModelState.AddModelError("", "Vui lòng đổi mật khẩu mới !!!");
        //                    kq = "true";
        //                }
        //                else
        //                {
        //                    kq = "Email không tồn tại !!!";
        //                    ModelState.AddModelError("", kq);
        //                }
        //            }
        //        }
        //        return Json(new { valu = kq }, JsonRequestBehavior.AllowGet);
        //    }
    }
}