using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; //Libreria Caracter '
using System.Windows.Forms; // Propiedades de Objetos y Formularios
using System.Drawing; // Manejo de los Colores

namespace ProgramacionInstructores //Nombre del Proyecto
{
    class Complementos
    {
        
        public static void PCEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) //comparamos si la tecla presionada es enter u otra
            {
                e.Handled = true; SendKeys.Send("{TAB}");//añadir la funcion de la tecla tab a la condicionada
            }
        }
        public static void ValChar(object Sender, EventArgs e)
        {
            if (Sender is ComboBox) { ComboBox txt = Sender as ComboBox; txt.Text = txt.Text.Replace("'", ""); }
            if (Sender is TextBox) { TextBox txt = Sender as TextBox; txt.Text = txt.Text.Replace("'", ""); }
        }
        public static void RecibeFoco(object Sender, EventArgs e)
        {
            if (Sender is ComboBox) { ComboBox txt = Sender as ComboBox; txt.BackColor = Color.LightSteelBlue; }
            if (Sender is TextBox) { TextBox txt = Sender as TextBox; txt.BackColor = Color.LightSteelBlue; } 
        }

        public static void PerderFoco(object Sender, EventArgs e)
        {//txt.Text = txt.Text.Replace("'","");
            if (Sender is ComboBox){ComboBox txt = Sender as ComboBox; txt.BackColor = Color.WhiteSmoke; txt.Text = txt.Text.Trim(); }
            if (Sender is TextBox){TextBox txt = Sender as TextBox; txt.BackColor = Color.WhiteSmoke; txt.Text = txt.Text.Trim();}     
        }//txt.Text = txt.Text.ToUpper(); //.ToUpper(); // Convertir a Mayuscula  

        public static void SoloTxT(KeyPressEventArgs txtN)
        {
            if (Char.IsLetter(txtN.KeyChar)) { txtN.Handled = false; }
            else if (Char.IsSeparator(txtN.KeyChar)) { txtN.Handled = false; }
            else if (Char.IsControl(txtN.KeyChar)) { txtN.Handled = false; }
            else { txtN.Handled = true; }
        }
        public static void SoloNumE(KeyPressEventArgs Num)
        {
            if (Char.IsDigit(Num.KeyChar)) { Num.Handled = false; }
            else if (Char.IsSeparator(Num.KeyChar)) { Num.Handled = false; }
            else if (Char.IsControl(Num.KeyChar)) { Num.Handled = false; }
            else { Num.Handled = true; }
        }
        public static void SoloNumD(KeyPressEventArgs txtN)
        {
            if (!char.IsControl(txtN.KeyChar) && !char.IsDigit(txtN.KeyChar) && txtN.KeyChar != '.')
            {
                txtN.Handled = true;
            }
        }
        public static void FormatoMoneda(TextBox xTbox)
        {
            if (xTbox.Text == string.Empty) {  return; }
            else
            {
                decimal Monto; Monto = Convert.ToDecimal(xTbox.Text); xTbox.Text = Monto.ToString("N0");
            }
        }
    }
}