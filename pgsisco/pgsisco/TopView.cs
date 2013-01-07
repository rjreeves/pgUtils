using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;


namespace pgsisco
{
    public partial class TopView : Form
    {
        public NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=Quest123;");
        public NpgsqlCommand cmd;

        public TopView()
        {
            InitializeComponent();

        }


        private string GetData(string sql, NpgsqlConnection conn)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);            
            conn.Open();
            string res = cmd.ExecuteScalar().ToString();
            conn.Close();
            return res;
        }

        private string GetDataTimeSpan(string sql)
        {
            cmd.CommandText = sql;
            conn.Open();
            TimeSpan ts = (TimeSpan)cmd.ExecuteScalar();
            string res = String.Format(@"Uptime: {0} days, {1} Hours, {2} Minutes.", ts.Days.ToString(),ts.Hours.ToString(),ts.Minutes.ToString());
            conn.Close();
            return res;
        }

        private DataTable GetDataTable(string sql, NpgsqlConnection conn)
        {   
            DataTable dt = new DataTable();          
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);                
                da.Fill(dt);               
            }
            catch (Exception e)
            { 
                MessageBox.Show(e.Message);
            }
            return dt;
        }

        private void TopView_Load(object sender, EventArgs e)
        {  
            cmd = new NpgsqlCommand("Select Version()", conn);
            conn.Open();
            string res = (string)cmd.ExecuteScalar();
            this.Text += " - " + res;
            conn.Close();

            label1.Text = GetData("Select Version()", conn);
            label2.Text = GetDataTimeSpan("Select date_trunc('minutes', current_timestamp - pg_postmaster_start_time())");

            label5.Text = "Data Directory: " + GetData("Select setting from pg_settings where name = 'data_directory'", conn);


            DataTable dt = GetDataTable("Select datname from pg_database order by datname", conn);
            foreach(DataRow dr in dt.Rows)
            {
                listBox1.Items.Add(dr[0].ToString());
            }
            label4.Text = GetData("SELECT sum(pg_database_size(datname)) from pg_database", conn).ToString();
            Refresh();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            string constr = string.Format("Server=127.0.0.1;Port=5432;Database={0};User Id=postgres;Password=Quest123", listBox1.SelectedItem);

            NpgsqlConnection conn = new NpgsqlConnection(constr);
            DataTable dt = GetDataTable("Select tablename from pg_tables order by tablename",conn);

            listBox2.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                listBox2.Items.Add(dr[0].ToString());
            }



            label4.Text = GetData(String.Format("SELECT sum(pg_database_size(datname)) from pg_database", listBox1.SelectedItem), conn).ToString();

            Refresh();
             
        }

        private void listBox2_Click(object sender, EventArgs e)
        {
            string cmd = String.Format(@"SELECT pg_relation_size('{0}') as size FROM information_schema.tables",listBox2.SelectedItem);
            label7.Text = GetData(cmd, conn);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbconnect dbc = new dbconnect();
            dbc._db = "Postgres";
            dbc._host = "localhost";
            dbc._user = "postgres";
            dbc._password = "postgres1";
            dbc.ShowDialog();
        }


    }
}
