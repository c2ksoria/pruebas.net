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
        StateForm State = new StateForm();
        private static string _path = @"C:\Users\Valdemar\Desktop\SFH\Pruebas\xxx\test\Recursos\setupBotones.json";
        List<Botones> xx = new List<Botones>();
        Dictionary<string, bool> habilitaciones = new Dictionary<string, bool>();
        
        private List<string> BotonesActuales = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }
        #region "LOAD Formulario"
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        #region "Setup de botones en base a permisos"
        private void Setup(List<(string, bool)> aProcesar)
        {
            try
            {
                for (int i = 0; i < aProcesar.Count; i++)
                {
                    if (aProcesar[i].Item2 == true)
                    {
                        for (int j = 0; j < PosicionesBotones.Count; j++)
                        {
                            if (aProcesar[i].Item1 == PosicionesBotones[j].Item1)
                            {
                                Inicialización(PosicionesBotones[j].Item4, PosicionesBotones[j].Item5, PosicionesBotones[j].Item6, PosicionesBotones[j].Item7, PosicionesBotones[j].Item3, PosicionesBotones[j].Item1, PosicionesBotones[j].Item8);
                                BotonesActuales.Add(aProcesar[i].Item1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error", ex.Message);
            }
            
        }
        private void Setup(List<(string, bool, string, string, string)> aProcesar)
        {
            try
            {
                for (int i = 0; i < aProcesar.Count; i++)
                {
                    if (aProcesar[i].Item2 == true)
                    {
                        for (int j = 0; j < PosicionesBotones.Count; j++)
                        {
                            if (aProcesar[i].Item1 == PosicionesBotones[j].Item1)
                            {
                                Inicialización(PosicionesBotones[j].Item4, PosicionesBotones[j].Item5, PosicionesBotones[j].Item6, PosicionesBotones[j].Item7, aProcesar[i].Item4, aProcesar[i].Item3, aProcesar[i].Item5);
                                BotonesActuales.Add(aProcesar[i].Item3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error",ex.Message);
            }
            
        }
        #endregion

        #region "Métodos DataGriedView"
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
        #region "Función de inicialización de DatagiedView"
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
        #endregion
        #region "Función Selección de Fila"
        private void Selected()
        {
            ///////////////////////////////////////////////////////////////////////////////////
            //Funcion de selección de Row. Segundo Intento más optimizado, utilizando una variable global
            DataGridViewRow row = dataGridView1.Rows[0];
            row.DefaultCellStyle.BackColor = Color.White;
            //entero = e.RowIndex;
            ///////////////////////////////////////////////////////////////////////////////////
        }
        #endregion
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
        #region "Limpieza de grilla"
        private void ClearData()
        {
            dataGridView1.DataSource = null;
        }
        #endregion
        private void data(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("----------");
            Console.WriteLine(dataGridView1.CurrentCell);
            if (e.KeyChar == '\r')
            {
                SendKeys.Send("{tab}");
            }
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
            for (int i = 0; i < (BotonesActuales.Count - 1); i++)
            {
                //Console.WriteLine(xx[i].Name);
                Button btn = (Button)this.Controls[BotonesActuales[i]];
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
        private void Inicialización(int xPos, int yPos, int xSize, int ySize, string textButton, string nameButton, string icono)
        {
            Button button = new Button();
            button.Text = textButton;
            button.Name = nameButton;
            button.Click += new EventHandler(btnDynamicButton_Click);
            button.Location = new Point(xPos, yPos);
            button.Size = new System.Drawing.Size(xSize, ySize);
            button.Image = Image.FromFile(icono);
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.TextAlign = ContentAlignment.MiddleRight;
            button.TextImageRelation= TextImageRelation.ImageBeforeText;
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
                    case "botonImprimir":
                        {
                            Console.WriteLine("Imprimiendo...");
                            break;
                        }
                    case "botonExportar":
                        {
                            Console.WriteLine("Exportando...");
                            break;
                        }
                    case "botonSucursales":
                        {
                            Console.WriteLine("Sucursales...");
                            break;
                        }
                    default:
                        break;
                }
            }
            if (btnDynamicButton.Name == "botonSalir")
            {
                this.Close();
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
        private void dataGridView1_Click(object sender, DataGridViewCellEventArgs e)
        {
            Selected();
        }
        public void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("No hace nada este boton....");
            ClearData();
            State.SetState("Espera");
            StateChanged();
            //textBox1.Text=button1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            BuscarDatos();
        }

        private void test(object sender, EventArgs e)
        {
            Console.WriteLine("**************//////////////************");
        }
        #endregion

        #region "Proceso de inicialización de botones del formulario"
        public void LoadData()
        {
            setupDatagriedView();
            ClearData();
            Setup(this.PermisosFuncionesNavegacion);
            Setup(this.PermisosFuncionesCustom);
            Setup(this.Basicas);
            State.SetState("Espera");
            StateChanged();
        }
        #region "Variables globales a ser pasadas como parámetros"
        public List<(string, bool)> Basicas { get; set; }
        public List<(string, bool, string, string, string)> PermisosFuncionesCustom { get; set; }
        public List<(string, bool, string, int, int, int, int, string)> PosicionesBotones { get; set; }
        public List<(string, bool)> PermisosFuncionesNavegacion { get; set; }
        #endregion

        #endregion
        #region Función de lectura de estado y cambio de visualización de botones
        public void StateChanged()
        {
            string EstadoActual = State.GetState();
            if (EstadoActual == "Espera")
            {
                BloqueoBotones(true);
            }
            else if (EstadoActual == "Buscar")
            {
                BloqueoBotones(true);
            }
            else if (EstadoActual == "Mostrar")
            {
                BloqueoBotones(false);
            }
        }
        #endregion
}
    #region "Clase que maneja el Estado del Formulario"
    class StateForm
    {
        public StateForm()
        {
            this.StateFormValue = "Inicio";
        }
        public string GetState()
        {
            return this.StateFormValue;
        }
        public void SetState(string StateFormValue)
        {;
            if (this.StateFormValue == "Inicio" && StateFormValue == "Espera")
            {
                this.StateFormValue = StateFormValue;
                //Console.WriteLine("entró al cambio de Inicio a Espera....: ",StateFormValue);
            }
            else if (this.StateFormValue == "Espera" && StateFormValue == "Buscar")
            {
                this.StateFormValue = StateFormValue;
            }
            else if (this.StateFormValue == "Buscar" && StateFormValue == "Mostrar")
            {
                this.StateFormValue = StateFormValue;
            }
            else if (this.StateFormValue == "Mostrar" && StateFormValue == "Espera")
            {
                this.StateFormValue = StateFormValue;
            }
        }
        private string StateFormValue = "";
    }
    #endregion
}
