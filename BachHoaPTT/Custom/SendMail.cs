using BachHoaPTT.Areas.Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BachHoaPTT.Custom
{
    public static class SendMail
    {
        public static void SendEmail(string email, string hostEmail, string hostPass)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(hostEmail);
            mail.Subject = "Subcibe Bách hoá PTT thành công !!!";
            string body = "Bạn đã đăng kí nhận thông tin qua email từ Bách Hoá PTT !";
            body += Environment.NewLine;
            body += "Hãy kiểm tra email thường xuyên để cập nhật thông tin về sản phẩm cũng như các chế độ ưu đãi của chúng tôi!";
            body += Environment.NewLine;
            body += "Xin cảm ơn !!!";
            mail.Body = body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = Convert.ToInt16("587");
            smtp.Credentials = new NetworkCredential(hostEmail, hostPass);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public static void SendNote(string email)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress("17110377@student.hcmute.edu.vn", "Bách Hóa PTT");
            mail.Subject = "Đặt hàng thành công !!!";
            string body = "Bạn đã đặt hàng thành công từ trang web Bách Hoá PTT";
            body += Environment.NewLine;
            body += "Chúng tôi sẽ giao hàng trong thời gian sớm nhất. Tại địa chỉ: " + ReposDetail.DiaChi;
            body += Environment.NewLine;
            body += "Tổng số tiền đơn hàng: " + ReposDetail.TongTien + "đ";
            body += Environment.NewLine;
            body += "Xin cảm ơn !!!";
            mail.Body = body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = Convert.ToInt16("587");
            smtp.Credentials = new System.Net.NetworkCredential("17110377@student.hcmute.edu.vn", "Gtien610");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        //public static void SendPass(string email, string hostEmail, string hostPass)
        //{
        //    MailMessage mail = new MailMessage();
        //    mail.To.Add(email);
        //    mail.From = new MailAddress(hostEmail);
        //    mail.Subject = "Lấy lại mật khẩu !!!";
        //    string body = "Yêu cầu cấp lại mật khẩu của bạn thành công !!!";
        //    body += Environment.NewLine;
        //    body += "Mã xác nhận của bạn là: " + ReposDetail.DiaChi;
        //    body += Environment.NewLine;
        //    body += "829752";
        //    body += Environment.NewLine;
        //    body += "Xin cảm ơn !!!";
        //    mail.Body = body;
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = "smtp.gmail.com";
        //    smtp.Port = Convert.ToInt16("587");
        //    smtp.Credentials = new NetworkCredential(hostEmail, hostPass);
        //    smtp.EnableSsl = true;
        //    smtp.Send(mail);
        //}
    }
}