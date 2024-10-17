using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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
            var fileBytes = File.ReadAllBytes(FilePathTextBox.Text);
            var fileSize = fileBytes.Length;

            using (var client = new TcpClient("192.168.0.100", 12345))
            using (var stream = client.GetStream())
            {
                int bytesSent = 0;
                int bufferSize = 4096;

                while (bytesSent < fileSize)
                {
                    int bytesToSend = Math.Min(bufferSize, fileSize - bytesSent);
                    stream.Write(fileBytes, bytesSent, bytesToSend);
                    bytesSent += bytesToSend;

                    StatusTextBlock.Text = $"Sent {bytesSent} of {fileSize} bytes.";
                }
            }

            MessageBox.Show("File sent successfully!");
        }

        private void ReceiveFile_Click(object sender, RoutedEventArgs e)
        {
            var listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();

            using (var client = listener.AcceptTcpClient())
            using (var stream = client.GetStream())
            using (var fileStream = File.Create("received_file.txt"))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                long totalBytesRead = 0;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;

                }

                MessageBox.Show("File received successfully!");
                listener.Stop();
            }
        }

        private void FilePathTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}



