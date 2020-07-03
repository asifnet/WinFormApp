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
using System.Configuration;
using System.Drawing.Imaging;

namespace WF1
{

    public partial class Form1 : Form
    {
        string pid;
        string str;
        string items = string.Empty;
        private readonly string DR1;
        string rbdtext, Status;
        string SQLselect;

        public Form1()
        {
            InitializeComponent();
        }
        //  string pid;

        private void Form1_Load(object sender, EventArgs e)
        {


            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //            DataSet ds = new DataSet();


            DataGrid();
            comboBoxValues();

            //// get values from App Settings
            //var reader = new AppSettingsReader();
            //SQLselect = reader.GetValue("SQL", typeof(string));
            //var dateTimeSetting = reader.GetValue("DateSetting", typeof(DateTime));

            ////MessageBox.Show("DateSetting: " + dateTimeSetting);



        }




        private void button5_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            pid = textBox1.Text;
            ChkBoxes();
            SqlCommand cmd = new SqlCommand(" UPDATE Persons  SET firstname = '" + textBox2.Text + "',lastname = '" + textBox3.Text + "',address = '" + textBox4.Text + "' ,city = '" + textBox5.Text + "' ,Status = '" + Status + "' Where personID =" + pid, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            DataGrid();
            SelectData();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DataGrid();
            SelectData();
            
        }

        public void SelectData()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            pid = textBox1.Text;
            SqlCommand cmd = new SqlCommand(" Select * from Persons   Where personID =" + pid, conn);
            SqlDataReader DR1 = cmd.ExecuteReader();
            bool temp = false;
            Clear();
            if (DR1.Read())
            {
                textBox1.Text = DR1.GetValue(0).ToString();
                textBox2.Text = DR1.GetValue(1).ToString();
                textBox3.Text = DR1.GetValue(2).ToString();
                textBox4.Text = DR1.GetValue(3).ToString();
                textBox5.Text = DR1.GetValue(4).ToString();
                if (DR1.GetValue(5).ToString() == "M")
                  {
                    radioButton1.Checked = true;
                  }
                else if (DR1.GetValue(5).ToString() == "F")
                {
                    radioButton2.Checked = true;
                }

                if (DR1.GetValue(6).ToString() == "Top")
                {
                    checkBox1.Checked = true;
                }
                else if (DR1.GetValue(6).ToString() == "Bottom")
                {
                    checkBox2.Checked = true;
                }
                else if (DR1.GetValue(6).ToString() == "Right")
                {
                    checkBox3.Checked = true;
                }
                else if (DR1.GetValue(6).ToString() == "Left")
                {
                    checkBox4.Checked = true;
                }

                temp = true;
            }
            if (temp == false)
                MessageBox.Show("not found");
            //ClearTxtBoxes();

            conn.Close();
            DataGrid();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            pid = textBox1.Text;
            SqlCommand cmd = new SqlCommand(" delete from Persons   Where personID =" + pid, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            DataGrid();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            

            if (radioButton1.Checked)
            {
                rbdtext = "M";
            }
           if (radioButton2.Checked)
            {  
                rbdtext = "F";
            }

            ChkBoxes();

            SqlCommand cmd = new SqlCommand("Insert into Persons values( '" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + rbdtext + "','" + Status + "' )", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            DataGrid();

        }

        private void DataGrid() 
        {
            this.personsTableAdapter.Fill(this.masterDataSet.Persons);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            TreeNode parentNode = null;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * from Persons", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                parentNode = treeView1.Nodes.Add(dr["FirstName"].ToString());
                PopulateTreeView(Convert.ToInt32(dr["personid"].ToString()), parentNode);
            }
            //treeView1.ExpandAll();
            conn.Close();
        }

        private void PopulateTreeView(int parentId, TreeNode parentNode)
        {
            TreeNode childNode;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(" Select * from cars where pid=" + parentId, conn);

            DataTable dtchildc = new DataTable();
            da.Fill(dtchildc);

            foreach (DataRow dr1 in dtchildc.Rows)
            {
                childNode = parentNode.Nodes.Add(dr1["carname"].ToString());


            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox2.Text = treeView1.SelectedNode.Text.ToString();

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
               CheckAllChildNodes(e.Node, true);
            }
            else
            {
                UncheckNodes(e.Node, false);
            }

            textBox4.Text = "'" + e.Node.Text + "'";


        }


        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            //string items = string.Empty;
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
                if (node.Checked == true)
                    {
                        items += "'" + node.Text + "'" + ",";
                    }

                
                textBox5.Text = "'" + treeNode.Text + "'," +  items;

            }

         
        }
        private void UncheckNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                if (node.Checked = nodeChecked) node.Checked = false;
                UncheckNodes(node, nodeChecked);
            }
            textBox5.Text = "";
        }

        private void comboBoxValues()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            // get values from App Settings
            //var reader = new AppSettingsReader();
            //var SQLselect = reader.GetValue("SQL", typeof(string));

            var SQLselect = System.Configuration.ConfigurationManager.AppSettings["SQL"];


            SqlCommand cmd = new SqlCommand(SQLselect +pid, conn);
            SqlDataReader DR1 = cmd.ExecuteReader();

            while (DR1.Read())
            {
                comboBox1.Items.Add((DR1[1].ToString(), DR1[0].ToString()));

            }
            conn.Close();

            

        



              
               
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
              textBox1.Text = comboBox1.SelectedItem.ToString();
                     
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string checkvalues = "";
            string sep = "";
            foreach (Control c in Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox b = (CheckBox)c;
                    checkvalues = string.Format("{0}{1}{2}={3}", checkvalues, sep, b.Text, b.Checked);
                    sep = ";";

                    ListViewItem item = new ListViewItem(checkvalues);
                    listView1.Items.Add(item);
                }
            }


        }

        private void ChkBoxes()
        {
              if (checkBox1.Checked == true)
            {
                Status = "Top";
            }
            else if (checkBox2.Checked == true)
            {
                Status = "Bottom";
            }

            else if (checkBox4.Checked == true)
            {
                Status = "Left";
            }
            else if (checkBox3.Checked == true)
            {
                Status = "Right";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;

        }

    }

       
    }

        
  
    

                
     



