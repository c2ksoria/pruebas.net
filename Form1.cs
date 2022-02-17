using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testButton
{


    public partial class Form1 : Form
    {
        int DynamicButtonCount = 1;
        int entero = 0;
        public Form1()
        {
            InitializeComponent();

        }



        private void Form1_Load(object sender, EventArgs e)
        {

            Inicialización(12, 33, "imprimir");
            Inicialización(12, 66, "exportar");
            //dataGridView1.SelectedRows.Clear();



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

        private void button1_Click(object sender, EventArgs e)
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
            /////////////////////////////////////////////////////////////////////////////////////
            /////
            //DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1,1);
            //selected(ee);
            /////////////////////////////////////////////////////////////////////////////////////
            /////



            //var dynamicButton = sender as Button;

            //textBox1.AppendText(dynamicButton.Text);
            //textBox1.AppendText(Environment.NewLine);
            ////MessageBox.Show("holas");
            ////dataGridView1.Rows[2].ReadOnly = true;
            //dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[2];
            //dataGridView1.Rows[0].Selected = true;

            moverAbajo();

        }

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    int index = dataGridView1.CurrentCell.RowIndex;
        //    textBox1.AppendText(index.ToString());
        //    Console.WriteLine(index);

        //}

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //int rowIndex = e.RowIndex;
            //DataGridViewRow row = dataGridView1.Rows[rowIndex];
            //textBox1.Text = dataGridView1.Rows[1].Cells[1].Value.ToString();// row.Cells[1].Value;
            //Console.WriteLine(rowIndex);
            Console.WriteLine("holas");
        }

        private void dataGridView1_Click(object sender, DataGridViewCellEventArgs e)
        {

            ///////////////////////////////////////////////////////////////////////////////////
            //Este primer intento con el foreach si funciona...voy a tratar de optimizarlo
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    Console.WriteLine(row.Index);
            //    DataGridViewRow row22 = dataGridView1.Rows[row.Index];
            //    row22.DefaultCellStyle.BackColor = Color.White;
            ///////////////////////////////////////////////////////////////////////////////////
            ///




            //}
            //dataGridView1.RowsDefaultCellStyle.BackColor = Color.Blue;

            ////dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;
            //dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Red;

            selected(e);
            //int countRows = getCount();
            //int indexRow = getRow();

           
            //textBox1.AppendText("La cantidad de filas es: "+countRows.ToString());
            //textBox1.AppendText(Environment.NewLine);
            //textBox1.AppendText("El índice de fila es: " + indexRow.ToString());
            //textBox1.AppendText(Environment.NewLine);
            //dataGridView1.DefaultCellStyle.BackColor = Color.Yellow;
            //dataGridView1.BackgroundColor = Color.Yellow;





        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1[1, 2];
        }

        private void selected(DataGridViewCellEventArgs e)
        {
            ///////////////////////////////////////////////////////////////////////////////////
            //Funcion de selección de Row. Segundo Intento más optimizado, utilizando una variable globla
            DataGridViewRow row = dataGridView1.Rows[entero];
            row.DefaultCellStyle.BackColor = Color.White;
            entero = e.RowIndex;
            DataGridViewRow row1 = dataGridView1.Rows[e.RowIndex];
            //row1.DefaultCellStyle.BackColor = Color.Green;
            ///////////////////////////////////////////////////////////////////////////////////
        }

        private int getCount() => dataGridView1.Rows.Count;
        private int getRow() => dataGridView1.CurrentCell.RowIndex;


   
        private void moverAbajo()
        {
            //int index = getRow();
            int countRows = getCount();
            int indexRow = getRow();
            textBox1.AppendText("La cantidad de filas es: " + countRows.ToString());
            textBox1.AppendText(Environment.NewLine);
            textBox1.AppendText("El índice de fila es: " + indexRow.ToString());
            textBox1.AppendText(Environment.NewLine);

            if (indexRow < (countRows-1))
            {
                DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, indexRow++);
                selected(ee);
                dataGridView1.CurrentCell = dataGridView1.Rows[indexRow++].Cells[0];
                //dataGridView1.Rows[indexRow].Selected = false;
                //dataGridView1.Rows[indexRow++].Selected = true;
                
            }
        }
        class Botones
        {

        }
    }
}
