using System;
using System.Net;
using System.Net.Mail;

namespace Common
{
    public class AppCommunication
    {

        public static void SendEmail(string mailBody, DateTime collectionDate, int activeCusCount)
        {
            try
            {

                var myEmail = "jawahar.subramanian83@gmail.com";
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(myEmail);
                message.To.Add(new MailAddress(myEmail));
                message.Subject = $"[{activeCusCount}]{collectionDate.ToShortDateString()} - Jeyam Finance Balance Report";
                message.IsBodyHtml = true; //to make message body as html  
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

        public static void SendCustomerTxnEmail(string mailBody, DateTime collectionDate, string attachmentFilePath)
        {
            try
            {

                var myEmail = "jawahar.subramanian83@gmail.com";
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(myEmail);
                message.To.Add(new MailAddress(myEmail));
                message.Subject = $"Transaction For {mailBody} On {collectionDate.ToShortDateString()}";
                message.IsBodyHtml = false; //to make message body as html  
                message.Body = mailBody;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(myEmail, "liamg@38nainamarbus");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                message.Attachments.Add(new Attachment(attachmentFilePath));


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
