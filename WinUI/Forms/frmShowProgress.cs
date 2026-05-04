using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WinUI
{
    public partial class frmShowProgress : Form
    {       
        public frmShowProgress()
        {
            InitializeComponent();

        }
        public frmShowProgress(string msg)
        {
            InitializeComponent();
            lblCurrentStatus.Text = msg;
        }
        public void CloseMe()
        {
            BeginInvoke((Action)(() => { Close(); }));
        }      
    }
}
