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

namespace OkCancelDialogs
{
    public partial class OkCancelDialog : Form
    {
        protected IniData fini;

        public OkCancelDialog( IniData ini )
        {
            InitializeComponent();
            fini = ini;
        }

        private void paBottom_Resize(object sender, EventArgs e)
        {
            int dx = btCancel.Left + btCancel.Width - btOK.Left;
            if (dx > 0) {
                dx = (paBottom.Width - dx) / 2;
                btOK.Left = btOK.Left + dx;
                btCancel.Left = btCancel.Left + dx;
            }
        }

        private void OkCancelDialog_Load(object sender, EventArgs e)
        {
            if (fini != null) fini.ReadForm(this);
        }

        private void OkCancelDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fini != null) fini.WriteForm(this);
        }
    }
}
