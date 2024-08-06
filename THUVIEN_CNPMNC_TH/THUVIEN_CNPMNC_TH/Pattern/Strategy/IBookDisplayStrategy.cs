using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;
namespace THUVIEN_CNPMNC_TH.Pattern.Strategy
{
    public interface IBookDisplayStrategy
    {
        IEnumerable<Book> GetBooksByCategory(string category);
    }



}