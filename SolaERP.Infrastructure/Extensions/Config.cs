using Microsoft.Extensions.Configuration;
using System.Net;

namespace SolaERP.Infrastructure.Extensions
{
    //public static class Config
    //{
    //    public static RemoteFileServer FileServer { get=> new(); }
    //}

    public class RemoteFileServer
    {
        private static ConfigurationManager _configuration;
        private object _serverLock = new object();

        public RemoteFileServer()
        {
            _configuration = CreateSingleConfigurationInstance();
            //_configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../SolaERP.API/"));
            _configuration.SetBasePath(Directory.GetCurrentDirectory());

            _configuration.AddJsonFile("appsettings.json");
        }

        private NetworkCredential _networkCredential;
        public string IP { get => _configuration["RemoteFileServer:ServerAdress"]; }
        public NetworkCredential Credential
        {
            get
            {
                _networkCredential = new(_configuration["RemoteFileServer:UserName"], _configuration["RemoteFileServer:Password"]);
                return _networkCredential;
            }
        }

        public string FolderPath { get => _configuration["RemoteFileServer:FolderPath"]; }

        private ConfigurationManager CreateSingleConfigurationInstance()
        {
            if (_configuration == null)
            {
                lock (_serverLock)
                {
                    if (_configuration is null)
                        _configuration = new();
                }
            }

            return _configuration;
        }



    }
}
