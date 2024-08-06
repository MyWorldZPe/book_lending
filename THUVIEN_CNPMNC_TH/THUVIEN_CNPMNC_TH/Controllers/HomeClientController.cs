using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;
using THUVIEN_CNPMNC_TH.Pattern.Strategy;
using THUVIEN_CNPMNC_TH.Pattern.Strategy_NXB_;
using THUVIEN_CNPMNC_TH.Pattern.Proxy;



namespace THUVIEN_CNPMNC_TH.Controllers
{
    public class HomeClientController : Controller
    {
        private readonly LIBRARY_FINALEntities db, db1;
        private readonly IBookDisplayStrategy _bookDisplayStrategy;
        private readonly IPublisherStrategy _publisherStrategy;
        private readonly IBookService _bookService;
        public HomeClientController()
        {
            db = new LIBRARY_FINALEntities();

            _bookDisplayStrategy = new BooksByCategoryDisplayStrategy(db);
            _publisherStrategy = new BooksByPublisherDisplayStrategy(db);
            _bookService = new BookService(db);
        }


        public ActionResult Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            var viewModel = new ViewModel
            {
                Books = db.Books.ToList(),
                DocGias = db.DocGias.ToList(),
                TheLoai = db.TheLoais.ToList(),
                NXB = db.NhaXuatBans.ToList()
            };
            return View(viewModel);
        }


        public ActionResult Library()
        {
            var viewModel = new ViewModel
            {
                Books = db.Books.ToList(),
            };
            return View(viewModel);
        }


        public ActionResult XemDsTheoTheLoai(string TenTheLoai)
        {
            ViewBag.TenTheLoai = TenTheLoai; // Set the category name in ViewBag

            var books = _bookDisplayStrategy.GetBooksByCategory(TenTheLoai);

            var viewModel = new ViewModel
            {
                Books = books.ToList()
            };

            return View(viewModel);
        }

        public ActionResult XemDsTheoNXB(string TenNXB)
        {
            ViewBag.TenNXB = TenNXB;

            var books = _publisherStrategy.GetBooksByPublisher(TenNXB);

            var viewModel = new ViewModel
            {
                Books = books.ToList()
            };

            return View(viewModel);
        }


        public ActionResult XemChiTietSach(string MaSach)
        {
            var viewModel = _bookService.GetBookDetail(MaSach);
            return View(viewModel);
        }

        public abstract class DocGiaRegistrationDecorator : Controller
        {
            protected readonly LIBRARY_FINALEntities db;

            public DocGiaRegistrationDecorator(LIBRARY_FINALEntities dbContext)
            {
                db = dbContext;
            }

