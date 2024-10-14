using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence
{
    internal class LinuxConnection : IDisposable
    {
        private SshClient _sshClient;
        private SftpClient _sftpClient;

        public LinuxConnection(string host, int port, string username, string password)
        {
            // Initialize SSH client
            _sshClient = new SshClient(host, port, username, password);
            _sftpClient = new SftpClient(host, port, username, password);

            try
            {
                // Connect to SSH server
                _sshClient.Connect();
                Console.WriteLine("SSH connection established.");

                // Connect to SFTP server (if needed)
                _sftpClient.Connect();
                Console.WriteLine("SFTP connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to server: {ex.Message}");
                throw; // Rethrow exception for handling
            }
        }

        public void ExecuteCommand(string command)
        {
            if (_sshClient.IsConnected)
            {
                var result = _sshClient.RunCommand(command);
                Console.WriteLine($"Command output: {result.Result}");
            }
        }

        public void UploadFile(string localFilePath, string remoteFilePath)
        {
            if (_sftpClient.IsConnected)
            {
                using (var fileStream = new FileStream(localFilePath, FileMode.Open))
                {
                    _sftpClient.UploadFile(fileStream, remoteFilePath);
                }
                Console.WriteLine("File uploaded successfully.");
            }
        }

        public void Dispose()
        {
            // Close connections
            if (_sshClient != null && _sshClient.IsConnected)
            {
                _sshClient.Disconnect();
                Console.WriteLine("SSH connection closed.");
            }

            if (_sftpClient != null && _sftpClient.IsConnected)
            {
                _sftpClient.Disconnect();
                Console.WriteLine("SFTP connection closed.");
            }
        }
    }
}
