using Microsoft.AspNetCore.Http;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFile(UploadFile uploadFile);
    }
}
