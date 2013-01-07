using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pgsisco
{
    public partial class dbconnect : Form
    {

        public string _host;
        public string _db;
        public string _user;
        public string _password;

        public dbconnect()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)  // cancel
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _host = this.textBox1.ToString();
            _db = this.textBox4.ToString();
            _user = this.textBox2.ToString();
            _password = this.textBox3.ToString();
        }

        private void dbconnect_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = _host.ToString();
            this.textBox4.Text = _db.ToString();
            this.textBox2.Text = _db.ToString();
            this.textBox3.Text = _password.ToString();
        }
    }
}
