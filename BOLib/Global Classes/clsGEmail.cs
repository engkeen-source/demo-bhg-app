using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net.Mime;
using System.IO;
using System.Reflection;
using System.Net;
using System.Data.SqlClient;
using System.Data;

namespace BOLib
{
    public class GEmail
    {
        public class Pop3Exception : System.ApplicationException
        {
            public Pop3Exception(string str)

                : base(str)
            {

            }
        }

        public class Pop3Message
        {

            public long number;

            public long bytes;

            public bool retrieved;

            public string message;

        }

        public class Pop3 : System.Net.Sockets.TcpClient
        {
            public void Connect(string server,int portNumber, string username, string password)
            {
                string message;
                string response;

                Connect(server, portNumber);
                response = Response();
                if (response.Substring(0, 3) != "+OK")
                {
                    throw new Pop3Exception(response);
                }

                message = "USER " + username + "\r\n";
                Write(message);
                response = Response();
                if (response.Substring(0, 3) != "+OK")
                {
                    throw new Pop3Exception(response);
                }

                message = "PASS " + password + "\r\n";
                Write(message);
                response = Response();
                if (response.Substring(0, 3) != "+OK")
                {
                    throw new Pop3Exception(response);
                }
            }

            private void Write(string message)
            {
                System.Text.ASCIIEncoding en = new System.Text.ASCIIEncoding();
                byte[] WriteBuffer = new byte[1024];
                WriteBuffer = en.GetBytes(message);
                NetworkStream stream = GetStream();
                stream.Write(WriteBuffer, 0, WriteBuffer.Length);
                Debug.WriteLine("WRITE:" + message);
            }

            private string Response()
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] serverbuff = new Byte[1024];
                NetworkStream stream = GetStream();
                int count = 0;
                while (true)
                {
                    byte[] buff = new Byte[2];
                    int bytes = stream.Read(buff, 0, 1);
                    if (bytes == 1)
                    {
                        serverbuff[count] = buff[0];
                        count++;
                        if (buff[0] == '\n')
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    };
                };
                string retval = enc.GetString(serverbuff, 0, count);
                Debug.WriteLine("READ:" + retval);
                return retval;
            }

            public void Disconnect()
            {
                string message;
                string response;
                message = "QUIT\r\n";
                Write(message);
                response = Response();
                if (response.Substring(0, 3) != "+OK")
                {
                    throw new Pop3Exception(response);
                }
            }

