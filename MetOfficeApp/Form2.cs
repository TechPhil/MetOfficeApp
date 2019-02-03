using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetOfficeApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.metoffice.gov.uk/datapoint/api");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Settings1.Default.mo_default_api;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings1.Default.mo_api_key = textBox1.Text;
            Settings1.Default.Save();
            this.Close();
        }
    }
}
