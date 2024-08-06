using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class MuonTraControllerProxy : Controller
    {
        private MuonTraController muonTraController = new MuonTraController();

        // Phương thức proxy
        public ActionResult QLMuonTra(ControllerContext context)
        {
            if (AuthenticateUser(context))
            {
                return muonTraController.QLMuonTra(context);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return View("AccessDenied");
            }
        }
        [HttpGet]
        public ActionResult ThemMuonTra(ControllerContext context)
        {

            if (AuthenticateUser(context))
            {
                return muonTraController.ThemMuonTra();
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return View("AccessDenied");
            }

        }
        [HttpPost]
        public ActionResult ThemMuonTra(ControllerContext context, PhieuMuonTra phieuMuonTra) 
        {

            if (AuthenticateUser(context))
            {
                return muonTraController.ThemMuonTra(phieuMuonTra);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return View("AccessDenied");
            }

        }
        [HttpPost]
        public ActionResult XoaPhieuMuonTra(ControllerContext context, string id)
        {
            if (AuthenticateUser(context))
            {
                return muonTraController.XoaPhieuMuonTra(id);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return Json(new { success = false, message = "Truy cập bị từ chối." });
            }
        }
        [HttpGet]
        public ActionResult TraSach(ControllerContext context, string id)
        {
            if (AuthenticateUser(context))
            {
                return muonTraController.TraSach(id);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return View("AccessDenied");
            }
        }
        [HttpPost]
        public ActionResult TraSach(ControllerContext context, PhieuMuonTra phieuMuonTra)
        {
            if (AuthenticateUser(context))
            {
                return muonTraController.TraSach(phieuMuonTra);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return Json(new { success = false, message = "Truy cập bị từ chối." });
            }
        }
        [HttpGet]
        public ActionResult ThemPhieuPhat(ControllerContext context, string id)
        {
            if (AuthenticateUser(context))
            {
                return muonTraController.ThemPhieuPhat(id);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return View("AccessDenied");
            }
        }
        [HttpPost]
        public ActionResult ThemPhieuPhat(ControllerContext context, PhieuPhat phieuPhat)
        {
            if (AuthenticateUser(context))
            {
                return muonTraController.ThemPhieuPhat(phieuPhat);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return View("AccessDenied");
            }
        }
        [HttpGet]
        public ActionResult DuyetPhieu(ControllerContext context, string id)
        {
            if (AuthenticateUser(context))
            {
                return muonTraController.DuyetPhieu(id);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return View("AccessDenied");
            }
        }
        [HttpPost]
        public ActionResult DuyetPhieu(ControllerContext context, PhieuMuonTra phieuMuonTra)
        {
            if (AuthenticateUser(context))
            {
                return muonTraController.DuyetPhieu(phieuMuonTra);
            }
            else
            {
                // Xử lý khi quyền truy cập không hợp lệ
                return View("AccessDenied");
            }
        }
        // Phương thức xác thực người dùng
        private bool AuthenticateUser(ControllerContext context)
        {
            return false;
        }
    }
}