using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Ini;
using xedits;
using xfiles;
using dlgLogin;

namespace tsDmw
{
    public partial class DfmTsDmw : Form
    {
        tsdb fdb = new tsdb();
        string fmsg;

        string fdataDir = "";

        IniData fini = new IniData();

        public DfmTsDmw()
        {
            InitializeComponent();
            fdb.notifyTask = taskCallback;
            fdb.beginTask = beginTask;
            fdb.endTask = endTask;
            fdb.message = task_msg;
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DfmTsDmw_Load(object sender, EventArgs e)
        {
            IniFile ini = new IniFile("");
            ini.ReadForm(this);

            fdataDir = ini.ReadDir("dataDir");
        }

        private void DfmTsDmw_FormClosed(object sender, FormClosedEventArgs e)
        {
            IniFile ini = new IniFile("");
            ini.Reset();
            ini.WriteForm(this);

            ini.WriteDir("dataDir", fdataDir);

            fini.CopyTo(ini);
        }

        private void DfmTsDmw_Shown(object sender, EventArgs e)
        {
            fdb.getVersion(api_version);
        }

        void taskCallback(object sender, tnotify cb)
        {
            if (cb != null)
            this.Invoke( new EventHandler(cb) );
        }

        private void message(string msg)
        {
            if (lbMsg.Text.Length > 0)
                lbMsg.AppendText(Environment.NewLine);
            lbMsg.AppendText(msg);
        }

        void invoke_msg(object sender, EventArgs e)
        {
            message(fmsg);
        }

        void task_msg(object sender, string msg)
        {
            fmsg = msg;
            this.Invoke(new EventHandler(invoke_msg));
        }

        void invoke_task(object sender, EventArgs e)
        {
            statusTask.Text = fmsg;
            statusProgress.Text = "";
            Cursor.Current = Cursors.Default;
        }

        void beginTask(object sender, string msg)
        {
            statusTask.Text = msg;
            statusProgress.Text = "";
            Cursor.Current = Cursors.WaitCursor;
        }

        void endTask(object sender, string msg) 
        {
            fmsg = msg;
            this.Invoke(new EventHandler(invoke_task));
        }

        void invoke_progress(object sender, EventArgs e)
        {
            statusProgress.Text = fmsg;
        }

        void task_progress(object sender, string msg)
        {
            fmsg = msg;
            this.Invoke(new EventHandler(invoke_progress));
        }

        void api_version(object sender, EventArgs e)
        {
            statusApiVersion.Text = fdb.version;
            fdb.getBranches(branches);
        }

        void branches(object sender, EventArgs e)
        {
            statusApiVersion.Text = fdb.version;
            XEdits.FillListBox(lbBranches, fdb.branches);
        }

        void commits(object sender, EventArgs e)
        {
            XEdits.FillMemo(lbMsg, fdb.Commits);
        }

        string selectedBranch()
        {
            int i = lbBranches.SelectedIndex;
            if (i >= 0 && i < lbBranches.Items.Count)
            {
                string s = lbBranches.Items[i].ToString();
                if (s.Length > 0) return s;
            }

            return null;
        }

        private void lbBranches_Click(object sender, EventArgs e)
        {
            string br = selectedBranch();
            if (br != null) { 
                lbMsg.Text="";
                fdb.getCommits(br,commits);
            }
        }

        private void dumpItem_Click(object sender, EventArgs e)
        {
            string br = selectedBranch();
            if (br != null) {
                string filter = "Files (*.json)|*.json";
                string title = "Save content as";
                string dest;
                bool rc = XFiles.dialSaveFile(ref fdataDir, 
                                              filter, title, 
                                              out dest);

                if (rc) {
                    lbMsg.Text = "";
                    fdb.Progress = task_progress;
                    fdb.getContent(br, dest);
                }
            }
        }

        private void saveContentAsText_Click(object sender, EventArgs e)
        {
            string filter = "Files (*.json)|*.json";
            string title = "json content";
            string path;
            bool rc = XFiles.dialFile(ref fdataDir, filter, title, true, out path);

            if (rc)
            {
                lbMsg.Clear();
                string dest = Path.ChangeExtension(path, ".txt");
                fdb.json_to_text(path, dest);
            }
        }

        private void saveContentAsMap_Click(object sender, EventArgs e)
        {
            string filter = "Files (*.json)|*.json";
            string title = "json content";
            string path;
            bool rc = XFiles.dialFile(ref fdataDir, filter, title, true, out path);

            if (rc)
            {
                lbMsg.Clear();
                string dest = Path.ChangeExtension(path, ".dm");
                fdb.json_to_map(path, dest);
            }
        }

        private void LoginItem_Click(object sender, EventArgs e)
        {
            LoginDialog dlg = new LoginDialog(fini);
            dlg.login = fdb.Login;
            dlg.password = fdb.Password;

            if (dlg.ShowDialog() == DialogResult.OK) {
                fdb.Login = dlg.login;
                fdb.Password = dlg.password;

                fdb.getVersion(api_version);
            }
        }

    }
}
