﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace TheoryOfInformation.lab3
{
    /// <summary>
    /// Логика взаимодействия для FileWindow.xaml
    /// </summary>
    public partial class FileWindow : UserControl
    {
        public bool encrypt = true;
        public FileWindow()
        {
            InitializeComponent();
        }

        private void InputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = encrypt ? "All files (*.*)|*.*" : "Encrypted files (*.enc)|*.enc|All files (*.*)|*.*",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    ((Button)sender).Tag = filename;
                }
            }
        }
    }
}
