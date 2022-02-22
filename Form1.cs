﻿using Newtonsoft.Json;
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
        //int entero = 0;
        private static string _path = @"C:\Users\Valdemar\Desktop\SFH\Pruebas\xxx\test\Recursos\setupBotones.json";
        List<Botones> xx = new List<Botones>();
        bool bloqueoBotones = true;
        StateForm State = new StateForm();
        public Form1()
        {
            InitializeComponent();
        }
        #region "LOAD Formulario"
        private void Form1_Load(object sender, EventArgs e)
        {
            Inicio();
            BloqueoBotones(bloqueoBotones);
            ClearData();
            Console.WriteLine(State.GetState());
            State.SetState("Espera");
            Console.WriteLine(State.GetState());
        }
        #endregion

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
                Selected();
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
                Selected();
                dataGridView1.CurrentCell = dataGridView1.Rows[ArrayInfo[1]--].Cells[0];
            }
        }

        private void moverFin()
        {
            int[] ArrayInfo = new int[2];
            ArrayInfo = getInfoRow();
            //DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, ArrayInfo[0] - 1);
            Selected();
            dataGridView1.CurrentCell = dataGridView1.Rows[ArrayInfo[0] - 1].Cells[0];
        }

        private void moverInicio()
        {
            //DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(1, 0);
            Selected();
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
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
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ScrollBars = ScrollBars.Vertical;
        }
        private void Selected()
        {
            ///////////////////////////////////////////////////////////////////////////////////
            //Funcion de selección de Row. Segundo Intento más optimizado, utilizando una variable global
            DataGridViewRow row = dataGridView1.Rows[0];
            row.DefaultCellStyle.BackColor = Color.White;
            //entero = e.RowIndex;
            ///////////////////////////////////////////////////////////////////////////////////
        }
        private void data(object sender, KeyEventArgs e)
        {
            Console.WriteLine("----------");
            Console.WriteLine(dataGridView1.CurrentCell);
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                SendKeys.Send("{tab}");
            }
        }
        private void ClearData()
        {
            dataGridView1.DataSource = null;
        }
        #endregion

        #region "Lectura de Datos"
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
        private void BuscarDatos()
        {
            State.SetState("Buscar");
            StateChanged();
            var path = @"C:\Users\Valdemar\Desktop\SFH\Pruebas\xxx\test\Recursos\test.xls";
            ImportarDatos(path);
            State.SetState("Mostrar");
            StateChanged();

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
        static List<Botones> DesearializaerJsonFile(string infoBotonesFromFiles) => JsonConvert.DeserializeObject<List<Botones>>(infoBotonesFromFiles);
        #endregion

        #region "Métodos de Botones de navegacion"
        public void changeLock(bool set)
        {
            for (int i = 0; i < (xx.Count); i++)
            {
                //Console.WriteLine(xx[i].Name);
                Button btn = (Button)this.Controls[xx[i].Name];
                if (set == true)
                {

                    btn.Enabled = false;
                }
                else
                {
                    btn.Enabled = true;
                }
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
        private void btnDynamicButton_Click(object sender, EventArgs e)
        {
            Button btnDynamicButton = sender as Button;
            textBox1.AppendText(btnDynamicButton.Text);
            textBox1.AppendText(Environment.NewLine);
            if (dataGridView1.Rows.Count != 0)
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
                    case "botonCancelar":
                        {

                            this.Close();
                            break;
                        }

                    default:
                        break;
                }
            }

        }
        public void BloqueoBotones(bool bloqueoBotones)
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
        #endregion

        #region "Código de ayuda para hacer pruebas"
        private void button3_Click(object sender, EventArgs e)
        {
            //bloqueoBotones = !bloqueoBotones;
            //BloqueBotones(bloqueoBotones);
        }





 
    //public static void StateChanged(string nuevoEstado, Form1 form1)
    //{
    //    if (nuevoEstado == "Espera")
    //    {
    //        form1.bloqueoBotones = !form1.bloqueoBotones;
    //        form1.BloqueoBotones(form1.bloqueoBotones);
    //    }
    //}
    private void testingCell(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("doble click!");

            //TODO:
            //Hacer una función genérica para el comportamento de doble click dentro de la grilla.
        }
        private void dataGridView1_Click(object sender, DataGridViewCellEventArgs e)
        {
            Selected();
        }
        public void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("No hace nada este boton....");
            //dataGridView1.Rows.Clear();
            //dataGridView1.Refresh();
            dataGridView1.DataSource = null;
            State.SetState("Espera");
            StateChanged();


        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            BuscarDatos();
            

        }
        #endregion

        private void data(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("----------");
            Console.WriteLine(dataGridView1.CurrentCell);
            if (e.KeyChar=='\r')
            {
                SendKeys.Send("{tab}");
            }
        }
        private void test(object sender, EventArgs e)
        {
            Console.WriteLine("**************//////////////************");
        }
        private void Inicio()
        {
            
            setupDatagriedView();

            var botones = getInfoBotonesFromFile();

            xx = DesearializaerJsonFile(botones);

            for (int i = 0; i < xx.Count; i++)
            {
                Inicialización(xx[i].Location.X, xx[i].Location.Y, xx[i].Size.X, xx[i].Size.Y, xx[i].Text, xx[i].Name);
            }
 
        }

        public void StateChanged()
        {
            string data = State.GetState();
            if (data == "Espera")
            {
                BloqueoBotones(true);
            }
            else if(data == "Buscar")
            {
                BloqueoBotones(true);
            }
            else if (data == "Mostrar")
            {
                BloqueoBotones(false);
            }
        }
         
    }
    class StateForm
    {
        public StateForm()
        {
            this.StateFormValue = "Inicio";
            Console.WriteLine(this.StateFormValue);
        }
        public string GetState()
        {
            return StateFormValue;
        }
        public void SetState(string StateFormValue)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine(StateFormValue);
            if (this.StateFormValue == "Inicio" && StateFormValue == "Espera")
                {
                this.StateFormValue = StateFormValue;
                Console.WriteLine("entró al cambio de Inicio a Espera....: ",StateFormValue);
                }
            else if (this.StateFormValue == "Espera" && StateFormValue == "Buscar")
                {
                this.StateFormValue = StateFormValue;
                }
            else if (this.StateFormValue == "Buscar" && StateFormValue == "Mostrar")
                {
                this.StateFormValue = StateFormValue;
                }
            else if(this.StateFormValue == "Mostrar" && StateFormValue == "Espera")
                {
                this.StateFormValue = StateFormValue;
                }
            
            //Console.WriteLine(StateFormValue);
            Console.WriteLine("-----------------");
            
        }
        private string StateFormValue = "";
    }
}