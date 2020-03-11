using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace terminus_webapp.Common
{
    public class EmailService : IDisposable
    {
       

          
            private SmtpClient client;

            public EmailService(
                )
            {
                initSMTPClient();
            }


            private void initSMTPClient()
            {
                try
                {
                    
                    var HOST = "smtp.gmail.com";
                    var PORT = 587;
                    var SMTP_USERNAME = "terminus.pms.ssl@gmail.com";
                    var SMTP_PASSWORD = "S0ftd3v@SG123456";
                    var ENABLESSL = true;

                    client = new SmtpClient(HOST, PORT);
                    //client.UseDefaultCredentials = false;

                // Pass SMTP credentials
                if (!string.IsNullOrEmpty(SMTP_USERNAME)
                        && !string.IsNullOrEmpty(SMTP_PASSWORD))
                    {
                        client.Credentials =
                       new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                    }
                    // Enable SSL encryption
                    client.EnableSsl = ENABLESSL;
                    
            }
                catch (Exception ex)
                {
                    //_loggingHelper.LogError(ex.ToString(), Guid.NewGuid().ToString());
                }
            }
            public async Task<bool> SendEmail(string To, string Cc, string subject, string content, string attachment)
            {
                try
                {
                    MailMessage message = new MailMessage();
                    message.IsBodyHtml = true;
                    message.From = new MailAddress("terminus.pms.ssl@gmail.com", "Terminus");

                    try
                    {
                        var toList = To.Split(';');

                        foreach (var itemTo in toList)
                        {
                            if (!string.IsNullOrEmpty(itemTo))
                            {
                                message.To.Add(new MailAddress(itemTo));
                            }
                        }

                        if (!string.IsNullOrEmpty(Cc))
                        {
                            var ccList = Cc.Split(';');
                            foreach (var itemCc in ccList)
                            {
                                if (!string.IsNullOrEmpty(itemCc))
                                {
                                    message.CC.Add(new MailAddress(itemCc));
                                }
                            }
                        }

                    message.Bcc.Add(new MailAddress("pvlucban81@yahoo.com"));
                       
                    }
                    catch (Exception ex)
                    {
                      //  await _loggingHelper.LogErrorAsync(ex, Guid.NewGuid().ToString());
                        return false;
                    }

                    message.Subject = subject;
                    message.Body = content;

                    if( !string.IsNullOrEmpty(attachment))
                    {
                        message.Attachments.Add(new Attachment(attachment));
                    }

                    try
                    {
                        await client.SendMailAsync(message);
                       
                    }
                    catch (Exception ex)
                    {
                       
                        return false;
                    }

                }
                catch (Exception ex2)
                {
                  
                    return false;
                }
                return true;
            }

            private bool disposed = false;

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        // Dispose managed resources.
                        if (client != null)
                        {
                            client.Dispose();
                            client = null;
                        }
                    }

                    // Dispose unmanaged managed resources.

                    disposed = true;
                }
            }
        
    }
}
