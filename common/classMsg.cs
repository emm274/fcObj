using System;
using System.Windows.Forms;
using System.Collections.Generic;
using otypes;

namespace classMsg
{
    public class ClassMsg {

        public tmessage message { get; set; }

        public void __message(string capt, string msg)
        {
            if (message != null) {
                string s = msg;
                if (capt != null) s = String.Format("{0}: {1}.", capt, msg);
                message(this, s);
            }
        }
    }
}
