using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class KhoController : Controller, IPhieuNhapTemplate
    {
        LIBRARY_FINALEntities database = new LIBRARY_FINALEntities();

        // GET: Admin/Kho
        public ActionResult QLKho()
        {
            return View(database.PhieuNhaps.ToList());
        }

        // GET: Admin/Kho/NhapSach
        [HttpGet]
        public ActionResult NhapSach()
        {
            var listBooks = database.NhanVienKhoes.ToList();
            ViewBag.TenNhanVienKho = new SelectList(listBooks, "MaNhanVienKho", "TenNhanVienKho");
            var nextMaPN = GenerateNextMaPN();
            var phieuNhap = new PhieuNhap { MaPN = nextMaPN };
            return View(phieuNhap);
        }

        // Phương thức triển khai từ Interface
        [HttpPost]
        public ActionResult NhapSach(PhieuNhap phieuNhap)
        {
            if (ModelState.IsValid)
            {
                var existingBook = database.PhieuNhaps.FirstOrDefault(p => p.TenSach == phieuNhap.TenSach);

                if (existingBook != null)
                {
                    existingBook.SoLuong += phieuNhap.SoLuong;
                }
                else
                {
                    database.PhieuNhaps.Add(phieuNhap);
                }

                database.SaveChanges();

                return RedirectToAction("QLKho");
            }

            var listBooks = database.NhanVienKhoes.ToList();
            ViewBag.TenNhanVienKho = new SelectList(listBooks, "MaNhanVienKho", "TenNhanVienKho");
            return View(phieuNhap);
        }

        // Phương thức triển khai từ Interface
        public string GenerateNextMaPN()
        {
            string defaultMaPN = "PN001";
            string nextMaPN = defaultMaPN;

            while (database.PhieuNhaps.Any(p => p.MaPN == nextMaPN))
            {
                string numericPart = nextMaPN.Substring(2);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaPN = $"PN{numericValue:D3}";
                }
                else
                {
                    nextMaPN = defaultMaPN;
                    break;
                }
            }

            return nextMaPN;
        }

        [HttpPost]
        public ActionResult DeletePhieuNhap(string maPN)
        {
            var phieuNhap = database.PhieuNhaps.FirstOrDefault(d => d.MaPN == maPN);
            if (phieuNhap != null)
            {
                database.PhieuNhaps.Remove(phieuNhap);
                database.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }

}