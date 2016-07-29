using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using xwin;

namespace lx2
{
    public class Client
    {

        public delegate void TOnData(int[] data);

        Thread m_thread;

        globalMem m_gm;

        int[] m_data;
        int m_tick;

        TOnData m_OnData;

        public TOnData OnData
        {
            set { m_OnData = value; }
        }

        public Client()
        {
            m_gm = new globalMem();
            m_gm.open("sau_data", 1024);
            m_data = new int[256];
            m_tick = 0;

            m_thread = new Thread(this.execute);
        }

        public void Start()
        {
            m_thread.Start();
        }

        public void Stop()
        {
            m_thread.Abort();
            m_thread.Join();
        }

        void execute()
        {
            if (m_gm.Enabled())
            {
                while (true)
                {
                    m_gm.GetData(m_data, 64);
                    if (m_data[0] != m_tick)
                    {
                        m_tick = m_data[0];
                        if (m_OnData != null)
                            m_OnData(m_data);

                    }

                    Thread.Sleep(10);
                }
            }
        }
    }

    [Guid("3232D99B-E7F0-421A-A0B7-F690D5C5F4D3"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IProject2D
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        int Project(double ix, double iy, out double ox, out double oy);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        int Backup(double ix, double iy, out double ox, out double oy);
    }

    public class JpegProj
    {
        const UInt32 S_OK = 0;

        [DllImport("dll_lnk.dll",
                   CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Interface)]

        extern static IProject2D GetJpgProj1(
            double wx, double wy, double wz,                // [WGS-84] Lat,Lon,Alt
            double rx, double ry, double rz,                // Gam,Tet,Psi
            double cam_w, double cam_h, double cam_f,       // width, height, focus
            double cam_rx, double cam_ry, double cam_rz,    // camera rotation on plane
            int cam_swap,                                   // camera direction
            double ground                                   // relative height on plane
            );

        IProject2D m_proj = null;

        public JpegProj(
            double wx, double wy, double wz,
            double rx, double ry, double rz,
            double cam_w, double cam_h, double cam_f,
            double cam_rx, double cam_ry, double cam_rz,
            int cam_swap,
            double ground)
        {
            m_proj = GetJpgProj1(wx, wy, wz, rx, ry, rz, cam_w, cam_h, cam_f, cam_rx, cam_rx, cam_rz, cam_swap, ground);
        }

        public bool enabled()
        {
            return m_proj != null;
        }

        public bool Project(double ix, double iy, out double ox, out double oy)
        {
            if (m_proj == null)
            {
                ox = 0;
                oy = 0;
                return false;
            }

            return m_proj.Project(ix, iy, out ox, out oy) == S_OK;
        }

    }


}
