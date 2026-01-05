using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramacionInstructores // Se modifica en cada proyecto
{
    class Conexion
    {
        // Toma la cadena desde App.config (name="Conexion")
        private static readonly string Cs =
            ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        public static SqlConnection ConDB = new SqlConnection(Cs);  // <<--- usa el MDF vía |DataDirectory|

        private SqlCommandBuilder Cmb;
        public DataSet Ds = new DataSet();
        public SqlDataAdapter Da;
        public SqlCommand Comando;
        public static class VarGlobal
        {
            public static string xNomDB = "dbProgInstructoresSQL"; //"dbFacturaSQL";
            public static int xEstIni = 2; // 1 = Invitado y 2 = Usuario Registrado
            public static int xswCNF = 0; //SWITCH CONEXION FORMULARIO
            public static int xImaFon = 0; // 1 = Actualizar Imagen Fondo y 0 = Sin Actualizar (xswCNF)
            public static int xOpcRte = 0;
            public static string xRuta = Application.StartupPath + "\\";// + "\\Fotos\\"; //Temporal
            public static string xNomU = "";
            public static string xTipoU = "01"; // 01 = Administrador - Tipo de Usuario Según PerfilRol.
            public static string xTipUN = "";   // 01 = Administrador - Tipo de Usuario Según PerfilRol
            public static Color xColorFondo; // poner colores RGB
            public static Color xColorPanel;
            public static Color xColorTitle;
            public static Color xColorFont;
            public static byte[] xFoto; // (byte[])fila["Foto"];
            public static byte[] xLogo; // (byte[])fila["Logo"];
            public static byte[] xFondo; // (byte[])fila["Fondo"];

            //Nuevos entornos

            public static Dictionary<string, Color> BotonColores = new Dictionary<string, Color>()
            {
                { "Nuevo", Color.LightGreen },
                { "Editar", Color.LightBlue },
                { "Eliminar", Color.IndianRed },
                { "Guardar", Color.LightGoldenrodYellow },
                { "Consultar", Color.LightGray },
                { "Imprimir", Color.LightSteelBlue },
                { "Salir", Color.LightPink }
            };

            public static Dictionary<string, Image> BotonIconos = new Dictionary<string, Image>()
            {
                { "Nuevo", null },
                { "Editar", null },
                { "Eliminar", null },
                { "Guardar", null },
                { "Consultar", null },
                { "Imprimir", null },
                { "Salir", null }
            };


        }
        public void CargarEstilosDesdeBD()
        {
            try
            {
                ConDB.Open();
                string sql = "SELECT NombreBoton, ColorARGB, Icono FROM EstilosBotones";
                SqlCommand cmd = new SqlCommand(sql, ConDB);
                SqlDataReader reader = cmd.ExecuteReader();

                VarGlobal.BotonColores.Clear();
                VarGlobal.BotonIconos.Clear();

                while (reader.Read())
                {
                    string nombre = reader["NombreBoton"].ToString();
                    int argb = Convert.ToInt32(reader["ColorARGB"]);

                    VarGlobal.BotonColores[nombre] = Color.FromArgb(argb);

                    if (reader["Icono"] != DBNull.Value)
                    {
                        byte[] iconoBytes = (byte[])reader["Icono"];
                        using (MemoryStream ms = new MemoryStream(iconoBytes))
                        {
                            VarGlobal.BotonIconos[nombre] = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        VarGlobal.BotonIconos[nombre] = null;
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar estilos desde la base de datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConDB.Close();
            }
        }
        
        public void GuardarEstilosEnBD()
        {
            try
            {
                ConDB.Open();

                foreach (var boton in VarGlobal.BotonColores.Keys)
                {
                    int colorArgb = VarGlobal.BotonColores[boton].ToArgb();
                    byte[] iconoBytes = null;

                    if (VarGlobal.BotonIconos.ContainsKey(boton) && VarGlobal.BotonIconos[boton] != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            VarGlobal.BotonIconos[boton].Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            iconoBytes = ms.ToArray();
                        }
                    }

                    // Verificar si ya existe
                    string sqlCheck = "SELECT COUNT(*) FROM EstilosBotones WHERE NombreBoton = @Nombre";
                    SqlCommand checkCmd = new SqlCommand(sqlCheck, ConDB);
                    checkCmd.Parameters.AddWithValue("@Nombre", boton);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        // INSERT
                        string insertSql = "INSERT INTO EstilosBotones (NombreBoton, ColorARGB, Icono) VALUES (@Nombre, @Color, @Icono)";
                        SqlCommand insertCmd = new SqlCommand(insertSql, ConDB);
                        insertCmd.Parameters.AddWithValue("@Nombre", boton);
                        insertCmd.Parameters.AddWithValue("@Color", colorArgb);
                        if (iconoBytes != null)
                            insertCmd.Parameters.Add("@Icono", System.Data.SqlDbType.VarBinary).Value = iconoBytes;
                        else
                            insertCmd.Parameters.Add("@Icono", System.Data.SqlDbType.VarBinary).Value = DBNull.Value;
                        insertCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // UPDATE
                        string updateSql = "UPDATE EstilosBotones SET ColorARGB = @Color, Icono = @Icono, UltimaMod = GETDATE() WHERE NombreBoton = @Nombre";
                        SqlCommand updateCmd = new SqlCommand(updateSql, ConDB);
                        updateCmd.Parameters.AddWithValue("@Color", colorArgb);
                        if (iconoBytes != null)
                            updateCmd.Parameters.Add("@Icono", System.Data.SqlDbType.VarBinary).Value = iconoBytes;
                        else
                            updateCmd.Parameters.Add("@Icono", System.Data.SqlDbType.VarBinary).Value = DBNull.Value;
                        updateCmd.Parameters.AddWithValue("@Nombre", boton);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar estilos en la base de datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConDB.Close();
            }
        }

        public bool Conectar() // Verificar la conexión de la base de datos
        {
            ConDB.Close(); //Linea nueva para controlar la longitud de los campos
            try { ConDB.Open();  return true;  }
            catch (Exception ex) { MessageBox.Show("No se puede conectar a la Base de Datos... Contacte al Administrador del Software \n\n" + ex.ToString());  return false; }
            finally  { ConDB.Close(); }
        }

        public bool ExisteT(string tabla) // Verifica si alguna tabla determinada Existe.
        {
            ConDB.Open();
            string ConsultaObj = @"SELECT Count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tabla + "'";
            Comando = new SqlCommand(ConsultaObj, ConDB);
            int i = Convert.ToInt32(Comando.ExecuteScalar());
            ConDB.Close();
            if (i > 0) { return true; }
            else { MessageBox.Show("No existe la tabla <<" + tabla + ">>"); return false; }
        }
        public SqlDataReader LenCampos(string tabla) //Consultar algun dato especifico como ID
        {
            ConDB.Close();
            ConDB.Open();
            string LenCampos = "Select Column_Name AS NombreCampo, Character_Maximum_Length AS LenCampo From Information_Schema.Columns Where Table_Name = '" + tabla + "'";
            Comando = new SqlCommand(LenCampos, ConDB);
            SqlDataReader LenC = Comando.ExecuteReader(); //Conn.Close();
            return LenC; 
        }
        public void ConsultaDatos(string sql, string tabla)
        {
            Ds.Tables.Clear();
            Da = new SqlDataAdapter(sql, ConDB);
            Cmb = new SqlCommandBuilder(Da);
            Da.Fill(Ds, tabla);
        }
        public bool ConsultaItem(string tabla, string condicion) //Consultar algun dato especifico como ID
        {
            ConDB.Open();
            string ConsultaItem = "Select Count(*) From " + tabla + " Where " + condicion;
            Comando = new SqlCommand(ConsultaItem, ConDB);
            int i = Convert.ToInt32(Comando.ExecuteScalar());
            ConDB.Close();
            if (i > 0) { return true; }
            else { return false; } //MessageBox.Show("Registro NO existente"); 
        }
        public DataTable ConsultaLista(string tabla, string condicion)
        {
            DataTable dt = new DataTable();
            ConDB.Open();
            string ConsultaLista = "SELECT * FROM " + tabla + " WHERE " + condicion;
            SqlDataAdapter adapter = new SqlDataAdapter(ConsultaLista, ConDB);
            adapter.Fill(dt);
            ConDB.Close();
            return dt;
        }
        public bool Insertar(string sql) //Metodo para llevar la información a la tabla (Guardar)
        {
            ConDB.Open();
            Comando = new SqlCommand(sql, ConDB);

            int i = Comando.ExecuteNonQuery();
            ConDB.Close();
            if (i > 0) { return true; }
            else { return false; }
        }

        public bool Actualizar(string tabla, string campos, string condicion) //Metodo para actualizar datos a la tabla (Guardar)
        {
            ConDB.Open();
            string Actualizar = "Update " + tabla + " set " + campos + " Where " + condicion;
            Comando = new SqlCommand(Actualizar, ConDB);
            int i = Comando.ExecuteNonQuery();
            ConDB.Close();
            if (i > 0) { return true; }
            else { return false; }
        }

        public bool ActualizarSinID(string tabla, string campos)
        {
            ConDB.Open();
            string Actualizar = "Update " + tabla + " set " + campos;
            Comando = new SqlCommand(Actualizar, ConDB);
            int i = Comando.ExecuteNonQuery();
            ConDB.Close();
            if (i > 0) { return true; }
            else { return false; }
        }

        public bool Eliminar(string tabla, string condicion) //Elimina un registro de la tabla
        {
            ConDB.Open();
            string Eliminar = "Delete From " + tabla + " Where " + condicion;
            Comando = new SqlCommand(Eliminar, ConDB);
            int i = Comando.ExecuteNonQuery();
            ConDB.Close();
            if (i > 0) { return true; }
            else { return false; }
        }
        public bool EliminarAll(string tabla) //Elimina todos los datos de la tabla
        {
            ConDB.Open();
            string Eliminar = "Delete * From " + tabla;
            Comando = new SqlCommand(Eliminar, ConDB);
            int i = Comando.ExecuteNonQuery();
            ConDB.Close();
            if (i > 0) { return true; }
            else { return false; }
        }

        internal bool ConsultaLista(string v1, object municipios, string v2, object idMpio, object lIKE, char v3, string v4)
        {
            throw new NotImplementedException();
        }

        internal bool ConsultaLista(string v1, bool v2)
        {
            throw new NotImplementedException();
        }
    }
}
