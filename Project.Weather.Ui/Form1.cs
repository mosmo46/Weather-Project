using Project.Weather.Dal;
using Project.Weather.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project.Weather.Ui
{
    public partial class Form1 : Form
    {
        APICityRequest apiCityRequest = new APICityRequest();

        Request request = new Request();
        public Form1()
        {
            string path = @"C:\Users\User\OneDrive\Moshe Yaso Work\C#Projects\Project.Weather\Project.Weather.Ui\bin\Debug\TableDictionary.txt";
            InitializeComponent();
            if (File.Exists(path))
            {
                request.Load();
                apiCityRequest.Load();  
            }
        }

     

        private async void button1_Click(object sender, EventArgs e)
        {
            string cityName = textBox1.Text;
            int seacndRefersh = int.Parse(textBox2.Text);

            

            if (cityName != "")
            {
                await request.AddOrUpdateFile(cityName);
                request.RefreshByUser(cityName, seacndRefersh);
            }
            else
            {
                MessageBox.Show("Please enter city!");
            }

            addToTable(cityName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            request.StopRefreshUI();
        }

        public async void addToTable(string cityName)
        {
            var add = await apiCityRequest.GetCityData(cityName);
       
    
            dataGridView1.Rows.Add(add.location.name, add.current.temp_c);

         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            request.Save();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var file = apiCityRequest.LoadOneCity();
            label3.Visible = true;
            label4.Visible = true;

            pictureBox1.Visible = true;
            label3.Text = file.current.temp_c.ToString();
            label4.Text = file.location.name.ToString();

            string imgUrl = $"https:{file.current.condition.icon}";
            pictureBox1.Load(imgUrl);
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await request.UpdateList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}
