using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Dependency
{
    public class TheLoaiService : ITheLoaiService
    {
        private readonly LIBRARY_FINALEntities _context;

        public TheLoaiService(LIBRARY_FINALEntities context)
        {
            _context = context;
        }

        public IEnumerable<TheLoai> GetAllTheLoais()
        {
            return _context.TheLoais.ToList();
        }

        public TheLoai GetTheLoaiById(string id)
        {
            return _context.TheLoais.Find(id);
        }

        public void AddTheLoai(TheLoai theLoai)
        {
            _context.TheLoais.Add(theLoai);
            _context.SaveChanges();
        }

        public void UpdateTheLoai(TheLoai theLoai)
        {
            _context.Entry(theLoai).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteTheLoai(string id)
        {
            TheLoai theLoai = _context.TheLoais.Find(id);
            if (theLoai != null)
            {
                _context.TheLoais.Remove(theLoai);
                _context.SaveChanges();
            }
        }

        public bool CanDeleteTheLoai(string maTheLoai)
        {
            return !_context.Books.Any(b => b.MaTheLoai == maTheLoai);
        }
    }
}