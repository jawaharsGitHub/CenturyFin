using System;
using System.Net;
using System.Net.Mail;

namespace Common
{
    public class AppCommunication
    {

        public static void SendEmail(string mailBody, DateTime collectionDate)
        {
            try
            {

                var myEmail = "jawahar.subramanian83@gmail.com";
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(myEmail);
                message.To.Add(new MailAddress(myEmail));
                message.Subject = $"Jeyam Finance - Balance Report For {collectionDate.ToShortDateString()}";
                message.IsBodyHtml = false; //to make message body as html  
                message.Body = mailBody;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(myEmail, "liamg@38nainamarbus");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendSms(string mailBody, DateTime collectionDate)
        {
            try
            {


            }
            catch (Exception ex)
            {
            }
        }

        public static void SendWhatsApp(string mailBody, DateTime collectionDate)
        {
            try
            {


            }
            catch (Exception ex)
            {
            }
        }
    }
}
