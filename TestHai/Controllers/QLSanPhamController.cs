using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestHai.Models;
using System.IO;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;

namespace TestHai.Controllers
{
    public class QLSanPhamController : Controller
    {
        dbQLSanPhamDataContext db = new dbQLSanPhamDataContext();
        //
        // GET: /QLSanPham/
        public ActionResult Index()
        {
           
            return View();

        }
        public ActionResult SanPham(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.TrangSucs.ToList());
            return View(db.ProductLs.ToList().OrderBy(n => n.ID).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu table Loai trang suc, sắp xep tang dan trheo ten loai, chon lay gia tri Ma loai, hien thi thi LoaiTrangSuc
            ViewBag.MaLoai = new SelectList(db.Brands.ToList().OrderBy(n => n.Name), "ID", "Name");
            ViewBag.TheLoai = new SelectList(db.CategoryLs.ToList().OrderBy(n => n.Name), "CategoryID","Name");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Product Product, HttpPostedFileBase fileupload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.Brands.ToList().OrderBy(n => n.Name), "ID", "Name");
            ViewBag.TheLoai = new SelectList(db.CategoryLs.ToList().OrderBy(n => n.Name), "CategoryID", "Name");
            //Kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                     Product.Photo = fileName;
                    //Luu vao CSDL
                   //  Product.CategoryID = 1;
                    db.Products.InsertOnSubmit(Product);
                    db.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }
	}
}