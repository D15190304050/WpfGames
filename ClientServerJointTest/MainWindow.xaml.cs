﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Threading;
using GobangClient;
using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ClientServerJointTest
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdNewWindow_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) e.OriginalSource;

            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window window = (Window) assembly.CreateInstance(type.Namespace + "." + button.Tag);

            window?.Show();
        }

        private void cmdStartGobangServer_Click(object sender, RoutedEventArgs e)
        {
            GobangServerWindow serverWindow = new GobangServerWindow();
            serverWindow.Show();

            //MessageBox.Show(Directory.GetCurrentDirectory());
            //Process serverProcess = Process.Start(Directory.GetCurrentDirectory() + "/GobangServer.exe");
        }

        private void cmdStartGobangClient_Click(object sender, RoutedEventArgs e)
        {
            //LoginWindow loginWindow = new LoginWindow();
            //loginWindow.Show();

            // There will be something wrong if the window is just created and shown in this process, but it will be totally OK if open the window using another process.
            Process.Start(Directory.GetCurrentDirectory() + "/GobangClient.exe");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //foreach (Process process in clientProcesses)
            //    process.Kill();
        }
    }
}
