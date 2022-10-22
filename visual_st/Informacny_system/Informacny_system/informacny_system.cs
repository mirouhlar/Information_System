using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Types;
using System.Globalization;

namespace Informacny_system
{
    public partial class informacny_system : Form
    {
        OracleConnection connection;
        String name;
        String surname;
        String position;
        DataTable dt = new DataTable();
        DataTable dt_sklad_prod = new DataTable();
        DataTable dt_sklad_mat = new DataTable();

        Int32 selected_product;
        Int32 selected_customer_cart;
        Int32 selected_product_sklad;
        Int32 selected_mat_sklad;


        public informacny_system()
        {
            InitializeComponent();
        }
        public informacny_system(OracleConnection connection, String name, String surname, String position)
        {
            InitializeComponent();
            this.name = name;
            this.surname = surname;
            this.connection = connection;
            this.position = position;
        }

        private void informacny_system_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            user.Text = "Prihlásený: " + name + " " + surname;
            pos.Text = "Pozícia: " + position;
            dt.Clear();
            dt.Columns.Add("ID_produktu", typeof(int));
            dt.Columns.Add("Produkt", typeof(string));
            dt.Columns.Add("Množstvo", typeof(int));
            dt.Columns.Add("Cena", typeof(float));


            dt_sklad_prod.Clear();
            dt_sklad_prod.Columns.Add("ID_produktu", typeof(int));
            dt_sklad_prod.Columns.Add("Produkt", typeof(string));
            dt_sklad_prod.Columns.Add("Množstvo", typeof(int));

            dt_sklad_mat.Clear();
            dt_sklad_mat.Columns.Add("ID_produktu", typeof(int));
            dt_sklad_mat.Columns.Add("Nazov", typeof(string));
            dt_sklad_mat.Columns.Add("Množstvo", typeof(int));
            dt_sklad_mat.Columns.Add("Jednotka", typeof(string));

            dataGridView2.DataSource = dt_sklad_prod;
            dataGridView5.DataSource = dt;
            dataGridView10.DataSource = dt_sklad_mat;




            switch (position)
            {
               case "Obsluha":
                    tabControl1.TabPages.Remove(obpz);
                    tabControl1.TabPages.Remove(pouzivatelia);
                    tabControl1.TabPages.Remove(zakaznici);
                    groupBox3.Hide();
                    groupBox11.Hide();
                    groupBox14.Hide();
                    groupBox8.Hide();
                    groupBox9.Hide();
                    tabControl4.TabPages.Remove(skladv);
                    break;
                case "Vedúci výroby":
                    tabControl2.TabPages.Remove(vobpz);
                    tabControl1.TabPages.Remove(pouzivatelia);
                    tabControl1.TabPages.Remove(zakaznici);
                    groupBox6.Hide();
                    groupBox13.Hide();
                    groupBox3.Hide();
                    break;
                case "Vedúci skladov":
                    tabControl1.TabPages.Remove(pouzivatelia);
                    tabControl1.TabPages.Remove(zakaznici);
                    tabControl2.TabPages.Remove(vobpz);
                    groupBox3.Hide();
                    break;
                case "Ekonóm":
                    groupBox8.Hide();
                    groupBox9.Hide();
                    groupBox7.Hide();
                    groupBox10.Hide();
                    groupBox2.Hide();
                    break;
                case "Obchodné oddelenie":
                    groupBox8.Hide();
                    groupBox9.Hide();
                    groupBox7.Hide();
                    groupBox10.Hide();
                    groupBox2.Hide();
                    break;
                case "Skladník":
                    tabControl1.TabPages.Remove(obpz);
                    tabControl1.TabPages.Remove(pouzivatelia);
                    tabControl1.TabPages.Remove(zakaznici);
                    groupBox3.Hide();
                    groupBox11.Hide();
                    groupBox14.Hide();

                    break;
                case "Administrátor":
                    break;
                case "Riaditeľ":
                    break;
                default:
                    tabControl1.Hide();
                    MessageBox.Show("Chyba, kontaktuje administrátora!", "POZOR");
                    break;
            }
            }

      

        private void button10_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            if (connection.State == ConnectionState.Open)
            {
                OracleCommand command = connection.CreateCommand();

                command.CommandText = "select * from zamestnanci";

                DataTable data = new DataTable();
                using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.Fill(data);
                }

