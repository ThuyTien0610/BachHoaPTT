using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BachHoaPTT.Models
{
    public class LayMaXacNhan
    {
        [Key]
        [Display(Name = "Nhập mã xác nhận")]
        [Required(ErrorMessage = "Mời nhập mã xác nhận")]
        public string Code { set; get; }
    }
}