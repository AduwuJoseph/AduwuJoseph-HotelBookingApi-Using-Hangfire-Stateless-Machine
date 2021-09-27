using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace HotelBooking.Api.Helpers
{
    public static class EmailSender
    {
        public static string BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            try
            {
                string from, to, bcc, cc, subject, body;
                from = "customerserviceme38@gmail.com";
                to = sendTo.Trim();
                bcc = "";
                cc = "";
                subject = subjectText;
                StringBuilder sb = new StringBuilder();
                sb.Append(bodyText);
                body = sb.ToString();
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(to));
                if (!string.IsNullOrEmpty(bcc))
                {
                    mail.Bcc.Add(new MailAddress(bcc));
                }
                if (!string.IsNullOrEmpty(cc))
                {
                    mail.CC.Add(new MailAddress(cc));
                }
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SendMail(mail);
                return "success";
            }
            catch (Exception ex)
            {
                return "failed " + ex.Message;
            }
        }

        public static void SendMail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("customerserviceme38@gmail.com", "Pa$$w00rd");

            //SmtpClient smtpClient = new SmtpClient();
            //smtpClient.UseDefaultCredentials = true;
            //smtpClient.Host = "213.175.217.26";
            //smtpClient.Port = 25;
            //smtpClient.EnableSsl = false;
            //smtpClient.Credentials = new System.Net.NetworkCredential("info@megamedxpharm.com.ng", "Pa$$w00rd2019@@");

            //ServicePointManager.ServerCertificateValidationCallback =
            //delegate (object s, X509Certificate certificate,
            //    X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //{ return true; };
            try
            {
                //smtpClient.Send(mail);
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
