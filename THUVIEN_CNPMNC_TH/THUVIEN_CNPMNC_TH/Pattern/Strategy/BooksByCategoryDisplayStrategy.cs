using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;
namespace THUVIEN_CNPMNC_TH.Pattern.Strategy
{
    public class BooksByCategoryDisplayStrategy : IBookDisplayStrategy
    {
        private readonly LIBRARY_FINALEntities _db;

        public BooksByCategoryDisplayStrategy(LIBRARY_FINALEntities db)
        {
            _db = db;
        }

        public IEnumerable<Book> GetBooksByCategory(string category)
        {
            return _db.Books.Where(s => s.TheLoai.TenTheLoai == category);
        }
    }


}