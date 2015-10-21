/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 17.06.2015
 * Time: 15:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

#define dll_ddw1

using System;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using Dmw;
using xwin;

namespace DmwAuto
{
	
	public class client
	{
        Thread fthread;

		globalMem fgm;
		int[] fdata;
		int ftick;

	  	public delegate void TOnRect(int x1, int y1, int x2, int y2);
	  	private TOnRect fOnRect;
	  	
		public TOnRect OnRect {
			set { fOnRect=value; }
		}
	  	
		public client()
		{
			fgm = new globalMem();
			fgm.open("Dmw Data Exchange",1024);
		    fdata = new int[256];
			ftick=0;

            fthread = new Thread(this.execute);
        }
		
        public void Start() {
            fthread.Start();
        }

        public void Stop() {
            fthread.Abort();
            fthread.Join();
        }

		void execute() 
		{
			if (fgm.Enabled()) {
				
				while (true) {
					fgm.GetData(fdata,64);
					if (fdata[0] != ftick) {
						ftick=fdata[0];
						
						int cmd = fdata[1];
						if (cmd == 4) {
							if (fOnRect != null) 
							fOnRect(fdata[2],fdata[3],fdata[4],fdata[5]);
						}
						
					}

                    Thread.Sleep(10);
				}
				
			}
		}
		
	}
	
	public class dmwAuto
	{
		
		public int pick_rect = 4;

#if (dll_ddw)
        [DllImport("dll_ddw.dll")]
        static extern byte dmw_connect();

        [DllImport("dll_ddw.dll")]
        static extern void dmw_disconnect();

        [DllImport("dll_ddw.dll")]
        static extern byte dmw_L_to_G(int ix, int iy, out double gx, out double gy);

        [DllImport("dll_ddw.dll")]
        static extern void dmw_XY_BL(double x, double y, out double b, out double l);

        [DllImport("dll_ddw.dll", CharSet = CharSet.Ansi)]
        static extern IntPtr dmw_GetSValue(int typ, int ind, IntPtr val);

        [DllImport("dll_ddw.dll", CharSet = CharSet.Ansi)]
        static extern int dmw_DrawVector2(int id, int loc, int color,
                                          [MarshalAs(UnmanagedType.LPArray)] byte[] mf,
                                          [MarshalAs(UnmanagedType.LPArray)] byte[] txt);

        [DllImport("dll_ddw.dll", CharSet = CharSet.Ansi)]
        static extern byte dmw_PickCaption([MarshalAs(UnmanagedType.LPArray)] byte[] str);

        [DllImport("dll_ddw.dll")]
        static extern void dmw_Pick_wm(int wm, int p1, int p2);

        static byte[] StringToBytes(string s)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(s);
        }


#else
		Dmw.dmw_auto dmw_auto = null;
		Dmw.Idmw_pick dmw_pick = null;
        Dmw.Idmw_misc dmw_misc = null;
#endif
		
		public dmwAuto()
		{
#if (!dll_ddw) 
			dmw_auto = new dmw_autoClass();
			dmw_pick = new dmw_pickClass();
            dmw_misc = new dmw_miscClass();
#endif
		}

        public void Connect()
        {
#if (dll_ddw)
            dmw_connect();
#endif
        }

        public void Disconnect()
        {
#if (dll_ddw)
            dmw_disconnect();
#endif
        }

		public void pick_Caption(string Capt) 
        {
#if (dll_ddw)
            dmw_PickCaption(StringToBytes(Capt));
#else
			dmw_pick.Pick_Caption=Capt;
#endif
		}
		
		public void pick(int pen, int p1, int p2) 
        {
#if (dll_ddw)
            dmw_Pick_wm(pen,p1,p2);
#else
			dmw_pick.Pick(pen,p1,p2);
#endif
		}
		
		public void l_to_r(int lx, int ly, out double b,out double l)
		{
			double gx,gy;
#if (dll_ddw)
            dmw_L_to_G(lx, ly, out gx, out gy);
            dmw_XY_BL(gx, gy, out b, out l);
#else
			dmw_auto.L_to_G(lx,ly, out gx, out gy);
			dmw_auto.XY_BL(gx,gy,out b, out l);
#endif
		}

        public string WorkDir()
        {
            string s = "";
#if (dll_ddw)
            var val = Marshal.AllocHGlobal(255); 
            dmw_GetSValue(13, 0, val);
            s = Marshal.PtrToStringAnsi(val);
#else
            dmw_auto.GetSValue(13 << 24,out s);
#endif
            return s;
        }

        public int draw_Vector(int Id, int Loc, int Color, double[] mf, string txt) {

            int rc = 0;
            string s1 = "", s2 = "";

            double k = Math.PI / 180 * 100000000;

            if (mf != null)
            foreach (var v in mf) { 
                int i = (int) (v*k);
                if (s1.Length > 0) s1 += " ";
                s1 += i.ToString();
            }

            if (txt != null) s2 = txt;

#if (dll_ddw)
            rc = dmw_DrawVector2(Id, Loc, Color, StringToBytes(s1), StringToBytes(s2));
#else
            dmw_misc.Draw_vector(Id, Loc, Color, s1, s2, out rc);
#endif
            return rc;

        }
	}
}
