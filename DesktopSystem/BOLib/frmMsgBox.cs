using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;


namespace BOLib
{
    public partial class MsgBox : Form 
    {
        private GEnum.MsgBoxButton btnReturn;
        private GEnum.MsgBoxDefaultButton btnDefault;       
        private Infragistics.Win.Misc.UltraButton[] btns;

        #region +++ Constructor +++
        
        public MsgBox()
        {
            InitializeComponent();
        }
        
        public MsgBox(string msgID, GEnum.MsgBoxIcon icon,GEnum.MsgBoxDefaultButton DefaultButtton, params GEnum.MsgBoxButton[] msgButtons)
            : this(null,msgID, icon, DefaultButtton, msgButtons)
        {            
                  
        }
        public MsgBox(SqlConnection cn, string msgID, GEnum.MsgBoxIcon icon, GEnum.MsgBoxDefaultButton DefaultButtton, params GEnum.MsgBoxButton[] msgButtons)
        {
            InitializeComponent();

            string outMsgID = string.Empty;

            MsgBoxFactory objMsgBoxFactory = new MsgBoxFactory();
            objMsgBoxFactory.Connection = cn;
            objMsgBoxFactory.Initialisation((GEnum.InstanceMode?)GEnum.InstanceMode.Normal, msgID, msgButtons);
            objMsgBoxFactory.FormWidth = this.Width;
            objMsgBoxFactory.PrepareData();

            #region +++ Build UI +++
            //this.Width = objMsgBoxFactory.FormWidth;
            this.Text = objMsgBoxFactory.Title;

            Image img = (Image)Properties.Resources.ResourceManager.GetObject(icon.ToString());
            if (!GFunc.IsNE(img))
                pic.Image = img;

            this.SuspendLayout();
            utxtMsgBody.AutoSize = false;            
            this.Text = objMsgBoxFactory.Title;
            
            utxtMsgBody.Value = objMsgBoxFactory.BodyText ;            

            btns = new Infragistics.Win.Misc.UltraButton[msgButtons.Length];


            for (int i = 0; i < msgButtons.Length; i++)
            {
                btns[i] = new Infragistics.Win.Misc.UltraButton();
                btns[i].Text = objMsgBoxFactory.ButtonCaptions[i];
                btns[i].Left =  objMsgBoxFactory.ButtonLefts[i];
                btns[i].Width = objMsgBoxFactory.ButtonWidth;
                btns[i].Height = objMsgBoxFactory.ButtonHeight;
                //btns[i].Top = objMsgBoxFactory.ButtonTop;

                btns[i].Visible = true;
                btns[i].Click += new EventHandler(Button_Click);
                btns[i].Tag = objMsgBoxFactory.buttonNos[i];
                this.pnlButton.Controls.Add(btns[i]);
            }
            this.ResumeLayout(true);

            #endregion

            this.btnDefault = DefaultButtton;

        }

