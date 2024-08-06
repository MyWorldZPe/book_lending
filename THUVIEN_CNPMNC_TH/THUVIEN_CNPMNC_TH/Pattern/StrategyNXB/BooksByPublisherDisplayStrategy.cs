using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;
namespace THUVIEN_CNPMNC_TH.Pattern.Strategy_NXB_
{
    public class BooksByPublisherDisplayStrategy : IPublisherStrategy
    {
        private readonly LIBRARY_FINALEntities _dbContext;

        public BooksByPublisherDisplayStrategy(LIBRARY_FINALEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Book> GetBooksByPublisher(string publisherName)
        {
            return _dbContext.Books.Where(b => b.NhaXuatBan.TenNXB == publisherName).ToList();
        }
    }
}