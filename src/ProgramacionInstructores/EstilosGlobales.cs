using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static ProgramacionInstructores.Conexion;

namespace ProgramacionInstructores
{
    public static class EstilosGlobales
    {
        public static void AplicarEstiloBoton(Button btn, string nombreBoton)
        {
            if (VarGlobal.BotonColores.ContainsKey(nombreBoton))
                btn.BackColor = VarGlobal.BotonColores[nombreBoton];

            if (VarGlobal.BotonIconos.ContainsKey(nombreBoton) && VarGlobal.BotonIconos[nombreBoton] != null)
                btn.Image = VarGlobal.BotonIconos[nombreBoton];
        }
    }
}
