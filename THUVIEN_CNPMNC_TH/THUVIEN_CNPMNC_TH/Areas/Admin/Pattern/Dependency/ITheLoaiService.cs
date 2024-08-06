using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Dependency
{
    public interface ITheLoaiService
    {
        IEnumerable<TheLoai> GetAllTheLoais();
        TheLoai GetTheLoaiById(string id);
        void AddTheLoai(TheLoai theLoai);
        void UpdateTheLoai(TheLoai theLoai);
        void DeleteTheLoai(string id);
        bool CanDeleteTheLoai(string id);
    }
}      