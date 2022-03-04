using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testButton
{
    internal class EstadoForm
    {
        public EstadoForm()
        {
            this.StateFormValue = "Inicio";
        }
        public string GetState()
        {
            return this.StateFormValue;
        }
        public void SetState(string StateFormValue)
        {
            ;
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
}
