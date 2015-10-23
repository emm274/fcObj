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

namespace enterText
{
    public partial class EnterText : Form {

        IniData fini;

        public string Caption { set { Text = value; } }

        public string Title { set { lbText.Text = value; } }

        public string text { 
            get { 
                return textBox.Text; 
            }
            set { 
                textBox.Text = value;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        public EnterText( IniData ini ) 
        {
            InitializeComponent();
            fini = ini;
        }

        private void EnterText_Load(object sender, EventArgs e)
        {
            if (fini != null) fini.ReadForm(this);
        }

        private void EnterText_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fini != null) fini.WriteForm(this);
        }
    }
}
