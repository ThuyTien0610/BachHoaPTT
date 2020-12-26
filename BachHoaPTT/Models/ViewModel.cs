using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BachHoaPTT.Models
{
    public class LoginView
    {
        [Required]
        [Display(Name = "Số điện thoại")]
        public string Sdt { get; set; }

        [Required]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }
    }

    //public class LayMK
    //{
    //    public string Email { get; set; }
    //    public string Code { get; set; }
    //}

    //public class Search
    //{
    //    public string SearchValue { get; set; }
    //}
}