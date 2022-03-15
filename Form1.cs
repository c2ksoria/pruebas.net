using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        List<(char, short)> aProcesar = new List<(char, short)> { ('N', 1), ('P', 2), ('E', 4), ('I', 8), ('C', 16), ('T', 32), ('Q', 64), ('B', 128), ('M', 256), ('A', 512) };
        public string nprog = "";
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

        #region

        public void LecturaPermisos(string NumeroDePrograma)
        {
            nprog = NumeroDePrograma;
            var myJsonString = File.ReadAllText(@"C:\Users\Valdemar\Desktop\SFH\Pruebas\xxx\test\Nivel3\Recursos\Programas.json");

            dynamic stuff = JsonConvert.DeserializeObject(myJsonString);
            foreach (var s in stuff)
            {
                Console.WriteLine(s.nProg);
                if (nprog == (string)s.nProg)
                    {
                    Console.WriteLine(s.Permisos);
                    //Console.WriteLine(s.PersonalizacionBotones[0].NombreBotonNuevo);
                    }
            }
        }
        public void GetCompare()
        {

        }

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
                MessageBox.Show("Hubo un error", ex.Message);
            }

        }
        #endregion

        #region "Lectura de Datos"
        void ImportarDatos(string nombrearchivo) //COMO PARAMETROS OBTENEMOS EL NOMBRE DEL ARCHIVO A IMPORTAR
        {
            DataTable ds = new DataTable();
            try
            {
                //UTILIZAMOS 12.0 DEPENDIENDO DE LA VERSION DEL EXCEL, EN CASO DE QUE LA VERSIÓN QUE TIENES ES INFERIOR AL DEL 2013, CAMBIAR A EXCEL 8.0 Y EN VEZ DE
                //ACE.OLEDB.12.0 UTILIZAR LO SIGUIENTE (Jet.Oledb.4.0)
                string conexion = string.Format("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {0}; Extended Properties = 'Excel 12.0;'", nombrearchivo);

                OleDbConnection conector = new OleDbConnection(conexion);

                conector.Open();

                //DEPENDIENDO DEL NOMBRE QUE TIENE LA PESTAÑA EN TU ARCHIVO EXCEL COLOCAR DENTRO DE LOS []
                OleDbCommand consulta = new OleDbCommand("select * from [test1$]", conector);

                OleDbDataAdapter adaptador = new OleDbDataAdapter
                {
                    SelectCommand = consulta
                };


                adaptador.Fill(ds);
                conector.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error", ex.Message);
            }

            DataSet data = new DataSet("empleado");
            newDataTable = ds;
            bind.DataSource = ds;
        }
        private void BuscarDatos()
        {
            State.SetState("Buscar");
            StateChanged();
            var path = @"C:\Users\Valdemar\Desktop\SFH\Pruebas\xxx\test\Recursos\test1.xls";
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
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            Controls.Add(button);
        }
        public virtual void btnDynamicButton_Click(object sender, EventArgs e)
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
            ConfigVentana(SetupVentana);
            Setup(this.PermisosFuncionesNavegacion);
            Setup(this.PermisosFuncionesCustom);
            Setup(this.Basicas);
            State.SetState("Espera");
            StateChanged();
        }

        #endregion

        private void ConfigVentana(Tuple<int, int, bool, bool, string, string> setupVentana)
        {
            this.MinimizeBox = setupVentana.Item3;
            this.MaximizeBox = setupVentana.Item4;
            this.Text = setupVentana.Item5;
            label1.Text = setupVentana.Item6;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Size = new System.Drawing.Size(setupVentana.Item1, setupVentana.Item2);

        }
        #region "Variables globales a ser pasadas como parámetros"
        public List<(string, bool)> Basicas { get; set; }
        public List<(string, bool, string, string, string)> PermisosFuncionesCustom { get; set; }
        public List<(string, bool, string, int, int, int, int, string)> PosicionesBotones { get; set; }
        public List<(string, bool)> PermisosFuncionesNavegacion { get; set; }
        public Tuple<int, int, bool, bool, string, string> SetupVentana { get; set; }


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

        #region Funciones de comparaciones de permisos
        private short PermisoBinario(char[] lectura)
        {
            //Console.WriteLine("función procesar...");
            short result = 0;

            foreach (char item in lectura)
            {
                foreach ((char, short) tupla in aProcesar)
                {
                    //Console.WriteLine(tupla);
                    if (item == tupla.Item1)
                    {
                        //Console.WriteLine("Hay una coincidencia: {0} -> {1}", tupla.Item1, tupla.Item2);
                        result += tupla.Item2;
                    }
                }
            }
            Console.WriteLine("El resultado es: {0} ", result);
            return result;
        }


        private void ComparaPermisos(string PermisoAcomparar, string PermisoReferencia)
        {
            char[] PaCompararArray = PermisoAcomparar.ToCharArray();
            char[] PrefArray = PermisoReferencia.ToCharArray();

            short dato1 = (short)PermisoBinario(PaCompararArray);
            short dato2 = (short)PermisoBinario(PrefArray);

            //TODO:Este proceso a continuación se podría hacer con un foreach
            //lo dejo planteado para más adelante por cuestiones de avances del proyecto.

            if ((dato1 & dato2 & aProcesar[9].Item2) == aProcesar[9].Item2)   //Letra A
            {
                //Console.WriteLine("TRUE: el resultado es A -> 512...procesar función. A Procesar: {0}, xx: {1}", aProcesar[9].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es A -> 512...procesar función. A Procesar: {0}, xx: {1}", aProcesar[9].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[8].Item2) == aProcesar[8].Item2)    //Letra M
            {
                //Console.WriteLine("TRUE: el resultado es M-> 256...procesar función. A Procesar: {0}, xx: {1}", aProcesar[8].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es M-> 256...procesar función. A Procesar: {0}, xx: {1}", aProcesar[8].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[7].Item2) == aProcesar[7].Item2)    //Letra B
            {
                //Console.WriteLine("TRUE: el resultado es B-> 128...procesar función. A Procesar: {0}, xx: {1}", aProcesar[7].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es B-> 128...procesar función. A Procesar: {0}, xx: {1}", aProcesar[7].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[6].Item2) == aProcesar[6].Item2)    //Letra Q
            {
                //Console.WriteLine("TRUE: el resultado es Q-> 64...procesar función. A Procesar: {0}, xx: {1}", aProcesar[6].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es Q-> 64...procesar función. A Procesar: {0}, xx: {1}", aProcesar[6].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[5].Item2) == aProcesar[5].Item2)    //Letra T
            {
                //Console.WriteLine("TRUE: el resultado es T-> 32...procesar función. A Procesar: {0}, xx: {1}", aProcesar[5].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es T-> 32...procesar función. A Procesar: {0}, xx: {1}", aProcesar[5].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[4].Item2) == aProcesar[4].Item2)    //Letra C
            {
                //Console.WriteLine("TRUE: el resultado es C-> 16...procesar función. A Procesar: {0}, xx: {1}", aProcesar[4].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es C-> 16...procesar función. A Procesar: {0}, xx: {1}", aProcesar[4].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[3].Item2) == aProcesar[3].Item2)    //Letra I
            {
                //Console.WriteLine("TRUE: el resultado es I-> 8...procesar función. A Procesar: {0}, xx: {1}", aProcesar[3].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es I-> 8...procesar función. A Procesar: {0}, xx: {1}", aProcesar[3].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[2].Item2) == aProcesar[2].Item2)    //Letra E
            {
                //Console.WriteLine("TRUE: el resultado es E-> 4...procesar función. A Procesar: {0}, xx: {1}", aProcesar[2].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es E-> 4 ...procesar función. A Procesar: {0}, xx: {1}", aProcesar[2].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[1].Item2) == aProcesar[1].Item2)    //Letra P
            {
                //Console.WriteLine("TRUE: el resultado es P-> 2 ...procesar función. A Procesar: {0}, xx: {1}", aProcesar[1].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es P-> 2 ...procesar función. A Procesar: {0}, xx: {1}", aProcesar[1].Item2, xx);
            }
            if ((dato1 & dato2 & aProcesar[0].Item2) == aProcesar[0].Item2)    //Letra N
            {
                //Console.WriteLine("TRUE: el resultado es N-> 1 ...procesar función. A Procesar: {0}, xx: {1}", aProcesar[9].Item2, xx);
            }
            else
            {
                //Console.WriteLine("FALSE: el resultado es N-> 1 ...procesar función. A Procesar: {0}, xx: {1}", aProcesar[0].Item2, xx);
            }

            Console.WriteLine("-----------FIN----------");
        }
        #endregion
    }
}
        