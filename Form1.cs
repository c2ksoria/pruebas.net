using Newtonsoft.Json;
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
using testButton.Models;

namespace testButton
{
    public partial class Form1 : Form
    {
        int DynamicButtonCount = 1;
        int entero = 0;
        private static string _path = @"C:\Users\Valdemar\Desktop\SFH\Pruebas\xxx\test\Recursos\setupBotones.json";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            Inicialización(12, 33, "imprimir");
            Inicialización(12, 66, "exportar");



            var botones = getInfoBotonesFromFile();
            DesearializaerJsonFile(botones);
        }

        private void Inicialización(int xPos, int yPos, string textButton)
        {
            Button button = new Button();
            button.Text = textButton;
            button.Click += new EventHandler(DynamicButton_Click);
            button.Location = new Point(xPos, yPos);
            Controls.Add(button);
        }

        protected void DynamicButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("holas");
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string name = "Dynamic Button_" + DynamicButtonCount.ToString();
            Button btnDynamicButton = new Button();
            btnDynamicButton.Name = name;
            btnDynamicButton.Text = name;
            btnDynamicButton.Size = new System.Drawing.Size(200, 30);
            btnDynamicButton.Location = new System.Drawing.Point(40, (DynamicButtonCount * 40));
            btnDynamicButton.Click += new EventHandler(btnDynamicButton_Click);
            Controls.Add(btnDynamicButton);
            DynamicButtonCount++;
            this.dataGridView1.Rows.Add("1", "XX", 34);

        }

        private void btnDynamicButton_Click(object sender, EventArgs e)
        {
            Button btnDynamicButton = sender as Button;
            textBox1.AppendText(btnDynamicButton.Text);
            textBox1.AppendText(Environment.NewLine);
            switch (btnDynamicButton.Text)
            {
                case "Dynamic Button_3":
                    {
                        moverAbajo();
                        break;
                    }
                case "Dynamic Button_2":
                    {
                        moverArriba();
                        break;
                    }
                case "Dynamic Button_4":
                    {
                        moverFin();
                        break;
                    }
                case "Dynamic Button_1":
                    {
                        moverInicio();
                        break;
                    }

                default:
                    break;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("holas");
        }

        private void dataGridView1_Click(object sender, DataGridViewCellEventArgs e)
        {
            selected(e);
        }


        private void selected(DataGridViewCellEventArgs e)
        {
            ///////////////////////////////////////////////////////////////////////////////////
            //Funcion de selección de Row. Segundo Intento más optimizado, utilizando una variable globla
            DataGridViewRow row = dataGridView1.Rows[entero];
            row.DefaultCellStyle.BackColor = Color.White;
            entero = e.RowIndex;
            ///////////////////////////////////////////////////////////////////////////////////
        }
        #region "DataGriedView Methods"
        private int getCount() => dataGridView1.Rows.Count;
        private int getRow() => dataGridView1.CurrentCell.RowIndex;

        private int[] getInfoRow()
        {
            int countRows = getCount();
            int indexRow = getRow();
            int[] ArraInfo = new int[2] { countRows, indexRow };

            return ArraInfo;
        }

        private void moverAbajo()
        {
            int[] ArrayInfo = new int[2];
            ArrayInfo = getInfoRow();

            if (ArrayInfo[1] < (ArrayInfo[0] - 1))
            {
                DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, ArrayInfo[1]++);
                selected(ee);
                dataGridView1.CurrentCell = dataGridView1.Rows[ArrayInfo[1]++].Cells[0];
            }
        }
        private void moverArriba()
        {
            int[] ArrayInfo = new int[2];
            ArrayInfo = getInfoRow();

            if (ArrayInfo[1] > 0)
            {
                DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, ArrayInfo[1]--);
                selected(ee);
                dataGridView1.CurrentCell = dataGridView1.Rows[ArrayInfo[1]--].Cells[0];
            }
        }

        private void moverFin()
        {
            int[] ArrayInfo = new int[2];
            ArrayInfo = getInfoRow();
            DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, ArrayInfo[0] - 1);
            selected(ee);
            dataGridView1.CurrentCell = dataGridView1.Rows[ArrayInfo[0] - 1].Cells[0];
        }

        private void moverInicio()
        {
            DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, 0);
            selected(ee);
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
        }
        #endregion
        class Movie
        {
            public Movie(string name)
            {
                this.name = name;
            }

            public string getName()
            {
                return name;
            }
            private string name = "";

        }
        public static string getInfoBotonesFromFile()
        {
            string botonesInfoFromfile;
            using (var reader = new StreamReader(_path))
            {
                botonesInfoFromfile = reader.ReadToEnd();
            }
            return botonesInfoFromfile;
        }
        public static void DesearializaerJsonFile(string infoBotonesFromFiles)
        {
            var botones = JsonConvert.DeserializeObject<List<Botones>>(infoBotonesFromFiles);
            Console.WriteLine(infoBotonesFromFiles);
            Console.WriteLine(botones[0].Text);
        }
    }
}
