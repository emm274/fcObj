using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lx2video
{
    public partial class MainForm : Form
    {
        lx2.Client m_client;

        lx2.JpegProj m_jpeg;

        Double m_latitude;
        Double m_longitude;
        Double m_altitude;
        Double m_roll;
        Double m_pitch;
        Double m_dir;

        public MainForm()
        {
            InitializeComponent();

            m_client = new lx2.Client();
            m_client.Start();
            m_client.OnData = lx2Data;

            m_jpeg = new lx2.JpegProj(

                53 * Math.PI / 180,
                27 * Math.PI / 180,
                272,

                0,
                0,
                0,

                640,
                480,
                670,

                0,
                15 * Math.PI,
                0,

                0,
                270);

            if (m_jpeg.enabled())
            {
                double x1, y1, x2, y2, x3, y3, x4, y4;
                m_jpeg.Project(0, 0, out x1, out y1);
                m_jpeg.Project(640, 0, out x2, out y2);
                m_jpeg.Project(640, 480, out x3, out y3);
                m_jpeg.Project(0, 480, out x4, out y4);
            }
        }

        ~MainForm()
        {
        }

        void displayLx2Data(object o, EventArgs e)
        {
            lbLatitude.Text = m_latitude.ToString();
            lbLongitude.Text = m_longitude.ToString();
            lbAltitude.Text = m_altitude.ToString();
            lbRoll.Text = m_roll.ToString();
            lbPitch.Text = m_pitch.ToString();
            lbDirection.Text = m_dir.ToString();
        }


        void lx2Data(int[] data)
        {
            m_latitude=data[1] / 1000000.0;
            m_longitude = data[2] / 1000000.0;
            m_altitude = data[3] / 1000.0;
            m_roll = data[4] / 1000000.0;
            m_pitch = data[5] / 1000000.0;
            m_dir = data[6] / 1000000.0;

            this.Invoke(new EventHandler(displayLx2Data));
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_client.Stop();
        }
    }
}
