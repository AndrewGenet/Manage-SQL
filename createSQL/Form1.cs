using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.OleDb;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;


namespace createSQL
{
    // location of database files: C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("SELECT * FROM myTable;");
            comboBox1.SelectedItem = "SELECT * FROM myTable;";
            comboBox1.Items.Add("CREATE TABLE myTable ( primaryKey int IDENTITY(1,1) PRIMARY KEY, empId varchar(255) NOT NULL, lastName varchar(255), firstName varchar(255), mail varchar(255), title varchar(255), description varchar(255));");
            comboBox1.Items.Add("DROP TABLE myTable;");
            //comboBox1.Items.Add("INSERT INTO myTable (empId, lastName, firstName, mail, title, description) VALUES ('999999', 'Genet', 'Andrew', 'mymail@mail.com', 'Super cool dude', 'really super cool dude');");
            comboBox1.Items.Add("SELECT COUNT(primaryKey) FROM myTable;");
            label2.Text = "waiting...";
            textBox1.Text = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";
            //testDatabaseExists();

            label5.Text = "";

            groupBox4.Dock = DockStyle.Top;
            groupBox5.Dock = DockStyle.Fill;
            dataGridView1.Dock = DockStyle.Fill;
            groupBox6.Dock = DockStyle.Top;
            groupBox7.Dock = DockStyle.Fill;
            listBox1.Dock = DockStyle.Fill;

        }
        
        DataTable clearData = new DataTable();
        ToolTip tt = new ToolTip();

        private void button2_Click(object sender, EventArgs e)
        {
            tt.SetToolTip(label2, "");
            dataGridView1.DataSource = clearData;

            SqlConnection myConn = new SqlConnection(textBox1.Text);

            String qry = comboBox1.Text.ToString();

            SqlCommand myCommand = new SqlCommand(qry, myConn);

            try
            {
                myConn.Open();

                //Use the above SqlCommand object to create a SqlDataReader object.
                SqlDataReader rdr = myCommand.ExecuteReader();

                listBox1.Items.Add("Executed Command: " + qry);
                label2.Text = "Success";
                label2.BackColor = Color.LightGreen;
                timer1.Start();

                if (!comboBox1.Items.Contains(qry))
                {
                    comboBox1.Items.Add(qry);
                }
                
                //Use the DataTable.Load(SqlDataReader) function to put the results of the query into a DataTable.
                DataTable dataTable = new DataTable();
                dataTable.Load(rdr);
                dataGridView1.DataSource = dataTable;
            }
            catch (System.Exception ex)
            {
                listBox1.Items.Add("Failed Command: " + qry);
                label2.Text = "Failure";
                label2.BackColor = Color.Pink;
                tt.ShowAlways = false;
                tt.SetToolTip(label2, ex.ToString());
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Dispose();
                    myConn.Close();
                }
            } // end of try/catch
        }// end display qry btn click

        // extra stuff to make the badgeID part execute
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (swipeInput.TextLength > 34)
            {
                swipeInput.Enabled = false;
                //parseData();
                textClear.Start();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        string empID = "null";
        string lastName = "null";
        string firstName = "null";
        string mail = "null";
        string title = "null";
        string desc = "null";
        string domain = "Domain: ";
        string path = "LDAP://DC=,DC=,DC=,DC=";

        // extract employee ID
        //private void parseData()
        //{
        //    char[] delimiterChars = { '=', '?' };
        //    string text = swipeInput.Text;
        //    swipeInput.Clear();
        //    string[] words = text.Split(delimiterChars);
        //    int count = 0;

        //    foreach (string s in words)
        //    {
        //        count++;
        //        if (count == 3)
        //        {
        //            empID = s.ToString();
        //        }
        //    }
        //    count = 0;
        //    myData();
        //    label5.Text = empID + ", " + lastName + ", " + firstName + ", " + mail + ", " + title + ", " + desc;
        //    textBox2.Text = "INSERT INTO myTable (empId, lastName, firstName, mail, title, description) VALUES ('" + empID + "', '" + lastName + "', '" + firstName + "', '" + mail + "', '" + title + "', '" + desc + "');";
            
        //}

        // search active directory
        //private void myData()
        //{
        //    try
        //    {
        //        DirectoryEntry entry = new DirectoryEntry(path);

        //        //search for a DirectoryEntry based on employeeID
        //        DirectorySearcher search = new DirectorySearcher(entry);
        //        search.Filter = "(employeeID=" + empID + ")";

        //        SearchResult result = search.FindOne();

        //        // if didnt find anything
        //        if (result == null)
        //        {
        //            // and the path isnt lerner
        //            if (path != "LDAP://DC=,DC=,DC=,DC=")
        //            {
        //                //close the open search
        //                entry.Dispose();
        //                entry.Close();

        //                // change the path
        //                path = "LDAP://DC=,DC=,DC=,DC=";
        //                domain = "Domain: ";

        //                // and try again
        //                myData();
        //                return;
        //            }
                    
        //            entry.Dispose();
        //            entry.Close();
        //            return;
        //        }
        //        else
        //        {
        //            bool a = result.Properties.Contains("sn");
        //            bool b = result.Properties.Contains("givenname");
        //            bool c = result.Properties.Contains("mail");
        //            bool d = result.Properties.Contains("title");
        //            bool e = result.Properties.Contains("description");

        //            if (a != false)
        //            {
        //                lastName = result.Properties["sn"][0].ToString();
        //            }
        //            if (b != false)
        //            {
        //                firstName = result.Properties["givenname"][0].ToString();
        //            }
        //            if (c != false)
        //            {
        //                mail = result.Properties["mail"][0].ToString();
        //            }
        //            if (d != false)
        //            {
        //                title = result.Properties["title"][0].ToString();
        //            }
        //            if (e != false)
        //            {
        //                desc = result.Properties["description"][0].ToString();
        //            }
        //        }

        //        domain = "Domain: ";
        //        path = "LDAP://DC=,DC=,DC=,DC=";
        //        entry.Dispose();
        //        entry.Close();
        //        return;
        //    }
        //    catch
        //    {

        //    }
        //}

        // cosmetic solution to clearing out the data grid
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = clearData;
        }

        // turns the label back to normal
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            label2.Text = "waiting...";
            label2.BackColor = Color.Empty;
        }

        //not sure yet if this works
        public Boolean testDatabaseExists()
        {
            String connString = (textBox1.Text);
            String cmdText = ("select * from master.dbo.sysdatabases");
            Boolean bRet;

            System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(connString);
            System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(cmdText, sqlConnection);

            try
            {
                sqlConnection.Open();
                System.Data.SqlClient.SqlDataReader reader = sqlCmd.ExecuteReader();
                bRet = reader.HasRows;
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                bRet = false;
                sqlConnection.Close();
                MessageBox.Show(e.Message);
                return false;
            } //End Try Catch Block

            if (bRet == true)
            {
                MessageBox.Show("DATABASE EXISTS");
                return true;
            }
            else
            {
                MessageBox.Show("DATABASE DOES NOT EXIST");
                return false;
            } //END OF IF


        } //END FUNCTION

        private void textClear_Tick(object sender, EventArgs e)
        {
            swipeInput.Enabled = true;
            swipeInput.Clear();
        }
    }// end class
}// end namespace