        /*
         public MsgBox(System.Data.SqlClient.SqlConnection cn,string msgID,params GEnum.MsgBoxButton[] msgButtons)
        {
            InitializeComponent();

            string outMsgID = string.Empty;


            MsgBoxFactory objMsgBoxFactory = new MsgBoxFactory();
            objMsgBoxFactory.Connection = cn;
            objMsgBoxFactory.Initialisation((GEnum.InstanceMode?)GEnum.InstanceMode.Normal, msgID, msgButtons);
            objMsgBoxFactory.FormWidth = this.Width;
            objMsgBoxFactory.PrepareData();

            #region +++ Build UI +++
            this.Width = objMsgBoxFactory.FormWidth;
            this.Text = objMsgBoxFactory.Title;

            pic.Visible = false;

            this.SuspendLayout();
            utxtMsgBody.AutoSize = false;
            this.Width = objMsgBoxFactory.FormWidth;
            this.Text = objMsgBoxFactory.Title;
            utxtMsgBody.Left = 12;
            utxtMsgBody.Value = objMsgBoxFactory.BodyText;
            utxtMsgBody.Width = this.Width - utxtMsgBody.Left - 10;

            if (utxtMsgBody.Text.Length > 135)
            {
                utxtMsgBody.Top = 17;
            }
            else if (utxtMsgBody.Text.Length > 90)
            {
                utxtMsgBody.Top = 25;
                utxtMsgBody.Height = 80;
            }
            else
            {
                utxtMsgBody.Top = 40;
                utxtMsgBody.Height = 40;
            }

            btns = new Infragistics.Win.Misc.UltraButton[msgButtons.Length];


            for (int i = 0; i < msgButtons.Length; i++)
            {
                btns[i] = new Infragistics.Win.Misc.UltraButton();
                btns[i].Text = objMsgBoxFactory.ButtonCaptions[i];
                btns[i].Left = objMsgBoxFactory.ButtonLefts[i];
                btns[i].Width = objMsgBoxFactory.ButtonWidth;
                btns[i].Height = objMsgBoxFactory.ButtonHeight;
                btns[i].Top = objMsgBoxFactory.ButtonTop;

                btns[i].Visible = true;
                btns[i].Click += new EventHandler(Button_Click);
                btns[i].Tag = objMsgBoxFactory.buttonNos[i];
                this.Controls.Add(btns[i]);
            }
            this.ResumeLayout(true);

            #endregion

            this.btnDefault = GEnum.MsgBoxDefaultButton.DefaultButton1;

        }
       public MsgBox(string msgID, GEnum.MsgBoxButton[] msgButtons)
       {
           InitializeComponent();

           string outMsgID = string.Empty;

            
           MsgBoxFactory objMsgBoxFactory = new MsgBoxFactory();
        
           objMsgBoxFactory.Initialisation((GEnum.InstanceMode?) GEnum.InstanceMode.Normal,msgID,msgButtons);
           objMsgBoxFactory.FormWidth = this.Width;
           objMsgBoxFactory.PrepareData();

           #region +++ Build UI +++
           this.Width = objMsgBoxFactory.FormWidth;
           this.Text = objMsgBoxFactory.Title;

           pic.Visible = false;

           this.SuspendLayout();
           //utxtMsgBody.AutoSize = false;
           this.Width = objMsgBoxFactory.FormWidth;
           this.Text = objMsgBoxFactory.Title;
           utxtMsgBody.Left = 12;
           utxtMsgBody.Value = objMsgBoxFactory.BodyText;
           utxtMsgBody.Width = this.Width - utxtMsgBody.Left - 10;

           if (utxtMsgBody.Text.Length > 135)
           {
               utxtMsgBody.Top = 17;
           }
           else if (utxtMsgBody.Text.Length > 90)
           {
               utxtMsgBody.Top = 25;
               utxtMsgBody.Height = 80;
           }
           else
           {
               utxtMsgBody.Top = 40;
               utxtMsgBody.Height = 40;
           }

           btns = new Infragistics.Win.Misc.UltraButton[msgButtons.Length];


           for (int i = 0; i < msgButtons.Length; i++)
           {
               btns[i] = new Infragistics.Win.Misc.UltraButton();
               btns[i].Text = objMsgBoxFactory.ButtonCaptions[i];
               btns[i].Left = objMsgBoxFactory.ButtonLefts[i];
               btns[i].Width = objMsgBoxFactory.ButtonWidth;
               btns[i].Height = objMsgBoxFactory.ButtonHeight;
               btns[i].Top = objMsgBoxFactory.ButtonTop;

               btns[i].Visible = true;
               btns[i].Click += new EventHandler(Button_Click);
               btns[i].Tag = objMsgBoxFactory.buttonNos[i];
               this.Controls.Add(btns[i]);
           }
           this.ResumeLayout(true);         
           
           #endregion            
            
           this.btnDefault = GEnum.MsgBoxDefaultButton.DefaultButton1;           
         
       }        

       
       public MsgBox(string msgID, GEnum.MsgBoxButton[] msgButtons, GEnum.MsgBoxDefaultButton DefaultButtton)
       {
           InitializeComponent();

           string outMsgID = string.Empty;


           MsgBoxFactory objMsgBoxFactory = new MsgBoxFactory();

           objMsgBoxFactory.Initialisation((GEnum.InstanceMode?)GEnum.InstanceMode.Normal, msgID, msgButtons);
           objMsgBoxFactory.FormWidth = this.Width;
           objMsgBoxFactory.PrepareData();

           #region +++ Build UI +++

           this.btnDefault = GEnum.MsgBoxDefaultButton.DefaultButton1;
           this.Width = objMsgBoxFactory.FormWidth;
           this.Text = objMsgBoxFactory.Title;

           utxtMsgBody.Value = objMsgBoxFactory.BodyText;
           utxtMsgBody.Left = 12;

           //**********************
           if (utxtMsgBody.Text.Length > 135)
           {
               utxtMsgBody.Top = 17;
           }
           else if (utxtMsgBody.Text.Length > 90)
           {
               utxtMsgBody.Top = 25;
               utxtMsgBody.Height = 80;
           }
           else
           {
               utxtMsgBody.Top = 40;
               utxtMsgBody.Height = 40;
           }
           //**********************
           pic.Visible = false;
           btns = new Infragistics.Win.Misc.UltraButton[msgButtons.Length];

           

           this.SuspendLayout();
           for (int i = 0; i < msgButtons.Length; i++)
           {
               btns[i] = new Infragistics.Win.Misc.UltraButton();
               btns[i].Text = objMsgBoxFactory.ButtonCaptions[i];
               btns[i].Left = objMsgBoxFactory.ButtonLefts[i];
               btns[i].Width = objMsgBoxFactory.ButtonWidth;
               btns[i].Height = objMsgBoxFactory.ButtonHeight;
               btns[i].Top = objMsgBoxFactory.ButtonTop;

               btns[i].Visible = true;
               btns[i].Click += new EventHandler(Button_Click);
               btns[i].Tag = objMsgBoxFactory.buttonNos[i];
               this.Controls.Add(btns[i]);
           }

           this.ResumeLayout(true);

           #endregion

           this.btnDefault = DefaultButtton;     
       }
       public MsgBox(SqlConnection cn, string msgID, GEnum.MsgBoxButton[] msgButtons, GEnum.MsgBoxDefaultButton DefaultButtton)
       {
           InitializeComponent();

           string outMsgID = string.Empty;


           MsgBoxFactory objMsgBoxFactory = new MsgBoxFactory();
           objMsgBoxFactory.Connection = cn;
           objMsgBoxFactory.Initialisation((GEnum.InstanceMode?)GEnum.InstanceMode.Normal, msgID, msgButtons);
           objMsgBoxFactory.FormWidth = this.Width;
           objMsgBoxFactory.PrepareData();

           #region +++ Build UI +++

           this.btnDefault = GEnum.MsgBoxDefaultButton.DefaultButton1;
           this.Width = objMsgBoxFactory.FormWidth;
           this.Text = objMsgBoxFactory.Title;

           utxtMsgBody.Value = objMsgBoxFactory.BodyText;
           utxtMsgBody.Left = 12;

           //**********************
           if (utxtMsgBody.Text.Length > 135)
           {
               utxtMsgBody.Top = 17;
           }
           else if (utxtMsgBody.Text.Length > 90)
           {
               utxtMsgBody.Top = 25;
               utxtMsgBody.Height = 80;
           }
           else
           {
               utxtMsgBody.Top = 40;
               utxtMsgBody.Height = 40;
           }
           //**********************
           pic.Visible = false;
           btns = new Infragistics.Win.Misc.UltraButton[msgButtons.Length];

           

           this.SuspendLayout();
           for (int i = 0; i < msgButtons.Length; i++)
           {
               btns[i] = new Infragistics.Win.Misc.UltraButton();
               btns[i].Text = objMsgBoxFactory.ButtonCaptions[i];
               btns[i].Left = objMsgBoxFactory.ButtonLefts[i];
               btns[i].Width = objMsgBoxFactory.ButtonWidth;
               btns[i].Height = objMsgBoxFactory.ButtonHeight;
               btns[i].Top = objMsgBoxFactory.ButtonTop;

               btns[i].Visible = true;
               btns[i].Click += new EventHandler(Button_Click);
               btns[i].Tag = objMsgBoxFactory.buttonNos[i];
               this.Controls.Add(btns[i]);
           }

           this.ResumeLayout(true);

           #endregion

           this.btnDefault = DefaultButtton;     
       }

       
       public MsgBox(string msgID, GEnum.MsgBoxButton[] msgButtons, GEnum.MsgBoxDefaultButton DefaultButtton,GEnum.MsgBoxIcon icon)
       {
           InitializeComponent();

           string outMsgID = string.Empty;


           MsgBoxFactory objMsgBoxFactory = new MsgBoxFactory();

           objMsgBoxFactory.Initialisation((GEnum.InstanceMode?)GEnum.InstanceMode.Normal, msgID, msgButtons);
           objMsgBoxFactory.FormWidth = this.Width;
           objMsgBoxFactory.PrepareData();

           #region +++ Build UI +++

           this.btnDefault = GEnum.MsgBoxDefaultButton.DefaultButton1;
           this.Width = objMsgBoxFactory.FormWidth;
           this.Text = objMsgBoxFactory.Title;

           utxtMsgBody.Value = objMsgBoxFactory.BodyText;
           //MsgBox.Show(System.IO.Path.GetFullPath(Application.ExecutablePath + @"\..\..\..\..\WinUI\Resources\" + icon.ToString() + ".png"));
           //Image img = Image.FromFile(System.IO.Path.GetFullPath(Application.ExecutablePath + @"\..\..\..\..\WinUI\Resources\" + icon.ToString() + ".png"));
           Image img = (Image)Properties.Resources.ResourceManager.GetObject(icon.ToString());
           if (!GFunc.IsNE(img))
               pic.Image = img;


           if (utxtMsgBody.Text.Length * 6 > this.Width)
           {
               utxtMsgBody.Left = pic.Width + pic.Left + 10;
               utxtMsgBody.Width = this.Width - (pic.Left + pic.Width) - 20;
           }
           else if (utxtMsgBody.Text.Length * 6 < this.Width - pic.Width - 20)
           {              
               utxtMsgBody.Width = utxtMsgBody.Text.Length * 6;
               utxtMsgBody.Left = pic.Width+(this.Width - utxtMsgBody.Width ) / 2-20;
           }
           else
           {
               utxtMsgBody.Left = pic.Width + pic.Left;
               utxtMsgBody.Width = this.Width - (pic.Left + pic.Width);
           }
           

           //**********************
           if (utxtMsgBody.Text.Length > 135)
           {
               utxtMsgBody.Top = 17;
           }
           else if (utxtMsgBody.Text.Length > 90)
           {
               utxtMsgBody.Top = 25;
               utxtMsgBody.Height = 80;
           }
           else
           {
               utxtMsgBody.Top = 40;
               utxtMsgBody.Height = 40;
           }
           //**********************

           btns = new Infragistics.Win.Misc.UltraButton[msgButtons.Length];

           this.SuspendLayout();
           for (int i = 0; i < msgButtons.Length; i++)
           {
               btns[i] = new Infragistics.Win.Misc.UltraButton();
               btns[i].Text = objMsgBoxFactory.ButtonCaptions[i];
               btns[i].Left = objMsgBoxFactory.ButtonLefts[i];
               btns[i].Width = objMsgBoxFactory.ButtonWidth;
               btns[i].Height = objMsgBoxFactory.ButtonHeight;
               btns[i].Top = objMsgBoxFactory.ButtonTop;

               btns[i].Visible = true;
               btns[i].Click += new EventHandler(Button_Click);
               btns[i].Tag = objMsgBoxFactory.buttonNos[i];
               this.Controls.Add(btns[i]);
           }
           this.ResumeLayout(true);

           #endregion

           this.btnDefault = DefaultButtton;
           msgID = "Picture file is missing";         
       }

       */

