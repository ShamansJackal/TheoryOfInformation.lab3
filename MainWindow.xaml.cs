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
using TheoryOfInformation.lab1.Encryptions.Keys;
using TheoryOfInformation.lab3.Encryptions;
using TheoryOfInformation.lab3.Encryptions.Models;
using static TheoryOfInformation.lab3.Encryptions.TextWorker;

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
                _key = new RabinKey(uint.Parse(pBox.Text), uint.Parse(qBox.Text), long.Parse(bBox.Text));
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

            if (!_encode)
            {
                var result = _key.Dencrypte(bytesRaw);
                string filename = path.Replace(".data", "");
                filename = filename.Insert(filename.LastIndexOf('\\') + 1, "dec_");
                File.WriteAllBytes(filename, null);
            }
            else
            {
                var result = _key.Encrypte(bytesRaw, out byte resize);

                if (_visualisation)
                {
                    ReportWindow report = new ReportWindow();
                    report.outputText.Text = string.Join(" ", result);
                    report.Show();
                }

                var resBytes = result.SelectMany(x => BitConverter.GetBytes(x).Take(resize).Reverse()).ToArray();
                File.WriteAllBytes(path + ".data", resBytes);
            }

            //if (_visualisation)
            //{
            //    string source = string.Join("", bytesRaw.Take(300).Select(x => ByteToStr(x)));
            //    string keyStr = string.Join("", bytes.Take(300).Select(x => ByteToStr(x)));
            //    string resStr = string.Join("", result.Take(300).Select(x => ByteToStr(x)));
            //    string reportStr = string.Join("\n", source, keyStr, resStr);

            //    ReportWindow report = new ReportWindow();
            //    report.outputText.Text = reportStr;
            //    report.Show();
            //}
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
