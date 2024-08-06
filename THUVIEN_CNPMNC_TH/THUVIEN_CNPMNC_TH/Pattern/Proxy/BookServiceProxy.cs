using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;
namespace THUVIEN_CNPMNC_TH.Pattern.Proxy
{
    public class BookServiceProxy : IBookService
    {
        private readonly IBookService _bookService;

        public BookServiceProxy(IBookService bookService)
        {
            _bookService = bookService;
        }

        public ViewModel GetBookDetail(string bookId)
        {
          

            return _bookService.GetBookDetail(bookId);
        }
    }

}