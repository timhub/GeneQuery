using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySQLDriverCS;
namespace CommonMysql
{
    public class BullQuery
    {
        public MySqlConnection conn = new MySqlConnection("server=127.0.0.1;user id=root; password=111111; database=bulldb; pooling=false;charset=utf8");
        public string sqlcom = " ";
        public DataSet ds = new DataSet();
        public SqlDataReader MyReader;
        public MySqlDataAdapter adapterfill(MySqlCommand cmd)
        {
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            return sda;
        }
        public void sqlExcute(MySqlCommand cmd)
        {
            ds = new DataSet();
            MySqlDataAdapter sda =new MySqlDataAdapter(cmd);
            sda.SelectCommand.CommandTimeout = 1000;
            sda.Fill(ds);

        }

        public void sqlExcute(string sqlcmd)
        {
            ds = new DataSet();
            MySqlDataAdapter sda = new MySqlDataAdapter(sqlcmd, conn);
            sda.Fill(ds);

        }

        public void sqlExcute(string sqlcmd, string tablename)
        {
            ds = new DataSet();
            MySqlDataAdapter sda = new MySqlDataAdapter(sqlcmd, conn);
            sda.Fill(ds, tablename);

        }

        public void sqlExcute(MySqlCommand cmd, string tablename)
        {
            ds = new DataSet();
            MySqlDataAdapter sda = adapterfill(cmd);
            sda.Fill(ds, tablename);
        }


        public BullQuery()
        {
            conn.Open();
        }
        public void KillConnection()
        {
            conn.Close();
        }   
    }
}
