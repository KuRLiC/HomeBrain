using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HomeBrain
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _tray.Visible = true;
            _timer_alive.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _tray.Visible = false;
            _timer_alive.Stop();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void _timer_alive_Tick(object sender, EventArgs e)
        {
            Brain.ImAlive();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _tray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Visible = !Visible;
        }
    }
}
