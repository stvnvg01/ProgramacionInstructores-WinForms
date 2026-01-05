using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Linq;
using System.Windows.Forms;

namespace ProgramacionInstructores
{
    // Arranca/crea LocalDB para el USUARIO ACTUAL
    static class LocalDbBootstrap
    {
        public static void EnsureLocalDbStarted()
        {
            var candidates = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),     @"Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),     @"Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),     @"Microsoft SQL Server\LocalDB\Binn\SqlLocalDB.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),  @"Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),  @"Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),  @"Microsoft SQL Server\LocalDB\Binn\SqlLocalDB.exe"),
                "SqlLocalDB.exe" // por si quedó en el PATH
            };

            string exe = candidates.FirstOrDefault(File.Exists) ?? "SqlLocalDB.exe";

            void Run(string args)
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = exe,
                        Arguments = args,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    using (var p = Process.Start(psi))
                    {
                        p?.WaitForExit(3000);
                    }
                }
                catch { /* ignorar: si falla, la conexión mostrará el error real */ }
            }

            // No falla si ya existe/está iniciada
            Run("create MSSQLLocalDB");
            Run("start MSSQLLocalDB");
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            // TLS por si haces HTTPS/correo
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // 1) Asegurar LocalDB para el usuario que ejecuta la app
            LocalDbBootstrap.EnsureLocalDbStarted();

            // 2) Definir DataDirectory -> %ProgramData%\ProgramacionInstructores\Data
            var dataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "ProgramacionInstructores", "Data");
            Directory.CreateDirectory(dataDir);

            // 3) Plan B: copiar MDF/LDF desde {app}\DB si faltan
            var appDir = Application.StartupPath;
            var srcDbDir = Path.Combine(appDir, "DB");
            var mdf = Path.Combine(dataDir, "dbProgInstructoresSQL.mdf");
            var ldf = Path.Combine(dataDir, "dbProgInstructoresSQL.ldf");

            try
            {
                if (!File.Exists(mdf) && Directory.Exists(srcDbDir))
                {
                    var mdfSrc = Path.Combine(srcDbDir, "dbProgInstructoresSQL.mdf");
                    var ldfSrc = Path.Combine(srcDbDir, "dbProgInstructoresSQL.ldf");
                    if (File.Exists(mdfSrc)) File.Copy(mdfSrc, mdf, true);
                    if (File.Exists(ldfSrc)) File.Copy(ldfSrc, ldf, true);
                }

                // 4) Quitar ReadOnly (evita error 3415 / cannot be upgraded)
                if (File.Exists(mdf))
                    File.SetAttributes(mdf, File.GetAttributes(mdf) & ~FileAttributes.ReadOnly);
                if (File.Exists(ldf))
                    File.SetAttributes(ldf, File.GetAttributes(ldf) & ~FileAttributes.ReadOnly);
            }
            catch
            {
                // Si algo falla aquí, el intento de conexión mostrará el motivo real
            }

            // 5) Fijar |DataDirectory| para que la cadena del App.config funcione (AttachDbFilename=|DataDirectory|...)
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

            // 6) UI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmAcceso());
        }
    }
}
