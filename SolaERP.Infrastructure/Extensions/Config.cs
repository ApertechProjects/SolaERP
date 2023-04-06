using Microsoft.Extensions.Configuration;
using System.Net;

namespace SolaERP.Infrastructure.Extensions
{
    //public static class Config
    //{
    //    public static RemoteFileServer FileServer { get=> new(); }
    //}

    public abstract class RemoteFileServer
    {
        static ConfigurationManager _configuration = new();
        private RemoteFileServer()
        {
            _configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../SolaERP.API"));
            _configuration.AddJsonFile("appsettings.json");
        }

        private static NetworkCredential _networkCredential;
        public static string IP { get => _configuration["RemoteFileServer:ServerAdress"]; }
        public static NetworkCredential Credential
        {
            get
            {
                _networkCredential = new(_configuration["RemoteFileServer:UserName"], _configuration["RemoteFileServer:Password"]);
                return _networkCredential;
            }
        }

        public static string FolderPath { get => _configuration["RemoteFileServer:FolderPath"]; }
    }
}
