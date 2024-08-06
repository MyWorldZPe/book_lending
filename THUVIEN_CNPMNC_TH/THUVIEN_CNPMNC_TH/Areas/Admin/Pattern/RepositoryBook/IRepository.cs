using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.RepositoryBook
{
    public interface IRepository<T> where T : class
    {
        T GetBookById(object id);
        IEnumerable<T> GetAllBooks();
        void AddBook(T entity);
        void UpdateBook(T entity);
        void DeleteBook(object id);
        void Save();
        void UpdateDisplayStatus();

    }
}