using System;
using System.Net.Mail;

namespace Findlover.Helper
{
    public class SendMail
    {
        public static bool WebMailTo(string toEmails, string subject, string emailText)
        {
            #region 此处参数使用时根据需要替换成自己的
            const string server = "ip"; //此处代表Mail Server地址
            const string formEmail = "yourmail@163.com"; //此处代表系统发邮件的时候的发件人地址
            const string formDispayName = "意中人"; //系统发件人的显示名称
            const string formPassword = "xxxx"; //此处代表系统发邮件的时候的发件人的密码
            const string formDomain = "xxxx"; //域名
            #endregion

            const string systermtxt = "<br/>该邮件为系统自动发送，请勿回复！详情请点击" + "<a href='http://http://cnwj6iapc006:81/'>这里~</a>";
            var mailMessage = new System.Net.Mail.MailMessage { IsBodyHtml = true };
            mailMessage.To.Add(toEmails);
            mailMessage.From = new System.Net.Mail.MailAddress(formEmail, formDispayName);
            mailMessage.Subject = subject;
            mailMessage.Body = emailText+systermtxt;//此处可以传递一个html
            mailMessage.Priority = MailPriority.High;
            var client = new System.Net.Mail.SmtpClient
                {
                    Host = server,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(formEmail, formPassword, formDomain),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = false
                };
            bool isSendOk;
            try
            {
                client.Send(mailMessage);//发送Mail
                isSendOk = true;

            }
            catch (Exception)
            {

                isSendOk = false;
            }
            return isSendOk;
        }
    }
}