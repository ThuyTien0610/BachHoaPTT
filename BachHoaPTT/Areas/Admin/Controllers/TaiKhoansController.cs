using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BachHoaPTT.Custom;
using BachHoaPTT.Models;
using PagedList;
using System.Reflection;

namespace BachHoaPTT.Areas.Admin.Controllers
{
    public class TaiKhoansController : Controller
    {
        private BachHoaPTTdb_CNPMEntities2 db = new BachHoaPTTdb_CNPMEntities2();

        public class HttpParamActionAttribute : ActionNameSelectorAttribute
        {
            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                var request = controllerContext.RequestContext.HttpContext.Request;
                return request[methodInfo.Name] != null;
            }
        }
        // GET: Admin/TaiKhoans
        public ActionResult Index(int? size, int? page, string searchString)
        {
            ViewBag.searchValue = searchString;
            ViewBag.page = page;

            // 2. Tạo danh sách chọn số trang
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });

            // 2.1. Thiết lập số trang đang chọn vào danh sách List<SelectListItem> items
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.size = items;
            ViewBag.currentSize = size;

            //4.Truy vấn lấy tất cả đường dẫn
            var first = from l in db.TaiKhoans
                        select l;







            // 5.1. Thêm phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                first = first.Where(s => s.HoVaTen.Contains(searchString)
                || s.GioiTinh.Contains(searchString)
                || s.Sdt.Contains(searchString)
                || s.DiaChi.Contains(searchString)
                || s.Email.Contains(searchString)
                );
            }

            IOrderedQueryable<TaiKhoan> orders = first.OrderBy(p => p.Id);


            // 5.2. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;

            // 5.3. Tạo kích thước trang (pageSize), mặc định là 5.
            int pageSize = (size ?? 5);

            ViewBag.pageSize = pageSize;

            // 6. Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber. --- dammio.com
            int pageNumber = (page ?? 1);


            // 6.2 Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            int checkTotal = (int)(first.ToList().Count / pageSize) + 1;
            // Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            int skip = pageSize * (pageNumber - 1);
            IQueryable<TaiKhoan> links = orders.Skip(0);

            // 7. Trả về các Link được phân trang theo kích thước và số trang.
            return View(links.ToPagedList(pageNumber, pageSize));

            //if (page == null) page = 1;
            //var tk = (from x in db.GiaoDiches select x).OrderBy(s => s.Id);
            //int pageSize = 5;
            //int pageNumber = (page ?? 1);
            //return View(tk.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index(int? page)
        //{
        //    if (page == null) page = 1;
        //    var tk = (from x in db.TaiKhoans select x).OrderBy(s => s.Id);
        //    int pageSize = 5;
        //    int pageNumber = (page ?? 1);
        //    return View(tk.ToPagedList(pageNumber, pageSize));
        //    return View(db.TaiKhoans.ToList());
        //}
        [HttpPost, HttpParamAction]
        public ActionResult Reset()
        {
            ViewBag.searchValue = "";
            Index(null, null, "");
            return View();
        }
        // GET: Admin/TaiKhoans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/TaiKhoans/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Sdt,MatKhau,Email,HoVaTen,GioiTinh,DiaChi")] TaiKhoan taiKhoan)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Regex reg = new Regex(@"\d");
        //        Match result = reg.Match(taiKhoan.Sdt);
        //        int count = 0;
        //        do
        //        {
        //            count++;
        //            result = result.NextMatch(); // Chuyển qua kết quả trùng khớp kế tiếp
        //        }
        //        while (result != Match.Empty);
        //        if (count != 10)
        //        {
        //            ModelState.AddModelError("", "Số điện thoại không hợp lệ");
        //            return View();
        //        }

        //        if (taiKhoan.Sdt.Length != 10)
        //        {
        //            ModelState.AddModelError("", "Số điện thoại không hợp lệ");
        //            return View();
        //        }
        //        if (db.TaiKhoans.Any(x => x.Sdt == taiKhoan.Sdt))
        //        {
        //            ModelState.AddModelError("", "Số điện thoại này đã được đăng kí!");
        //            return View();
        //        }
        //        if(db.TaiKhoans.Any(x=>x.Email==taiKhoan.Email))
        //        {
        //            ModelState.AddModelError("", "Email này đã được đăng kí!");
        //            return View();
        //        }
        //        if (String.IsNullOrWhiteSpace(taiKhoan.Sdt) || String.IsNullOrWhiteSpace(taiKhoan.MatKhau) || String.IsNullOrWhiteSpace(taiKhoan.Email) || String.IsNullOrWhiteSpace(taiKhoan.HoVaTen) || String.IsNullOrWhiteSpace(taiKhoan.GioiTinh) || String.IsNullOrWhiteSpace(taiKhoan.DiaChi))
        //        {
        //            ModelState.AddModelError("", "Vui lòng nhập chính xác !!!");
        //            return View();
        //        }
        //            taiKhoan.Id = DateTime.Now.Year.ToString() + db.TaiKhoans.Count().ToString();
        //        taiKhoan.MatKhau = Encryptor.MD5Hash(taiKhoan.MatKhau);
        //        db.TaiKhoans.Add(taiKhoan);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(taiKhoan);
        //}

        // GET: Admin/TaiKhoans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: Admin/TaiKhoans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Sdt,MatKhau,Email,HoVaTen,GioiTinh,DiaChi")] TaiKhoan taiKhoan)
        {
            TaiKhoan tk = db.TaiKhoans.Find(taiKhoan.Id);
            if (String.IsNullOrWhiteSpace(taiKhoan.HoVaTen) || String.IsNullOrWhiteSpace(taiKhoan.GioiTinh) || String.IsNullOrWhiteSpace(taiKhoan.DiaChi))
            {
                
                ModelState.AddModelError("", "Vui lòng nhập chính xác");
                return View(tk);
            }


            tk.HoVaTen = taiKhoan.HoVaTen;
            tk.GioiTinh = taiKhoan.GioiTinh;
            tk.DiaChi = taiKhoan.DiaChi;
            //if (ModelState.IsValid)
            //{
            db.SaveChanges();
            ModelState.AddModelError("", "Sửa thông tin thành công!");
            return View(tk);
            //}
            //return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
        //    if (taiKhoan == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(taiKhoan);
        //}

        //// POST: Admin/TaiKhoans/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
        //    db.TaiKhoans.Remove(taiKhoan);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
