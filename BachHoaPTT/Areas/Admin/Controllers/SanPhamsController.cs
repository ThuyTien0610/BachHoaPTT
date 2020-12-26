using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BachHoaPTT.Models;
using System.Reflection;
using PagedList;


namespace BachHoaPTT.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
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
        // GET: Admin/SanPhams
        public ActionResult Index(int? size, int? page, string searchString)
        {
            ViewBag.searchValue = searchString;
            ViewBag.page = page;

            // 2. Tạo danh sách chọn số trang
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "2", Value = "2" });
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            items.Add(new SelectListItem { Text = "8", Value = "8" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });

            // 2.1. Thiết lập số trang đang chọn vào danh sách List<SelectListItem> items
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.size = items;
            ViewBag.currentSize = size;

            //4.Truy vấn lấy tất cả đường dẫn
            var first = from l in db.SanPhams
                        select l;

            // 5.1. Thêm phần tìm kiếm
            if (!String.IsNullOrEmpty(searchString))
            {
                first = first.Where(s => s.MaLoai.Contains(searchString)
                || s.TenSanPham.Contains(searchString)
                || (s.Gia).ToString().Contains(searchString)
                || s.MaLoai.Contains(searchString)
                );
            }

            IOrderedQueryable<SanPham> orders = first.OrderBy(p => p.Id);


            // 5.2. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;

            // 5.3. Tạo kích thước trang (pageSize), mặc định là 5.
            int pageSize = (size ?? 3);

            ViewBag.pageSize = pageSize;

            // 6. Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber. --- dammio.com
            int pageNumber = (page ?? 1);


            // 6.2 Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            int checkTotal = (int)(first.ToList().Count / pageSize) + 1;
            // Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            int skip = pageSize * (pageNumber - 1);
            IQueryable<SanPham> links = orders.Skip(0);

            // 7. Trả về các Link được phân trang theo kích thước và số trang.
            return View(links.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index()
        //{
        //    return View(db.SanPhams.ToList());
        //}
        [HttpPost, HttpParamAction]
        public ActionResult Reset()
        {
            ViewBag.searchValue = "";
            Index(null, null, "");
            return View();
        }
        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenSanPham,MoTa,Gia,HinhAnh,MaLoai,Status")] SanPham sanPham)
        {
            
            if (ModelState.IsValid)
            {
                sanPham.Id = sanPham.MaLoai + "0" + (db.SanPhams.Where(x => x.MaLoai == sanPham.MaLoai).Count() + 1).ToString();
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sanPham);
        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenSanPham,MoTa,Gia,HinhAnh,MaLoai,Status")] SanPham sanPham)
        {
            SanPham sp = db.SanPhams.Find(sanPham.Id);
            if (String.IsNullOrWhiteSpace(sanPham.TenSanPham) || String.IsNullOrWhiteSpace(sanPham.MoTa) || String.IsNullOrWhiteSpace(sanPham.Gia.ToString()))
            {

                ModelState.AddModelError("", "Vui lòng nhập chính xác");
                return View(sp);
            }


            sp.TenSanPham = sanPham.TenSanPham;
            sp.MoTa = sanPham.MoTa;
            sp.Gia = sanPham.Gia;
            sp.Status = sanPham.Status;
        
            //sp.HinhAnh =sanPham.HinhAnh;
            //if (ModelState.IsValid)
            //{
            db.SaveChanges();
            ModelState.AddModelError("", "Sửa thông tin thành công!");
            return View(sp);
        } 
            // GET: Admin/SanPhams/Delete/5
        //    public ActionResult Delete(string id)
        //    {

        //    SanPham sp = db.SanPhams.Find(id);
        //    if (sp.Status == true)
        //    {
        //        sp.Status = false;
        //    }
        //    else
        //    {
        //        sp.Status = true;
        //    }
        //    db.SaveChanges();
        //    ModelState.AddModelError("", "Sửa status thành công!");
        //    return RedirectToAction("Index"); ;
        //}
        /*
        // POST: Admin/SanPhams/Delete/5
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
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
