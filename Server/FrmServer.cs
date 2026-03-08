using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class FrmServer : Form
    {
        private Server _server;
        public FrmServer()
        {
            InitializeComponent();
            btnStart.Enabled = false;
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _server = new Server();
            _server.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            textBox1.Text = "Server je pokrenut!";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            textBox1.Text = "Server je zaustavljen!!";
            _server.Stop();

        }
    }
}
