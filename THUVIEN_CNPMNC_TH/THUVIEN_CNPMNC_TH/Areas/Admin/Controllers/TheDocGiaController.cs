using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Areas.Admin.Command;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class TheDocGiaController : Controller
    {
        private readonly LIBRARY_FINALEntities database = new LIBRARY_FINALEntities();

        // GET: Admin/TheDocGia
        public ActionResult QLTheDocGia()
        {
            var viewModel = new ViewModel
            {
                TheDocGias = database.TheDocGias.ToList(),
                // Other properties need to be populated here as well
            };

            return View(viewModel);
        }

        public ActionResult ThemTheDocGia()
        {
            var listTenDocGia = database.DocGias.Select(dg => new { dg.TenDocGia, dg.MaDocGia }).ToList();
            ViewBag.ListTenDocGia = new SelectList(listTenDocGia, "MaDocGia", "TenDocGia");

            return View();
        }
        [HttpPost]
        public ActionResult LuuTheDocGia(TheDocGia newTheDocGia, int HanSuDungOption)
        {
            if (ModelState.IsValid)
            {
                // Tạo NgayCap là ngày hiện tại
                newTheDocGia.NgayCap = DateTime.Now;

                if (newTheDocGia.NgayCap > DateTime.Now)
                {
                    ModelState.AddModelError("NgayCap", "Ngày cấp không được lớn hơn ngày hiện tại.");
                    return View("ThemTheDocGia", newTheDocGia);
                }

                // Tính toán ngày hết hạn dựa trên thời gian đã chọn và ngày cấp
                switch (HanSuDungOption)
                {
                    case 3:
                        newTheDocGia.HanSuDung = newTheDocGia.NgayCap.Value.AddMonths(3);
                        break;
                    case 6:
                        newTheDocGia.HanSuDung = newTheDocGia.NgayCap.Value.AddMonths(6);
                        break;
                    case 12:
                        newTheDocGia.HanSuDung = newTheDocGia.NgayCap.Value.AddYears(1);
                        break;
                }

                // Tự động sinh mã thẻ mới và lưu vào cơ sở dữ liệu
                newTheDocGia.MaThe = GenerateNextMaThe();
                database.TheDocGias.Add(newTheDocGia);
                database.SaveChanges();
                return RedirectToAction("QLTheDocGia");
            }
            return View("ThemTheDocGia", newTheDocGia);
        }


        private string GenerateNextMaThe()
        {
            string defaultMaThe = "T001";
            string nextMaThe = defaultMaThe;

            // Kiểm tra xem MaThe đã tồn tại trong cơ sở dữ liệu hay chưa
            while (database.TheDocGias.Any(t => t.MaThe == nextMaThe))
            {
                // Tách phần số hậu tố ra khỏi MaThe
                string numericPart = nextMaThe.Substring(1);

                // Tăng giá trị số lên 1 và cập nhật MaThe mới
                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaThe = $"T{numericValue:D03}";
                }
                else
                {
                    // Nếu không thể phân tích phần số hậu tố, trở lại mã mặc định "T01"
                    nextMaThe = defaultMaThe;
                    break;
                }
            }

            return nextMaThe;
        }

        [HttpPost]
        public ActionResult XoaTheDocGia(string maThe)
        {
            if (string.IsNullOrEmpty(maThe))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tạo và thực thi lệnh xóa thẻ độc giả
            ICommand deleteCommand = new DeleteTheDocGiaCommand(maThe, database);
            deleteCommand.Execute();

            return RedirectToAction("QLTheDocGia");
        }
    }
}
