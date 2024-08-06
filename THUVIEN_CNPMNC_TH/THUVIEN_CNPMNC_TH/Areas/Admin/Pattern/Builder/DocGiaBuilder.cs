using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Pattern.Builder
{
    public class DocGiaBuilder
    {
        private string MaDocGia;
        private string TenDocGia;
        private DateTime? NgaySinh;
        private string GioiTinh;
        private string DiaChi;
        private string SoDienThoai;
        private string Email;

        public DocGiaBuilder SetMaDocGia(string maDocGia)
        {
            this.MaDocGia = maDocGia;
            return this;
        }
        public DocGiaBuilder SetTenDocGia(string tenDocGia)
        {
            this.TenDocGia = tenDocGia;
            return this;
        }

        public DocGiaBuilder SetNgaySinh(DateTime? ngaySinh)
        {
            this.NgaySinh = ngaySinh;
            return this;
        }

        public DocGiaBuilder SetGioiTinh(string gioiTinh)
        {
            this.GioiTinh = gioiTinh;
            return this;
        }

        public DocGiaBuilder SetDiaChi(string diaChi)
        {
            this.DiaChi = diaChi;
            return this;
        }

        public DocGiaBuilder SetSoDienThoai(string soDienThoai)
        {
            this.SoDienThoai = soDienThoai;
            return this;
        }

        public DocGiaBuilder SetEmail(string email)
        {
            this.Email = email;
            return this;
        }

        public DocGia Build()
        {
            return new DocGia
            {
                MaDocGia = this.MaDocGia,
                TenDocGia = this.TenDocGia,
                NgaySinh = this.NgaySinh,
                GioiTinh = this.GioiTinh,
                DiaChi = this.DiaChi,
                SoDienThoai = this.SoDienThoai,
                Email = this.Email
            };
        }
    }

}