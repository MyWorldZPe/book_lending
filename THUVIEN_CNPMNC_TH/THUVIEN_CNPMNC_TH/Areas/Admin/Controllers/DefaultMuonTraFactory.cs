using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class DefaultMuonTraFactory : IMuonTraFactory
    {
        private LIBRARY_FINALEntities database;

        public DefaultMuonTraFactory(LIBRARY_FINALEntities dbContext)
        {
            database = dbContext;
        }

        public PhieuMuonTra CreatePhieuMuonTra()
        {
            return new PhieuMuonTra();
        }
    }

}