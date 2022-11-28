using SolaERP.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
{
    public class EmailService : IEmailService
    {
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            if (email.Contains("@"))
            {
                string[] host = (email.Split('@'));
                string hostname = host[1];

                IPHostEntry IPhst = Dns.GetHostEntry(hostname);
                IPEndPoint endPt = new IPEndPoint(IPhst.AddressList[0], 25);
                Socket s = new Socket(endPt.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                if (endPt != null)
                    return true;
                else
                    return false;
            }
            return false;

        }
    }
}
