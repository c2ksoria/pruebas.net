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
        EstadoForm State = new EstadoForm();
        public DataTable newDataTable = new DataTable();
        public List<string> BotonesActuales = new List<string>();
        public BindingSource bind = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }
        #region "LOAD Formulario"
        private void Form1_Load(object sender, EventArgs e)
        {
            //LoadData();
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

            DataTable ds = new DataTable();
            adaptador.Fill(ds);
            conector.Close();
            DataSet data = new DataSet("empleado");
            newDataTable = ds;
            bind.DataSource = ds;
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

                switch (btnDynamicButton.Name)
                {
                    case "botonAbajo":
                        {
                        bind.MoveNext();
                        Console.WriteLine("moviendo abajo... Posición: ", bind.Position);
                        break;
                        }
                    case "botonArriba":
                        {
                        bind.MovePrevious();
                        Console.WriteLine("moviendo Arriba... Posición: ", bind.Position);
                        break;
                        }
                    case "botonFin":
                        {
                        bind.MoveLast();
                        Console.WriteLine("moviendo FIN... Posición: ", bind.Position);
                        break;
                        }
                    case "botonInicio":
                        {
                        bind.MoveFirst();
                        Console.WriteLine("moviendo Inicio... Posición: ", bind.Position);
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
                    case "botonConfig":
                        {
                            Console.WriteLine("Configurar...");
                            break;
                        }
                    case "botonUnidad":
                        {
                            Console.WriteLine("Unidad...");
                            break;
                        }


                    default:
                        break;
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

        public void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("No hace nada este boton....");
            State.SetState("Espera");
            StateChanged();
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
}
