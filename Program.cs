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
                MessageBox.Show($"��������� ������: {e.Exception.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show($"��������� ����������� ������: {e.ExceptionObject}", "����������� ������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            try
            {
                DIContainer.Initialize();

                var databaseService = DIContainer.Resolve<IDatabaseService>();
                if (!databaseService.InitializeDatabase())
                {
                    MessageBox.Show("�� ������� ������������ � ���� ������. ��������� ��������� PostgreSQL.",
                        "������ ���� ������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var loginForm = DIContainer.Resolve<LoginForm>();
                Application.Run(loginForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����������� ������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DIContainer.Dispose();
            }
        }
    }
}
