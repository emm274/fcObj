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
using enterText;

namespace tsDmw
{
    public partial class DfmTsDmw : Form
    {
        tsdb fdb = new tsdb();
        string fmsg;

        string fdataDir = "";
        string fworkDir = "";

        IniData fini = new IniData();

        public DfmTsDmw()
        {
            InitializeComponent();
            fdb.notifyTask = taskCallback;
            fdb.beginTask = beginTask;
            fdb.endTask = endTask;
            fdb.message = task_msg;
            fdb.Progress = task_progress;
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
            fworkDir = ini.ReadDir("workDir");
            statusWorkDir.Text = "["+fworkDir+"]";
        }

        private void DfmTsDmw_FormClosed(object sender, FormClosedEventArgs e)
        {
            IniFile ini = new IniFile("");
            ini.Reset();
            ini.WriteForm(this);

            ini.WriteDir("dataDir", fdataDir);
            ini.WriteDir("workDir", fworkDir);

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

        void invoke_end_task(object sender, EventArgs e)
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
            this.Invoke(new EventHandler(invoke_end_task));
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
            if (lbBranches.Items.Count > 0)
            {
                lbBranches.SelectedIndex = 0;
                lbBranches_Click(null, null);
            }
        }

        void commits(object sender, EventArgs e)
        {
            lbCommit.BeginUpdate();
            lbCommit.Items.Clear();

            foreach (var c in fdb.Commits)
            lbCommit.Items.Add(c.Value);

            lbCommit.EndUpdate();
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
            string branch = selectedBranch();
            if (branch != null) 
            fdb.getCommits(branch, commits);
        }

        private void dumpItem_Click(object sender, EventArgs e)
        {
            string br = selectedBranch();
            if (br != null) {
                string filter = "Files (*.json)|*.json|"+
                                "Files (*.txt)|*.txt|"+
                                "Files (*.dm)|*.dm";
                string title = "Сохранить карту как";
                string dest;
                int rc = fini.ReadInt("saveIndex");
                    
                rc=XFiles.dialSaveFile(ref fdataDir, 
                                       filter, title, 
                                       rc,out dest);

                if (rc >= 0) {
                    fini.WriteInt("saveIndex", rc);

                    lbMsg.Clear();
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

        void after_commit(object sender, EventArgs e)
        {
            if (fdb.status_success())
                lbBranches_Click(null, null);
        }

        private void LoadMap_Click(object sender, EventArgs e)
        {
            lbMsg.Clear();

            string branch = selectedBranch();
            if (branch == null)
                message("Не указана ветка!");
            else
            {
                string filter = "Files (*.dm)|*.dm";
                string title = "Загрузить карту";
                string path;
                bool rc = XFiles.dialFile(ref fdataDir, filter, title, true, out path);

                if (rc)
                {
                    DateTime now = DateTime.Now;

                    EnterText dlg = new EnterText(fini);
                    dlg.Caption = "commit";
                    dlg.Title = branch;
                    dlg.text = now.ToString();

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        string comment = dlg.text;

                        message(String.Format("{0}: +commit \"{1}\"", branch, comment));

                        string json = Path.ChangeExtension(path, ".json");

                        tsLoaderMap loader = new tsLoaderMap();
                        loader.message = task_msg;

                        loader.workDir(fworkDir);
                        rc = loader.exec(path, json, branch, comment, false);

                        if (rc) fdb.commit(branch, json, after_commit);
                    }
                }

            }
        }

        private void dialWorkDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = fworkDir;

            if (dlg.ShowDialog() == DialogResult.OK) {
                fworkDir = dlg.SelectedPath;
                statusWorkDir.Text = "["+fworkDir+"]";
            }

        }

        private void lbMsg_SizeChanged(object sender, EventArgs e)
        {
            btClear.Left = lbMsg.Left + lbMsg.Width - 24 - btClear.Width;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            lbMsg.Clear();
        }

        private void deleteCommit_Click(object sender, EventArgs e)
        {
            string branch = selectedBranch();
            if (branch == null)
                message("Не указана ветка!");
            else
            {
                int i = lbCommit.Items.Count-1;
                if (i < 0)
                    message("В ветке нет изменений!");
                else 
                {
                    string commit = fdb.Commits.ElementAt(i).Key;
                    fdb.undo_commit(branch,commit,after_commit);
                }
            }
        }

        private void lbCommit_Click(object sender, EventArgs e)
        {
            string branch = selectedBranch();
            if (branch == null)
                message("Не указана ветка!");
            else
            {
                int i = lbCommit.SelectedIndex;

                if (i >= 0) {
                    KeyValuePair<string,string> kv = fdb.Commits.ElementAt(i);
                    message(kv.Key+" \""+kv.Value+"\"");
                }
            }
        }
    }
}
