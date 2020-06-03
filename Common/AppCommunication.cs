using Common.ExtensionMethod;
using System;
using System.Net;
using System.Net.Mail;

namespace Common
{
    public class AppCommunication
    {
        private static (MailMessage, SmtpClient) GetMailMessage(string subject, string mailBody, bool haveCC = false)
        {

            var myEmail = "jawahar.subramanian83@gmail.com";
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(myEmail);
            message.To.Add(new MailAddress(myEmail));

            if (haveCC) message.CC.Add(new MailAddress("jeyapriyagopal@gmail.com"));
            message.Subject = subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = mailBody;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(myEmail, "nainamarbus");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            return (message, smtp);


        }
        public static void SendBalanceEmail(string mailBody, DateTime collectionDate, string activeCusCount, string subject, bool haveCC = false)
        {
            try
            {

                //BalanceDetail
                var sub = $"[{activeCusCount}] {subject} - {collectionDate.Ddmmyy()}";
                var smtp = GetMailMessage(sub, mailBody, haveCC);
                smtp.Item2.Send(smtp.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendReportEmail(EmailStructure emailStructure)
        {
            try
            {
                if (emailStructure == null) return;

                var sub = $"{emailStructure.CollectionDate.Ddmmyy()} {emailStructure.Subject} Report";
                var smtp = GetMailMessage(sub, emailStructure.HtmlContent);
                smtp.Item2.Send(smtp.Item1);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendCustomerTxnEmail(string mailBody, DateTime collectionDate, string attachmentFilePath)
        {
            try
            {
                var sub = $"Transaction For {mailBody} On {collectionDate.Ddmmyy()}";
                var smtp = GetMailMessage(sub, mailBody);
                smtp.Item1.Attachments.Add(new Attachment(attachmentFilePath)); // attachments
                smtp.Item2.Send(smtp.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
