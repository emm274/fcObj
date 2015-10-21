using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace xedits
{
    public static class XEdits
    {
        private static KeyPressEventHandler intHandler = IntKeyPressed;
        private static KeyPressEventHandler floatHandler = FloatKeyPressed;

        static string editText(TextBox edit, char c) 
        {
            string s = edit.Text;
            int p = edit.SelectionStart;
            int l = edit.SelectionLength;

            string before = s.Substring(0, p);
            string after = s.Substring(p + l);

            return before + c + after;
        }

        static void IntKeyPressed(Object sender, KeyPressEventArgs e)
        {
            TextBox edit = (TextBox)sender;
            char c=e.KeyChar;

            if ((c < '0' || c > '9') && (c != 8))
                e.Handled = true;
            else  {
                string s = editText(edit, c);

                int v;
                if (!int.TryParse(s,out v))
                e.Handled = true;
            }
        }

        static void FloatKeyPressed(Object sender, KeyPressEventArgs e)
        {
            TextBox edit = (TextBox)sender;
            char c = e.KeyChar;

            if ((c < '0' || c > '9') &&
                (c != '.') && (c != 8))
                e.Handled = true;
            else
            {
                string s = editText(edit, c);

                double v;
                if (!double.TryParse(s, out v))
                    e.Handled = true;
            }
        }

        public static void IntTextBox(TextBox edit)
        {
            edit.KeyPress += intHandler;
        }

        public static void FloatTextBox(TextBox edit)
        {
            edit.KeyPress += floatHandler;
        }

        public static int FillListBox(ListBox lb, List<string> list) 
        {
            lb.BeginUpdate();
            foreach(var s in list)
            lb.Items.Add(s);
            lb.EndUpdate();
            return lb.Items.Count;
        }

        public static void FillMemo(TextBox lb, List<string> list)
        {
            lb.Clear();
            foreach(var s in list) {
                if (lb.Text.Length > 0)
                lb.AppendText(Environment.NewLine);
                lb.AppendText(s);
            }
        }

    }
}
