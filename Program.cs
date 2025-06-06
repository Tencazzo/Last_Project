using System;
using System.Windows.Forms;
using Project3.Infrastructure;
using Project3.Forms;
using Project3.Data;

namespace Project3
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += (sender, e) =>
            {
                MessageBox.Show($"Произошла ошибка: {e.Exception.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show($"Произошла критическая ошибка: {e.ExceptionObject}", "Критическая ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            try
            {
                DIContainer.Initialize();

                var databaseService = DIContainer.Resolve<IDatabaseService>();
                if (!databaseService.InitializeDatabase())
                {
                    MessageBox.Show("Не удалось подключиться к базе данных. Проверьте настройки PostgreSQL.",
                        "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var loginForm = DIContainer.Resolve<LoginForm>();
                Application.Run(loginForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DIContainer.Dispose();
            }
        }
    }
}
