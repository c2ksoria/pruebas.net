using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
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
        List<Botones> xx = new List<Botones>();
        bool bloqueoBotones = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            //Inicialización(12, 33, "imprimir");
            //Inicialización(12, 66, "exportar");

            setupDatagriedView();

            var botones = getInfoBotonesFromFile();

            
            xx=DesearializaerJsonFile(botones);

            for (int i = 0; i < 4; i++)
            {
                Inicialización(xx[i].Location.X, xx[i].Location.Y, xx[i].Size.X, xx[i].Size.Y, xx[i].Text, xx[i].Name);
            }

        }

        private void Inicialización(int xPos, int yPos, int xSize, int ySize, string textButton, string nameButton)
        {
            Button button = new Button();
            button.Text = textButton;
            button.Name = nameButton;
            button.Click += new EventHandler(btnDynamicButton_Click);
            button.Location = new Point(xPos, yPos);
            button.Size = new System.Drawing.Size(xSize, ySize);
            Controls.Add(button);
        }

        protected void DynamicButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("holas");
        }

        public void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Add("1", "XX", 34);

        }
        private void btnDynamicButton_Click(object sender, EventArgs e)
        {
            Button btnDynamicButton = sender as Button;
            textBox1.AppendText(btnDynamicButton.Text);
            textBox1.AppendText(Environment.NewLine);
            if (dataGridView1.Rows.Count!=0)
            {
                switch (btnDynamicButton.Name)
                {
                    case "botonAbajo":
                        {
                            moverAbajo();
                            break;
                        }
                    case "botonArriba":
                        {
                            moverArriba();
                            break;
                        }
                    case "botonFin":
                        {
                            moverFin();
                            break;
                        }
                    case "botonInicio":
                        {
                            moverInicio();
                            break;
                        }

                    default:
                        break;
                }
            }
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("holas");
        }

        private void dataGridView1_Click(object sender, DataGridViewCellEventArgs e)
        {
            Selected(e);
        }
        private void Selected(DataGridViewCellEventArgs e)
        {
            ///////////////////////////////////////////////////////////////////////////////////
            //Funcion de selección de Row. Segundo Intento más optimizado, utilizando una variable globla
            DataGridViewRow row = dataGridView1.Rows[0];
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
                Selected(ee);
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
                Selected(ee);
                dataGridView1.CurrentCell = dataGridView1.Rows[ArrayInfo[1]--].Cells[0];
            }
        }

        private void moverFin()
        {
            int[] ArrayInfo = new int[2];
            ArrayInfo = getInfoRow();
            DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, ArrayInfo[0] - 1);
            Selected(ee);
            dataGridView1.CurrentCell = dataGridView1.Rows[ArrayInfo[0] - 1].Cells[0];
        }

        private void moverInicio()
        {
            DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, 0);
            Selected(ee);
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
        }
        #endregion
        public static string getInfoBotonesFromFile()
        {
            string botonesInfoFromfile;
            using (var reader = new StreamReader(_path))
            {
                botonesInfoFromfile = reader.ReadToEnd();
            }
            return botonesInfoFromfile;
        }
        static List <Botones> DesearializaerJsonFile(string infoBotonesFromFiles)=> JsonConvert.DeserializeObject<List<Botones>>(infoBotonesFromFiles);

        private void button2_Click(object sender, EventArgs e)
        {
            BuscarDatos();
            changeLock(true);
        }

        private void BuscarDatos()
        {
            var path= @"C:\Users\Valdemar\Desktop\SFH\Pruebas\xxx\test\Recursos\test.xls";
            ImportarDatos(path);
        }

        void ImportarDatos(string nombrearchivo) //COMO PARAMETROS OBTENEMOS EL NOMBRE DEL ARCHIVO A IMPORTAR
        {
            //UTILIZAMOS 12.0 DEPENDIENDO DE LA VERSION DEL EXCEL, EN CASO DE QUE LA VERSIÓN QUE TIENES ES INFERIOR AL DEL 2013, CAMBIAR A EXCEL 8.0 Y EN VEZ DE
            //ACE.OLEDB.12.0 UTILIZAR LO SIGUIENTE (Jet.Oledb.4.0)
            string conexion = string.Format("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {0}; Extended Properties = 'Excel 12.0;'", nombrearchivo);

            OleDbConnection conector = new OleDbConnection(conexion);

            conector.Open();

            //DEPENDIENDO DEL NOMBRE QUE TIENE LA PESTAÑA EN TU ARCHIVO EXCEL COLOCAR DENTRO DE LOS []
            OleDbCommand consulta = new OleDbCommand("select * from [test$]", conector);

            OleDbDataAdapter adaptador = new OleDbDataAdapter
            {
                SelectCommand = consulta
            };

            DataSet ds = new DataSet();

            adaptador.Fill(ds);

            conector.Close();

           
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            ///////////////////////////////////////////////////////////////////////////////////
            //no sorteable script
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        
        }

        public void changeLock(bool set)
        {

            for (int i = 0; i < xx.Count; i++)
            {
                Console.WriteLine(xx[i].Name);
                Button btn = (Button)this.Controls[xx[i].Name];
                if (set == true)
                {

                    btn.Enabled= false;
                }
                else
                {
                    btn.Enabled = true;
                }
            }

            //    if (set == true)
            //{
            //    button1.Enabled = false;
            //    button2.Enabled = false;
            //    button3.Enabled = false;
            //    button4.Enabled = false;
            //    button5.Enabled = false;
            //    button6.Enabled = false;
            //    button7.Enabled = false;
            //    button8.Enabled = false;
            //    button9.Enabled = false;
            //}
            //else
            //{
            //    button1.Enabled = true;
            //    button2.Enabled = true;
            //    button3.Enabled = true;
            //    button4.Enabled = true;
            //    button5.Enabled = true;
            //    button6.Enabled = true;
            //    button7.Enabled = true;
            //    button8.Enabled = true;
            //    button9.Enabled = true;
            //}


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bloqueoBotones)
            {
                changeLock(bloqueoBotones);
                bloqueoBotones = false;
            }
            else
            {
                changeLock(bloqueoBotones);
                bloqueoBotones = true;
            }
        }
        private void setupDatagriedView()
        {
            dataGridView1.AllowDrop = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode=DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ScrollBars = ScrollBars.Vertical;

        }


}
}
