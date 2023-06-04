using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace EduSpaceAPI.Helpers
{
    public class SendVerificationCode
    {
        private static string MyEmail = "lmseduspace@gmail.com";
        private static string MyPassCode = "wpzwudqvemjukxjo";
        public static string SendCode(string emailaddress, string subject, string body )
        {
            try
            {
                // Generate a random verification code
            //    var verificationCode = GenerateVerificationCode.VerificationCode();

                // Configure the SMTP client
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(MyEmail, MyPassCode);

                // Compose the email message
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(MyEmail);
                mailMessage.To.Add(emailaddress);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                // Send the email
                smtpClient.Send(mailMessage);

                return "successfully sent.";
            }
            catch (Exception ex)
            {
                return "Failed to send verification code. Error: " + ex.Message;
            
        }
  


    }
}
}
