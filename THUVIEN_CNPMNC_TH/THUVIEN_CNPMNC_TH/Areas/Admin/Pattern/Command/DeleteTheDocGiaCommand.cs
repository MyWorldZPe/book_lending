using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Command
{
    public class DeleteTheDocGiaCommand : ICommand
    {
        private readonly string maThe;
        private readonly LIBRARY_FINALEntities libraryDbContext;

        public DeleteTheDocGiaCommand(string maThe, LIBRARY_FINALEntities database)
        {
            this.maThe = maThe;
            this.libraryDbContext = database;
        }

        public void Execute()
        {
            var theDocGia = libraryDbContext.TheDocGias.FirstOrDefault(t => t.MaThe == maThe);
            if (theDocGia != null)
            {
                libraryDbContext.TheDocGias.Remove(theDocGia);
                libraryDbContext.SaveChanges();
            }
        }
    }
}