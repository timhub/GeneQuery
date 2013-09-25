using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQLDriverCS;
using CommonMysql;
namespace sqlinsert
{
    public partial class Form1 : Form
    {
        MySQLConnection conn = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BullAction ba = new BullAction();
            try
            {
                //ba.InsertBullInfo(ID.Text, MId.Text, FId.Text, Action.Text, Nation.Text);
                MessageBox.Show("SUCCESS!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
            
            
           
            
 


           

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySQLConnection(new MySQLConnectionString("localhost", "BullDb", "root", "881217").AsString);
            conn.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BullAction ba = new BullAction();
            try
            {
                ba.DeleteBullInfo(ID.Text);
                MessageBox.Show("SUCCESS!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }
    }
}
