using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using System.IO;

namespace SimpleClock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboboxInitialzation();
            timerClock.Start();
        }

        List<string> hours = new List<string>();
        List<string> minutes = new List<string>();

        private string strSelectTime;
        private WaveOutEvent waveOut;
        private AudioFileReader audioFileReader;
        private void timerAlert_Tick(object sender, EventArgs e)
        {
            if (strSelectTime == DateTime.Now.ToString("HH:mm"))
            {
                try
                {
                    stopWaveOut();

                    string audioFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "alert.wav");

                    audioFileReader = new AudioFileReader(audioFilePath);

                    waveOut = new WaveOutEvent();
                    waveOut.Init(audioFileReader);

                    waveOut.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("無法播放聲音檔，錯誤資訊: " + ex.Message);
                }
                finally
                {
                    timerAlert.Stop();
                }
            }
        }
        private void stopWaveOut()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
        }
        private void comboboxInitialzation()
        {
            for (int i = 0; i <= 23; i++)
                cmbHour.Items.Add(string.Format("{0:00}", i));
            cmbHour.SelectedIndex = 0;

            for (int i = 0; i <= 59; i++)
                cmbMin.Items.Add(string.Format("{0:00}", i));
            cmbMin.SelectedIndex = 0;
        }
        private void timerClock_Tick(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtWeekDay.Text = DateTime.Now.ToString("dddd");
        }

        private void btnSetAlert_Click(object sender, EventArgs e)
        {
            timerAlert.Start();
            btnSetAlert.Enabled = false;
            btnCancelAlert.Enabled = true;
            strSelectTime = cmbHour.SelectedItem.ToString() + ":" + cmbMin.SelectedItem.ToString();
        }

        private void btnCancelAlert_Click(object sender, EventArgs e)
        {
            stopWaveOut();
            timerAlert.Stop();
            btnSetAlert.Enabled = true;
            btnCancelAlert.Enabled = false;
        }
    }
}
