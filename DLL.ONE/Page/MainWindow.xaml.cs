using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace DLL.ONE
{
    public partial class MainWindow : Window
    {
        [DllImport("Password.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetPassword();

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        public MainWindow()
        {
            InitializeComponent();
            //////////////////////
            GET_STARTED_INFO_DLL();
        }

        private void GET_STARTED_INFO_DLL()
        {
            string _Name = "DLL.ONE.Resources.Password.dll";
            string _Path = Path.Combine(Path.GetTempPath(), "Password.dll");

            try
            {
                using (Stream _resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(_Name))
                {
                    if (_resource == null)
                        throw new FileNotFoundException($"Ресурс '{_Name}' не найден.");

                    using (var file = File.Create(_Path))
                    {
                        _resource.CopyTo(file);
                    }
                }

                if (LoadLibrary(_Path) == IntPtr.Zero)
                    throw new Exception("Не удалось загрузить DLL.");

                string pass = Marshal.PtrToStringAnsi(GetPassword());

                // Если нужно вывести пароль как уведомление:
                MessageBox.Show($"Пароль из DLL: {pass}");

                // Если нужно вывести пароль в GUI:
                Pass.Text = pass;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}