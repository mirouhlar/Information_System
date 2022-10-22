using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Informacny_system
{
    public partial class Form1 : Form
    {
        DBconnect connect;
        //    OracleConnection connection;
        OracleCommand command;

        public Form1()
        {
            InitializeComponent();
            connect = new DBconnect();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            connect.closeConnection();
            System.Windows.Forms.Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            try {
                connect.openConnection();
                command = connect.getConnection().CreateCommand();

                command.CommandText = "select * from zamestnanci where prihl_meno = '" + textBox1.Text.Trim() + "' and heslo = '" + textBox2.Text.Trim() + "'";

                OracleDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("Prihlásenie úspešné!", "Výsledok prihlásenia");
                    this.Hide();

                    informacny_system infsys = new informacny_system(connect.getConnection(), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                    infsys.ShowDialog();

                    this.Show();
                    textBox2.Text = "";
                }
                else
                {
                    MessageBox.Show("Prihlásenie neúspešné! \nZlé prihlasovacie údaje!", "Výsledok prihlásenia");
                }
                reader.Dispose();
                command.Dispose();
                connect.closeConnection();
            }

            catch (OracleException ex) {
                MessageBox.Show("Problém s pripojením k databáze!\nDôvod: " + ex.Message+"\nKód: "+ex.ErrorCode.ToString()+"", "Varovanie");
            }
        }
    }
}
