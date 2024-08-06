using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Pattern.SingleTon
{
    public class AuthManager
    {
        private static AuthManager instance;
        private LIBRARY_FINALEntities db;

        private AuthManager()
        {
            db = new LIBRARY_FINALEntities();
        }

        public static AuthManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuthManager();
                }
                return instance;
            }
        }

        public User AuthenticateUser(string username, string password)
        {
            return db.Users.FirstOrDefault(u => u.UserName == username && u.Passwords == password);
        }

        public DocGia AuthenticateDocGia(string email, string password)
        {
            return db.DocGias.FirstOrDefault(d => d.Email == email && d.Passwords == password);
        }
    }
}