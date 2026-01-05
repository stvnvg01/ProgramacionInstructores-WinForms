// EmailSettings.cs
using System;
using System.Data.SqlClient;

namespace ProgramacionInstructores
{
    public class EmailSettings
    {
        public string FromAddress { get; set; }
        public string AppPassword { get; set; }
        public string FromDisplay { get; set; }

        public static EmailSettings LoadFromDb(SqlConnection con)
        {
            const string sql = @"SELECT TOP 1 Correo, PinCor, NombreE FROM Empresa";
            using (var cmd = new SqlCommand(sql, con))
            {
                if (con.State != System.Data.ConnectionState.Open) con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) throw new Exception("Falta registro en Empresa con Correo/PinCor.");
                    return new EmailSettings
                    {
                        FromAddress = rd["Correo"] as string,
                        AppPassword = rd["PinCor"] as string,   // si cifras, aquí desencriptas
                        FromDisplay = rd["NombreE"] as string
                    };
                }
            }
        }
    }
}
