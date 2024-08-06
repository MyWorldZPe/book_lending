using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;
using THUVIEN_CNPMNC_TH.Pattern.SingleTon;



namespace THUVIEN_CNPMNC_TH.Controllers
{
    public class LoginController : Controller
    {
        LIBRARY_FINALEntities db = new LIBRARY_FINALEntities();

        public ActionResult LoginAndRegistration()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult KiemTraDangNhap(string username, string password)
        {
            var user = AuthManager.Instance.AuthenticateUser(username, password);
            var docGia = AuthManager.Instance.AuthenticateDocGia(username, password);

            if (user != null)
            {
                if (user.Roles == 3)
                {
                    Session["UserRoles"] = user.Roles;
                    return RedirectToAction("Index", "HomeClient");
                }
                else if (user.Roles == 1 || user.Roles == 2)
                {
                    Session["UserRoles"] = user.Roles;
                    TempData["SuccessMessage"] = "Đăng nhập thành công.";
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            else if (docGia != null)
            {
                if (docGia.Roles == 3)
                {
                    Session["DocGiaRoles"] = docGia.Roles;
                    Session["DocGiaID"] = docGia.MaDocGia;
                    TempData["SuccessMessage"] = "Đăng nhập thành công.";
                    return RedirectToAction("Index", "HomeClient");
                }
            }
            TempData["ErrorMessage"] = "Đăng nhập không thành công. Vui lòng kiểm tra lại thông tin đăng nhập.";
            return RedirectToAction("LoginAndRegistration");
        }

        public ActionResult Logout()
        {
            Session["DocGiaRoles"] = null;
            Session["UserRoles"] = null;
            TempData["SuccessMessage"] = "Đăng xuất  thành công.";
            return RedirectToAction("Index", "HomeClient");
        }

        [HttpPost]
        public ActionResult SaveRegistration(DocGia newDocGia)
        {
            if (ModelState.IsValid)
            {
                newDocGia.Roles = 3;
                newDocGia.MaDocGia = GenerateNextMaDocGia();
                db.DocGias.Add(newDocGia);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công.";
                return RedirectToAction("LoginAndRegistration");
            }
            return View("LoginAndRegistration", newDocGia);
        }

        private string GenerateNextMaDocGia()
        {
            string defaultMaDocGia = "DG001";
            string nextMaDocGia = defaultMaDocGia;

            while (db.DocGias.Any(d => d.MaDocGia == nextMaDocGia))
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