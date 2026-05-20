using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinUI
{
    public partial class frmInputDialog : Form
    {
        public delegate void CustomEvent(object sender, EventArgs e);
        public event CustomEvent inputEvent;
        private bool hideForm = false;

        public frmInputDialog()
        {
            InitializeComponent();
        }
        public frmInputDialog(string prompt, string title,string defaultValue,string okButtonCaption,string cancelButtonCaption)
        {
            InitializeComponent();
            lblLabel.Text = prompt;
            this.Text = title;
            txtInput.Text = defaultValue;
            btn1.Text = okButtonCaption;
            btn2.Text = cancelButtonCaption;
        }

        public frmInputDialog(string prompt, string title, string defaultValue, string okButtonCaption, string cancelButtonCaption,bool hide)
        {
            InitializeComponent();
            lblLabel.Text = prompt;
            this.Text = title;
            txtInput.Text = defaultValue;
            btn1.Text = okButtonCaption;
            btn2.Text = cancelButtonCaption;
            hideForm = hide;
        }
        private void btn1_Click(object sender, EventArgs e)
        {
            if (inputEvent != null)
                this.inputEvent(txtInput, null);
            if (hideForm)
                this.Hide();
            else
                this.DialogResult = DialogResult.OK;
        }

        private void btn2_Click(object sender, EventArgs e)
        {            
            this.Hide();
        }
    }
}