        #endregion

        #region +++ UI Control Events +++

        private void Button_Click(object sender, EventArgs e)
        {
            Infragistics.Win.Misc.UltraButton Btn = sender as Infragistics.Win.Misc.UltraButton;
            this.btnReturn = (GEnum.MsgBoxButton)Btn.Tag;
            this.DialogResult = DialogResult.OK;
        }        
        private void MsgBox_Shown(object sender, EventArgs e)
        {
            #region Default Button

            switch (btnDefault)
            {
                case GEnum.MsgBoxDefaultButton.DefaultButton1:
                    this.AcceptButton = btns[0] ;
                    btns[0].Focus();
                    break;

                case GEnum.MsgBoxDefaultButton.DefaultButton2:
                    this.AcceptButton = btns[1];
                    btns[1].Focus();
                    break;

                case GEnum.MsgBoxDefaultButton.DefaultButton3:
                    this.AcceptButton = btns[2];
                    btns[2].Focus();
                    break;
                case GEnum.MsgBoxDefaultButton.DefaultButton4:
                    this.AcceptButton = btns[3];
                    btns[3].Focus();
                    break;
            }
            #endregion    
        }

        private void utxtMsgBody_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                this.AcceptButton.PerformClick();
        }
        #endregion

        private static GEnum.MsgBoxButton ShowMe(MsgBox frm)
        {
            try
            {
                frm.TopMost = true;
                frm.ShowDialog();
                GEnum.MsgBoxButton btnReturn = frm.btnReturn;
              
                frm.Dispose();
                return btnReturn;
            }
            catch(Exception ex)
            {
                MsgBox.Show(ex.Message);
                //SysAuditLogUtility.AddErrorLog( GEnum.AuditLogMode.Error, GEnum.SystemCode.Alert_Log, msgID, null);                    
            }
            return GEnum.MsgBoxButton.Cancel;
        }

