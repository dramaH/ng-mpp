using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ARchGLCloud.Core
{
    public class EmailHelper
    {
        private static bool SendMail(MimeMessage mailMessage, MailOptions config)
        {
            try
            {
                var smtpClient = new SmtpClient();
                smtpClient.Timeout = 10 * 1000;   
                smtpClient.Connect(config.Host, config.Port, MailKit.Security.SecureSocketOptions.Auto);
                smtpClient.Authenticate(config.Address, config.Password);
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
                return true;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="receives">接收人</param>
        /// <param name="sender">发送人</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <param name="attachments">附件</param>
        /// <param name="fileName">附件名</param>
        /// <returns></returns>
        public static bool SendMail(MailOptions config, List<string> receives, string sender, string subject, string body, byte[] attachments = null, string fileName = "")
        {
            var fromMailAddress = new MailboxAddress(config.Name, config.Address);
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(fromMailAddress);
            foreach (var add in receives)
            {
                var toMailAddress = new MailboxAddress(add);
                mailMessage.To.Add(toMailAddress);
            }
            if (!string.IsNullOrEmpty(sender))
            {
                var replyTo = new MailboxAddress(config.Name, sender);
                mailMessage.ReplyTo.Add(replyTo);
            }

            //附件
            var bodyBuilder = new BodyBuilder() { HtmlBody = body };
            if (attachments != null)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = "未命名文件.txt";
                }

                //解决中文文件名乱码
                var attachment = bodyBuilder.Attachments.Add(fileName, attachments);
                var charset = "GB18030";
                attachment.ContentType.Parameters.Clear();
                attachment.ContentDisposition.Parameters.Clear();
                attachment.ContentType.Parameters.Add(charset, "name", fileName);
                //解决文件名不能超过41字符
                attachment.ContentDisposition.Parameters.Add(charset, "filename", fileName);

                foreach (var param in attachment.ContentDisposition.Parameters)
                {
                    param.EncodingMethod = ParameterEncodingMethod.Rfc2047;
                }

                foreach (var param in attachment.ContentType.Parameters)
                {
                    param.EncodingMethod = ParameterEncodingMethod.Rfc2047;
                }
            }

            mailMessage.Body = bodyBuilder.ToMessageBody();
            mailMessage.Subject = subject;

            return SendMail(mailMessage, config);
        }
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpServer">SMTP服务器</param>
        /// <param name="enableSsl">是否启用SSL加密</param>
        /// <param name="userName">登录帐号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="nickName">发件人昵称</param>
        /// <param name="fromEmail">发件人</param>
        /// <param name="toEmail">收件人</param>
        /// <param name="subj">主题</param>
        /// <param name="bodys">内容</param>
        public static async Task SendMailAsync(string smtpServer, bool enableSsl, string userName, string pwd, string nickName, string fromMail, string toMail, string subj, string bodys)
        {
            System.Net.Mail.SmtpClient smtpClient = new  System.Net.Mail.SmtpClient();
            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer;//指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(userName, pwd);//用户名和密码
            smtpClient.EnableSsl = enableSsl;


            var fromAddress = new System.Net.Mail.MailAddress(fromMail, nickName);
            var toAddress = new System.Net.Mail.MailAddress(toMail);

            var mailMessage = new System.Net.Mail.MailMessage(fromAddress, toAddress);
            mailMessage.Subject = subj;//主题
            mailMessage.Body = bodys;//内容
            mailMessage.BodyEncoding = Encoding.Default;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;//优先级

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
