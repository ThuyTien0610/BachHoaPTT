//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BachHoaPTT.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonHang
    {
        public string Id { get; set; }
        public string MaSanPham { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public Nullable<decimal> ThanhTien { get; set; }
    
        public virtual GiaoDich GiaoDich { get; set; }
    }
}
