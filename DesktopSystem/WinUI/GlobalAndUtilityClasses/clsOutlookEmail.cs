using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using BOLib;
using System.Reflection;

namespace WinUI
{
    public static class OutlookEmail
    {
        private static bool invalid = false;

        public static bool IsValidEmailString(string strIn)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(strIn);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
//            invalid = false;
//            if (String.IsNullOrEmpty(strIn))
//                return false;

//            // Use IdnMapping class to convert Unicode domain names. 
//            try
//            {
//                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper, RegexOptions.None);
//            }
//            catch (Exception)
//            {
//                return false;
//            }

//            if (invalid)
//                return false;

//            // Return true if strIn is in valid e-mail format. 
//            try
//            {
//                return Regex.IsMatch(strIn,
//                      @"^(?("")(""[^""]+?""@)|
//                      (([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
//                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$",
//                      RegexOptions.IgnoreCase);
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public static void SendEmail(string[] to, string subject,
        string body, string[] attachments,string ccs, bool display = true, bool useSignature = true)
        {
            try
            {
                //Create main objects
                Outlook.Application outlookApp = new Outlook.Application();
                Outlook.NameSpace nameSpace = outlookApp.GetNamespace("MAPI");
                Outlook.MAPIFolder mapiFolder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);
                Outlook.MailItem mailItem = mapiFolder.Items.Add(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
                Outlook.Recipients recipients = mailItem.Recipients as Outlook.Recipients;
               

                //Set email subject
                mailItem.Subject = subject;

                //Set email recipients
                int i = 0;
                for (; i < to.Length;i++ )
                {
                    //Set recipient only if it is an Valid email address
                    if (to[i] != string.Empty && IsValidEmailString(to[i]))
                        recipients.Add(to[i]);
                }

                string[] cc = ccs.Replace(";", ",").Split(',');
                for (int j=0; j < cc.Length; j++)
                {
                    //Set recipient only if it is an Valid email address
                    if (cc[j] != string.Empty && IsValidEmailString(cc[j]))
                    {
                        recipients.Add(cc[j]);
                        recipients[j + i+1].Type = (int)Outlook.OlMailRecipientType.olCC;
                    }
                }
                
                //Resolve recipients
                recipients.ResolveAll();

                //Add email attachments if there are some
                foreach (string attachment in attachments)
                {
                    //Add attachment only if the attachment path really exists
                    if (File.Exists(attachment))
                        mailItem.Attachments.Add(attachment);
                }

                string BHMBankDetailFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SOAEmailAtt\BHM Bank Account Detail.pdf");                

                //string SOAEmailBankAcPath = GFunc.ExecuteQuery("SELECT OpValue FROM SYS_Option WHERE OpID = 'SOAEmailAtt'").Rows[0][0].ToString();
                
                //mailItem.Attachments.Add(BHMBankDetailFilePath);
                mailItem.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
                //Get the signature in the email body
                mailItem.GetInspector.Activate();
                var signature = mailItem.HTMLBody;

                //Set the email body
                mailItem.HTMLBody = body;

                //If useSignature is true add the signature to the body
                if (useSignature)
                    mailItem.HTMLBody += signature;

                //Display or directly send email depending on if there are recipients or the bool display
                if (mailItem.Recipients.Count == 0 || display)
                {
                    mailItem.Display();
                }
                else
                {
                    mailItem.Send();
                }
            }
            catch (Exception ex)
            {
                if(BOLib.MsgBox.Show("Failed to access outlook. Would you like to open the attached pdf file?",
                    BOLib.GEnum.MsgBoxButton.Yes,BOLib.GEnum.MsgBoxButton.No)==BOLib.GEnum.MsgBoxButton.Yes)
                    foreach (string attachment in attachments)
                    {
                        //Add attachment only if the attachment path really exists
                        if (File.Exists(attachment))
                            System.Diagnostics.Process.Start(attachment);
                    }
                    
            }
        }

        public static void SendEmail(string to, string subject, string body,
        string attachment,string ccs, bool display = true, bool useSignature = true)
        {
            SendEmail(new string[] { to }, subject, body, new string[] { attachment },ccs, display, useSignature);
        }

        public static void SendEmail(string to, string subject,
        string body, bool display = true, bool useSignature = true)
        {
            SendEmail(to, subject, body, null,null, display, useSignature);
        }
    }

}
