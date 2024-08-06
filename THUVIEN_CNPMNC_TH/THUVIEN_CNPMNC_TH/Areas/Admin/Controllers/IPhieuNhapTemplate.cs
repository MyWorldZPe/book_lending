using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public interface IPhieuNhapTemplate
    {
        //Template Method Pattern
        string GenerateNextMaPN();
        ActionResult NhapSach(PhieuNhap phieuNhap);
    }
}