                userstab.DataSource = data;
                command.Dispose();
                connection.Close();
            }

        }



        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            if (!String.IsNullOrEmpty(meno.Text) && !String.IsNullOrEmpty(priezvisko.Text) && !String.IsNullOrEmpty(adresa.Text) && !String.IsNullOrEmpty(cislo.Text) && !String.IsNullOrEmpty(email.Text) && !String.IsNullOrEmpty(cislo.Text) && !String.IsNullOrEmpty(prihl_meno.Text))
            {
                if (connection.State == ConnectionState.Open && oprtn.SelectedIndex == 1 && heslo.Text == potvr_heslo.Text && !String.IsNullOrEmpty(heslo.Text))
                {


                    OracleCommand command = connection.CreateCommand();

                    command.CommandText = "insert into zamestnanci(meno,priezvisko,pozicia,adresa,cislo,email,prihl_meno,heslo) values('" + meno.Text + "','" + priezvisko.Text + "','" + pozicia.SelectedItem + "','" + adresa.Text + "','" + cislo.Text + "','" + email.Text + "','" + prihl_meno.Text + "','" + heslo.Text + "')";

                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                    MessageBox.Show("Používateľ vytvorený!","Oznam");
                    meno.Text = "";
                    priezvisko.Text = "";
                    adresa.Text = "";
                    cislo.Text = "";
                    email.Text = "";
                    prihl_meno.Text = "";
                    heslo.Text = "";
                    potvr_heslo.Text = "";
                }
                else if (connection.State == ConnectionState.Open && oprtn.SelectedIndex == 0 && pozicia.SelectedItem != null)
                {

                    OracleCommand command = connection.CreateCommand();

                    Int32 selected_employee = System.Convert.ToInt32(userstab.Rows[userstab.CurrentRow.Index].Cells[0].Value);

                    command.CommandText = "update zamestnanci set zamestnanci.meno ='" + meno.Text + "',zamestnanci.pozicia = '" + pozicia.SelectedItem.ToString() + "',zamestnanci.priezvisko = '" + priezvisko.Text + "', zamestnanci.adresa = '" + adresa.Text + "', zamestnanci.cislo = '" + cislo.Text + "', zamestnanci.prihl_meno = '" + prihl_meno.Text + "',zamestnanci.email ='" + email.Text + "' where zamestnanci.id = " + selected_employee.ToString();
                    command.ExecuteNonQuery();

                    if (checkBox2.Checked == true && heslo.Text == potvr_heslo.Text & !String.IsNullOrEmpty(heslo.Text))
                    {
                        command.CommandText = "update zamestnanci set heslo = '" + heslo.Text + "' where id = " + selected_employee.ToString();
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Používateľ upravený!", "Oznam");
                    meno.Text = "";
                    priezvisko.Text = "";
                    adresa.Text = "";
                    cislo.Text = "";
                    email.Text = "";
                    prihl_meno.Text = "";
                    heslo.Text = "";
                    potvr_heslo.Text = "";
                    pozicia.SelectedItem = null;
                    oprtn.SelectedItem = null;
                    command.Dispose();
                    connection.Close();

                }
            }
            else
            {
                MessageBox.Show("Skontorluj všetky polia!", "Oznam");
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            meno.Text = "";
            priezvisko.Text = "";
            adresa.Text = "";
            cislo.Text = "";
            email.Text = "";
            prihl_meno.Text = "";
            heslo.Text = "";
            potvr_heslo.Text = "";
        }



        private void userstab_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.userstab.Rows[userstab.CurrentRow.Index];
                meno.Text = row.Cells[1].Value.ToString();
                priezvisko.Text = row.Cells[2].Value.ToString();
                adresa.Text = row.Cells[4].Value.ToString();
                cislo.Text = row.Cells[5].Value.ToString();
                email.Text = row.Cells[6].Value.ToString();
                prihl_meno.Text = row.Cells[7].Value.ToString();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Odstrániť zamestnanca?", "Varovanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                if (connection.State == ConnectionState.Open)
                {
                    OracleCommand command = connection.CreateCommand();
                    Int32 selected_employee = System.Convert.ToInt32(userstab.Rows[userstab.CurrentRow.Index].Cells[0].Value);
                    command.CommandText = "delete from zamestnanci where id = " + selected_employee.ToString() + "";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                    userstab.Rows.RemoveAt(userstab.CurrentRow.Index);
                    userstab.Refresh();

                    meno.Text = "";
                    priezvisko.Text = "";
                    adresa.Text = "";
                    cislo.Text = "";
                    email.Text = "";
                    prihl_meno.Text = "";
                    heslo.Text = "";
                    potvr_heslo.Text = "";
                }
                else
                {
                    MessageBox.Show("Problém s databázou", "Varovanie");
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                heslo.Visible = true;
                potvr_heslo.Visible = true;
                label17.Visible = true;
                label18.Visible = true;
            }
            else
            {
                heslo.Visible = false;
                potvr_heslo.Visible = false;
                label17.Visible = false;
                label18.Visible = false;
            }
        }


        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView6.Rows[dataGridView6.CurrentRow.Index];
                if (!String.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                {
                    label5.Text = row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString();
                    selected_customer_cart = System.Convert.ToInt32(row.Cells[0].Value);
                }
                else
                {
                    label5.Text = "Pozor nie je zvolený zákazník!";
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();

            command.CommandText = "select * from zakaznici";

            DataTable data = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data);
            }

            customers2.DataSource = data;
            command.Dispose();
            connection.Close();
        }

        private void customers2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = customers2.Rows[customers2.CurrentRow.Index];
                namec.Text = row.Cells[1].Value.ToString();
                surnamec.Text = row.Cells[2].Value.ToString();
                addrc.Text = row.Cells[3].Value.ToString();
                numc.Text = row.Cells[4].Value.ToString();
                emailc.Text = row.Cells[5].Value.ToString();

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            if (!String.IsNullOrEmpty(namec.Text) && !String.IsNullOrEmpty(surnamec.Text) && !String.IsNullOrEmpty(addrc.Text) && !String.IsNullOrEmpty(numc.Text) && !String.IsNullOrEmpty(emailc.Text))
            {
                if (connection.State == ConnectionState.Open && opt.SelectedIndex == 1)
                {
                    OracleCommand command = connection.CreateCommand();

                    command.CommandText = "insert into zakaznici(meno,priezvisko,adresa,cislo,email) values('" + namec.Text + "','" + surnamec.Text + "','" + addrc.Text + "','" + numc.Text + "','" + emailc.Text + "')";

                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                    MessageBox.Show("Zákazník pridaný", "Okno");
                }
                else if (connection.State == ConnectionState.Open && opt.SelectedIndex == 0)
                {

                    OracleCommand command = connection.CreateCommand();

                    Int32 selected_customer = System.Convert.ToInt32(customers2.Rows[customers2.CurrentRow.Index].Cells[0].Value);
                    command.CommandText = "update zakaznici set meno='" + namec.Text + "',priezvisko = '" + surnamec.Text + "',adresa='" + addrc.Text + "',cislo='" + numc.Text + "',email='" + emailc.Text + "' where id = " + selected_customer;
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                    MessageBox.Show("Zákazník upravený", "Okno");


                }
                else
                {
                    MessageBox.Show("Správne vyplnte polia!", "Varovanie");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Odstrániť zákazníka?", "Varovanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                if (connection.State == ConnectionState.Open)
                {
                    OracleCommand command = connection.CreateCommand();
                    Int32 selected_customer = System.Convert.ToInt32(customers2.Rows[customers2.CurrentRow.Index].Cells[0].Value);
                    command.CommandText = "delete from zakaznici where id = " + selected_customer + "";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();

                    namec.Text = "";
                    surnamec.Text = "";
                    addrc.Text = "";
                    numc.Text = "";
                    emailc.Text = "";
                }
                else
                {
                    MessageBox.Show("Problém s databázou", "Varovanie");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            namec.Text = "";
            surnamec.Text = "";
            addrc.Text = "";
            numc.Text = "";
            emailc.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();

            command.CommandText = "select * from produkty";

            DataTable data = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data);
            }

            dataGridView4.DataSource = data;
            command.Dispose();
            connection.Close();
        }


        private void dataGrid4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView4.Rows[dataGridView4.CurrentRow.Index];

                if (!String.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                {
                    prod.Text = row.Cells[1].Value.ToString();
                    selected_product = System.Convert.ToInt32(row.Cells[0].Value);
                }
                else
                {
                    prod.Text = "Pozor nie je zvolený produkt!";
                }

            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (prod.Text != "Pozor nie je zvolený produkt!" && prod.Text != " " && prod.Text != null && mnozstvo.Value > 0)
            {
                
                DataRow[] rows = dt.Select("ID_produktu = '" + selected_product + "'");
                if (rows.Length > 0)
                {
                    int index = dt.Rows.IndexOf(rows[0]);
                    dt.Rows[index]["Cena"] = float.Parse(dt.Rows[index]["Cena"].ToString()) + (float.Parse((string)dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[3].Value.ToString()) * Convert.ToInt32(mnozstvo.Value.ToString()));
                    dt.Rows[index]["Množstvo"] = System.Convert.ToInt32(dt.Rows[index]["Množstvo"].ToString()) + System.Convert.ToInt32(mnozstvo.Value.ToString());
                }
                else
                {
                    DataRow r = dt.NewRow();
                    r["ID_produktu"] = System.Convert.ToInt32(selected_product);
                    r["Produkt"] = prod.Text;
                    r["Množstvo"] = System.Convert.ToInt32(mnozstvo.Value.ToString());
                    r["Cena"] = float.Parse((string)dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[3].Value.ToString()) * Convert.ToInt32(mnozstvo.Value.ToString());
                    dt.Rows.Add(r);
                }

                celkova_cena.Text = dt.Compute("Sum(Cena)", string.Empty).ToString();
                dataGridView5.Refresh();
                mnozstvo.Value = 0;

            }
        }

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();

            command.CommandText = "select * from produkty";

            DataTable data1 = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data1);
            }

            dataGridView4.DataSource = data1;
            data1.Dispose();
            DataTable data2 = new DataTable();

            command.CommandText = "select * from zakaznici";

            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data2);
            }

            dataGridView6.DataSource = data2;
            data2.Dispose();
            command.Dispose();
            connection.Close();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (dataGridView5.CurrentRow != null)
            {
                DataRow dr = dt.Rows[dataGridView5.CurrentRow.Index];
                dr.Delete();
                dt.AcceptChanges();
                dataGridView5.Refresh();
                celkova_cena.Text = dt.Compute("Sum(Cena)", string.Empty).ToString();
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {

            
                if (dataGridView5.Rows.Count != 0 && label5.Text != " " && label5.Text != "Pozor nie je zvolený zákazník!" && !String.IsNullOrEmpty(dataGridView6.Rows[dataGridView6.CurrentRow.Index].Cells[0].Value.ToString()))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (OracleCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO objednavky(zakaznici_id, stav, datum, suma) VALUES ("+selected_customer_cart.ToString()+ ",'Evidovaná','" + DateTime.Now.ToString("dd-MMM-yyyy") + "'," + float.Parse(celkova_cena.Text).ToString(CultureInfo.CreateSpecificCulture("en-GB")) + ") RETURNING id INTO :my_id_param";
                    OracleParameter outputParameter = new OracleParameter("my_id_param", OracleDbType.Decimal);
                    outputParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParameter);
                    cmd.ExecuteNonQuery();
                    decimal info = (decimal)(OracleDecimal)cmd.Parameters["my_id_param"].Value;

                    foreach (DataRow dsRow in dt.Rows)
                    {
                        cmd.CommandText = "INSERT INTO detail(objednavky_id, produkty_id,mnozstvo) VALUES (" + info.ToString() + ","+dsRow["ID_produktu"].ToString() + "," + dsRow["Množstvo"].ToString() + ")";
                        cmd.ExecuteNonQuery();

                    }

                    cmd.Dispose();
                    connection.Close();
                    dt.Clear();
                    dataGridView5.Refresh();
                    celkova_cena.Text = "0";
                    MessageBox.Show("Objednávka odoslaná", "Oznam");
                }
            }
            else
            {
                MessageBox.Show("Skontroluj všetky polia!", "POZOR!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();
            if (comboBox2.SelectedItem != null)
            {
                if (comboBox2.SelectedItem.ToString() == "Všetky")
                {
                    command.CommandText = "select objednavky.id, zakaznici.meno || ' ' || zakaznici.priezvisko as meno ,objednavky.stav,objednavky.suma,objednavky.datum from objednavky inner join zakaznici on objednavky.zakaznici_id = zakaznici.id ";
                }
                else
                {
                    command.CommandText = "select objednavky.id, zakaznici.meno || ' ' || zakaznici.priezvisko as meno ,objednavky.stav,objednavky.suma,objednavky.datum from objednavky inner join zakaznici on objednavky.zakaznici_id = zakaznici.id where datum between '" + dateTimePicker1.Value.ToString("dd-MMM-yyyy") + "' and '" + dateTimePicker2.Value.ToString("dd-MMM-yyyy") + "' and stav = '" + comboBox2.SelectedItem.ToString() + "' ";
                }


                DataTable data = new DataTable();
                using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.Fill(data);
                }

                dataGridView1.DataSource = data;
                command.Dispose();
                connection.Close();
            }
            else
            {
                MessageBox.Show("Vyplň všetky potrebné polia", "POZOR");
            }
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index];
                if (!String.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                {
                    Int32 selected_order = System.Convert.ToInt32(row.Cells[0].Value.ToString());
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    OracleCommand command = connection.CreateCommand();

                    command.CommandText = "select produkty.nazov, produkty.cena as cena_za_kus, detail.mnozstvo from produkty inner join detail on produkty.id = detail.produkty_id where detail.OBJEDNAVKY_ID = " + selected_order.ToString() + " ";


                    DataTable data1 = new DataTable();
                    using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                    {
                        dataAdapter.SelectCommand = command;
                        dataAdapter.Fill(data1);
                    }

                    dataGridView7.DataSource = data1;
                    data1.Dispose();
                }
                else
                {
                    dataGridView7.DataSource = null;
                    dataGridView7.Refresh();
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null) {
                if (MessageBox.Show("Odstrániť objednávku?", "Varovanie!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Int32 index_of_order = System.Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "delete from detail where objednavky_id = " + index_of_order.ToString() + "";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "delete from objednavky where id = " + index_of_order.ToString() + "";
                        cmd.ExecuteNonQuery();


                        cmd.Dispose();
                        connection.Close();
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                        dataGridView1.Refresh();
                        dataGridView7.DataSource = null;
                        dataGridView7.Refresh();
                        MessageBox.Show("Objednávka odstránená", "Oznam");
                    }
                }
            }
        }



        private void button20_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            if (!String.IsNullOrEmpty(nazov.Text) && !String.IsNullOrEmpty(popis.Text) && !String.IsNullOrEmpty(cena1.Text))
            {
                //OracleCommand command = connection.CreateCommand();

                if (connection.State == ConnectionState.Open && comboBox3.SelectedIndex == 1)
                {
                    OracleCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO produkty(nazov,popis,cena) values('" + nazov.Text + "','" + popis.Text + "'," + cena1.Text+") RETURNING id INTO :idd";
                    OracleParameter outputParameter = new OracleParameter("id", OracleDbType.Decimal);
                    outputParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParameter);
                    command.ExecuteNonQuery();
                    decimal info = (decimal)(OracleDecimal)command.Parameters["id"].Value;
                    //                    command.ExecuteNonQuery();
                    //                  command.CommandText = "insert into sklad_produktov(nazov,popis,cena) values('" + nazov.Text + "','" + popis.Text + "','" + float.Parse(cena.Text).ToString() + "')";
                    command.CommandText = "INSERT INTO sklad_produktov(id_produktu, pocet_kusov) VALUES (" + info.ToString() + ",0)";

                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                    MessageBox.Show("Produkt bol pridaný", "Okno");
                }
                else if (connection.State == ConnectionState.Open && comboBox3.SelectedIndex == 0 && dataGridView8.CurrentRow != null)
                {

                   OracleCommand command = connection.CreateCommand();

                    Int32 selected_prod = System.Convert.ToInt32(dataGridView8.Rows[dataGridView8.CurrentRow.Index].Cells[0].Value);
                    command.CommandText = "update produkty set produkty.nazov='" + nazov.Text + "',produkty.popis = '" + popis.Text + "',produkty.cena=" + cena1.Text + " where produkty.id = "+ selected_prod.ToString();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                    MessageBox.Show("Produkt upravený", "Okno");
                }

                else
                {
                    MessageBox.Show("Správne vyplnte polia!", "Varovanie");
                    connection.Close();
                }
            }
        }

        private void dataGridView8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = dataGridView8.Rows[dataGridView8.CurrentRow.Index];
                nazov.Text = row.Cells[1].Value.ToString();
                popis.Text = row.Cells[2].Value.ToString();
                cena1.Text = row.Cells[3].Value.ToString().Replace(",",".");
                

            }
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            nazov.Text = " ";
            popis.Text = " ";
            cena1.Text = " ";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (dataGridView8.CurrentRow != null)
            {
                if (MessageBox.Show("Odstrániť produkt?", "Varovanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    if (connection.State == ConnectionState.Open)
                    {
                        OracleCommand command = connection.CreateCommand();
                        Int32 selected_pr = System.Convert.ToInt32(dataGridView8.Rows[dataGridView8.CurrentRow.Index].Cells[0].Value);



                        command.CommandText = "delete from sklad_produktov where id_produktu = " + selected_pr.ToString();
                        command.ExecuteNonQuery();
                        command.CommandText = "delete from produkty where produkty.id = " + selected_pr.ToString();
                        command.ExecuteNonQuery();
                        dataGridView8.Rows.RemoveAt(dataGridView8.CurrentRow.Index);
                        dataGridView8.Refresh();
                        command.Dispose();
                        connection.Close();

                        nazov.Text = " ";
                        popis.Text = " ";
                        cena1.Text = " ";
                        MessageBox.Show("Produkt zmazaný!", "Varovanie");

                    }
                    else
                    {
                        MessageBox.Show("Problém s databázou", "Varovanie");
                    }
                }
                else
                {
                    MessageBox.Show("Zvoľ produkt!", "Varovanie");
                }
            }
           
        }



        private void button4_Click_1(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();

            command.CommandText = "select * from produkty";

            DataTable data = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data);
            }

            dataGridView8.DataSource = data;
            command.Dispose();
            connection.Close();
        }

        private void dataGridView12_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView12.Rows[dataGridView12.CurrentRow.Index];

                if (!String.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                {
                    label38.Text = row.Cells[1].Value.ToString();
                    selected_product_sklad = System.Convert.ToInt32(row.Cells[0].Value);
                }
                else
                {
                    label38.Text = "Pozor nie je zvolený produkt!";
                }

            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (label38.Text != "Pozor nie je zvolený produkt!" && label38.Text != " " && label38.Text != null && numericUpDown8.Value > 0)
            {

                DataRow[] rows = dt_sklad_prod.Select("ID_produktu = '" + selected_product_sklad + "'");
                if (rows.Length > 0)
                {
                    int index = dt_sklad_prod.Rows.IndexOf(rows[0]);
                    dt_sklad_prod.Rows[index]["Množstvo"] = System.Convert.ToInt32(dt_sklad_prod.Rows[index]["Množstvo"].ToString()) + System.Convert.ToInt32(numericUpDown8.Value.ToString());
                }
                else
                {
                    DataRow r = dt_sklad_prod.NewRow();
                    r["ID_produktu"] = System.Convert.ToInt32(selected_product_sklad);
                    r["Produkt"] = label38.Text;
                    r["Množstvo"] = System.Convert.ToInt32(numericUpDown8.Value.ToString());
                    dt_sklad_prod.Rows.Add(r);
                }

                dataGridView2.Refresh();
                numericUpDown8.Value = 0;

            }
        }

        private void tabControl4_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();

            command.CommandText = "select * from produkty";

            DataTable data1 = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data1);
            }

            dataGridView12.DataSource = data1;
            data1.Dispose();
         
            command.Dispose();
            connection.Close();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();

            command.CommandText = "select produkty.id, produkty.nazov, produkty.popis, produkty.cena, sklad_produktov.pocet_kusov from produkty inner join sklad_produktov on produkty.id = sklad_produktov.id_produktu";

            DataTable data1 = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data1);
            }

            dataGridView13.DataSource = data1;
            data1.Dispose();

            command.Dispose();
            connection.Close();
        }

        private void button27_Click(object sender, EventArgs e)
        {

            if (dataGridView2.Rows.Count != 0 && label38.Text != " " && label38.Text != "Pozor nie je zvolený produkt!" )
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (OracleCommand cmd = connection.CreateCommand())
                {

                    foreach (DataRow dsRow in dt_sklad_prod.Rows)
                    {
                      //  cmd.CommandText = "INSERT INTO detail(objednavky_id, produkty_id,mnozstvo) VALUES (" + info.ToString() + "," + dsRow["ID_produktu"].ToString() + "," + dsRow["Množstvo"].ToString() + ")";
                        cmd.CommandText = "Update sklad_produktov set sklad_produktov.pocet_kusov = sklad_produktov.pocet_kusov+" + dsRow["Množstvo"].ToString() + " where sklad_produktov.id_produktu = " + dsRow["ID_produktu"].ToString();
                        cmd.ExecuteNonQuery();

                    }
                    if (dt_sklad_prod.Rows.Count == 0)
                    {
                        MessageBox.Show("Žiaden produkt na naskladnenie!", "Oznam");
                    }
                    else
                    {
                        MessageBox.Show("Produkty naskladnené!", "Oznam");
                    }
                    cmd.Dispose();
                    connection.Close();
                    dt_sklad_prod.Clear();
                    dataGridView2.Refresh();

                    
                }
            }
            else
            {
                MessageBox.Show("Skontroluj všetky polia!", "POZOR!");
            }
        }

        private void cena1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count != 0 && label38.Text != " " && label38.Text != "Pozor nie je zvolený produkt!")
            {
               // dataGridView2.DataSource = dt_sklad_prod;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (OracleCommand cmd = connection.CreateCommand())
                {
                    String sprava;
                    DataTable temp = new DataTable();
                    temp.Clear();
                    temp.Columns.Add("ID_produktu", typeof(int));
                    temp.Columns.Add("Produkt", typeof(string));
                    temp.Columns.Add("Množstvo", typeof(int));

                    foreach (DataRow dsRow in dt_sklad_prod.Rows)
                    {
                        cmd.CommandText = "select * from sklad_produktov where id =" + dsRow["ID_produktu"].ToString();

                        OracleDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                           Int32 rozdiel =  reader.GetInt32(2) - (Int32)dsRow["Množstvo"];
                            if (rozdiel < 0)
                            {
                                temp.Rows.Add(dsRow.ItemArray);
                            }
                            else
                            {
                                //  cmd.CommandText = "INSERT INTO detail(objednavky_id, produkty_id,mnozstvo) VALUES (" + info.ToString() + "," + dsRow["ID_produktu"].ToString() + "," + dsRow["Množstvo"].ToString() + ")";
                                cmd.CommandText = "Update sklad_produktov set sklad_produktov.pocet_kusov = sklad_produktov.pocet_kusov-" + dsRow["Množstvo"].ToString() + " where sklad_produktov.id_produktu = " + dsRow["ID_produktu"].ToString();
                                cmd.ExecuteNonQuery();

                            }

                        }
                    }
                    if (dt_sklad_prod.Rows.Count == 0)
                    {
                        sprava = "Žiaden produkt na vyskladnenie!";
                    }
                    else if(temp.Rows.Count > 0)
                    {
                        sprava = "Zostávajúcich produktov je nedostatok!";
                    }
                    else
                    {
                        sprava = "Produkty vyskladnené!";
                    }
                    MessageBox.Show(sprava, "Oznam");

                    dt_sklad_prod.Clear();

                    foreach (DataRow dr in temp.Rows)
                    {
                        dt_sklad_prod.Rows.Add(dr.ItemArray);
                    }
                   
                 //   dataGridView2.DataSource = temp;
                    dataGridView2.Refresh();
                    cmd.Dispose();
                    connection.Close();
                   
                   
                }
            }
            else
            {
                MessageBox.Show("Skontroluj všetky polia!", "POZOR!");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                DataRow dr = dt_sklad_prod.Rows[dataGridView2.CurrentRow.Index];
                dr.Delete();
                dt_sklad_prod.AcceptChanges();
                dataGridView2.Refresh();
                
            }
        }

        private void tabControl4_Selected(object sender, TabControlEventArgs e)
        {

        }

        private void tabControl4_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();

            command.CommandText = "select * from produkty";

            DataTable data1 = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data1);
            }

            dataGridView12.DataSource = data1;
            data1.Dispose();

            command.CommandText = "select id, nazov, merna_jednotka from sklad_materialu";

            DataTable data2 = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data2);
            }

            dataGridView11.DataSource = data2;
            data2.Dispose();

            command.Dispose();
            connection.Close();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();

            command.CommandText = "select * from sklad_materialu";

            DataTable data1 = new DataTable();
            using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data1);
            }

            dataGridView9.DataSource = data1;
            data1.Dispose();

            command.Dispose();
            connection.Close();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (label52.Text != "Pozor nie je zvolený materiál!" && label52.Text != " " && label52.Text != null && numericUpDown9.Value > 0)
            {

                DataRow[] rows = dt_sklad_mat.Select("ID_produktu = '" + selected_mat_sklad + "'");
                if (rows.Length > 0)
                {
                    int index = dt_sklad_mat.Rows.IndexOf(rows[0]);
                    dt_sklad_mat.Rows[index]["Množstvo"] = System.Convert.ToInt32(dt_sklad_mat.Rows[index]["Množstvo"].ToString()) + System.Convert.ToInt32(numericUpDown9.Value.ToString());
                }
                else
                {
                    DataRow r = dt_sklad_mat.NewRow();
                    r["ID_produktu"] = System.Convert.ToInt32(selected_mat_sklad);
                    r["Nazov"] = label52.Text;
                    r["Množstvo"] = System.Convert.ToInt32(numericUpDown9.Value.ToString());
                    r["Jednotka"] = "ks";
                    dt_sklad_mat.Rows.Add(r);
                }

                dataGridView10.Refresh();
                numericUpDown9.Value = 0;

            }
        }

        private void dataGridView11_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView11.Rows[dataGridView11.CurrentRow.Index];

                if (!String.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                {
                    label52.Text = row.Cells[1].Value.ToString();
                    selected_mat_sklad = System.Convert.ToInt32(row.Cells[0].Value);
                }
                else
                {
                    label52.Text = "Pozor nie je zvolený materiál!";
                }

            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (dataGridView10.Rows.Count != 0 && label52.Text != " " && label52.Text != "Pozor nie je zvolený materiál!")
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (OracleCommand cmd = connection.CreateCommand())
                {

                    foreach (DataRow dsRow in dt_sklad_mat.Rows)
                    {
                        //  cmd.CommandText = "INSERT INTO detail(objednavky_id, produkty_id,mnozstvo) VALUES (" + info.ToString() + "," + dsRow["ID_produktu"].ToString() + "," + dsRow["Množstvo"].ToString() + ")";
                        cmd.CommandText = "Update sklad_materialu set pocet_kusov = pocet_kusov+" + dsRow["Množstvo"].ToString() + " where id = " + dsRow["ID_produktu"].ToString();
                        cmd.ExecuteNonQuery();

                    }
                    if (dt_sklad_mat.Rows.Count == 0)
                    {
                        MessageBox.Show("Žiaden materiál na naskladnenie!", "Oznam");
                    }
                    else
                    {
                        MessageBox.Show("Materiál naskladnený!", "Oznam");
                    }
                    cmd.Dispose();
                    connection.Close();
                    dt_sklad_mat.Clear();
                    dataGridView10.Refresh();


                }
            }
            else
            {
                MessageBox.Show("Skontroluj všetky polia!", "POZOR!");
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (dataGridView10.CurrentRow != null)
            {
                DataRow dr = dt_sklad_mat.Rows[dataGridView10.CurrentRow.Index];
                dr.Delete();
                dt_sklad_mat.AcceptChanges();
                dataGridView10.Refresh();

            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (dataGridView10.Rows.Count != 0 && label52.Text != " " && label52.Text != "Pozor nie je zvolený materiál!")
            {

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (OracleCommand cmd = connection.CreateCommand())
                {
                    String sprava;
                    DataTable temp = new DataTable();
                    temp.Clear();

                    temp.Columns.Add("ID_produktu", typeof(int));
                    temp.Columns.Add("Nazov", typeof(string));
                    temp.Columns.Add("Množstvo", typeof(int));
                    temp.Columns.Add("Jednotka", typeof(string));

                    foreach (DataRow dsRow in dt_sklad_mat.Rows)
                    {
                        cmd.CommandText = "select * from sklad_materialu where id =" + dsRow["ID_produktu"].ToString();

                        OracleDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Int32 rozdiel = reader.GetInt32(2) - (Int32)dsRow["Množstvo"];
                            if (rozdiel < 0)
                            {
                                temp.Rows.Add(dsRow.ItemArray);
                            }
                            else
                            {
                                //  cmd.CommandText = "INSERT INTO detail(objednavky_id, produkty_id,mnozstvo) VALUES (" + info.ToString() + "," + dsRow["ID_produktu"].ToString() + "," + dsRow["Množstvo"].ToString() + ")";
                                cmd.CommandText = "Update sklad_materialu set pocet_kusov = pocet_kusov-" + dsRow["Množstvo"].ToString() + " where id = " + dsRow["ID_produktu"].ToString();
                                cmd.ExecuteNonQuery();

                            }

                        }
                    }
                    if (dt_sklad_mat.Rows.Count == 0)
                    {
                        sprava = "Žiaden materiál na vyskladnenie!";
                    }
                    else if (temp.Rows.Count > 0)
                    {
                        sprava = "Zostávajúceho materiálu je nedostatok!";
                    }
                    else
                    {
                        sprava = "Materiál vyskladnený!";
                    }
                    MessageBox.Show(sprava, "Oznam");

                    dt_sklad_mat.Clear();

                    foreach (DataRow dr in temp.Rows)
                    {
                        dt_sklad_mat.Rows.Add(dr.ItemArray);
                    }

                    //   dataGridView2.DataSource = temp;
                    dataGridView10.Refresh();
                    cmd.Dispose();
                    connection.Close();


                }
            }
            else
            {
                MessageBox.Show("Skontroluj všetky polia!", "POZOR!");
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OracleCommand command = connection.CreateCommand();
            if (comboBox1.SelectedItem != null)
            {
                if (comboBox1.SelectedItem.ToString() == "Všetky")
                {
                    command.CommandText = "select id, mat1 as Vrece_25kg_ks, mat2 as Vrece_50kg_ks, mat3 as Kukurica_kg, mat4 as Psenica_kg, mat5 as Krmne_vapno_ks, mat6 as Rybia_mucka_kg, mat7 as Vitaminova_zmes_kg, datum as Datum_objednania, stav from objednavky_material";
                }
                else
                {
                    command.CommandText = "select id, mat1 as Vrece_25kg_ks, mat2 as Vrece_50kg_ks, mat3 as Kukurica_kg, mat4 as Psenica_kg, mat5 as Krmne_vapno_ks, mat6 as Rybia_mucka_kg, mat7 as Vitaminova_zmes_kg, datum as Datum_objednania, stav from objednavky_material where datum between '" + dateTimePicker4.Value.ToString("dd-MMM-yyyy") + "' and '" + dateTimePicker3.Value.ToString("dd-MMM-yyyy") + "' and stav = '" + comboBox1.SelectedItem.ToString() + "' "; 

                    // command.CommandText = "select objednavky.id, zakaznici.meno || ' ' || zakaznici.priezvisko as meno ,objednavky.stav,objednavky.suma,objednavky.datum from objednavky inner join zakaznici on objednavky.zakaznici_id = zakaznici.id where datum between '" + dateTimePicker1.Value.ToString("dd-MMM-yyyy") + "' and '" + dateTimePicker2.Value.ToString("dd-MMM-yyyy") + "' and stav = '" + comboBox2.SelectedItem.ToString() + "' ";
                }


                DataTable data = new DataTable();
                using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.Fill(data);
                }

                dataGridView3.DataSource = data;
                command.Dispose();
                connection.Close();
            }
            else
            {
                MessageBox.Show("Vyplň všetky potrebné polia", "POZOR");
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            if(connection.State == ConnectionState.Open && (numericUpDown10.Value != 0 || numericUpDown11.Value != 0 || numericUpDown12.Value != 0 || numericUpDown13.Value != 0 || numericUpDown14.Value != 0 || numericUpDown15.Value != 0 || numericUpDown16.Value != 0))
            {
                OracleCommand command = connection.CreateCommand();
                command.CommandText = "insert into objednavky_material(mat1,mat2,mat3,mat4,mat5,mat6,mat7,datum,stav) values ("+numericUpDown16.Value.ToString()+"," + numericUpDown10.Value.ToString() + "," + numericUpDown11.Value.ToString() + "," + numericUpDown12.Value.ToString() + "," + numericUpDown13.Value.ToString() + "," + numericUpDown14.Value.ToString() + "," + numericUpDown15.Value.ToString() + ",'" + DateTime.Now.ToString("dd-MMM-yyyy") + "','Evidovaná')";
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                MessageBox.Show("Objednávka odoslaná!","Oznam");
                numericUpDown10.Value = 0;
                numericUpDown11.Value = 0;
                numericUpDown12.Value = 0;
                numericUpDown13.Value = 0;
                numericUpDown14.Value = 0;
                numericUpDown15.Value = 0;
                numericUpDown16.Value = 0;
            }
            else
            {
                MessageBox.Show("Objednávka neodoslaná!\nŽiaden zvolený materiál!", "Oznam");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null)
            {
                if (MessageBox.Show("Odstrániť objednávku?", "Varovanie!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Int32 index_of_order = System.Convert.ToInt32(dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[0].Value);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "delete from objednavky_material where id = " + index_of_order.ToString() + "";
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        connection.Close();
                        dataGridView3.Rows.RemoveAt(dataGridView3.CurrentRow.Index);
                        dataGridView3.Refresh();
                        MessageBox.Show("Objednávka odstránená", "Oznam");
                    }
                }
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if (MessageBox.Show("Zmeniť stav objednávky ?", "Varovanie!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Int32 index_of_order = System.Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select * from objednavky where id=" + index_of_order.ToString();
                        OracleDataReader reader = cmd.ExecuteReader();
                        string stav = " ";
                        if (reader.Read())
                        {
                            stav = reader.GetString(2);

                        }
                        if (stav != "Vybavená")
                        {
                            cmd.CommandText = "update objednavky set stav = '" + comboBox4.SelectedItem.ToString() + "' where id = " + index_of_order.ToString() + "";
                            cmd.ExecuteNonQuery();

                            if (comboBox4.SelectedItem.ToString() == "Vybavená")
                            {
                                cmd.CommandText = "select * from detail where objednavky_id=" + index_of_order.ToString();
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Int32 produkt = (Int32)reader.GetInt32(1);
                                    Int32 mnozstvo = (Int32)reader.GetInt32(2);
                                    cmd.CommandText = "update sklad_produktov set pocet_kusov = pocet_kusov + " + mnozstvo.ToString() + "where id_produktu = " + produkt.ToString(); ;
                                    cmd.ExecuteNonQuery();

                                    reader.NextResult();
                                }
                            }
                            cmd.Dispose();

                            connection.Close();
                            MessageBox.Show("Stav objednávky bol zmenený", "Oznam");
                        }

                        else
                        {
                            MessageBox.Show("Objednávka je už vybavená!", "Oznam");
                        }
                    }
                }
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null)
            {
                if (MessageBox.Show("Zmeniť stav objednávky ?", "Varovanie!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Int32 index_of_order = System.Convert.ToInt32(dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[0].Value);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select * from objednavky_material where id=" + index_of_order.ToString();
                        OracleDataReader reader = cmd.ExecuteReader();
                        string stav = " ";
                        if (reader.Read())
                        {
                            stav = reader.GetString(9);
                            
                        }
                        if (stav != "Vybavená")
                        {

                            cmd.CommandText = "update objednavky_material set stav = '" + comboBox5.SelectedItem.ToString() + "' where id = " + index_of_order.ToString() + "";
                            cmd.ExecuteNonQuery();

                            if (comboBox5.SelectedItem.ToString() == "Vybavená")
                            {
                                cmd.CommandText = "select * from sklad_materialu where id=" + index_of_order.ToString();
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Int32 mnozstvo = (Int32)reader.GetInt32(2);
                                    cmd.CommandText = "update sklad_materialu set pocet_kusov = pocet_kusov + " + mnozstvo.ToString() + "where id = " + index_of_order.ToString();
                                    cmd.ExecuteNonQuery();

                                    reader.NextResult();
                                }
                            }
                            cmd.Dispose();

                            connection.Close();
                            MessageBox.Show("Stav objednávky bol zmenený", "Oznam");
                        }
                        else
                        {
                            MessageBox.Show("Objednávka je už vybavená!", "Oznam");
                        }
                    }
                }
            
            
                }
            }
        }
    }

