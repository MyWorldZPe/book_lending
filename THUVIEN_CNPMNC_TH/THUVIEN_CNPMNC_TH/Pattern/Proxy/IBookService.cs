using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THUVIEN_CNPMNC_TH.Models;
namespace THUVIEN_CNPMNC_TH.Pattern.Proxy
{
    public interface IBookService
    {
        ViewModel GetBookDetail(string bookId);
    }

}
