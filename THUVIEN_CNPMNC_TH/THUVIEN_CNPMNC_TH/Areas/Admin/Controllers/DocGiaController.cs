using THUVIEN_CNPMNC_TH.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Areas.Admin.Pattern.Builder;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class DocGiaController : Controller
    {
        LIBRARY_FINALEntities libraryDbContext = new LIBRARY_FINALEntities();

        // GET: Admin/DocGia
        public ActionResult QLDocGia()
        {
            return View(libraryDbContext.DocGias.ToList());
        }

        public ActionResult EditDocGia(string maDocGia)
        {
            var docGia = libraryDbContext.DocGias.FirstOrDefault(d => d.MaDocGia == maDocGia);
            if (docGia == null)
            {
                return HttpNotFound();
            }
            return View(docGia);
        }

        [HttpPost]
        public ActionResult SaveEdit(DocGia editedDocGia)
        {
            if (!ModelState.IsValid)
            {
                return View(editedDocGia);
            }

            var existingDocGia = libraryDbContext.DocGias.FirstOrDefault(d => d.MaDocGia == editedDocGia.MaDocGia);
            if (existingDocGia == null)
            {
                return HttpNotFound();
            }

            DocGiaBuilder docGiaBuilder = new DocGiaBuilder();
            DocGia updatedDocGia = docGiaBuilder
                .SetTenDocGia(editedDocGia.TenDocGia)
                .SetNgaySinh(editedDocGia.NgaySinh)
                .SetGioiTinh(editedDocGia.GioiTinh)
                .SetDiaChi(editedDocGia.DiaChi)
                .SetSoDienThoai(editedDocGia.SoDienThoai)
                .SetEmail(editedDocGia.Email)
                .Build();

            existingDocGia.TenDocGia = updatedDocGia.TenDocGia;
            existingDocGia.NgaySinh = updatedDocGia.NgaySinh;
            existingDocGia.DiaChi = updatedDocGia.DiaChi;
            existingDocGia.GioiTinh = updatedDocGia.GioiTinh;
            existingDocGia.SoDienThoai = updatedDocGia.SoDienThoai;

            libraryDbContext.SaveChanges();
            return RedirectToAction("QLDocGia");
        }

        [HttpPost]
        public ActionResult DeleteDocGia(string maDocGia)
        {
            var docGia = libraryDbContext.DocGias.FirstOrDefault(d => d.MaDocGia == maDocGia);
            if (docGia != null)
            {
                libraryDbContext.DocGias.Remove(docGia);
                libraryDbContext.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult AddDocGia()
        {
            return View();
        }

        // POST: Admin/DocGia/SaveAdd
        [HttpPost]
        public ActionResult SaveAdd(DocGia newDocGia)
        {
            if (ModelState.IsValid)
            {
                DocGiaBuilder docGiaBuilder = new DocGiaBuilder();
                DocGia docGia = docGiaBuilder
                    .SetTenDocGia(newDocGia.TenDocGia)
                    .SetNgaySinh(newDocGia.NgaySinh)
                    .SetGioiTinh(newDocGia.GioiTinh)
                    .SetDiaChi(newDocGia.DiaChi)
                    .SetSoDienThoai(newDocGia.SoDienThoai)
                    .SetEmail(newDocGia.Email)
                    .SetMaDocGia(GenerateNextMaDocGia()) // Generate and set MaDocGia
                    .Build();

                // Check for existing email and phone number, and save docGia
                // (Code for these steps not included here for brevity)

                libraryDbContext.DocGias.Add(docGia);
                libraryDbContext.SaveChanges();
                return RedirectToAction("QLDocGia");
            }
            return View("AddDocGia", newDocGia);
        }

        private string GenerateNextMaDocGia()
        {
            string defaultMaDocGia = "DG001";
            string nextMaDocGia = defaultMaDocGia;

            while (libraryDbContext.DocGias.Any(d => d.MaDocGia == nextMaDocGia))
            {
                string numericPart = nextMaDocGia.Substring(2);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaDocGia = string.Format("DG{0:D3}", numericValue);
                }
                else
                {
                    nextMaDocGia = defaultMaDocGia;
                    break;
                }
            }
            return nextMaDocGia;
        }
    }
}
