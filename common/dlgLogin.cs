using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ini;

namespace dlgLogin
{
    public partial class LoginDialog : Form
    {
        IniData fini;

        public LoginDialog( IniData ini )
        {
            InitializeComponent();
            fini = ini;
        }

        public string login
        {
            get { return tbLogin.Text; }
            set { tbLogin.Text = value; }
        }

        public string password
        {
            get { return tbPassword.Text; }
            set { tbPassword.Text = value; }
        }

        private void LoginDialog_Load(object sender, EventArgs e)
        {
            if (fini != null) fini.ReadForm(this);
        }

        private void LoginDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fini != null) fini.WriteForm(this);
        }
    }
}
