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
using TheoryOfInformation.lab1.Service;
using TheoryOfInformation.lab3.Encryptions.Keys;
using TheoryOfInformation.lab3.Encryptions.Models;
using TheoryOfInformation.lab3.Service;
using static TheoryOfInformation.lab3.Encryptions.TextWorker;

namespace TheoryOfInformation.lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ElGamal _key;

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

            string reportStr;
            if (_encode)
            {
                var resultUINT = Resizer.FromFile(bytesRaw, 1);
                var resultTMP = _key.Encrypte(resultUINT);
                var result = Resizer.ToFile(resultTMP, _key.resize);

                string source = string.Join(" ", resultUINT.Take(300).Select(x => $"({x:d9})"));
                string resStr = "";
                for (int i = 0; i < resultTMP.Take(600).Count(); i += 2)
                    resStr += $"({resultTMP[i]:d4},{resultTMP[i + 1]:d4}) ";
                reportStr = string.Join("\n", _key.y, source, resStr);

                File.WriteAllBytes(path + ".data", result);
            }
            else
            {
                var resultUINT = Resizer.FromFile(bytesRaw, _key.resize);
                var resultTMP = _key.Dencrypte(resultUINT);
                var result = Resizer.ToFile(resultTMP, 1);

                string resStr = string.Join(" ", resultTMP.Take(300).Select(x => $"({x:d9})"));
                string source = "";
                for (int i=0; i< resultUINT.Take(600).Count(); i += 2)
                    source += $"({resultUINT[i]:d4},{resultUINT[i + 1]:d4}) ";
                reportStr = string.Join("\n", _key.y, source, resStr);

                string filename = path.Replace(".data", "");
                filename = filename.Insert(filename.LastIndexOf('\\') + 1, "dec_");
                File.WriteAllBytes(filename, result);
            }

            if (_visualisation)
            {
                ReportWindow report = new ReportWindow();
                report.outputText.Text = reportStr;
                report.Show();
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

        bool IsGood(char c) => c > 47 && c < 59;

        private void BuildBTN_Click(object sender, RoutedEventArgs e)
        {
            BuildBTN.IsEnabled = false;
            try
            {
                _key = new ElGamal(uint.Parse(pBox.Text), uint.Parse(xBox.Text), uint.Parse(kBox.Text));
                gBox.ItemsSource = _key.roots;
                CounterLBL.Content = $"Кол-во: {_key.roots.Count}";
                gBox.SelectedIndex = 0;
                MainBTN.IsEnabled = true;
            } catch (Exception exp) {
                BuildBTN.IsEnabled = true;
                MessageBox.Show(exp.Message);
            }
        }

        private void ResetAll()
        {
            MainBTN.IsEnabled = false;
            BuildBTN.IsEnabled = true;
            CounterLBL.Content = $"Кол-во: {0}";
            gBox.ItemsSource = null;
            _key = null;
        }

        private void gBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex<0) return; 
            int s = ((ComboBox)sender).SelectedIndex;
            _key.ChangeG(s);
        }

        private void pBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ResetAll();
        }
    }
}
