using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Command
{
    public class AddTheDocGiaCommand : ICommand
    {
        private readonly TheDocGia newTheDocGia;
        private readonly LIBRARY_FINALEntities libraryDbContext;

        public AddTheDocGiaCommand(TheDocGia newTheDocGia, LIBRARY_FINALEntities database)
        {
            this.newTheDocGia = newTheDocGia;
            this.libraryDbContext = database;
        }

        public void Execute()
        {
            libraryDbContext.TheDocGias.Add(newTheDocGia);
            libraryDbContext.SaveChanges();
        }
    }
}