            public ArrayList List()
            {
                string message;
                string response;

                ArrayList retval = new ArrayList();
                message = "LIST\r\n";
                Write(message);
                response = Response();
                if (response.Substring(0, 3) != "+OK")
                {
                    throw new Pop3Exception(response);
                }

                while (true)
                {
                    response = Response();
                    if (response == ".\r\n")
                    {
                        return retval;
                    }
                    else
                    {
                        Pop3Message msg = new Pop3Message();
                        char[] seps = { ' ' };
                        string[] values = response.Split(seps);
                        msg.number = Int32.Parse(values[0]);
                        msg.bytes = Int32.Parse(values[1]);
                        msg.retrieved = false;
                        retval.Add(msg);
                        continue;
                    }
                }
            }
            public Pop3Message Retrieve(Pop3Message rhs)
            {
                string message;
                string response;

                Pop3Message msg = new Pop3Message();
                msg.bytes = rhs.bytes;
                msg.number = rhs.number;

                message = "RETR " + rhs.number + "\r\n";
                Write(message);
                response = Response();
                if (response.Substring(0, 3) != "+OK")
                {
                    throw new Pop3Exception(response);
                }

                msg.retrieved = true;
                while (true)
                {
                    response = Response();
                    if (response == ".\r\n")
                    {
                        break;
                    }
                    else
                    {
                        msg.message += response;
                    }
                }

                return msg;
            }
        }


        private static void PopMailRetrieve(string incomingMailServer,int incomingMailServerPortNo,string AuthEmail,string password)
        {
            try
            {
                Pop3 obj = new Pop3();
                obj.Connect(incomingMailServer, 25, AuthEmail, password);
                ArrayList list = obj.List();
                foreach (Pop3Message msg in list)
                {
                    Pop3Message msg2 = obj.Retrieve(msg);
                    System.Console.WriteLine("Message {0}: {1}",
                    msg2.number, msg2.message);
                    break;
                }
                obj.Disconnect();
            }
            catch (Pop3Exception pe)
            {
                System.Console.WriteLine(pe.ToString());
            }
            catch (System.Exception ee)
            {
                System.Console.WriteLine(ee.ToString());
            }
        }

        private static bool PopMailCheck(string incomingMailServer, int incomingMailServerPortNo, string AuthEmail, string password)
        {
            try
            {
                Pop3 obj = new Pop3();                
                obj.Connect(incomingMailServer, 25, AuthEmail, password);
                return true;
            }
            catch (Pop3Exception pe)
            {
                System.Console.WriteLine(pe.ToString());
                return false;
            }
            catch (System.Exception ee)
            {
                System.Console.WriteLine(ee.ToString());
                return false;
            }
        }

        //Report Related Functions
        public static bool SendEmailAsSystemUser(string emails, string subject, string messageBody, System.Net.Mail.Attachment Attachment)
        {
            try
            {
                string[] emailAddress = emails.Split(';');
                string AuthEmailAcc = SysOptionUtility.GetSysOpStr("EmailAccount");
                string oMSever = SysOptionUtility.GetSysOpStr("OutgoingMailServer");
                string iMSever = SysOptionUtility.GetSysOpStr("IncomingMailServer");
                string displayName = SysOptionUtility.GetSysOpStr("EmailDisplayUserName");
                string emailPassword = TAUtil.Decoder.Decode(SysOptionUtility.GetSysOpStr("EmailPassword"));
                //Prepering Mail               
                System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
                for (int i = 0; i < emailAddress.Length; i++)
                    if (Regex.IsMatch(emailAddress[i], GVar.EmailCheck))
                    {
                        email.To.Add(emailAddress[i]);
                    }
                    else
                    {
                        if (emailAddress[i].Trim() != string.Empty)
                        {
                            MsgBox.Show("The system cannot send to incorrect email address ->" + emailAddress[i] + ". The other valid mails will be sent out.");
                        }
                    }

                email.From = new System.Net.Mail.MailAddress(SysOptionUtility.GetSysOpStr("ReplyTo"), displayName);

                //Preparing Mail For BCC                 
                if (Regex.IsMatch(AuthEmailAcc, GVar.EmailCheck))
                {
                    if (SysOptionUtility.GetBool("BCC"))
                    {
                        email.Bcc.Add(new System.Net.Mail.MailAddress(AuthEmailAcc, displayName));
                    }
                }
                else
                {
                    if (AuthEmailAcc.Trim() != string.Empty)
                    {
                        MsgBox.Show("The system cannot send to incorrect BCC email address ->" + AuthEmailAcc + ".");
                        return false;
                    }
                }
                if (oMSever.Trim() == string.Empty)
                {
                    MsgBox.Show("Invalid Outgoing Mail Server.");
                    return false;
                }

                email.Subject = subject;
                email.Body = messageBody;
                email.IsBodyHtml = true;

                if (Attachment != null)
                    email.Attachments.Add(Attachment);

                //Sending Mail
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(oMSever, SysOptionUtility.GetInt("OutgoingMailServerPortNumber"));
                client.EnableSsl = true;
                client.TargetName = "STARTTLS/smtp.office365.com";
                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential(AuthEmailAcc, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{ return true; };

                //Not Ready
                //if(PopMailCheck(iMSever,SysOptionUtility.GetInt("IncomingMailServerPortNumber"),AuthEmailAcc,emailPassword))
                client.Send(email);

                return true;
            }
            catch (TAUtil.TAException tex)
            {
                throw tex;
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    System.Windows.Forms.MessageBox.Show("Error Inner Exception" + ex.InnerException.Message + " Trace:" + ex.StackTrace);
                else
                    System.Windows.Forms.MessageBox.Show("Error Trace:" + ex.StackTrace);
                throw ex;
            }
        }

        public static bool SendEmail(string emails, string subject, string messageBody, System.Net.Mail.Attachment Attachment)
        {
            try
            {
                // enable Ssl3, Tls, TLS v 1.1 and 1.2  
                //SecurityProtocolType { Ssl3 = 48, Tls = 192, Tls11 = 768, Tls12 = 3072, }     //added by KKAung on 18 Oct 2021
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)48 | (System.Net.SecurityProtocolType)192 | (System.Net.SecurityProtocolType)768 | (System.Net.SecurityProtocolType)3072;    

                string[] emailAddress = emails.Split(';');
                string AuthEmailAcc = SysOptionUtility.GetStr("EmailAccount");
                string oMSever = SysOptionUtility.GetSysOpStr("OutgoingMailServer");
                string iMSever = SysOptionUtility.GetSysOpStr("IncomingMailServer");
                string displayName = SysOptionUtility.GetStr("EmailDisplayUserName");
                string emailPassword = TAUtil.Decoder.Decode(SysOptionUtility.GetStr("EmailPassword"));
                //Prepering Mail               
                System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
                for (int i = 0; i < emailAddress.Length; i++)
                    if (Regex.IsMatch(emailAddress[i], GVar.EmailCheck))
                    {
                        email.To.Add(emailAddress[i]);
                    }
                    else
                    {
                        if (emailAddress[i].Trim() != string.Empty)
                        {
                            MsgBox.Show("The system cannot send to incorrect email address ->" + emailAddress[i] + ". The other valid mails will be sent out.");
                        }
                    }

                email.From = new System.Net.Mail.MailAddress(SysOptionUtility.GetStr("ReplyTo"), displayName);

                //Preparing Mail For BCC                 
                if (Regex.IsMatch(AuthEmailAcc, GVar.EmailCheck))
                {
                    if (SysOptionUtility.GetBool("BCC"))
                    {
                        email.Bcc.Add(new System.Net.Mail.MailAddress(AuthEmailAcc, displayName));
                    }
                }
                else
                {
                    if (AuthEmailAcc.Trim() != string.Empty)
                    {
                        MsgBox.Show("The system cannot send to incorrect BCC email address ->" + AuthEmailAcc + ".");
                        return false;
                    }
                }
                if (oMSever.Trim() == string.Empty)
                {
                    MsgBox.Show("Invalid Outgoing Mail Server.");
                    return false;
                }
                

                email.Subject = subject;
                email.Body = messageBody;
                email.IsBodyHtml = true;

                if (Attachment != null)
                    email.Attachments.Add(Attachment);

                //Sending Mail
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(oMSever, SysOptionUtility.GetInt("OutgoingMailServerPortNumber"));
                client.EnableSsl = true;
                client.TargetName = "STARTTLS/smtp.office365.com";
                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential(AuthEmailAcc, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{ return true; };

                //Not Ready
                //if(PopMailCheck(iMSever,SysOptionUtility.GetInt("IncomingMailServerPortNumber"),AuthEmailAcc,emailPassword))
                client.Send(email);

                return true;
            }
            catch (TAUtil.TAException tex)
            {
                throw tex;
            }

            catch (Exception ex)
            {
                if(ex.InnerException!=null)
                    System.Windows.Forms.MessageBox.Show("Error Inner Exception"+ex.InnerException.Message+" Trace:"+ ex.StackTrace);
                else
                    System.Windows.Forms.MessageBox.Show("Error Trace:" + ex.StackTrace);
                throw ex;
            }
        }

        //added by nnt on 7th May 2019 to send SOA and reminder emails by payment@bhglobal.com.sg
        public static void SendEmailCC(string emails, string subject, string messageBody, System.Net.Mail.Attachment Attachment,string ccs)
        {
            try
            {
                /* 
                 * added by YST on 2022/01/12
                 * SecurityProtocolType { Ssl3 = 48, Tls = 192, Tls11 = 768, Tls12 = 3072 } 
                 * https://stackoverflow.com/questions/58019568/tls-1-2-or-tls-1-0-with-net-framework-4-0 
                 */
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(48 | 192 | 768 | 3072);

                string[] ccemailAddress = ccs.Split(';');
                string[] emailAddress = emails.Split(';');
                string AuthEmailAcc = SysOptionUtility.GetSysOpStr("EmailAccount");
                //string oMSever = SysOptionUtility.GetSysOpStr("OutgoingMailServer");
                //string iMSever = SysOptionUtility.GetSysOpStr("IncomingMailServer");
                //string displayName = SysOptionUtility.GetStr("EmailDisplayUserName");
                //string emailPassword = TAUtil.Decoder.Decode(SysOptionUtility.GetStr("EmailPassword"));   
                
                string oMSever = SysOptionUtility.GetSysOpStr("OutgoingMailServer");
                string iMSever = SysOptionUtility.GetSysOpStr("IncomingMailServer");
                string displayName = "BHG Payment";
                string emailPassword = TAUtil.Decoder.Decode(SysOptionUtility.GetSysOpStr("EmailPassword"));

                //Prepering Mail               
                System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
                for (int i = 0; i < emailAddress.Length; i++)
                    if (Regex.IsMatch(emailAddress[i], GVar.EmailCheck))
                    {
                        email.To.Add(emailAddress[i]);
                    }
                    else
                    {
                        if (emailAddress[i].Trim() != string.Empty)
                        {
                            MsgBox.Show("The system cannot send to incorrect email address ->" + emailAddress[i] + ". The other valid mails will be sent out.");
                        }
                    }

                for (int i = 0; i < ccemailAddress.Length; i++)
                    if (Regex.IsMatch(ccemailAddress[i], GVar.EmailCheck))
                    {
                        email.CC.Add(ccemailAddress[i]);
                    }
                    else
                    {
                        if (ccemailAddress[i].Trim() != string.Empty)
                        {
                            MsgBox.Show("The system cannot send to incorrect cc email address ->" + emailAddress[i] + ". The other valid mails will be sent out.");
                        }
                    }

                email.From = new System.Net.Mail.MailAddress(SysOptionUtility.GetSysOpStr("ReplyTo"), displayName);
               

                //Preparing Mail For BCC                 
                //if (Regex.IsMatch(AuthEmailAcc, GVar.EmailCheck))
                //{
                //    if (SysOptionUtility.GetBool("BCC"))
                //    {
                //        email.Bcc.Add(new System.Net.Mail.MailAddress(AuthEmailAcc, displayName));
                //    }
                //}
                //else
                //{
                //    if (AuthEmailAcc.Trim() != string.Empty)
                //    {
                //        MsgBox.Show("The system cannot send to incorrect BCC email address ->" + AuthEmailAcc + ".");
                //        //return false;
                //    }
                //}
                if (oMSever.Trim() == string.Empty)
                {
                    MsgBox.Show("Invalid Outgoing Mail Server.");
                    //return false;
                }

                //add inline image
                string inLineLogoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailCmpLogoPath\bh global.png");
                //string inLineLogoPath = GFunc.ExecuteQuery("SELECT OpValue FROM SYS_Option WHERE OpID = 'EmailCmpLogoPath'").Rows[0][0].ToString();
                //inLineLogoPath = "@"d:\Email Signature 2019 (APM 2020).jpg"";

                Attachment inlineLogo = new Attachment(inLineLogoPath);
                email.Attachments.Add(inlineLogo);
                string contentID = "Image";
                inlineLogo.ContentId = contentID;


                string inLineLogo1Path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailSigPath\Email Signature 2020 (new).jpg");
                //string inLineLogo1Path = GFunc.ExecuteQuery("SELECT OpValue FROM SYS_Option WHERE OpID = 'EmailSigPath'").Rows[0][0].ToString();
                Attachment inlineLogo1 = new Attachment(inLineLogo1Path);
                email.Attachments.Add(inlineLogo1);
                string contentID1 = "Image1";
                inlineLogo1.ContentId = contentID1;

                string inLineLogo2Path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailSigPath\paynow.png");
                //string inLineLogo1Path = GFunc.ExecuteQuery("SELECT OpValue FROM SYS_Option WHERE OpID = 'EmailSigPath'").Rows[0][0].ToString();
                Attachment inlineLogo2 = new Attachment(inLineLogo2Path);
                email.Attachments.Add(inlineLogo2);
                string contentID2 = "Image2";
                inlineLogo2.ContentId = contentID2;

                string BHMBankDetailFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SOAEmailAtt\BHM Bank Account Detail.pdf");
                //string SOAEmailBankAcPath = GFunc.ExecuteQuery("SELECT OpValue FROM SYS_Option WHERE OpID = 'SOAEmailAtt'").Rows[0][0].ToString();
                Attachment SOAEmailBankAcAtt = new Attachment(BHMBankDetailFilePath);
                //if (SOAEmailBankAcAtt != null)
                //    email.Attachments.Add(SOAEmailBankAcAtt);

                //To make the image display as inline and not as attachment

                inlineLogo.ContentDisposition.Inline = true;
                inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                inlineLogo1.ContentDisposition.Inline = true;
                inlineLogo1.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                //To embed image in email

                //
                email.Subject = subject;
                email.Body = messageBody;
                email.Body += "<htm><body><br/><p style=\"color:#0d49aa;font-size:14px;font-family:Calibri;\">Best Regards,<br/><span style=\"color:#34495E;font-size:16px;font-family:Calibri;\">BHG Payment</span></p> <img src=\"cid:" + contentID + "\"><br/><div style=\"color:#0d49aa;font-size:12px;font-family:Calibri;margin:0px 0px;\">Tel: +65 6210 4746| Fax: +65 6291 5777"
                    + "</div><div style=\"color:#0d49aa;font-size:12px;font-family:Times New Roman;margin:0px 0px;\">EMail: payment@bhglobal.com.sg | Web: www.bhglobal.com.sg |  Visit our new eStore at www.bh-estore.com</div>"
                    + "<div><table style=\"font - size:14px; font - family:Calibri; font - weight:bold;\"><tr><td width=\"180px\" rowspan=\"2\"><img src=\"cid:" + contentID2 + "\"/></td><td>Payee : BENG HUI MARINE ELECTRICAL PTE LTD</td></tr><tr><td width=\"500px\">UEN : 199900682G</td></tr></table></div>"
                    + "<div style=\"margin:3px;\"><a href=\"http://bh-estore.com\"><img src=\"cid:" + contentID1 + "\"></a></div>"                                                    
                    + "<div style=\"color:#A6ACAF;font-size:12px;font-weight:bold;font-family:Calibri;margin:0px 0px;\">Please consider the environment before printing this Email.</div>"
                    + "<p style=\"color:#A6ACAF;font-size:11px;font-family:Calibri;margin:0px 0px;\">This message contains confidential information and is intended only for the individual or the group named. Attachments or contents of this mail are for information and co-ordination purposes only. At any time, without prejudice, express liability or claims from or towards BH GLOBAL CORPORATION LTD and its Subsidiaries. Information disclosed and enclosed may not be revealed or circulated to any addressee; this mail is not intended for. If you are not the named addressee you should not disseminate, distribute or copy this e-mail. The sender therefore does not accept liability for any errors, omissions or virus transmitted in the contents of this message, which arise as a result of e-mail transmission. ----<span style=\"color:#031C9A;font-size:11px;font-weight:bold;font-family:Calibri;margin:0px 0px;\"> BH GLOBAL CORPORATION LTD, No: 8 Penjuru Lane, Singapore 609189, www.bhglobal.com.sg </span></p>"
                    + ""
                    + "</body></html>";
                email.IsBodyHtml = true;                

                if (Attachment != null)
                    email.Attachments.Add(Attachment);

                
                //Sending Mail
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(oMSever, SysOptionUtility.GetInt("OutgoingMailServerPortNumber"));
                client.EnableSsl = true;
                client.TargetName = "STARTTLS/smtp.office365.com";
                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential(AuthEmailAcc, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{ return true; };

                //Not Ready
                //if(PopMailCheck(iMSever,SysOptionUtility.GetInt("IncomingMailServerPortNumber"),AuthEmailAcc,emailPassword))
               
                client.Send(email);
                email.Dispose();

                //return true;
            }
            catch (TAUtil.TAException tex)
            {
                throw tex;
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    System.Windows.Forms.MessageBox.Show("Error Inner Exception" + ex.InnerException.Message + " Trace:" + ex.StackTrace);
                else
                    System.Windows.Forms.MessageBox.Show("Error Trace:" + ex.StackTrace);
                    //System.Windows.Forms.MessageBox.Show("Error Trace:" + "Need to set up the user email,attachment path in BOSS System.");
                throw ex;
            }
        }

        //end added by nnt

        //Report Related Functions
        public static bool SendEmailwithAttachments(string emails, string subject, string messageBody, System.Net.Mail.Attachment[] Attachments)
        {
            try
            {
                string[] emailAddress = emails.Split(';');
                string AuthEmailAcc = SysOptionUtility.GetStr("EmailAccount");
                string oMSever = SysOptionUtility.GetSysOpStr("OutgoingMailServer");
                string iMSever = SysOptionUtility.GetSysOpStr("IncomingMailServer");
                string displayName = SysOptionUtility.GetStr("EmailDisplayUserName");
                string emailPassword = TAUtil.Decoder.Decode(SysOptionUtility.GetStr("EmailPassword"));
                //Prepering Mail               
                System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
                for (int i = 0; i < emailAddress.Length; i++)
                    if (Regex.IsMatch(emailAddress[i], GVar.EmailCheck))
                    {
                        email.To.Add(emailAddress[i]);
                    }
                    else
                    {
                        if (emailAddress[i].Trim() != string.Empty)
                        {
                            MsgBox.Show("The system cannot send to incorrect email address ->" + emailAddress[i] + ". The other valid mails will be sent out.");
                        }
                    }

                email.From = new System.Net.Mail.MailAddress(SysOptionUtility.GetStr("ReplyTo"), displayName);

                //Preparing Mail For BCC                 
                if (Regex.IsMatch(AuthEmailAcc, GVar.EmailCheck))
                {
                    if (SysOptionUtility.GetBool("BCC"))
                    {
                        email.Bcc.Add(new System.Net.Mail.MailAddress(AuthEmailAcc, displayName));
                    }
                }
                else
                {
                    if (AuthEmailAcc.Trim() != string.Empty)
                    {
                        MsgBox.Show("The system cannot send to incorrect BCC email address ->" + AuthEmailAcc + ".");
                        return false;
                    }
                }
                if (oMSever.Trim() == string.Empty)
                {
                    MsgBox.Show("Invalid Outgoing Mail Server.");
                    return false;
                }

                email.Subject = subject;
                email.Body = messageBody;
                email.IsBodyHtml = true;

                if (Attachments != null)
                {
                    foreach(System.Net.Mail.Attachment Attachment in Attachments)
                        email.Attachments.Add(Attachment);
                }

                //Sending Mail
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(oMSever, SysOptionUtility.GetInt("OutgoingMailServerPortNumber"));
                client.EnableSsl = true;
                client.TargetName = "STARTTLS/smtp.office365.com";
                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential(AuthEmailAcc, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{ return true; };

                //Not Ready
                //if(PopMailCheck(iMSever,SysOptionUtility.GetInt("IncomingMailServerPortNumber"),AuthEmailAcc,emailPassword))
                client.Send(email);

                return true;
            }
            catch (TAUtil.TAException tex)
            {
                throw tex;
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    System.Windows.Forms.MessageBox.Show("Error Inner Exception" + ex.InnerException.Message + " Trace:" + ex.StackTrace);
                else
                    System.Windows.Forms.MessageBox.Show("Error Trace:" + ex.StackTrace);
                throw ex;
            }
        }

        public static bool SendSystemEmail(string emails, string subject, string messageBody, System.Net.Mail.Attachment Attachment)
        {
            try
            {
                // enable Ssl3, Tls, TLS v 1.1 and 1.2  
                //SecurityProtocolType { Ssl3 = 48, Tls = 192, Tls11 = 768, Tls12 = 3072, }     //added by KKAung on 18 Oct 2021
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)48 | (System.Net.SecurityProtocolType)192 | (System.Net.SecurityProtocolType)768 | (System.Net.SecurityProtocolType)3072;

                string[] emailAddress = emails.Split(';');
                string AuthEmailAcc = SysOptionUtility.GetSysOpStr("eServicesEmailAccount");
                string oMSever = SysOptionUtility.GetSysOpStr("OutgoingMailServer");
                string iMSever = SysOptionUtility.GetSysOpStr("IncomingMailServer");
                string displayName = SysOptionUtility.GetStr("EmailDisplayUserName");
                string emailPassword = SysOptionUtility.GetStr("eServicesEmailPassword");
                //Prepering Mail               
                System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
                for (int i = 0; i < emailAddress.Length; i++)
                    if (Regex.IsMatch(emailAddress[i], GVar.EmailCheck))
                    {
                        email.To.Add(emailAddress[i]);
                    }
                    else
                    {
                        if (emailAddress[i].Trim() != string.Empty)
                        {
                            MsgBox.Show("The system cannot send to incorrect email address ->" + emailAddress[i] + ". The other valid mails will be sent out.");
                        }
                    }

                email.From = new System.Net.Mail.MailAddress(AuthEmailAcc, displayName);
                email.CC.Add(new System.Net.Mail.MailAddress(SysOptionUtility.GetStr("ReplyTo"), displayName));

                //Preparing Mail For BCC                 
                if (Regex.IsMatch(AuthEmailAcc, GVar.EmailCheck))
                {
                    if (SysOptionUtility.GetBool("BCC"))
                    {
                        email.Bcc.Add(new System.Net.Mail.MailAddress(AuthEmailAcc, displayName));
                    }
                }
                else
                {
                    if (AuthEmailAcc.Trim() != string.Empty)
                    {
                        MsgBox.Show("The system cannot send to incorrect BCC email address ->" + AuthEmailAcc + ".");
                        return false;
                    }
                }
                if (oMSever.Trim() == string.Empty)
                {
                    MsgBox.Show("Invalid Outgoing Mail Server.");
                    return false;
                }


                email.Subject = subject;
                email.Body = messageBody;
                email.IsBodyHtml = true;

                if (Attachment != null)
                    email.Attachments.Add(Attachment);

                //Sending Mail
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(oMSever, SysOptionUtility.GetInt("OutgoingMailServerPortNumber"));
                client.EnableSsl = true;
                client.TargetName = "STARTTLS/smtp.office365.com";
                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential(AuthEmailAcc, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{ return true; };

                //Not Ready
                //if(PopMailCheck(iMSever,SysOptionUtility.GetInt("IncomingMailServerPortNumber"),AuthEmailAcc,emailPassword))
                client.Send(email);

                return true;
            }
            catch (TAUtil.TAException tex)
            {
                throw tex;
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    System.Windows.Forms.MessageBox.Show("Error Inner Exception" + ex.InnerException.Message + " Trace:" + ex.StackTrace);
                else
                    System.Windows.Forms.MessageBox.Show("Error Trace:" + ex.StackTrace);
                throw ex;
            }
        }

        public static bool SendeStoreEmail(string emails, string subject, string messageBody, System.Net.Mail.Attachment Attachment)
        {
            try
            {
                // enable Ssl3, Tls, TLS v 1.1 and 1.2  
                //SecurityProtocolType { Ssl3 = 48, Tls = 192, Tls11 = 768, Tls12 = 3072, }     //added by KKAung on 18 Oct 2021
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)48 | (System.Net.SecurityProtocolType)192 | (System.Net.SecurityProtocolType)768 | (System.Net.SecurityProtocolType)3072;

                string[] emailAddress = emails.Split(';');
                string AuthEmailAcc = SysOptionUtility.GetSysOpStr("eStoreEmailAccount");
                // string emailPassword = TAUtil.Decoder.Decode(SysOptionUtility.GetStr("EmailPassword"));
                string emailPassword = SysOptionUtility.GetStr("eStoreEmailPassword");//Since password will be updated from backend without encryption
                string oMSever = SysOptionUtility.GetSysOpStr("OutgoingMailServer");
                string iMSever = SysOptionUtility.GetSysOpStr("IncomingMailServer");
                string displayName = SysOptionUtility.GetStr("EmailDisplayUserName");
                
                
                //Prepering Mail               
                System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
                for (int i = 0; i < emailAddress.Length; i++)
                    if (Regex.IsMatch(emailAddress[i], GVar.EmailCheck))
                    {
                        email.To.Add(emailAddress[i]);
                    }
                    else
                    {
                        if (emailAddress[i].Trim() != string.Empty)
                        {
                            MsgBox.Show("The system cannot send to incorrect email address ->" + emailAddress[i] + ". The other valid mails will be sent out.");
                        }
                    }
                
                email.From=new System.Net.Mail.MailAddress(AuthEmailAcc, displayName);

                email.CC.Add(new System.Net.Mail.MailAddress(SysOptionUtility.GetStr("ReplyTo"), displayName));

                //Preparing Mail For BCC                 
                if (Regex.IsMatch(AuthEmailAcc, GVar.EmailCheck))
                {
                    if (SysOptionUtility.GetBool("BCC"))
                    {
                        email.Bcc.Add(new System.Net.Mail.MailAddress(AuthEmailAcc, displayName));
                    }
                }
                else
                {
                    if (AuthEmailAcc.Trim() != string.Empty)
                    {
                        MsgBox.Show("The system cannot send to incorrect BCC email address ->" + AuthEmailAcc + ".");
                        return false;
                    }
                }
                if (oMSever.Trim() == string.Empty)
                {
                    MsgBox.Show("Invalid Outgoing Mail Server.");
                    return false;
                }


                email.Subject = subject;
                email.Body = messageBody;
                email.IsBodyHtml = true;

                if (Attachment != null)
                    email.Attachments.Add(Attachment);

                //Sending Mail
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(oMSever, SysOptionUtility.GetInt("OutgoingMailServerPortNumber"));
                client.EnableSsl = true;
                client.TargetName = "STARTTLS/smtp.office365.com";
                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential(AuthEmailAcc, emailPassword);
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //    System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{ return true; };

                //Not Ready
                //if(PopMailCheck(iMSever,SysOptionUtility.GetInt("IncomingMailServerPortNumber"),AuthEmailAcc,emailPassword))
                client.Send(email);

                return true;
            }
            catch (TAUtil.TAException tex)
            {
                throw tex;
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    System.Windows.Forms.MessageBox.Show("Error Inner Exception" + ex.InnerException.Message + " Trace:" + ex.StackTrace);
                else
                    System.Windows.Forms.MessageBox.Show("Error Trace:" + ex.StackTrace);
                throw ex;
            }
        }
        
        /* added by YST on 2022/03/18 */
        public static bool SendEmail(string emailSender, string senderPassword, string emailReceiver, string emailCC, string emailSubject, 
                                     string emailBody, string senderDisplayName, string emailAttachment)
        {
            bool HasError = false;
            try
            {
                /*                 
                 * SecurityProtocolType { Ssl3 = 48, Tls = 192, Tls11 = 768, Tls12 = 3072 } 
                 * https://stackoverflow.com/questions/58019568/tls-1-2-or-tls-1-0-with-net-framework-4-0 
                 */
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(48 | 192 | 768 | 3072);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                      | SecurityProtocolType.Tls11
                                      | SecurityProtocolType.Tls12;

                string[] ccemailAddress = emailCC.Split(';');
                string[] emailAddress = emailReceiver.Trim().Split(';');
                string[] fileAttachment = emailAttachment.Trim().Split('#');
                string AuthEmailAcc = emailSender;
                string emailPassword = senderPassword;
                string displayName = senderDisplayName;
                string errorEmail = "";

                string oMSever = SysOptionUtility.GetSysOpStr("OutgoingMailServer");
                string iMSever = SysOptionUtility.GetSysOpStr("IncomingMailServer");

                Attachment Attachment = null;
                MailMessage email = new MailMessage();

                /* get receiver emails */
                for (int i = 0; i < emailAddress.Length; i++)
                {
                    if (Regex.IsMatch(emailAddress[i], GVar.EmailCheck))
                    {
                        email.To.Add(emailAddress[i]);
                    }
                    else
                    {
                        if (emailAddress[i].Trim() != string.Empty)
                        {
                            errorEmail = emailAddress[i].Trim() + ';' + errorEmail; 
                            //MsgBox.Show("System cannot send to incorrect email address ->" + emailAddress[i] + ". Please click OK to continue.");
                        }
                    }
                }

                /* get CC emails */
                for (int i = 0; i < ccemailAddress.Length; i++)
                {
                    if (Regex.IsMatch(ccemailAddress[i], GVar.EmailCheck))
                    {
                        email.CC.Add(ccemailAddress[i]);
                    }
                    else
                    {
                        if (ccemailAddress[i].Trim() != string.Empty)
                        {
                            errorEmail = ccemailAddress[i].Trim() + ';' + errorEmail;
                            //MsgBox.Show("System cannot send to incorrect cc email address ->" + emailAddress[i] + ". Please click OK to continue.");
                        }
                    }
                }

                /* get file attachments */
                for (int i = 0; i < fileAttachment.Length; i++)
                {
                    if (!string.IsNullOrEmpty(fileAttachment[i]))
                    {
                        Attachment = new Attachment(fileAttachment[i]);
                        if (Attachment != null)
                            email.Attachments.Add(Attachment);
                    }
                }

                //add inline image
                string inLineLogoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailCmpLogoPath\bh global.png");
                Attachment inlineLogo = new Attachment(inLineLogoPath);
                email.Attachments.Add(inlineLogo);
                inlineLogo.ContentId = "Image";


                string inLineLogo1Path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailSigPath\Email Signature 2020 (new).jpg");
                Attachment inlineLogo1 = new Attachment(inLineLogo1Path);
                email.Attachments.Add(inlineLogo1);
                inlineLogo1.ContentId = "Image1";

                string inLineLogo2Path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailSigPath\paynow.png");
                Attachment inlineLogo2 = new Attachment(inLineLogo2Path);
                inlineLogo2.ContentId = "Image2";

                //To make the image display as inline and not as attachment
                inlineLogo.ContentDisposition.Inline = true;
                inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                inlineLogo1.ContentDisposition.Inline = true;
                inlineLogo1.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                if (oMSever.Trim() == string.Empty)
                {
                    MsgBox.Show("Invalid Outgoing Mail Server.");
                }

                email.From = new MailAddress(AuthEmailAcc, displayName);
                email.Subject = emailSubject;
                email.Body = emailBody;
                email.IsBodyHtml = true;

                //Sending Mail
                SmtpClient client = new SmtpClient(oMSever, SysOptionUtility.GetInt("OutgoingMailServerPortNumber"));
                client.EnableSsl = true;
                client.TargetName = "STARTTLS/smtp.office365.com";
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(AuthEmailAcc, emailPassword);
                client.Send(email);

                email.Dispose();

                if (errorEmail != "")
                {
                    MsgBox.Show("System have sent out emails to the correct addresses by ignoring the incorrect email addresses ( " + errorEmail + " )");
                }                
            }
            catch (TAUtil.TAException tex)
            {
                HasError = true;
                throw tex;
            }

            catch (Exception ex)
            {
                HasError = true;
                if (ex.InnerException != null)
                    System.Windows.Forms.MessageBox.Show("Error Inner Exception" + ex.InnerException.Message + " Trace:" + ex.StackTrace);
                else
                    System.Windows.Forms.MessageBox.Show("Error Trace:" + ex.StackTrace);
                throw ex;
            }

            return HasError;
        }

        public static bool SendDBMail(string emailProfile, string emailSender, string senderPassword, string emailReceiver, string emailCC, string emailBCC, string emailSubject,
                            string emailBody, string senderDisplayName, string emailAttachment)
        {
            bool HasError = false;
            try
            {
                string[] fileAttachment = emailAttachment.Trim().Split('#');
                string DBMailAttachmentFilePath = "";
                emailAttachment = "";

                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                    DBMailAttachmentFilePath = "\\\\172.16.0.55\\Temp\\eInvoice";
                else if (SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                    DBMailAttachmentFilePath = "\\\\172.16.0.55\\Temp\\eInvoiceITS";
              
                /* get file attachments */
                for (int i = 0; i < fileAttachment.Length; i++)
                {
                    if (!string.IsNullOrEmpty(fileAttachment[i]))
                    {
                        emailAttachment = emailAttachment + CopyFile(fileAttachment[i], DBMailAttachmentFilePath) + ";";
                    }
                }

                //add inline image
                string inLineLogoPath = "";
                string contentID = "";
                if (SysOptionUtility.DatabaseBranchCode == DBCode.BHM)
                {
                    string inLineLogo1Path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailSigPath\EmailSignature2020(new).jpg");
                    string contentID1 = "EmailSignature2020(new).jpg";
                    emailAttachment = emailAttachment + CopyFile(inLineLogo1Path, DBMailAttachmentFilePath) + ";";
                    emailBody = emailBody.Replace("cid:Image1", "cid:" + contentID1);

                    inLineLogoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailCmpLogoPath\bhglobal.png");
                    contentID = "bhglobal.png";
                    emailAttachment = emailAttachment + CopyFile(inLineLogoPath, DBMailAttachmentFilePath);
                    emailBody = emailBody.Replace("cid:Image", "cid:" + contentID);


                  
                }
                else if (SysOptionUtility.DatabaseBranchCode == DBCode.ITS)
                {
                    inLineLogoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EmailCmpLogoPath\ITS.jpg");
                    contentID = "ITS.jpg";
                    emailAttachment = emailAttachment + CopyFile(inLineLogoPath, DBMailAttachmentFilePath);
                    emailBody = emailBody.Replace("cid:Image", "cid:" + contentID);
                }  

                //Sending Mail
                using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(Database.BossDemoConnection))
                {
                    using (SqlCommand cm = cn.CreateCommand())
                    {

                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "Send_DBMailWithAttachments";

                        #region "Testing"
                        //cm.Parameters.AddWithValue("@senderemail", "TestProfile"); //testing
                        //cm.Parameters.AddWithValue("@receivedemail", "kay.zinmoe@bhglobal.com.sg"); //testing
                        //cm.Parameters.AddWithValue("@copyemail", ""); //testing
                        #endregion

                        cm.Parameters.AddWithValue("@senderemail", emailProfile); // MailProfile_AR
                        cm.Parameters.AddWithValue("@receivedemail", emailReceiver);
                        cm.Parameters.AddWithValue("@copyemail", emailCC);
                        cm.Parameters.AddWithValue("@bccemail", emailBCC);
                        cm.Parameters.AddWithValue("@emailsubject", emailSubject);
                        cm.Parameters.AddWithValue("@emailbody", emailBody);
                        cm.Parameters.AddWithValue("@fileNames", emailAttachment);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                }
                //if (errorEmail != "")
                //{
                //    MsgBox.Show("System have sent out emails to the correct addresses by ignoring the incorrect email addresses ( " + errorEmail + " )");
                //}
            }
            catch (TAUtil.TAException tex)
            {
                HasError = true;
                throw tex;
            }

            catch (Exception ex)
            {
                HasError = true;
                if (ex.InnerException != null)
                    System.Windows.Forms.MessageBox.Show("Error Inner Exception" + ex.InnerException.Message + " Trace:" + ex.StackTrace);
                else
                    System.Windows.Forms.MessageBox.Show("Error Trace:" + ex.StackTrace);
                throw ex;
            }

            return HasError;
        }

        public static string MoveFile(string fromPath, string ToPath)
        {
            FileInfo fi = new FileInfo(fromPath);
            ToPath = ToPath + "\\" + fi.Name;
            File.Move(fromPath, ToPath);
            return ToPath;
        }
        public static string CopyFile(string fromPath, string ToPath)
        {
            //ToPath = ToPath + "\\" + fi.Name;
            //File.Copy(fromPath, ToPath, true);

            FileInfo fi = new FileInfo(fromPath);

            ToPath = ToPath + "\\" + fi.Name;
            fi.CopyTo(ToPath, true); // existing file will be overwritten

            return ToPath;

        }

    }
}