        #region +++ Expose Methods +++

        public static GEnum.MsgBoxButton Show(string msgID)
        {            
            MsgBox frm = new MsgBox(msgID, GEnum.MsgBoxIcon.Information, GEnum.MsgBoxDefaultButton.DefaultButton1, GEnum.MsgBoxButton.OK);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(System.Data.SqlClient.SqlConnection cn, string msgID)
        {            
            MsgBox frm = new MsgBox(cn,msgID,GEnum.MsgBoxIcon.Information,GEnum.MsgBoxDefaultButton.DefaultButton1,  GEnum.MsgBoxButton.OK );            
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxIcon icon,params GEnum.MsgBoxButton[] msgButton)
        {
            MsgBox frm = new MsgBox(msgID, icon, GEnum.MsgBoxDefaultButton.DefaultButton1, msgButton);           
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(System.Data.SqlClient.SqlConnection cn,string msgID, GEnum.MsgBoxIcon icon, params GEnum.MsgBoxButton[] msgButton)
        {
            MsgBox frm = new MsgBox(cn, msgID, icon, GEnum.MsgBoxDefaultButton.DefaultButton1, msgButton);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(string msgID, params GEnum.MsgBoxButton[] msgButton)
        {
            MsgBox frm = new MsgBox(msgID, GEnum.MsgBoxIcon.Information,GEnum.MsgBoxDefaultButton.DefaultButton1, msgButton);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(System.Data.SqlClient.SqlConnection cn, string msgID,  params GEnum.MsgBoxButton[] msgButton)
        {
            MsgBox frm = new MsgBox(cn, msgID, GEnum.MsgBoxIcon.Information,GEnum.MsgBoxDefaultButton.DefaultButton1, msgButton);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(string msgID,GEnum.MsgBoxDefaultButton DefaultButton, params GEnum.MsgBoxButton[] msgButton)
        {
            MsgBox frm = new MsgBox(msgID, GEnum.MsgBoxIcon.Information, DefaultButton, msgButton);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(System.Data.SqlClient.SqlConnection cn, string msgID, GEnum.MsgBoxDefaultButton DefaultButton, params GEnum.MsgBoxButton[] msgButton)
        {
            MsgBox frm = new MsgBox(cn, msgID, GEnum.MsgBoxIcon.Information, DefaultButton, msgButton);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxIcon icon, GEnum.MsgBoxDefaultButton DefaultButton, params GEnum.MsgBoxButton[] msgButton)
        {
            MsgBox frm = new MsgBox(msgID, icon, DefaultButton, msgButton);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(System.Data.SqlClient.SqlConnection cn, string msgID, GEnum.MsgBoxIcon icon, GEnum.MsgBoxDefaultButton DefaultButton, params GEnum.MsgBoxButton[] msgButton)
        {
            MsgBox frm = new MsgBox(cn, msgID, icon, DefaultButton, msgButton);
            return ShowMe(frm);
        }
        /*
        public static GEnum.MsgBoxButton Show(System.Data.SqlClient.SqlConnection cn, string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2)
        {
            MsgBox frm = new MsgBox(cn,msgID, new GEnum.MsgBoxButton[] { msgButton1,msgButton2 });
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(System.Data.SqlClient.SqlConnection cn, string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(cn, msgID, new GEnum.MsgBoxButton[] { msgButton1 },icon);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2 });
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2,GEnum.MsgBoxButton msgButton3)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2,msgButton3});
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxButton msgButton3, GEnum.MsgBoxButton msgButton4)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2, msgButton3,msgButton4 });
            return ShowMe(frm);
        }


        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxDefaultButton btndefault)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1 }, btndefault);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxDefaultButton btndefault)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2},btndefault);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(SqlConnection cn, string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxDefaultButton btndefault)
        {
            MsgBox frm = new MsgBox(cn,msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2 }, btndefault);
            return ShowMe(frm);
        }
        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxButton msgButton3, GEnum.MsgBoxDefaultButton btndefault)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2, msgButton3 },btndefault);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxButton msgButton3, GEnum.MsgBoxButton msgButton4, GEnum.MsgBoxDefaultButton btndefault)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2, msgButton3, msgButton4 },btndefault);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxDefaultButton btndefault,GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1 }, btndefault,icon);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxDefaultButton btndefault, GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2 }, btndefault, icon);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxButton msgButton3, GEnum.MsgBoxDefaultButton btndefault, GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2, msgButton3 }, btndefault,  icon);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxButton msgButton3, GEnum.MsgBoxButton msgButton4, GEnum.MsgBoxDefaultButton btndefault, GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2, msgButton3, msgButton4 }, btndefault,  icon);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1 }, GEnum.MsgBoxDefaultButton.DefaultButton1, icon);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2,  GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2 }, GEnum.MsgBoxDefaultButton.DefaultButton1, icon);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxButton msgButton3,  GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2, msgButton3 }, GEnum.MsgBoxDefaultButton.DefaultButton1, icon);
            return ShowMe(frm);
        }

        public static GEnum.MsgBoxButton Show(string msgID, GEnum.MsgBoxButton msgButton1, GEnum.MsgBoxButton msgButton2, GEnum.MsgBoxButton msgButton3, GEnum.MsgBoxButton msgButton4, GEnum.MsgBoxIcon icon)
        {
            MsgBox frm = new MsgBox(msgID, new GEnum.MsgBoxButton[] { msgButton1, msgButton2, msgButton3, msgButton4 }, GEnum.MsgBoxDefaultButton.DefaultButton1, icon);
            return ShowMe(frm);
        } 
        */
        #endregion 

        #region Commented
        //private string[] GetButtonCaptions(int msgButtons,out int[]msgBtnVals)
        //{
        //    char[] msgBtnBinarys = ToBinary(msgButtons);
        //    int[] msgBtns = new int[msgBtnBinarys.Length];
        //    int j = 0;

        //    for (int i = 0; i < msgBtnBinarys.Length; i++)
        //        if (msgBtnBinarys[i].Equals('1'))
        //        {
        //            msgBtns[j] = Convert.ToInt32(Math.Pow(2, i));
        //            j++;
        //        }

        //    msgBtnVals = msgBtns;

        //    if (j <= 0) return null;
        //    if (j <= 1) return new string[] { GetBtnCaption(msgBtns[0]) };
        //    if (j <= 2) return new string[] { GetBtnCaption(msgBtns[0]), GetBtnCaption(msgBtns[1]) };
        //    if (j <= 3) return new string[] { GetBtnCaption(msgBtns[0]), GetBtnCaption(msgBtns[1]), GetBtnCaption(msgBtns[2]) };
        //    if (j <= 4) return new string[] { GetBtnCaption(msgBtns[0]), GetBtnCaption(msgBtns[1]), GetBtnCaption(msgBtns[2]), GetBtnCaption(msgBtns[3]) };
        //    return null;
        //}


        //// private string ToBinary(Int64 Decimal)
        //private char[] ToBinary(Int64 Decimal)
        //{
        //    // Declare a few variables we're going to need
        //    Int64 BinaryHolder;
        //    char[] BinaryArray;
        //    string BinaryResult = "";

        //    while (Decimal > 0)
        //    {
        //        BinaryHolder = Decimal % 2;
        //        BinaryResult += BinaryHolder;
        //        Decimal = Decimal / 2;
        //    }

        //    // The algoritm gives us the binary number in reverse order (mirrored)
        //    // We store it in an array so that we can reverse it back to normal
        //    BinaryArray = BinaryResult.ToCharArray();
        //    // Array.Reverse(BinaryArray);
        //    // BinaryResult = new string(BinaryArray);

        //    // return BinaryResult;
        //    return BinaryArray;
        //}

        //private void AdjustSize()
        //{     
        //    this.AutoSize = false;
        //    this.ubtn1.AutoSize = false;
        //    this.ubtn2.AutoSize = false;
        //    this.ubtn3.AutoSize = false;
        //    this.ubtn4.AutoSize = false;
        //    if ((nBtnWidth * nBtnCount) + (20 * nBtnCount) > this.Width)
        //        this.Width = (nBtnWidth * nBtnCount) + (20 * nBtnCount);
        //    if (nBtnWidth < 50)
        //        nBtnWidth = 50;
        //    this.ubtn1.Width = nBtnWidth;
        //    this.ubtn2.Width = nBtnWidth;
        //    this.ubtn3.Width = nBtnWidth;
        //    this.ubtn4.Width = nBtnWidth;
        //    this.ubtn1.Height += 5;
        //    this.ubtn2.Height += 5;
        //    this.ubtn3.Height += 5;
        //    this.ubtn4.Height += 5;

        //    if (nBtnCount == 1)
        //    {
        //        this.ubtn1.Left = this.Width / 2 - this.ubtn1.Width / 2;
        //    }
        //    if (nBtnCount == 2)
        //    {
        //        //this.ubtn1.Left = this.Width / 3 - this.ubtn1.Width / 2;
        //        //this.ubtn2.Left = ((this.Width / 3) * 2) - this.ubtn1.Width / 2;
                
        //        this.ubtn1.Left = this.Width / 2 - this.ubtn1.Width - 10;
        //        this.ubtn2.Left = ubtn1.Left + ubtn1.Width + 10; //this.Width / 2 + 10;
        //    }
        //    if (nBtnCount == 3)
        //    {
        //        //this.ubtn1.Left = this.Width / 4 - this.ubtn1.Width / 2;
        //        //this.ubtn2.Left = ((this.Width / 4) * 2) - this.ubtn2.Width / 2;
        //        //this.ubtn3.Left = ((this.Width / 4) * 3) - this.ubtn3.Width / 2;
        //        this.ubtn1.Left = this.Width / 3 - this.ubtn1.Width - 10;
        //        this.ubtn2.Left = ubtn1.Left + ubtn1.Width + 10;
        //        this.ubtn3.Left = ubtn2.Left + ubtn2.Width + 10;
        //    }
        //    if (nBtnCount == 4)
        //    {
        //        this.ubtn1.Left = this.Width / 4 - this.ubtn1.Width - 15;
        //        this.ubtn2.Left = ubtn1.Left + ubtn1.Width + 10;
        //        this.ubtn3.Left = ubtn2.Left + ubtn2.Width + 10;
        //        this.ubtn4.Left = ubtn3.Left + ubtn3.Width + 10;
        //    }
        //}
        #endregion
    }
}
       