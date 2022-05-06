using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TheoryOfInformation.lab3.Encryptions.Keys;
using TheoryOfInformation.lab3.Service;

namespace TheoryOfInformation.lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IKey _key;

        private bool _encode = false;
        private bool Encode { get => _encode; set { if (fileUnit_in != null) fileUnit_in.encrypt = value; _encode = value; } }
        public bool _visualisation { get; set; } = true;


        public MainWindow()
        {
            InitializeComponent();
            encCheck.IsChecked = true;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e) => Encode = encCheck.IsChecked.Value;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _key = new RabinKey(uint.Parse(pBox.Text), uint.Parse(qBox.Text), uint.Parse(bBox.Text));
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "My App");
                return;
            }

            MainBTN.IsEnabled = false;
            await Task.Delay(100);
            await EncodeFucntion();
            MainBTN.IsEnabled = true;
        }

        private async Task EncodeFucntion()
        {
            string path = fileUnit_in.OutputFile.Text;
            byte[] bytesRaw;

            using (FileStream SourceStream = new FileStream(path, FileMode.Open))
            {
                bytesRaw = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(bytesRaw, 0, (int)SourceStream.Length);
            }

            if (_encode)
            {
                var resultUINT = Resizer.FromFile(bytesRaw, 1);
                var resultTMP = _key.Encrypte(resultUINT);
                var result = Resizer.ToFile(resultTMP, _key.resize);


                File.WriteAllBytes(path + ".enc", result);

                if (_visualisation)
                {
                    string source = string.Join(" ", resultUINT.Take(300).Select(x => $"{x:d10}"));
                    string resStr = string.Join(" ", resultTMP.Take(300).Select(x => $"{x:d10}"));
                    string reportStr = string.Join("\n", source, resStr);

                    ReportWindow report = new ReportWindow();
                    report.outputText.Text = reportStr;
                    report.Show();
                }
            }
            else
            {
                var resultUINT = Resizer.FromFile(bytesRaw, _key.resize);
                var resultTMP = _key.Dencrypte(resultUINT);
                var result = Resizer.ToFile(resultTMP, 1);

                string filename = path.Replace(".enc", "");
                filename = filename.Insert(filename.LastIndexOf('\\') + 1, "dec_");
                File.WriteAllBytes(filename, result);
            }
        }

        private void keyBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(IsGood);
        }

        private void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            var stringData = (string)e.DataObject.GetData(typeof(string));
            if (stringData == null || !stringData.All(IsGood))
                e.CancelCommand();
        }

        bool IsGood(char c)
        {
            if (c > 47 && c < 58)
                return true;
            else
                return false;
        }
    }
}
