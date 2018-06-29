using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Reflection;

namespace DestopToApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class penduduks
        {
            public string _id{get;set;}
            public string name { get; set; }
            public int __v { get; set; }
            public IList<string> status { get; set; }
            public DateTime Created_date { get; set; }
        }

        void get_data()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/penduduks");
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
            {
                string data = reader.ReadToEnd();
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(data, (typeof(DataTable)));
                dataGridView1.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            get_data();
        }

        public static DataTable convertStringToDataTable(string data)
        {
            DataTable dataTable = new DataTable();
            bool columnsAdded = false;
            foreach (string row in data.Split('$'))
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (string cell in row.Split('|'))
                {
                    string[] keyValue = cell.Split('~');
                    if (!columnsAdded)
                    {
                        DataColumn dataColumn = new DataColumn(keyValue[0]);
                        dataTable.Columns.Add(dataColumn);
                    }
                    dataRow[keyValue[0]] = keyValue[1];
                }
                columnsAdded = true;
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        public static DataTable ObjectToData(object o)
        {
            DataTable dt = new DataTable("OutputData");

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            o.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(o, null);
                    dt.Columns.Add(f.Name, f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(o, null);
                }
                catch { }
            });
            return dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/penduduks");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string data = "name="+textBox1.Text+"&NIP="+textBox2.Text;
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
            {
                MessageBox.Show(reader.ReadToEnd());
            }

            get_data();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/penduduks/"+textBox4.Text);
            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string data = "name=" + textBox1.Text + "&NIP=" + textBox2.Text;
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
            {
                MessageBox.Show(reader.ReadToEnd());
            }

            get_data();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int RowIndex = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[RowIndex];
            textBox4.Text = row.Cells[0].Value.ToString();
            textBox1.Text = row.Cells[1].Value.ToString();
            textBox2.Text = row.Cells[3].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/penduduks/" + textBox4.Text);
            request.Method = "DELETE";
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encode))
            {
                MessageBox.Show(reader.ReadToEnd());
            }

            get_data();
        }
    }
}
