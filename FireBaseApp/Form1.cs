using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace FireBaseApp
{
    public partial class Form1 : Form
    {
        DataTable dt = new DataTable();
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "gCqZBoeODeG29Dgm8nzWGXkVhXRlWBRfEbTmAhge",
            BasePath = "https://fir-app-126f5.firebaseio.com/"
        };

        IFirebaseClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if (client!=null)
            {
                MessageBox.Show("Conection start");
            }
            dt.Columns.Add("LOGIN");
            dt.Columns.Add("NAME");
            dt.Columns.Add("PHONE");
            dt.Columns.Add("AGE");

            dataGridView1.DataSource = dt;


        }

        private async void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse resp = await client.GetTaskAsync("Counter/node");
            Counter_Class get = resp.ResultAs<Counter_Class>();

            //MessageBox.Show(get.cnt);

            var data = new Data
            {
                LOGIN = (Convert.ToInt32(get.cnt)+1).ToString(),
                NAME = textBox2.Text,
                PHONE = textBox3.Text,
                AGE = textBox4.Text
            };
            SetResponse response = await client.SetTaskAsync("information/"+ textBox1.Text, data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("Данные " + result.LOGIN + " записаны");

            var obj = new Counter_Class()
            {
                cnt = data.LOGIN
            };

            SetResponse response1 = await client.SetTaskAsync("Counter/node", obj);



        }

        private async void button2_Click(object sender, EventArgs e)
        {
 
            //FirebaseResponse response = await client.GetTaskAsync("Информация " + textBox1.Text);
            //Data obj = response.ResultAs<Data>();

            //MessageBox.Show($"{obj.LOGIN} Имя {obj.LOGIN} Телефон {obj.PHONE} Возраст {obj.AGE}");
            //textBox1.Text = obj.LOGIN;
            //textBox2.Text = obj.NAME;
            //textBox3.Text = obj.PHONE;
            //textBox4.Text = obj.AGE;

            //MessageBox.Show("Данные извлечены");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            export();
        }
        private async void export()
        {
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("Counter/node");
            Counter_Class obj1 = resp1.ResultAs<Counter_Class>();
            int cnt = Convert.ToInt32(obj1.cnt);



            while (true)
            {
                if (i==cnt)
                {
                    break;
                }
                i++;
                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("information/"+i);
                    Data obj2 = resp2.ResultAs<Data>();

                    DataRow row = dt.NewRow();
                    row["NAME"] = obj2.NAME;
                    row["LOGIN"] = obj2.LOGIN;
                    row["PHONE"] = obj2.PHONE;
                    row["AGE"] = obj2.AGE;

                    dt.Rows.Add(row);

                }
                catch
                {

                     
                }
            }
           

           

        }
    }
}
