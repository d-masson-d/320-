using System;
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
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using Microsoft.Win32;


namespace _320файлообмен
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void SendFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileBytes = File.ReadAllBytes(FilePathTextBox.Text);

                using (var client = new TcpClient("192.168.0.100", 12345))
                using (var stream = client.GetStream())
                {
                    stream.Write(fileBytes, 0, fileBytes.Length);
                    MessageBox.Show("File sent successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending file: {ex.Message}");
            }
        }

        private void ReceiveFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var listener = new TcpListener(IPAddress.Any, 12345);
                listener.Start();

                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                using (var fileStream = File.Create("received_file.txt"))
                {
                    stream.CopyTo(fileStream);
                    MessageBox.Show("File received successfully!");
                }

                listener.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving file: {ex.Message}");
            }
        }
    }
}

