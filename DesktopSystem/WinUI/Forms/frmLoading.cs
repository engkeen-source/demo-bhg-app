using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WinUI
{
    public partial class frmLoading : Form
    {       
        public frmLoading()
        {
            InitializeComponent();

        }
        public frmLoading(string msg)
        {
            InitializeComponent();
        }
        public void CloseMe()
        {
            BeginInvoke((Action)(() => { Close(); }));
        }      
    }
}
