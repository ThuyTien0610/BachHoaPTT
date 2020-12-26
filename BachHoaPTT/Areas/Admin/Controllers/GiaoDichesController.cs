using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BachHoaPTT.Areas.Admin.Models;
using BachHoaPTT.Models;
using PagedList;

namespace BachHoaPTT.Areas.Admin.Controllers
{
    public class GiaoDichesController : Controller
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
        // GET: Admin/GiaoDiches
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
            var first = from l in db.GiaoDiches
                        select l;

            
            

            

            // 5.1. Thêm phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                first = first.Where(s => s.Id.Contains(searchString)
                || s.MaKhachHang.Contains(searchString)
                || s.TrangThai.Contains(searchString)
                //|| s.ThoiGian.GetValueOrDefault().ToString("MM/dd/yyyy").Contains(searchString)
                || s.TongTien.ToString().Contains(searchString)
                );
            }

            IOrderedQueryable<GiaoDich> orders = first.OrderBy(p => p.Id);
            

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
            IQueryable<GiaoDich> links = orders.Skip(0);

            // 7. Trả về các Link được phân trang theo kích thước và số trang.
            return View(links.ToPagedList(pageNumber, pageSize));

            //if (page == null) page = 1;
            //var tk = (from x in db.GiaoDiches select x).OrderBy(s => s.Id);
            //int pageSize = 5;
            //int pageNumber = (page ?? 1);
            //return View(tk.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost, HttpParamAction]
        public ActionResult Reset()
        {
            ViewBag.searchValue = "";
            Index(null, null, "");
            return View();
        }

        // GET: Admin/GiaoDiches/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GiaoDich giaoDich = db.GiaoDiches.Find(id);
        //    if (giaoDich == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(giaoDich);
        //}

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailDonHang donHang = new DetailDonHang(id);
            return View(donHang);
        }

        // GET: Admin/GiaoDiches/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/GiaoDiches/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,MaDon,MaKhachHang,TongTien,ThoiGian,TrangThai")] GiaoDich giaoDich)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.GiaoDiches.Add(giaoDich);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(giaoDich);
        //}

        // GET: Admin/GiaoDiches/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaoDich giaoDich = db.GiaoDiches.Find(id);
            if (giaoDich == null)
            {
                return HttpNotFound();
            }
            return View(giaoDich);
        }

        // POST: Admin/GiaoDiches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaDon,MaKhachHang,TongTien,ThoiGian,TrangThai")] GiaoDich giaoDich)
        {
            GiaoDich tk = db.GiaoDiches.Find(giaoDich.Id);
            if (String.IsNullOrWhiteSpace(giaoDich.TrangThai))
            {
                ModelState.AddModelError("", "Vui lòng nhập chính xác");
                return View(tk);
            }


            tk.TrangThai = giaoDich.TrangThai;
            //if (ModelState.IsValid)
            //{
            db.SaveChanges();
            ModelState.AddModelError("", "Sửa trạng thái thành công!");
            return View(tk);
        }

        // GET: Admin/GiaoDiches/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    GiaoDich giaoDich = db.GiaoDiches.Find(id);
        //    if (giaoDich == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(giaoDich);
        //}

        //// POST: Admin/GiaoDiches/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    GiaoDich giaoDich = db.GiaoDiches.Find(id);
        //    db.GiaoDiches.Remove(giaoDich);
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
