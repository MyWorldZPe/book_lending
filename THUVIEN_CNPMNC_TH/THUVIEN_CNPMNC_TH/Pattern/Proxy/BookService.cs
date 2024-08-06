using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;
namespace THUVIEN_CNPMNC_TH.Pattern.Proxy
{
    public class BookService : IBookService
    {
        private readonly LIBRARY_FINALEntities libraryDbContext;

        public BookService(LIBRARY_FINALEntities dbContext)
        {
            libraryDbContext = dbContext;
        }

        public ViewModel GetBookDetail(string bookId)
        {
            var book = libraryDbContext.Books.FirstOrDefault(s => s.MaSach == bookId);

            if (book == null)
            {
                return new ViewModel { Books = new List<Book>(), Books2 = new List<Book>() };
            }

            var relatedBooks = libraryDbContext.Books.Where(b => b.TheLoai.TenTheLoai == book.TheLoai.TenTheLoai && b.MaSach != bookId).ToList();

            return new ViewModel
            {
                Books = new List<Book> { book },
                Books2 = relatedBooks
            };
        }
    }
}