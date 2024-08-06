using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.RepositoryBook
{
    public class BookRepository : IRepository<Book>
    {
        private LIBRARY_FINALEntities _libraryDbContext;

        public BookRepository(LIBRARY_FINALEntities context)
        {
            _libraryDbContext = context;
        }

        public Book GetBookById(object id)
        {
            return _libraryDbContext.Books.Find(id);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _libraryDbContext.Books.ToList();
        }

        public void AddBook(Book entity)
        {
            _libraryDbContext.Books.Add(entity);
        }

        public void UpdateBook(Book entity)
        {
            _libraryDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteBook(object id)
        {
            Book book = _libraryDbContext.Books.Find(id);
            if (book != null)
                _libraryDbContext.Books.Remove(book);
        }

        public void Save()
        {
            _libraryDbContext.SaveChanges();
        }

        // Dùng để update status của sách 
        public void UpdateDisplayStatus()
        {
            var books = _libraryDbContext.Books.ToList();
            foreach (var book in books)
            {
                if (book.SoLuong <= 3)
                {
                    book.TrungBay = false;
                }
            }
            _libraryDbContext.SaveChanges();
        }

    }
}