            public abstract ActionResult DangKyTheDocGia();
        }
        public ActionResult DangKyTheDocGia()
        {

            if (Session["DocGiaRoles"] != null && Session["DocGiaID"] != null)
            {
                var maDocGia = Session["DocGiaID"].ToString();
                ViewBag.MaDocGia = maDocGia;
                if (db.TheDocGias.Any(t => t.MaDocGia == maDocGia))
                {
                    TempData["ErrorMessage"] = "Độc giả đã sở hữu thẻ độc giả. Không thể đăng ký thêm thẻ.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("LoginAndRegistration", "Login");
            }
            return View();
        }
        // Builder
        public class TheDocGiaBuilder
        {
            private TheDocGia theDocGia;

            public TheDocGiaBuilder()
            {
                theDocGia = new TheDocGia();
            }

            public TheDocGiaBuilder SetNgayCap(DateTime ngayCap)
            {
                theDocGia.NgayCap = ngayCap;
                return this;
            }

            public TheDocGiaBuilder SetHanSuDung(int hanSuDungOption)
            {
                switch (hanSuDungOption)
                {
                    case 3:
                        theDocGia.HanSuDung = theDocGia.NgayCap.Value.AddMonths(3);
                        break;
                    case 6:
                        theDocGia.HanSuDung = theDocGia.NgayCap.Value.AddMonths(6);
                        break;
                    case 12:
                        theDocGia.HanSuDung = theDocGia.NgayCap.Value.AddYears(1);
                        break;
                }
                return this;
            }

            public TheDocGiaBuilder SetMaDocGia(string maDocGia)
            {
                theDocGia.MaDocGia = maDocGia;
                return this;
            }

            public TheDocGiaBuilder SetMaThe(string maThe)
            {
                theDocGia.MaThe = maThe;
                return this;
            }

            public TheDocGia Build()
            {
                return theDocGia;
            }
        }
        // Usage
        [HttpPost]
        public ActionResult LuuTheDocGia(TheDocGia newTheDocGia, int HanSuDungOption)
        {
            if (ModelState.IsValid)
            {
                if (newTheDocGia.NgayCap > DateTime.Now)
                {
                    ModelState.AddModelError("NgayCap", "Ngày cấp không được lớn hơn ngày hiện tại.");
                    return View("ThemTheDocGia", newTheDocGia);
                }

                // Lấy MaDocGia của DocGia đang đăng nhập
                string maDocGia = Session["DocGiaID"]?.ToString();
                if (string.IsNullOrEmpty(maDocGia))
                {
                    return RedirectToAction("LoginAndRegistration", "Login");
                }

                // Sử dụng Builder Pattern để tạo đối tượng TheDocGia
                var theDocGiaBuilder = new TheDocGiaBuilder()
                    .SetNgayCap(newTheDocGia.NgayCap.Value)
                    .SetHanSuDung(HanSuDungOption)
                    .SetMaDocGia(maDocGia)
                    .SetMaThe(GenerateNextMaThe());

                var theDocGia = theDocGiaBuilder.Build();

                db.TheDocGias.Add(theDocGia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("ThemTheDocGia", newTheDocGia);
        }


        private string GenerateNextMaThe()
        {
            string defaultMaThe = "T001";
            string nextMaThe = defaultMaThe;

            // Kiểm tra xem MaThe đã tồn tại trong cơ sở dữ liệu hay chưa
            while (db.TheDocGias.Any(t => t.MaThe == nextMaThe))
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


        [HttpGet]
        public ActionResult DangKySach(string MaSach)
        {
            if (string.IsNullOrEmpty(MaSach))
            {
                TempData["ErrorMessage"] = "Vui lòng cung cấp mã sách để đăng ký.";
                return RedirectToAction("Index", "HomeClient");
            }

            var listBooks = db.Books.Where(b => b.TrungBay == true).ToList();
            ViewBag.TenSach = new SelectList(listBooks, "MaSach", "TenSach");

            string docGiaID = Session["DocGiaID"] as string;

            if (string.IsNullOrEmpty(docGiaID))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đăng ký sách.";
                return RedirectToAction("LoginAndRegistration", "Login");
            }

            var selectedBook = db.Books.FirstOrDefault(b => b.MaSach == MaSach);

            if (selectedBook != null)
            {
                ViewBag.TenSach = selectedBook.TenSach;
                ViewBag.HinhAnh = selectedBook.HinhAnh;
            }

            var phieuMuonTra = new PhieuMuonTra
            {
                MaSach = MaSach,
                MaDocGia = docGiaID
            };
            return View();
        }


        [HttpPost]
        public ActionResult DangKySach(PhieuMuonTra phieuMuonTra)
        {
            if (ModelState.IsValid)
            {
                if (phieuMuonTra.NgayMuon.HasValue && phieuMuonTra.NgayTraDuKien.HasValue &&
                    phieuMuonTra.NgayTraDuKien.Value > phieuMuonTra.NgayMuon.Value.AddDays(15))
                {
                    ModelState.AddModelError("NgayTra", "Ngày trả không được lớn hơn 15 ngày so với ngày mượn.");
                }

                if (ModelState.IsValid)
                {
                    phieuMuonTra.MaPhieu = GenerateNextMaPhieu();
                    var selectedBook = db.Books.FirstOrDefault(b => b.MaSach == phieuMuonTra.MaSach);
                    if (selectedBook != null)
                    {
                        phieuMuonTra.MaSach = selectedBook.MaSach;

                        // Trừ đi số lượng sách còn lại
                        if (selectedBook.SoLuong > 0)
                        {
                            selectedBook.LuotMuonTra++;
                            selectedBook.SoLuong--;
                        }
                        else
                        {
                            ModelState.AddModelError("MaSach", "Số lượng sách đã hết.");
                            return View(phieuMuonTra);
                        }
                    }

                    string docGia = Session["DocGiaID"] as string;

                    if (string.IsNullOrEmpty(docGia))
                    {
                        TempData["ErrorMessage"] = "Vui lòng đăng nhập để đăng ký sách.";
                        return RedirectToAction("LoginAndRegistration", "Login");
                    }

                    int countUnreturnedPhieuMuonTra = db.PhieuMuonTras
                        .Count(pmt => pmt.MaDocGia == docGia && (pmt.TrangThai != "Đã Trả" && pmt.TrangThai != "Đã Phạt"));

                    if (countUnreturnedPhieuMuonTra >= 3)
                    {
                        TempData["ErrorMessage"] = "Bạn không thể đăng ký thêm sách vì đã vượt quá giới hạn số phiếu mượn trả chưa trả là tối đa 3 quyển.";
                        return RedirectToAction("Index", "HomeClient");
                    }

                    phieuMuonTra.MaDocGia = docGia;
                    phieuMuonTra.TrangThai = "Đang đăng ký";

                    db.PhieuMuonTras.Add(phieuMuonTra);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Đăng ký sách thành công.";
                    return RedirectToAction("Index", "HomeClient");
                }
            }

            var listBooks = db.Books.Where(b => b.TrungBay == true).ToList();
            ViewBag.TenSach = new SelectList(listBooks, "MaSach", "TenSach");

            string docGiaID = Session["DocGiaID"] as string;

            if (string.IsNullOrEmpty(docGiaID))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đăng ký sách.";
                return RedirectToAction("LoginAndRegistration", "HomeClient");
            }

            phieuMuonTra.MaDocGia = docGiaID;

            return View(phieuMuonTra);
        }





        private string GenerateNextMaPhieu()
        {
            string defaultMaPhieu = "PM001";
            string nextMaPhieu = defaultMaPhieu;

            while (db.PhieuMuonTras.Any(pmt => pmt.MaPhieu == nextMaPhieu))
            {
                string numericPart = nextMaPhieu.Substring(2);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaPhieu = $"PM{numericValue:D03}";
                }
                else
                {
                    nextMaPhieu = defaultMaPhieu;
                    break;
                }
            }

            return nextMaPhieu;
        }

       
        public ActionResult ViewUserProfile()
        {
            if (Session["DocGiaRoles"] != null && Session["DocGiaID"] != null)
            {
                string maDocGia = Session["DocGiaID"].ToString();
                var docGia = db.DocGias.FirstOrDefault(d => d.MaDocGia == maDocGia);

                if (docGia != null)
                {
                    var viewModel = new ViewModel
                    {
                        DocGias = new List<DocGia> { docGia },
                    };

                    return View(viewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return RedirectToAction("LoginAndRegistration", "Login");
            }
        }

        // Command 
        public interface ICommand
        {
            void Execute();
        }
        public class UpdateUserProfileCommand : ICommand
        {
            private readonly LIBRARY_FINALEntities _dbContext;
            private readonly DocGia _updatedDocGia;
            private readonly Controller _controller; // Thêm một trường để giữ tham chiếu đến Controller

            public UpdateUserProfileCommand(LIBRARY_FINALEntities dbContext, DocGia updatedDocGia, Controller controller)
            {
                _dbContext = dbContext;
                _updatedDocGia = updatedDocGia;
                _controller = controller; // Lưu trữ tham chiếu đến Controller
            }

            public void Execute()
            {
                try
                {
                    DocGia existingDocGia = _dbContext.DocGias.FirstOrDefault(d => d.MaDocGia == _updatedDocGia.MaDocGia);
                    if (existingDocGia != null)
                    {
                        existingDocGia.TenDocGia = _updatedDocGia.TenDocGia;
                        existingDocGia.NgaySinh = _updatedDocGia.NgaySinh;
                        existingDocGia.GioiTinh = _updatedDocGia.GioiTinh;
                        existingDocGia.DiaChi = _updatedDocGia.DiaChi;
                        existingDocGia.SoDienThoai = _updatedDocGia.SoDienThoai;
                        existingDocGia.Email = _updatedDocGia.Email;

                        _dbContext.SaveChanges();

                        _controller.TempData["SuccessMessage"] = "Thông tin đã được cập nhật thành công."; // Truy cập TempData thông qua Controller
                    }
                    else
                    {
                        _controller.TempData["ErrorMessage"] = "Không tìm thấy hồ sơ độc giả."; // Truy cập TempData thông qua Controller
                    }
                }
                catch (Exception ex)
                {
                    _controller.TempData["ErrorMessage"] = "Lỗi khi cập nhật thông tin: " + ex.Message; // Truy cập TempData thông qua Controller
                }
            }
        }
    
        [HttpPost]
        public ActionResult SaveUserProfile(DocGia updatedDocGia)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateUserProfileCommand(db, updatedDocGia, this);
                command.Execute();

                return RedirectToAction("ViewUserProfile");
            }

            return View("ViewUserProfile", updatedDocGia);
        }
        public ActionResult BorrowingSchedule()
        {
            if (Session["DocGiaRoles"] != null && Session["DocGiaID"] != null)
            {
                string maDocGia = Session["DocGiaID"].ToString();

                // Retrieve borrowing history for the currently logged-in user (MaDocGia)
                var borrowingHistory = db.PhieuMuonTras
                    .Where(pmt => pmt.MaDocGia == maDocGia)
                    .ToList();

                var viewModel = new ViewModel
                {
                    BorrowingHistory = borrowingHistory
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("LoginAndRegistration", "Login");
            }
        }

    }
}